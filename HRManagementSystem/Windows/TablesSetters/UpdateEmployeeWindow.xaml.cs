using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
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
    public partial class UpdateEmployeeWindow : Window
    {
        HrManagementDb hrDb;
        private Employee RecievedEmployee;
        DateTime BirthDate;
        public UpdateEmployeeWindow()
        {
            InitializeComponent();

            RecievedEmployee = CurrentUserTransfer.employeeForAddSet;
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



                foreach (ComboBoxItem comboBoxItem in cbJiJob.Items) 
                {
                    if (comboBoxItem.Content == RecievedEmployee.EmployeePostInfo.Job.JobName)
                    {
                        cbJiJob.SelectedItem = comboBoxItem;
                        break;
                    }
                }
                foreach (ComboBoxItem comboBoxItem in cbJiDepartment.Items)
                {
                    if (comboBoxItem.Content == RecievedEmployee.EmployeePostInfo.Department.DepartmentName)
                    {
                        cbJiDepartment.SelectedItem = comboBoxItem;
                        break;
                    }
                }
                tbJiHireDate.Text = RecievedEmployee.EmployeePostInfo.HireDate.Value.ToShortDateString();
                tbJiHours.Text = RecievedEmployee.EmployeePostInfo.HoursPerWeek.ToString();
                tbJiSalary.Text = RecievedEmployee.EmployeePostInfo.Salary.ToString();

                tbPiLastName.Text = RecievedEmployee.LastName;
                tbPiFisrtName.Text = RecievedEmployee.FirstName;
                tbPiBirthdate.Text = RecievedEmployee.BirthDate.Value.ToShortDateString();
                tbPiEmail.Text = RecievedEmployee.Email;

                tbUiLogin.Text = RecievedEmployee.User.Login;
                tbUiPassword.Text = RecievedEmployee.User.Password;
                foreach (ComboBoxItem comboBoxItem in cbUiLevel.Items)
                {
                    if (comboBoxItem.Content == RecievedEmployee.User.Level.LevelName)
                    {
                        cbUiLevel.SelectedItem = comboBoxItem;
                        break;
                    }
                }
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
                        else if (child is TextBox textBox && textBox.Name != "tbJiHours" && textBox.Text.Length < 3)
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
                RecievedEmployee.LastName = tbPiLastName.Text;
                RecievedEmployee.FirstName = tbPiFisrtName.Text;
                RecievedEmployee.BirthDate = BirthDate;
                RecievedEmployee.Email = tbPiEmail.Text;

                RecievedEmployee.EmployeePostInfo.Job = hrDb.GetJobByName((cbJiJob.SelectedItem as ComboBoxItem).Content.ToString());
                RecievedEmployee.EmployeePostInfo.Department = hrDb.GetDepartmentByName((cbJiDepartment.SelectedItem as ComboBoxItem).Content.ToString());
                RecievedEmployee.EmployeePostInfo.HoursPerWeek = int.Parse(tbJiHours.Text);
                RecievedEmployee.EmployeePostInfo.Salary = float.Parse(tbJiSalary.Text);

                RecievedEmployee.User.Login = tbUiLogin.Text;
                RecievedEmployee.User.Password = tbUiPassword.Text;
                RecievedEmployee.User.Level = hrDb.GetLevelByName((cbUiLevel.SelectedItem as ComboBoxItem).Content.ToString());

                CurrentUserTransfer.employeeForAddSet = RecievedEmployee;
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
