using HRManagementSystem.ControlClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRManagementSystem.Pages
{
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Employee employee = new Employee { LastName = "Solod", FirstName = "Ihor", Phone = "(+380) 50-524-09-34" };
            DataGridRow dataGridRow = new DataGridRow();
            listViewEmployees.Items.Add(employee);
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                foreach (GridViewColumn col in dgv.Columns)
                {
                    col.Width = (double)((MainProgramPageControl.currentWindow.Width / 5 * 4) - 100) / dgv.Columns.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error\n" + ex.Message);
            }
        }

        private void dgvEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listViewEmployees_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
