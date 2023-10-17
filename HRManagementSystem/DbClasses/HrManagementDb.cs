using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.DbClasses
{
    internal class HrManagementDb
    {
        private HRManagementSystemDbEntities hrDb;

        public HrManagementDb()
        {
            hrDb = new HRManagementSystemDbEntities();
        }

        public bool UserAnyByLoginPassword(string login, string password)
        {
            try
            {
                return hrDb.Users.Any(u => u.Login == password && u.Password == password)?true:false;
            }
            catch
            {
                throw;
            }
        }
    }
}
