using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public string OwnerId { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public float TargetAmount { get; set; }
        public float CurrentAmount { get; set; }

        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public Budget()
        {
            BankAccounts = new HashSet<BankAccount>();
        }

    }
}