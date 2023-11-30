using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using System;
using System.Windows;

namespace HRManagementSystem.Windows.TablesSetters
{
    public partial class AddDepaertmentWindow : Window
    {
        HrManagementDb hrDb;
        public AddDepaertmentWindow()
        {
            InitializeComponent();

            hrDb = new HrManagementDb();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBlockHeader.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "DPAWHeader");
            btnAddDep.Content = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "DPAWbtnAdd");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbDepartmentName.Text) || string.IsNullOrWhiteSpace(tbDepartmentName.Text) || tbDepartmentName.Text.Length < 5)
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXDAWEmpty"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (hrDb.AnyDepartmentByName(tbDepartmentName.Text))
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXDAWjExists"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                hrDb.AddDepartment(tbDepartmentName.Text);
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
