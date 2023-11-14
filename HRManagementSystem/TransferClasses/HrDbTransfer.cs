using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRManagementSystem.DbClasses;

namespace HRManagementSystem.TransferClasses
{
    internal static class HrDbTransfer
    {
        private static HrManagementDb currentHrDb;

        public static HrManagementDb hrManagementDb { get { return currentHrDb; } }
        public static void SetHrManagementDb(HrManagementDb newHrDb)
        {
            currentHrDb = newHrDb;
        }
    }
}
