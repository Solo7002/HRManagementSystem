using HRManagementSystem.ControlClasses;
using HRManagementSystem.DbClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
using HRManagementSystem.Windows.TablesSetters;
using HRManagementSystem.TransferClasses;

namespace HRManagementSystem.Pages
{
    public partial class EmployeesPage : Page
    {
        private HrManagementDb hrDb;
        private string depName;
        public EmployeesPage()
        {
            InitializeComponent();

            hrDb = new HrManagementDb();
            HrDbTransfer.SetHrManagementDb(hrDb);
            depName = "All Departments";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbDepartments.Items.Clear();
            cbDepartments.Items.Add(new ComboBoxItem { Content = "All Departments", Style = (Style)this.FindResource("cbItemOrange"), Tag=true});
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
                foreach (GridViewColumn col in dgv.Columns)
                {
                    double StandartWidth = (double)((MainProgramPageControl.currentWindow.Width / 5 * 4) - 115) / dgv.Columns.Count;
                    col.Width = (double)StandartWidth*col.Width;
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
                    MessageBox.Show("User added successfully");
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
                        MessageBox.Show("User updated successfully");
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
                if (listViewEmployees.SelectedItem != null && MessageBox.Show("Are you sure you want dissmiss that employee?\n\nWarning: All reviews with that employee also will be deleted!", "Dissmiss employee", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) 
                {
                    hrDb.DelEmployee(listViewEmployees.SelectedItem as Employee);
                    FillEmployeesListView();
                    MessageBox.Show("Employee dissmissed successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
