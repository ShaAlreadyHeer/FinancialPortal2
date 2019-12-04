using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class WidgetView
    {
        public Household household { get; set; }
        public virtual ICollection<Household> myHouseholds { get; set; }
        public virtual ICollection<BankAccount> myBankAccounts { get; set; }
        public virtual ICollection<ApplicationUser> myUsers { get; set; }


        public WidgetView()
        {
            household = new Household();
            myHouseholds = new HashSet<Household>();
            myBankAccounts = new HashSet<BankAccount>();
            myUsers = new HashSet<ApplicationUser>();
        }
    }
}