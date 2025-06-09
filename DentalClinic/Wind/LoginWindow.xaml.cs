using DentalClinic.Data;
using DentalClinic.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace DentalClinic.Wind
{
    public partial class LoginWindow : Window
    {
        private readonly LibraryContext _dbContext;
        public User LoggedInUser { get; private set; }

        public LoginWindow(LibraryContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _dbContext.Users
                .Include(u => u.BorrowedBooks)
                .FirstOrDefault(u => u.Login == LoginTextBox.Text && u.Password == PasswordBox.Password);

            if (user != null)
            {
                LoggedInUser = user;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}