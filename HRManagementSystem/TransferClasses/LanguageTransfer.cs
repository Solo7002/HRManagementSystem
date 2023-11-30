using System.Windows.Controls;

namespace HRManagementSystem.TransferClasses
{
    public static class LanguageTransfer
    {
        public static string CurrentLanguage { get; set; }
        public static ListBox lbMain { get; set; }
        public static ListBox lbLogOut { get; set; }
        static LanguageTransfer()
        {
            CurrentLanguage = "ua";
        }
    }
}
