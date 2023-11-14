using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;

namespace HRManagementSystem.Pages
{
    public partial class ProfilePage : Page
    {
        private HrManagementDb hrDb;
        public ProfilePage()
        {
            InitializeComponent();

            hrDb = new HrManagementDb();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                tbJiJob.Text = CurrentUserTransfer.employee.EmployeePostInfo.Job.JobName;
                tbJiDepartment.Text = CurrentUserTransfer.employee.EmployeePostInfo.Department.DepartmentName;
                tbJiHireDate.Text = CurrentUserTransfer.employee.EmployeePostInfo.HireDate.Value.ToShortDateString();
                tbJiSalary.Text = CurrentUserTransfer.employee.EmployeePostInfo.Salary.ToString() + "$";
                tbJiHours.Text = CurrentUserTransfer.employee.EmployeePostInfo.HoursPerWeek.ToString();

                tbPiLastName.Text = CurrentUserTransfer.employee.LastName;
                tbPiFisrtName.Text = CurrentUserTransfer.employee.FirstName;
                tbPiBirthdate.Text = CurrentUserTransfer.employee.BirthDate.Value.ToShortDateString();
                tbPiEmail.Text = CurrentUserTransfer.employee.Email;

                tbUiLogin.Text = CurrentUserTransfer.employee.User.Login;
                tbUiPassword.Text = CurrentUserTransfer.employee.User.Password;
                tbUiLevel.Text = CurrentUserTransfer.employee.User.Level.LevelName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbUiLogin.Text.Length <= 2 || tbUiLogin.Text.Any(s=>s == ' '))
                {
                    throw new Exception("Login can't contain spaces, or be less than 3 symbols");
                }
                else if (tbUiPassword.Text.Length <= 2 || tbUiPassword.Text.Any(s => s == ' '))
                {
                    throw new Exception("Password can't contain spaces, or be less than 3 symbols");
                }
                else if (tbPiEmail.Text.Length <= 3 || tbPiEmail.Text.Any(s => s == ' ') || !tbPiEmail.Text.Any(s => s == '@'))
                {
                    throw new Exception("Wrong email address");
                }
                CurrentUserTransfer.employee.User.Login = tbUiLogin.Text;
                CurrentUserTransfer.employee.User.Password = tbUiPassword.Text;
                CurrentUserTransfer.employee.Email = tbPiEmail.Text;
                hrDb.UpdateEmployee(CurrentUserTransfer.employee);

                MessageBox.Show("Your profile updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
