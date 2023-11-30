using HRManagementSystem.ControlClasses;
using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using HRManagementSystem.Windows.TablesSetters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HRManagementSystem.Pages
{
    public partial class EmployeesPage : Page
    {
        private HrManagementDb hrDb;
        private string depName;
        private List<double> widthes;
        public EmployeesPage()
        {
            InitializeComponent();
            depName = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EPCbAllDep");

            widthes = new List<double>();
            widthes.Add(1);
            widthes.Add(1);
            widthes.Add(1.5);
            widthes.Add(0.9);
            widthes.Add(0.8);
            widthes.Add(0.8);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TranslatePage();
            hrDb = TransferClasses.HrDbTransfer.hrManagementDb;

            cbDepartments.Items.Clear();
            cbDepartments.Items.Add(new ComboBoxItem { Content = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EPCbAllDep"), Style = (Style)this.FindResource("cbItemOrange"), Tag=true});
            foreach (Department dep in hrDb.GetDepartments())
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem { Content = dep.DepartmentName, Style = (Style)this.FindResource("cbItemOrange"), Tag=false };
                cbDepartments.Items.Add(comboBoxItem);
            }

            for (int i = 0; i < cbDepartments.Items.Count;i++ )
            {
                if ((cbDepartments.Items[i] as ComboBoxItem).Content.ToString()==depName)
                {
                    cbDepartments.SelectedIndex = i;
                    break;
                }
            }

            FillEmployeesListView();
            VisibilityButtons();
            cbDepartments.SelectedIndex = 0;
        }

        private void TranslatePage()
        {
            textBlockMainHeader.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EPMainHeader");
            foreach (var item in gridWithTbs.Children)
            {
                if (item is TextBlock tb)
                {
                    tb.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, tb.Tag.ToString());
                }
            }
            List<string> list = new List<string>();
            list.Add("EPLLn");
            list.Add("EPLFn");
            list.Add("EPLJ");
            list.Add("EPLS");
            list.Add("EPLPros");
            list.Add("EPLCons");
            for (int i = 0; i < dgv.Columns.Count;i++)
            {
                dgv.Columns[i].Header = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, list[i]);
            }

            foreach (var item in gridWithButtons.Children)
            {
                if (item is Button btn)
                {
                    btn.Content = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, btn.Tag.ToString());
                }
            }
        }

        private void cbDepartments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillEmployeesListView();
        }
        private void FillEmployeesListView()
        {
            listViewEmployees.Items.Clear();
            try
            {
                if ((cbDepartments.SelectedItem as ComboBoxItem) == null)
                {
                    return;
                }

                if ((bool)(cbDepartments.SelectedItem as ComboBoxItem).Tag == true)
                {
                    foreach (Department dep in hrDb.GetDepartments())
                    {
                        foreach (EmployeePostInfo emp in dep.EmployeePostInfoes)
                        {
                            Employee employee = emp.Employee;
                            listViewEmployees.Items.Add(employee);
                        }
                    }
                }
                else
                {
                    foreach (EmployeePostInfo emp in hrDb.GetDepartmentByName((cbDepartments.SelectedItem as ComboBoxItem).Content.ToString()).EmployeePostInfoes)
                    {
                        Employee employee = emp.Employee;
                        listViewEmployees.Items.Add(employee);
                    }
                }

                SeachEmployeeByName();

                depName = (cbDepartments.SelectedItem as ComboBoxItem).Content.ToString();

                VisibilityButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private void SeachEmployeeByName()
        {
            try
            {
                if (tbSeach.Text.Length > 0 && !tbSeach.Text.All(s => s == ' '))
                {
                    List<Employee> tempEmpList = new List<Employee>();
                    foreach (Employee emp in listViewEmployees.Items)
                    {
                        if ((emp.LastName + " " + emp.FirstName).ToLower().StartsWith(tbSeach.Text.ToLower()) || (emp.LastName).ToLower().StartsWith(tbSeach.Text.ToLower()) || (emp.FirstName).ToLower().StartsWith(tbSeach.Text.ToLower()))
                        {
                            tempEmpList.Add(emp);
                        }
                    }
                    listViewEmployees.Items.Clear();
                    foreach (Employee emp in tempEmpList)
                    {
                        listViewEmployees.Items.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tbSeach_TextChanged(object sender, TextChangedEventArgs e)
        {
            FillEmployeesListView();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                for(int i = 0; i < dgv.Columns.Count;i++)
                {
                    double StandartWidth = (double)((MainProgramPageControl.currentWindow.Width / 5 * 4) - 115) / dgv.Columns.Count;
                    dgv.Columns[i].Width = (double)StandartWidth*widthes[i];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddReview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listViewEmployees.SelectedItem != null) 
                {
                    if ((listViewEmployees.SelectedItem as Employee).LastName == CurrentUserTransfer.employee.LastName && (listViewEmployees.SelectedItem as Employee).FirstName == CurrentUserTransfer.employee.FirstName)
                    {
                        MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEReviewYourself"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    CurrentUserTransfer.employeeForAddSet = hrDb.GetEmployeeByLogin((listViewEmployees.SelectedItem as Employee).User.Login);
                    if (new AddReviewWindow().ShowDialog() == true)
                    {
                        hrDb.AddReview(CurrentUserTransfer.AddedReview);
                        FillEmployeesListView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new AddEmployeeWindow().ShowDialog() == true) 
                {
                    hrDb.AddEmployee(CurrentUserTransfer.employeeForAddSet);
                    FillEmployeesListView();
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEUserAdded"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((listViewEmployees.SelectedItem as Employee) != null)
                {
                    CurrentUserTransfer.employeeForAddSet = listViewEmployees.SelectedItem as Employee;
                    if (new UpdateEmployeeWindow().ShowDialog() == true)
                    {
                        hrDb.UpdateEmployee(CurrentUserTransfer.employeeForAddSet);
                        FillEmployeesListView();
                        MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEUserUpdated"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VisibilityButtons()
        {
            btnAddEmployee.Visibility = (CurrentUserTransfer.employee.User.Level.LevelName != "Worker") ? Visibility.Visible : Visibility.Hidden;
            btnUpdateEmployee.Visibility = (CurrentUserTransfer.employee.User.Level.LevelName != "Worker" && listViewEmployees.SelectedItem != null) ? Visibility.Visible : Visibility.Hidden;
            btnDelEmployee.Visibility = (CurrentUserTransfer.employee.User.Level.LevelName != "Worker" && listViewEmployees.SelectedItem != null) ? Visibility.Visible : Visibility.Hidden;
            btnAddReview.Visibility = (listViewEmployees.SelectedItem != null) ? Visibility.Visible : Visibility.Hidden;

            if (btnAddReview.Visibility == btnAddEmployee.Visibility && btnAddReview.Visibility == btnUpdateEmployee.Visibility && btnAddReview.Visibility == btnDelEmployee.Visibility && btnAddReview.Visibility == Visibility.Hidden) 
            {
                gdButtons.Visibility = Visibility.Hidden;
            }
            else
            {
                gdButtons.Visibility = Visibility.Visible;
            }
        }

        private void listViewEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisibilityButtons();
        }

        private void btnDelEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listViewEmployees.SelectedItem != null && MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXETryDel"), "Dissmiss employee", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) 
                {
                    if ((listViewEmployees.SelectedItem as Employee).LastName == CurrentUserTransfer.employee.LastName && (listViewEmployees.SelectedItem as Employee).FirstName == CurrentUserTransfer.employee.FirstName)
                    {
                        MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEDelYourself"), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    hrDb.DelEmployee(listViewEmployees.SelectedItem as Employee);
                    FillEmployeesListView();
                    MessageBox.Show(OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "EXEUserDeleted"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
