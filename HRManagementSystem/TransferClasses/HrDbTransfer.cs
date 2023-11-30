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
