using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.TransferClasses
{
    public static class CurrentUserTransfer
    {
        public static Employee employee { get; set; }
        public static Employee employeeForAddSet { get; set; }
        public static Review AddedReview { get; set; }
    }
}
