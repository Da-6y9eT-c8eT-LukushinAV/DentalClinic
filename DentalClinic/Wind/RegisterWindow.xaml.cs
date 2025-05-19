using DentalClinic.Data;
using DentalClinic.Models;
using System;
using System.Linq;
using System.Windows;

namespace DentalClinic.Wind
{
    public partial class RegisterWindow : Window
    {
        private readonly DentalContext _dbContext;

        public RegisterWindow(DentalContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var login = LoginTextBox.Text.Trim();
                var password = PasswordBox.Password;
                var fullName = FullNameTextBox.Text.Trim();
                var phoneNumber = PhoneNumberTextBox.Text.Trim();

                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                    throw new Exception("Логин и пароль обязательны.");
                if (_dbContext.Users.Any(u => u.Login == login))
                    throw new Exception("Пользователь с таким логином уже существует.");
                if (string.IsNullOrEmpty(fullName))
                    throw new Exception("ФИО обязательно.");
                if (string.IsNullOrEmpty(phoneNumber))
                    throw new Exception("Номер телефона обязателен.");

                var user = new User
                {
                    Login = login,
                    Password = password,
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                    FullName = fullName,
                    PhoneNumber = phoneNumber
                };

                _dbContext.Users.Add(user);
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