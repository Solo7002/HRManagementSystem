using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HRManagementSystem.DbClasses
{
    internal class HrManagementDb
    {
        private HRManagementDbEntities hrDb;

        public HrManagementDb()
        {
            hrDb = new HRManagementDbEntities();
        }

        public bool UserAnyByLoginPassword(string login, string password)
        {
            try
            {
                return hrDb.Users.Any(u => u.Login == login && u.Password == password)?true:false;
            }
            catch
            {
                throw;
            }
        }

        public bool UserAnyByLogin(string login)
        {
            try
            {
                return hrDb.Users.Any(u => u.Login == login) ? true : false;
            }
            catch
            {
                throw;
            }
        }

        public Employee GetEmployeeByLogin(string login)
        {
            try
            {
                if (hrDb.Users.Any(u => u.Login == login))
                {
                    return hrDb.Employees.First(e => e.User.Login == login);
                }
                throw new Exception("No employees with such login!");
            }
            catch
            {
                throw;
            }
        }

        public void UpdateEmployee(Employee emp)
        {
            try
            {
                Employee employee = hrDb.Employees.FirstOrDefault(e => e.EmployeeId == emp.EmployeeId);
                User user = hrDb.Users.FirstOrDefault(e => e.UserId == emp.EmployeeId);
                EmployeePostInfo empPostInfo = hrDb.EmployeePostInfoes.FirstOrDefault(e => e.EmployeePostInfoId == emp.EmployeeId);
                if (employee != null && emp != null)
                {
                    hrDb.Entry(employee).CurrentValues.SetValues(emp);
                    hrDb.Entry(user).CurrentValues.SetValues(emp.User);
                    hrDb.Entry(empPostInfo).CurrentValues.SetValues(emp.EmployeePostInfo);
                    hrDb.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            foreach (Department dep in hrDb.Departments) 
            {
                departments.Add(dep);
            }
            return departments.Count>0?departments:null;
        }
        public Department GetDepartmentByName(string depName)
        {
            try
            {
                if (hrDb.Departments.Any(d=>d.DepartmentName==depName))
                {
                    return hrDb.Departments.First(d => d.DepartmentName == depName);
                }
                throw new Exception("Any departments with such name!");
            }
            catch
            {
                throw;
            }
        }

        public void AddReview(Review review)
        {
            try
            {
                Employee EmployeeFrom = hrDb.Employees.FirstOrDefault(e => e.User.Login == review.Employee1.User.Login); 
                Employee EmployeeTo = hrDb.Employees.FirstOrDefault(e => e.User.Login == review.Employee.User.Login); 

                review.Employee1 = EmployeeFrom;
                review.Employee = EmployeeTo;

                hrDb.Reviews.Add(review);
                hrDb.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void AddEmployee(Employee employee)
        {
            try
            {
                if (hrDb.Employees.Any(e=>e.LastName==employee.LastName && e.FirstName == employee.FirstName && e.BirthDate == employee.BirthDate))
                {
                    throw new Exception("Employee with such Name and birthdate already exists!");
                }
                hrDb.Employees.Add(employee);
                hrDb.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void DelEmployee(Employee employeeForDel)
        {
            try
            {
                if (!hrDb.Employees.Any(e=>e.User.Login==employeeForDel.User.Login))
                {
                    throw new Exception("No employees with such login!");
                }
                Employee employee = hrDb.Employees.First(e => e.User.Login == employeeForDel.User.Login);
                

                List<Review> reviewsToDel = new List<Review>();
                foreach (Review review in employee.Reviews) 
                {
                    reviewsToDel.Add(review);
                }
                foreach (Review review in employee.Reviews1)
                {
                    reviewsToDel.Add(review);
                }

                foreach(Review review in reviewsToDel)
                {
                    hrDb.Reviews.Remove(review);
                }
                hrDb.Employees.Remove(employee);
                hrDb.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public List<Job> GetJobs()
        {
            List<Job> jobs = new List<Job>();
            foreach (Job job in hrDb.Jobs)
            {
                jobs.Add(job);
            }
            return jobs.Count > 0 ? jobs : null;
        }

        public Job GetJobByName(string jN)
        {
            try
            {
                if (!hrDb.Jobs.Any(j => j.JobName == jN))
                {
                    throw new Exception("Any Jobs with such name");
                }
                return hrDb.Jobs.First(j => j.JobName == jN);
            }
            catch
            {
                throw;
            }
        }

        public List<Level> GetLevels()
        {
            List<Level> levels = new List<Level>();
            foreach (Level lev in hrDb.Levels)
            {
                levels.Add(lev);
            }
            return levels.Count > 0 ? levels : null;
        }

        public Level GetLevelByName(string lN)
        {
            try
            {
                if (!hrDb.Levels.Any(l => l.LevelName == lN))
                {
                    throw new Exception("Any Levels with such name");
                }
                return hrDb.Levels.First(l => l.LevelName == lN);
            }
            catch
            {
                throw;
            }
        }
    }
}
