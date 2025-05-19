using DentalClinic.Data;
using DentalClinic.Wind;
using System.Windows;

namespace DentalClinic
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var loginWindow = new LoginWindow();
            var result = loginWindow.ShowDialog();

            Console.WriteLine($"Результат ShowDialog: {result}"); // Отладка

            if (result == true)
            {
                var dbContext = loginWindow.GetDbContext();
                var currentUser = loginWindow.CurrentUser;

                if (dbContext == null)
                {
                    Console.WriteLine("Ошибка: DbContext равен null.");
                    MessageBox.Show("Ошибка инициализации базы данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (currentUser == null)
                {
                    Console.WriteLine("Ошибка: CurrentUser равен null.");
                    MessageBox.Show("Ошибка авторизации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Console.WriteLine($"Переход к MainWindow: User={currentUser.FullName}, DbContext={dbContext != null}");
                var mainWindow = new MainWindow(dbContext, currentUser);
                mainWindow.Show();
            }
            
        }
    }
}