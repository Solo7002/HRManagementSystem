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
    public partial class AddEmployeeWindow : Window
    {
        HrManagementDb hrDb;
        public AddEmployeeWindow()
        {
            InitializeComponent();
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

                tbJiHireDate.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TranslatePage()
        {
            string lang = LanguageTransfer.CurrentLanguage;
            btnHire.Content = OpenTranslation.GetTranslation(lang, "PPbtnHire");
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
                        else if (child is TextBox textBox && textBox.Name!= "tbJiHours" && textBox.Text.Length<3)
                        {
                            MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEAEWFilledssymbols"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
            }
            
            if (hrDb.UserAnyByLogin(tbUiLogin.Text))
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
                CurrentUserTransfer.employeeForAddSet = new Employee
                {
                    LastName = tbPiLastName.Text,
                    FirstName = tbPiFisrtName.Text,
                    BirthDate = dpPiBirthdate.SelectedDate,
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
                MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEAEWWrongInput"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
