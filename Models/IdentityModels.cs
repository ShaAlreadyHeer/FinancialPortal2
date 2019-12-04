using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancialPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Display(Name = "Display")]
        public string DisplayName { get; set; }
        public string AvatarPath { get; set; }
        public int? HouseholdId { get; set; }
        public virtual Household Household {get;set;}
        public virtual BankAccount BankAccount { get; set; }

        //Children
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Household> Households { get; set; }

        public ApplicationUser()
        {
            Households = new HashSet<Household>();
            Transactions = new HashSet<Transaction>();
            Budgets = new HashSet<Budget>();
            Notifications = new HashSet<Notification>();
            BankAccounts = new HashSet<BankAccount>();
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<FinancialPortal.Models.Budget> Budgets { get; set; }

        public System.Data.Entity.DbSet<FinancialPortal.Models.BankAccount> BankAccounts { get; set; }

        public System.Data.Entity.DbSet<FinancialPortal.Models.BudgetItem> BudgetItems { get; set; }

        public System.Data.Entity.DbSet<FinancialPortal.Models.Household> Households { get; set; }

        public System.Data.Entity.DbSet<FinancialPortal.Models.Invitation> Invitations { get; set; }

        public System.Data.Entity.DbSet<FinancialPortal.Models.Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<FinancialPortal.Models.Transaction> Transactions { get; set; }
    }
}