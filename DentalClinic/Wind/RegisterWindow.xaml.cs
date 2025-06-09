using DentalClinic.Data;
using DentalClinic.Models;
using System;
using System.Linq;
using System.Windows;

namespace DentalClinic.Wind
{
    public partial class RegisterWindow : Window
    {
        private readonly LibraryContext _dbContext;
        public User RegisteredUser { get; private set; }

        public RegisterWindow(LibraryContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_dbContext.Users.Any(u => u.Login == LoginTextBox.Text))
            {
                MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newUser = new User
                {
                    Login = LoginTextBox.Text,
                    Password = PasswordBox.Password,
                    FullName = FullNameTextBox.Text,
                    PhoneNumber = PhoneNumberTextBox.Text,
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Today)
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                RegisteredUser = newUser;
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}