using System;
using System.Collections.Generic;
using System.Linq;

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

        public Employee GetEmployeeByLastFirstName(string LastFirstName)
        {
            try
            {
                if (!hrDb.Employees.Any(e=>(LastFirstName.ToLower().Contains(e.LastName.ToLower())) && (LastFirstName.ToLower().Contains(e.FirstName.ToLower()))))
                {
                    throw new Exception("No employees with such Name!");
                }
                return hrDb.Employees.First(e => (LastFirstName.ToLower().Contains(e.LastName.ToLower())) && (LastFirstName.ToLower().Contains(e.FirstName.ToLower())));
            }
            catch
            {
                throw;
            }
        }

        public Employee GetEmployeeByDepartment(string depName)
        {
            try
            {
                if (!hrDb.Employees.Any(e=>e.EmployeePostInfo.Department.DepartmentName==depName))
                {
                    throw new Exception("No employees with such department!");
                }
                return hrDb.Employees.First(e => e.EmployeePostInfo.Department.DepartmentName == depName);
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
        public void AddDepartment(string depName)
        {
            try
            {
                if (hrDb.Departments.Any(d => d.DepartmentName.ToLower() == depName.ToLower()))
                {
                    throw new Exception("Such Department already exists!");
                }
                hrDb.Departments.Add(new Department { DepartmentName = depName, DepartmentHead = null });
                hrDb.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public void UpdateDepartment(Department department, string OldName)
        {
            try
            {
                Department DepartmentToSet;
                if (!hrDb.Departments.Any(d=>d.DepartmentName==OldName))
                {
                    throw new Exception("Any Departments with such name!");
                }
                DepartmentToSet = hrDb.Departments.First(d => d.DepartmentName == OldName);
                department.DepartmentId = DepartmentToSet.DepartmentId;
                hrDb.Entry(DepartmentToSet).CurrentValues.SetValues(department);
                hrDb.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void DelDepartment(Department departmentForDel)
        {
            try
            {
                if (!hrDb.Departments.Any(d=>d.DepartmentName==departmentForDel.DepartmentName))
                {
                    throw new Exception("No departments with such name");
                }
                Department department = hrDb.Departments.First(d => d.DepartmentName == departmentForDel.DepartmentName);

                hrDb.Departments.Remove(department);
                hrDb.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public bool AnyDepartmentByName(string depName)
        {
            try
            {
                return hrDb.Departments.Any(d => d.DepartmentName.ToLower() == depName.ToLower());
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

        public void AddJob(string jobName)
        {
            try
            {
                if (hrDb.Jobs.Any(j => j.JobName.ToLower() == jobName.ToLower()))
                {
                    throw new Exception("Such job already exists!");
                }
                hrDb.Jobs.Add(new Job { JobName = jobName});
                hrDb.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateJob(Job job, string OldName)
        {
            try
            {
                Job JobToSet;
                if (!hrDb.Jobs.Any(j => j.JobName == OldName))
                {
                    throw new Exception("Any Jobs with such name!");
                }
                JobToSet = hrDb.Jobs.First(j => j.JobName == OldName);
                job.JobId = JobToSet.JobId;
                hrDb.Entry(JobToSet).CurrentValues.SetValues(job);
                hrDb.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void DelJob(Job jobForDel)
        {
            try
            {
                if (!hrDb.Jobs.Any(j => j.JobName == jobForDel.JobName))
                {
                    throw new Exception("No jobs with such name!");
                }
                Job job = hrDb.Jobs.First(j => j.JobName == jobForDel.JobName);

                hrDb.Jobs.Remove(job);
                hrDb.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public bool AnyJobByName(string jobName)
        {
            try
            {
                return hrDb.Jobs.Any(j => j.JobName.ToLower() == jobName.ToLower());
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
