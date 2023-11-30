using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HRManagementSystem.Pages
{
    public partial class SettingsPage : Page
    {
        private bool IsFirstTime;
        public SettingsPage()
        {
            InitializeComponent();

            IsFirstTime = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsFirstTime = true;
            textBlockHeader.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "SPMainHeader");
            textBlockLanguage.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "SPLanguage");
            for (int i = 0; i < cbLanguages.Items.Count;i++)
            {
                if ((cbLanguages.Items[i] as ComboBoxItem).Tag.ToString() == LanguageTransfer.CurrentLanguage)
                {
                    cbLanguages.SelectedIndex = i;
                    break;
                }
            }
            IsFirstTime = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsFirstTime) 
            {
                LanguageTransfer.CurrentLanguage = (cbLanguages.SelectedItem as ComboBoxItem).Tag.ToString();
                textBlockHeader.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "SPMainHeader");
                textBlockLanguage.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "SPLanguage");

                using (StreamWriter writer = new StreamWriter("../../Translation/LastLanguage/LastLang.txt"))
                {
                    writer.WriteLine(LanguageTransfer.CurrentLanguage);
                }

                UpdateListBoxes(LanguageTransfer.lbMain, LanguageTransfer.lbLogOut);
            }
        }

        private void UpdateListBoxes(ListBox lbMenu, ListBox lbLogOut)
        {
            lbMenu.Items.Clear();
            lbLogOut.Items.Clear();

            List<string> list = new List<string>();
            list.Add("Profile");
            list.Add("Employees");
            if (CurrentUserTransfer.employee.User.Level.LevelName != "Worker")
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
        }
    }
}
