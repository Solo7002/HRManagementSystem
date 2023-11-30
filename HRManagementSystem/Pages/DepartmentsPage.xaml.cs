using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using HRManagementSystem.Windows.TablesSetters;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HRManagementSystem.Pages
{
    public partial class DepartmentsPage : Page
    {
        private HrManagementDb hrDb;
        private Department CurrentDepartment;
        public DepartmentsPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TranslatePage();

            hrDb = TransferClasses.HrDbTransfer.hrManagementDb;
            FillLbDepartments();

            EnableDisableFields();
            FillData();
        }

        private void TranslatePage()
        {
            string lang = LanguageTransfer.CurrentLanguage;

            textBlockHeader.Text = OpenTranslation.GetTranslation(lang, "DPMainHeader");
            textBlockSeach.Text = OpenTranslation.GetTranslation(lang, "DJPSmallHeaderDep");
            textBlockInfo.Text = OpenTranslation.GetTranslation(lang, "DPSmallHeaderInfo");
            foreach (var item in gdWithTbCb.Children)
            {
                if (item is Grid grid)
                {
                    foreach (var element in grid.Children)
                    {
                        if (element is TextBlock block)
                        {
                            block.Text = OpenTranslation.GetTranslation(lang, block.Tag.ToString());
                        }
                        else if (element is Button button)
                        {
                            button.Content = OpenTranslation.GetTranslation(lang, button.Tag.ToString());
                        }
                    }
                }
            }
        }

        private void tbSeach_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSeach.Text) && string.IsNullOrEmpty(tbSeach.Text))
            {
                FillLbDepartments();
            }
            else
            {
                FillLbDepartments();
                List<ListBoxItem> list = new List<ListBoxItem>();
                foreach (ListBoxItem item in lbDepartments.Items)
                {
                    if (item.Content.ToString().ToLower().StartsWith(tbSeach.Text.ToLower()))
                    {
                        list.Add(item);
                    }
                }
                lbDepartments.Items.Clear();
                foreach (ListBoxItem item in list)
                {
                    lbDepartments.Items.Add(item);
                }
            }
            EnableDisableFields();
            FillData();
        }

        private void lbDepartments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableDisableFields();
            FillData();
        }

        private void EnableDisableFields()
        {
            foreach (var child in gdWithTbCb.Children)
            {
                if (child is Grid gd && gd.Tag != null && gd.Tag.ToString() == "tbCb")
                {
                    foreach (var item in gd.Children)
                    {
                        if (item is TextBox tb)
                        {
                            tb.Text = "";
                            tb.IsEnabled = (lbDepartments.SelectedItem != null && bool.Parse(tb.Tag.ToString()));
                        }
                        else if (item is ComboBox cb)
                        {
                            cb.Items.Clear();
                            cb.IsEnabled = lbDepartments.SelectedItem != null;
                        }
                    }
                }
            }
            btnDelDep.Visibility = btnSaveDep.Visibility = lbDepartments.SelectedItem != null?Visibility.Visible:Visibility.Hidden;
        }

        private void FillData()
        {
            if (lbDepartments.SelectedItem != null)
            {
                try
                {
                    CurrentDepartment = hrDb.GetDepartmentByName((lbDepartments.SelectedItem as ListBoxItem).Content.ToString());
                    tbDepName.Text = CurrentDepartment.DepartmentName;

                    cbDepartmentHead.Items.Add(new ComboBoxItem { Content = "None", Style = (Style)this.FindResource("cbItemOrange"), Tag = false });
                    foreach (EmployeePostInfo emp in CurrentDepartment.EmployeePostInfoes)
                    {
                        cbDepartmentHead.Items.Add(new ComboBoxItem { Content = $"{emp.Employee.LastName} {emp.Employee.FirstName}", Style = (Style)this.FindResource("cbItemOrange"), Tag = true });
                    }
                    cbDepartmentHead.SelectedIndex = 0;
                    for (int i = 0; i < cbDepartmentHead.Items.Count; i++)
                    {
                        if (CurrentDepartment.Employee == null)
                        {
                            cbDepartmentHead.SelectedIndex = 0;
                            break;
                        }
                        else if ($"{CurrentDepartment.Employee.LastName} {CurrentDepartment.Employee.FirstName}" == (cbDepartmentHead.Items[i] as ComboBoxItem).Content.ToString())
                        {
                            cbDepartmentHead.SelectedIndex = i;
                            break;
                        }
                    }

                    tbAmountOfWorkers.Text = CurrentDepartment.EmployeePostInfoes.Count.ToString();

                    if (CurrentDepartment.EmployeePostInfoes.Count > 0)
                    {
                        double salary = 0;
                        int hours = 0;
                        foreach (EmployeePostInfo emp in CurrentDepartment.EmployeePostInfoes)
                        {
                            salary += (double)emp.Salary;
                            hours += (int)emp.HoursPerWeek;
                        }
                        tbAverageSalary.Text = (Math.Round(((double)(salary / CurrentDepartment.EmployeePostInfoes.Count)), 2)).ToString();
                        tbAverageWorkload.Text = (Math.Round(((double)((double)hours / CurrentDepartment.EmployeePostInfoes.Count)), 2)).ToString();
                    }
                    else
                    {
                        tbAverageSalary.Text = "0";
                        tbAverageWorkload.Text = "0";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FillLbDepartments()
        {
            lbDepartments.Items.Clear();
            foreach (Department dep in hrDb.GetDepartments())
            {
                lbDepartments.Items.Add(new ListBoxItem
                {
                    Content = dep.DepartmentName,
                    Padding = new Thickness(10),
                    BorderThickness = new Thickness(0, 0, 0, 0.5),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    Style = (Style)this.FindResource("lbItemOrange")
                });
            }
        }

        private void btnSaveDep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbDepName.Text) || string.IsNullOrWhiteSpace(tbDepName.Text) || tbDepName.Text.Length<5)
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXDAWEmpty"));
                    return;
                }
                else if (hrDb.AnyDepartmentByName(tbDepName.Text))
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXDAWjExists"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CurrentDepartment.DepartmentName = tbDepName.Text;
                if ((bool)(cbDepartmentHead.SelectedItem as ComboBoxItem).Tag==true)
                {
                    CurrentDepartment.Employee = hrDb.GetEmployeeByLastFirstName((cbDepartmentHead.SelectedItem as ComboBoxItem).Content.ToString());
                }
                else
                {
                    CurrentDepartment.Employee = null;
                }

                hrDb.UpdateDepartment(CurrentDepartment, (lbDepartments.SelectedItem as ListBoxItem).Content.ToString());

                MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXDUpdated"));
                FillLbDepartments();
                EnableDisableFields();
                FillData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelDep_Click(object sender, RoutedEventArgs e)
        {
            if (lbDepartments.SelectedItem != null)
            {
                try
                {
                    if (CurrentDepartment.EmployeePostInfoes.Count>0)
                    {
                        MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXDHavePeopleDel"), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    hrDb.DelDepartment(CurrentDepartment);
                    FillLbDepartments();
                    EnableDisableFields();
                    FillData();
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXDDeleted"), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAddDep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new AddDepaertmentWindow().ShowDialog()==true)
                {
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXDAdded"), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    FillLbDepartments();
                    EnableDisableFields();
                    FillData();
                    lbDepartments.SelectedIndex = lbDepartments.Items.Count-1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
