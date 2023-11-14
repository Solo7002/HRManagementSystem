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
using HRManagementSystem.TransferClasses;

namespace HRManagementSystem.Windows.TablesSetters
{
    /// <summary>
    /// Interaction logic for AddReviewWindow.xaml
    /// </summary>
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbReviewText.Text.Length < 3 || tbReviewText.Text.Length >= 100 || tbReviewText.Text.All(s => s == ' '))
            {
                MessageBox.Show("Review text must have more than 3 symbols and less than 100", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (cbIsGoodReview.SelectedIndex != 0 && cbIsGoodReview.SelectedIndex != 1)
            {
                MessageBox.Show("Field review type is empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
