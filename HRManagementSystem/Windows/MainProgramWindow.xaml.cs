using HRManagementSystem.ControlClasses;
using HRManagementSystem.Pages;
using HRManagementSystem.TransferClasses;
using System;
using System.Collections.Generic;
using System.Linq;
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
using HRManagementSystem.Translation;
using System.IO;

namespace HRManagementSystem.Windows
{
    public partial class MainProgramWindow : Window
    {
        public MainProgramWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MainProgramPageControl.currentWindow = this;

                List<string> list = new List<string>();
                list.Add("Profile");
                list.Add("Employees");
                if (CurrentUserTransfer.employee.User.Level.LevelName!="Worker")
                {
                    list.Add("Departments");
                    list.Add("Jobs");
                }
                list.Add("Reviews");
                list.Add("Settings");

                foreach (string item in list)
                {
                    lbMenu.Items.Add(new ListBoxItem { Content = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, $"mpwL{item}"), HorizontalContentAlignment = HorizontalAlignment.Center, Height = 70, Tag = item });

                    (lbMenu.Items[lbMenu.Items.Count - 1] as ListBoxItem).Style = (Style)this.FindResource("lbItemOrange");
                }

                lbLogOut.Items.Add(new ListBoxItem { Content = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, $"mpwLLogOut"), HorizontalContentAlignment = HorizontalAlignment.Center, Height = 80 });
                (lbLogOut.Items[0] as ListBoxItem).Style = (Style)this.FindResource("lbItemOrange");

                if (lbMenu.Items.Count>0)
                {
                    lbMenu.SelectedIndex = 0;
                }

                LanguageTransfer.lbMain = lbMenu;
                LanguageTransfer.lbLogOut = lbLogOut;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lbMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                foreach (ListBoxItem item in lbMenu.Items)
                {
                    item.FontWeight = FontWeights.Normal;
                }

                if (lbMenu.SelectedIndex == -1)
                {
                    return;
                }
                (lbMenu.SelectedItem as ListBoxItem).FontWeight = FontWeights.Medium;

                MainFrame.Content = MainProgramPageControl.GetPageByName((lbMenu.SelectedItem as ListBoxItem).Tag.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lbLogOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbLogOut.SelectedIndex == 0)  
            {
                if (MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXMPWLogOut"), "Loging out", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    new SignInWindow().Show();
                    Close();
                }
                else
                {
                    lbLogOut.SelectedIndex = -1;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("../../Translation/LastLanguage/LastLang.txt"))
                {
                    writer.WriteLine(LanguageTransfer.CurrentLanguage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
