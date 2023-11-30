using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HRManagementSystem.Windows
{
    public partial class SignInWindow : Window
    {
        private HrManagementDb hrDb;
        private string PasswordText;
        private bool IsPasswording;
        public SignInWindow()
        {
            InitializeComponent();
            hrDb = new HrManagementDb();
            HrDbTransfer.SetHrManagementDb(hrDb);
            PasswordText = "";
            IsPasswording = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader reader = new StreamReader("../../Translation/LastLanguage/LastLang.txt"))
                {
                    string line = reader.ReadLine();
                    if (line == null) 
                    {
                        line = "en";
                    }
                    LanguageTransfer.CurrentLanguage = line;
                }
                TranslateWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LanguageTransfer.CurrentLanguage = "en";
            }
        }

        private void TranslateWindow()
        {
            textBlockHeader.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "siwMainHeader");
            tbLogin.Tag = tbLogin.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "siwLogin");
            tbPassword.Tag = tbPassword.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "siwPassword");
            btnLogIn.Content = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "siwbtnLogin");
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
                tb.BorderThickness = new Thickness(1);
            }
        }

        private void tbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            pbPassword.Visibility = Visibility.Visible;
            pbPassword.Focus();
            tbPassword.Visibility = Visibility.Hidden;
        }

        private void pbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pbPassword.Password))
            {
                tbPassword.Visibility = Visibility.Visible;
                pbPassword.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbLogin.Text) ||  tbLogin.Text == tbLogin.Tag.ToString())
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXSIWLogEmpty"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (string.IsNullOrEmpty(pbPassword.Password))
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXSIWPasEmpty"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!hrDb.UserAnyByLoginPassword(tbLogin.Text, pbPassword.Password))
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXSIWWrongPasLog"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
