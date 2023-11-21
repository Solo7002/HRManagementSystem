using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HRManagementSystem.Pages;

namespace HRManagementSystem.ControlClasses
{
    internal static class MainProgramPageControl
    {
        public static Window currentWindow { get; set; } 

        private static List<Page> pages = new List<Page> { new ProfilePage(), new EmployeesPage(), new DepartmentsPage(), new JobsPage(), new ReviewsPage(), new SettingsPage() };

        public static Page GetPageByName(string name)
        {
            foreach (var item in pages) 
            {
                if (item.GetType().FullName.Contains(name)) return item;
            }
            return null;
        }
    }
}
