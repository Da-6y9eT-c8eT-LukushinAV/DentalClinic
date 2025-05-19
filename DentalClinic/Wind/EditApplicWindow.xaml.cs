using DentalClinic.Data;
using DentalClinic.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DentalClinic.Wind
{
    public partial class EditApplicWindow : Window
    {
        private readonly DentalContext _dbContext;
        private Applic _applic;

        public EditApplicWindow(DentalContext dbContext, Applic applic)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _applic = applic ?? new Applic { RegistrationDate = DateOnly.FromDateTime(DateTime.Now) };

            // Загрузка пользователей в ComboBox
            UserComboBox.ItemsSource = _dbContext.Users.ToList();
            UserComboBox.SelectedValue = _applic.UserId; // Устанавливаем текущего исполнителя

            ApplicationNumberTextBox.Text = _applic.ApplicationNumber;
            ShortDescriptionTextBox.Text = _applic.ShortDescription;
            TypeTextBox.Text = _applic.Type;
            FullDescriptionTextBox.Text = _applic.FullDescription;
            StatusComboBox.SelectedItem = StatusComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _applic.Status.ToString());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ApplicationNumberTextBox.Text) || string.IsNullOrEmpty(ShortDescriptionTextBox.Text))
                    throw new Exception("Артикул и краткое описание обязательны.");

                if (_applic.Id == 0 && _dbContext.Applications.Any(a => a.ApplicationNumber == ApplicationNumberTextBox.Text))
                    throw new Exception("Заявка с таким артикулом уже существует.");

                _applic.ApplicationNumber = ApplicationNumberTextBox.Text;
                _applic.ShortDescription = ShortDescriptionTextBox.Text;
                _applic.Type = TypeTextBox.Text;
                _applic.FullDescription = FullDescriptionTextBox.Text;
                _applic.Status = (Status)Enum.Parse(typeof(Status), (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString());
                _applic.UserId = (int?)UserComboBox.SelectedValue; // Обновляем исполнителя

                if (_applic.Id == 0)
                    _dbContext.Applications.Add(_applic);
                else
                    _dbContext.Applications.Update(_applic);

                _dbContext.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}