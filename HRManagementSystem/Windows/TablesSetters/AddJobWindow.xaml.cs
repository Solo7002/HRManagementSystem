using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using System;
using System.Windows;

namespace HRManagementSystem.Windows.TablesSetters
{
    public partial class AddJobWindow : Window
    {
        HrManagementDb hrDb;
        public AddJobWindow()
        {
            InitializeComponent();

            hrDb = new HrManagementDb();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBlockHeader.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "JPAWHeader");
            btnAddJob.Content = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "JPAWbtnAdd");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbJobName.Text) || string.IsNullOrWhiteSpace(tbJobName.Text) || tbJobName.Text.Length < 5)
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXJAWEmpty"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (hrDb.AnyJobByName(tbJobName.Text))
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXJAWjExists"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                hrDb.AddJob(tbJobName.Text);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
