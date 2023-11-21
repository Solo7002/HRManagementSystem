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
using HRManagementSystem.DbClasses;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbDepartmentName.Text) || string.IsNullOrWhiteSpace(tbDepartmentName.Text) || tbDepartmentName.Text.Length < 5)
                {
                    MessageBox.Show("Field Department name is empty or have less than 5 symbols!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (hrDb.AnyDepartmentByName(tbDepartmentName.Text))
                {
                    MessageBox.Show("Department with such name already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
