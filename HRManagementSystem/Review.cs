//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRManagementSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public int ReviewId { get; set; }
        public string ReviewName { get; set; }
        public Nullable<bool> IsGoodReview { get; set; }
        public Nullable<System.DateTime> ReviewDate { get; set; }
        public Nullable<int> EmployeeFor_id { get; set; }
        public Nullable<int> EmployeeFrom_id { get; set; }
    
        public virtual EmployeePostInfo EmployeePostInfo { get; set; }
        public virtual Employee Employee { get; set; }
    }
}