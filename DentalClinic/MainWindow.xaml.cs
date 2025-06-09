using DentalClinic.Data;
using DentalClinic.Models;
using DentalClinic.Wind;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DentalClinic
{
    public partial class MainWindow : Window
    {
        private readonly LibraryContext _dbContext;
        private User _currentUser;

        public MainWindow()
        {
            InitializeComponent();
            _dbContext = new LibraryContext();
            LoadBooks();
            UpdateUIState();
        }

        private void LoadBooks()
        {
            try
            {
                BooksGrid.ItemsSource = _dbContext.Books
                    .Include(b => b.Reader)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки книг: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateUIState()
        {
            bool isLoggedIn = _currentUser != null;
            LoginButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
            RegisterButton.Visibility = isLoggedIn ? Visibility.Collapsed : Visibility.Visible;
            LogoutButton.Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
            UserInfo.Text = isLoggedIn ? $"Текущий пользователь: {_currentUser.FullName} (Логин: {_currentUser.Login})" : "Пожалуйста, войдите в систему";
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Необходимо войти в систему", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var addWindow = new EditBookWindow(_dbContext);
            if (addWindow.ShowDialog() == true)
            {
                LoadBooks();
            }
        }

        private void BooksGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Необходимо войти в систему", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                var editWindow = new EditBookWindow(_dbContext, selectedBook);
                if (editWindow.ShowDialog() == true)
                {
                    LoadBooks();
                }
            }
        }

        private void BorrowBook_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Необходимо войти в систему", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (BooksGrid.SelectedItem is Book selectedBook && selectedBook.Status == BookStatus.Available)
            {
                selectedBook.ReaderId = _currentUser.Id;
                selectedBook.Status = BookStatus.Borrowed;
                _dbContext.SaveChanges();
                LoadBooks();
                MessageBox.Show("Книга выдана.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Книга недоступна или не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ReturnBook_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Необходимо войти в систему", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (BooksGrid.SelectedItem is Book selectedBook && selectedBook.ReaderId == _currentUser.Id)
            {
                selectedBook.ReaderId = null;
                selectedBook.Status = BookStatus.Available;
                _dbContext.SaveChanges();
                LoadBooks();
                MessageBox.Show("Книга возвращена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Книга не выдана вам или не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Необходимо войти в систему", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                var statusWindow = new ChangeBookStatusWindow(selectedBook.Status);
                if (statusWindow.ShowDialog() == true)
                {
                    selectedBook.Status = statusWindow.SelectedStatus;
                    if (selectedBook.Status != BookStatus.Borrowed)
                    {
                        selectedBook.ReaderId = null;
                    }
                    _dbContext.SaveChanges();
                    LoadBooks();
                }
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Необходимо войти в систему", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить книгу \"{selectedBook.Title}\"?", 
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _dbContext.Books.Remove(selectedBook);
                        _dbContext.SaveChanges();
                        LoadBooks();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow(_dbContext);
            if (loginWindow.ShowDialog() == true)
            {
                _currentUser = loginWindow.LoggedInUser;
                UpdateUIState();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(_dbContext);
            if (registerWindow.ShowDialog() == true)
            {
                _currentUser = registerWindow.RegisteredUser;
                UpdateUIState();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _currentUser = null;
            UpdateUIState();
        }
    }
}