using HRManagementSystem.DbClasses;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbJobName.Text) || string.IsNullOrWhiteSpace(tbJobName.Text) || tbJobName.Text.Length < 5)
                {
                    MessageBox.Show("Field Job name is empty or have less than 5 symbols!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (hrDb.AnyJobByName(tbJobName.Text))
                {
                    MessageBox.Show("Job with such name already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
