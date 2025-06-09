using DentalClinic.Data;
using DentalClinic.Models;
using System;
using System.Windows;

namespace DentalClinic.Wind
{
    public partial class EditBookWindow : Window
    {
        private readonly LibraryContext _dbContext;
        private readonly Book _book;

        public EditBookWindow(LibraryContext dbContext, Book book = null)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _book = book ?? new Book();

            // Заполняем комбобокс статусов
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(BookStatus));

            if (book != null)
            {
                // Режим редактирования
                Title = "Редактирование книги";
                ArticleNumberTextBox.Text = _book.ArticleNumber;
                TitleTextBox.Text = _book.Title;
                GenreTextBox.Text = _book.Genre;
                DescriptionTextBox.Text = _book.Description;
                ReleaseDatePicker.SelectedDate = _book.ReleaseDate.ToDateTime(TimeOnly.MinValue);
                StatusComboBox.SelectedItem = _book.Status;
            }
            else
            {
                // Режим создания
                Title = "Добавление книги";
                ReleaseDatePicker.SelectedDate = DateTime.Today;
                StatusComboBox.SelectedItem = BookStatus.Available;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ArticleNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(GenreTextBox.Text) ||
                ReleaseDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _book.ArticleNumber = ArticleNumberTextBox.Text;
                _book.Title = TitleTextBox.Text;
                _book.Genre = GenreTextBox.Text;
                _book.Description = DescriptionTextBox.Text;
                _book.ReleaseDate = DateOnly.FromDateTime(ReleaseDatePicker.SelectedDate.Value);
                _book.Status = (BookStatus)StatusComboBox.SelectedItem;

                if (_book.Id == 0)
                {
                    _dbContext.Books.Add(_book);
                }

                _dbContext.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
} 