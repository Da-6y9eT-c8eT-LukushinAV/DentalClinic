using DentalClinic.Models;
using System;
using System.Windows;

namespace DentalClinic.Wind
{
    public partial class ChangeBookStatusWindow : Window
    {
        public BookStatus SelectedStatus { get; private set; }

        public ChangeBookStatusWindow(BookStatus currentStatus)
        {
            InitializeComponent();

            // Заполняем комбобокс статусов
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(BookStatus));
            StatusComboBox.SelectedItem = currentStatus;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите статус", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedStatus = (BookStatus)StatusComboBox.SelectedItem;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
} 