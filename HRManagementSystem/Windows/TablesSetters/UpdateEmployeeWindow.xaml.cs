using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HRManagementSystem.Windows.TablesSetters
{
    public partial class UpdateEmployeeWindow : Window
    {
        HrManagementDb hrDb;
        private Employee RecievedEmployee;
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
                TranslatePage();

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
                dpPiBirthdate.SelectedDate = RecievedEmployee.BirthDate.Value;
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
        private void TranslatePage()
        {
            string lang = LanguageTransfer.CurrentLanguage;
            btnUpdate.Content = OpenTranslation.GetTranslation(lang, "PPbtnUpdate");
            foreach (var item in gdMain.Children)
            {
                if (item is Grid grid)
                {
                    foreach (var element in grid.Children)
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
            foreach (var item in gdMain.Children)
            {
                if (item is Grid gdChild)
                {
                    foreach (var child in gdChild.Children)
                    {
                        if ((child is TextBox tb && string.IsNullOrEmpty(tb.Text)) || (child is ComboBox cb && cb.SelectedItem == null))
                        {
                            MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEAEWFilledsFilled"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else if (child is TextBox textBox && textBox.Name != "tbJiHours" && textBox.Text.Length < 3)
                        {
                            MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEAEWFilledssymbols"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
            }

            if (tbUiLogin.Text != CurrentUserTransfer.employeeForAddSet.User.Login && hrDb.UserAnyByLogin(tbUiLogin.Text))
            {
                MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEAEWUserExists"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (tbPiEmail.Text.Length <= 3 || !IsValidEmail(tbPiEmail.Text))
            {
                MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXPEmail"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                RecievedEmployee.LastName = tbPiLastName.Text;
                RecievedEmployee.FirstName = tbPiFisrtName.Text;
                RecievedEmployee.BirthDate = dpPiBirthdate.SelectedDate;
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

        private void dpPiBirthdate_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                var textBox = GetChildOfType<TextBox>(datePicker);

                if (textBox != null)
                {
                    textBox.Background = new SolidColorBrush(Color.FromRgb(68, 68, 68));
                    textBox.Foreground = Brushes.White;
                }
            }
        }
        private T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                if (child != null && child is T)
                {
                    return (T)child;
                }

                var childItem = GetChildOfType<T>(child);
                if (childItem != null) return childItem;
            }

            return null;
        }
    }
}
