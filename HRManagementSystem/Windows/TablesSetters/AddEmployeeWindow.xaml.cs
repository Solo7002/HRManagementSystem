using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
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
using HRManagementSystem.TransferClasses;

namespace HRManagementSystem.Windows.TablesSetters
{
    public partial class AddEmployeeWindow : Window
    {
        HrManagementDb hrDb;
        private DateTime BirthDate;
        public AddEmployeeWindow()
        {
            InitializeComponent();
            hrDb = HrDbTransfer.hrManagementDb;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Job job in hrDb.GetJobs())
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem { Content = job.JobName, Style = (Style)this.FindResource("cbItemOrange") };
                    cbJiJob.Items.Add(comboBoxItem);
                }

                foreach (Department dep in hrDb.GetDepartments())
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem { Content = dep.DepartmentName, Style = (Style)this.FindResource("cbItemOrange") };
                    cbJiDepartment.Items.Add(comboBoxItem);
                }

                foreach (Level lev in hrDb.GetLevels())
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem { Content = lev.LevelName, Style = (Style)this.FindResource("cbItemOrange") };
                    cbUiLevel.Items.Add(comboBoxItem);
                }

                tbJiHireDate.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in gdMain.Children) 
            { 
                if (item is Grid gdChild)
                {
                    foreach (var child in gdChild.Children)
                    {
                        if ((child is TextBox tb && string.IsNullOrEmpty(tb.Text)) || (child is ComboBox cb && cb.SelectedItem == null))
                        {
                            MessageBox.Show("Not all fields were filled! Please Fill all fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else if (child is TextBox textBox && textBox.Name!= "tbJiHours" && textBox.Text.Length<3)
                        {
                            MessageBox.Show("Each field must have 3 and more symbols!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
            }
            tbPiBirthdate.Text.Replace('.', '-');
            tbPiBirthdate.Text.Replace('/', '-');
            if (!DateTime.TryParse(tbPiBirthdate.Text, out BirthDate))
            {
                MessageBox.Show("Wrong date input! Try that way (dd-MM-yyyy) or (dd.MM.yyyy)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (hrDb.UserAnyByLogin(tbUiLogin.Text))
            {
                MessageBox.Show("User with such Login already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                CurrentUserTransfer.employeeForAddSet = new Employee
                {
                    LastName = tbPiLastName.Text,
                    FirstName = tbPiFisrtName.Text,
                    BirthDate = BirthDate,
                    Email = tbPiEmail.Text,
                    EmployeePostInfo = new EmployeePostInfo
                    {
                        Job = hrDb.GetJobByName((cbJiJob.SelectedItem as ComboBoxItem).Content.ToString()),
                        Department = hrDb.GetDepartmentByName((cbJiDepartment.SelectedItem as ComboBoxItem).Content.ToString()),
                        HoursPerWeek = int.Parse(tbJiHours.Text),
                        Salary = float.Parse(tbJiSalary.Text),
                        HireDate = DateTime.Now
                    },
                    User = new User
                    {
                        Login = tbUiLogin.Text,
                        Password = tbUiPassword.Text,
                        Level = hrDb.GetLevelByName((cbUiLevel.SelectedItem as ComboBoxItem).Content.ToString())
                    }
                };
                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Wrong input of something, maybe you put letters into numeric field", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
