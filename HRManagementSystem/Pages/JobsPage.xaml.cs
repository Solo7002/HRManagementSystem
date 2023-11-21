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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
using HRManagementSystem.Windows.TablesSetters;

namespace HRManagementSystem.Pages
{
    public partial class JobsPage : Page
    {
        private HrManagementDb hrDb;
        private Job CurrentJob;
        public JobsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            hrDb = TransferClasses.HrDbTransfer.hrManagementDb;
            FillLbDepartments();

            EnableDisableFields();
            FillData();
        }

        private void FillData()
        {
            if (lbJobs.SelectedItem != null)
            {
                try
                {
                    CurrentJob = hrDb.GetJobByName((lbJobs.SelectedItem as ListBoxItem).Content.ToString());
                    tbJobName.Text = CurrentJob.JobName;

                    tbAmountOfWorkers.Text = CurrentJob.EmployeePostInfoes.Count.ToString();

                    if (CurrentJob.EmployeePostInfoes.Count > 0)
                    {
                        double salary = 0;
                        int hours = 0;
                        foreach (EmployeePostInfo emp in CurrentJob.EmployeePostInfoes)
                        {
                            salary += (double)emp.Salary;
                            hours += (int)emp.HoursPerWeek;
                        }
                        tbAverageSalary.Text = (Math.Round(((double)(salary / CurrentJob.EmployeePostInfoes.Count)), 2)).ToString();
                        tbAverageWorkload.Text = (Math.Round(((double)((double)hours / CurrentJob.EmployeePostInfoes.Count)), 2)).ToString();
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
                            tb.IsEnabled = (lbJobs.SelectedItem != null && bool.Parse(tb.Tag.ToString()));
                        }
                    }
                }
            }
            btnDelJob.Visibility = btnSaveJob.Visibility = lbJobs.SelectedItem != null ? Visibility.Visible : Visibility.Hidden;
        }

        private void FillLbDepartments()
        {
            lbJobs.Items.Clear();
            foreach (Job job in hrDb.GetJobs())
            {
                lbJobs.Items.Add(new ListBoxItem
                {
                    Content = job.JobName,
                    Padding = new Thickness(10),
                    BorderThickness = new Thickness(0, 0, 0, 0.5),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    Style = (Style)this.FindResource("lbItemOrange")
                });
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
                foreach (ListBoxItem item in lbJobs.Items)
                {
                    if (item.Content.ToString().ToLower().StartsWith(tbSeach.Text.ToLower()))
                    {
                        list.Add(item);
                    }
                }
                lbJobs.Items.Clear();
                foreach (ListBoxItem item in list)
                {
                    lbJobs.Items.Add(item);
                }
            }
            EnableDisableFields();
            FillData();
        }

        private void lbJobs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableDisableFields();
            FillData();
        }

        private void btnSaveJob_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbJobName.Text) || string.IsNullOrWhiteSpace(tbJobName.Text) || tbJobName.Text.Length < 5)
                {
                    MessageBox.Show("Field job name is empty or have less than 5 symbols!");
                    return;
                }
                else if (hrDb.AnyJobByName(tbJobName.Text))
                {
                    MessageBox.Show("Job with such name already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CurrentJob.JobName = tbJobName.Text;

                hrDb.UpdateJob(CurrentJob, (lbJobs.SelectedItem as ListBoxItem).Content.ToString());

                MessageBox.Show("Job updated");
                FillLbDepartments();
                EnableDisableFields();
                FillData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelJob_Click(object sender, RoutedEventArgs e)
        {
            if (lbJobs.SelectedItem != null)
            {
                try
                {
                    if (CurrentJob.EmployeePostInfoes.Count > 0)
                    {
                        MessageBox.Show("You can't delete department with people in it.\nRemove everybody from job to delete it", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    hrDb.DelJob(CurrentJob);

                    FillLbDepartments();
                    EnableDisableFields();
                    FillData();
                    MessageBox.Show("Job deleted", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAddJob_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new AddJobWindow().ShowDialog() == true)
                {
                    MessageBox.Show("New job added", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    FillLbDepartments();
                    EnableDisableFields();
                    FillData();
                    lbJobs.SelectedIndex = lbJobs.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
