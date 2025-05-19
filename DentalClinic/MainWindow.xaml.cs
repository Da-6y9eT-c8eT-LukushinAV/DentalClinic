using DentalClinic.Data;
using DentalClinic.Models;
using DentalClinic.Wind;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DentalClinic
{
    public partial class MainWindow : Window
    {
        private readonly DentalContext _dbContext;
        private readonly User _currentUser;

        public MainWindow(DentalContext dbContext, User currentUser)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _currentUser = currentUser;
            LoadApplics();
            UserInfo.Text = $"Текущий пользователь: {_currentUser.FullName} (Логин: {_currentUser.Login})";
        }

        private void LoadApplics()
        {
            try
            {
                ApplicsGrid.ItemsSource = _dbContext.Applications
                    .Include(a => a.User)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddApplic_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new EditApplicWindow(_dbContext, null);
            if (addWindow.ShowDialog() == true)
            {
                LoadApplics();
            }
        }

        private void ApplicsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ApplicsGrid.SelectedItem is Applic selectedApplic)
            {
                var editWindow = new EditApplicWindow(_dbContext, selectedApplic);
                if (editWindow.ShowDialog() == true)
                {
                    LoadApplics();
                }
            }
        }

        private void AssignApplic_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicsGrid.SelectedItem is Applic selectedApplic && selectedApplic.UserId == null)
            {
                selectedApplic.UserId = _currentUser.Id;
                _dbContext.SaveChanges();
                LoadApplics();
                MessageBox.Show("Заявка выдана текущему пользователю.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Заявка уже выдана или не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UnassignApplic_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicsGrid.SelectedItem is Applic selectedApplic && selectedApplic.UserId == _currentUser.Id)
            {
                selectedApplic.UserId = null;
                _dbContext.SaveChanges();
                LoadApplics();
                MessageBox.Show("Заявка списана с текущего пользователя.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Заявка не выдана текущему пользователю или не выбрана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteApplic_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicsGrid.SelectedItem is Applic selectedApplic)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить заявку №{selectedApplic.ApplicationNumber}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _dbContext.Applications.Remove(selectedApplic);
                        _dbContext.SaveChanges();
                        LoadApplics();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления заявки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}