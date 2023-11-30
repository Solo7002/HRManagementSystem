using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HRManagementSystem.Windows.TablesSetters
{
    public partial class AddReviewWindow : Window
    {
        public AddReviewWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TranslatePage();

                tbFrom.Text = CurrentUserTransfer.employee.LastName + " " + CurrentUserTransfer.employee.FirstName;
                tbTo.Text = CurrentUserTransfer.employeeForAddSet.LastName + " " + CurrentUserTransfer.employeeForAddSet.FirstName;
                tbDate.Text = DateTime.Now.ToShortDateString();
                cbIsGoodReview.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TranslatePage()
        {
            string lang = LanguageTransfer.CurrentLanguage;
            btnSendReview.Content = OpenTranslation.GetTranslation(lang, "EPRWbtnSend");
            foreach (var item in gdMain.Children)
            {
                if (item is TextBlock block)
                {
                    block.Text = OpenTranslation.GetTranslation(lang, block.Tag.ToString());
                }
            }
            foreach (ComboBoxItem item in cbIsGoodReview.Items) 
            {
                item.Content = OpenTranslation.GetTranslation(lang, item.Tag.ToString());
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbReviewText.Text.Length < 3 || tbReviewText.Text.Length >= 100 || tbReviewText.Text.All(s => s == ' '))
            {
                MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXERWReviewSymbols"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (cbIsGoodReview.SelectedIndex != 0 && cbIsGoodReview.SelectedIndex != 1)
            {
                MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXERWReviewType"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                CurrentUserTransfer.AddedReview = new Review { Employee = CurrentUserTransfer.employeeForAddSet, Employee1 = CurrentUserTransfer.employee, ReviewDate = DateTime.Now, ReviewName = tbReviewText.Text, IsGoodReview = (cbIsGoodReview.SelectedIndex == 0 ? true : false) };
                DialogResult = true;
                Close();
            }
        }
    }
}
