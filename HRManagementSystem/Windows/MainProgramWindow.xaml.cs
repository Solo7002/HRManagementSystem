using HRManagementSystem.ControlClasses;
using HRManagementSystem.Pages;
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
            MainProgramPageControl.currentWindow = this;

            List<string> list = new List<string>();
            list.Add("Profile");
            list.Add("Employees");
            list.Add("Reviews");
            list.Add("Settings");

            foreach (string item in list)
            {
                lbMenu.Items.Add(new ListBoxItem { Content = item, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 70, Tag = item });

                (lbMenu.Items[lbMenu.Items.Count - 1] as ListBoxItem).Style = (Style)this.FindResource("lbItemOrange");
            }

            lbLogOut.Items.Add(new ListBoxItem { Content = "Log out", HorizontalContentAlignment = HorizontalAlignment.Center, Height = 80});
            (lbLogOut.Items[0] as ListBoxItem).Style = (Style)this.FindResource("lbItemOrange");
        }

        private void lbMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (ListBoxItem item in lbMenu.Items)
            {
                item.FontWeight = FontWeights.Normal;
            }
            (lbMenu.SelectedItem as ListBoxItem).FontWeight = FontWeights.Medium;

            MainFrame.Content = MainProgramPageControl.GetPageByName((lbMenu.SelectedItem as ListBoxItem).Tag.ToString());
        }

        private void lbLogOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbLogOut.SelectedIndex == 0 && MessageBox.Show("Are you sure you want log out?", "Loging out", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)  
            {
                new SignInWindow().Show();
                Close();
            }
        }
    }
}
