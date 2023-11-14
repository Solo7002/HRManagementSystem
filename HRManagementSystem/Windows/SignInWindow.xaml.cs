using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;

namespace HRManagementSystem.Windows
{
    public partial class SignInWindow : Window
    {
        private HrManagementDb hrDb;
        public SignInWindow()
        {
            InitializeComponent();
            hrDb = new HrManagementDb();
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == tb.Tag.ToString())
            {
                tb.Text = "";
                tb.Foreground = Brushes.White;
                tb.FontSize = 24;
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == "")
            {
                tb.Text = tb.Tag.ToString();
                tb.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
                tb.FontSize = 22;
                tb.BorderBrush = Brushes.Gray;
                tb.BorderThickness = new Thickness(0.56);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _tempSum = 0;
                foreach (var item in grid.Children)
                {
                    if (item is TextBox tb)
                    {
                        if (tb.Text == tb.Tag.ToString() || tb.Text.All(t => t == ' '))
                        {
                            tb.BorderThickness = new Thickness(1);
                            tb.BorderBrush = Brushes.Red;
                            _tempSum++;
                        }
                        else
                        {
                            tb.BorderThickness = new Thickness(1);
                            tb.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
                        }
                    }
                }
                if (_tempSum > 0)
                {
                    MessageBox.Show($"Fields can't be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!hrDb.UserAnyByLoginPassword(tbLogin.Text, tbPassword.Text))
                {
                    MessageBox.Show("No users with such login and password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    CurrentUserTransfer.employee = hrDb.GetEmployeeByLogin(tbLogin.Text);

                    new MainProgramWindow().Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
