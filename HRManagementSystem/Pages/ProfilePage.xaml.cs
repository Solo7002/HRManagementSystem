using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HRManagementSystem.Pages
{
    public partial class ProfilePage : Page
    {
        private HrManagementDb hrDb;
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            hrDb = TransferClasses.HrDbTransfer.hrManagementDb;
            try
            {
                TranslatePage();

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

        private void TranslatePage()
        {
            string lang = LanguageTransfer.CurrentLanguage;
            tbHeader.Text = OpenTranslation.GetTranslation(lang, "PPMainHeader");
            btnSave.Content = OpenTranslation.GetTranslation(lang, "PPbtnSave");
            foreach (var item in gridWithGrids.Children)
            {
                if (item is Grid grid)
                {
                    foreach(var element in grid.Children)
                    {
                        if (element is TextBlock block)
                        {
                            block.Text = OpenTranslation.GetTranslation(lang, block.Tag.ToString());
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string lang = LanguageTransfer.CurrentLanguage;
                if (tbUiLogin.Text.Length <= 2 || tbUiLogin.Text.Any(s=>s == ' '))
                {
                    throw new Exception(OpenTranslation.GetTranslation(lang, "EXPLogin"));
                }
                else if (tbUiPassword.Text.Length <= 2 || tbUiPassword.Text.Any(s => s == ' '))
                {
                    throw new Exception(OpenTranslation.GetTranslation(lang, "EXPPassword"));
                }
                else if (tbPiEmail.Text.Length <= 3 || !IsValidEmail(tbPiEmail.Text))
                {
                    throw new Exception(OpenTranslation.GetTranslation(lang, "EXPEmail"));
                }
                CurrentUserTransfer.employee.User.Login = tbUiLogin.Text;
                CurrentUserTransfer.employee.User.Password = tbUiPassword.Text;
                CurrentUserTransfer.employee.Email = tbPiEmail.Text;
                hrDb.UpdateEmployee(CurrentUserTransfer.employee);

                MessageBox.Show(OpenTranslation.GetTranslation(lang, "EXPSuccesfully"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
