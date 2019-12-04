using FinancialPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FinancialPortal.Helpers
{
    public class HelperClass
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }
        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }

        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName)) resultList.Add(user);
            }
            return resultList;
        }

        public string GetFirstName(string id)
        {
            var user = db.Users.Find(id);
            return user.FirstName;
        }

        public bool IsUserOnHousehold(string userId, int householdId)
        {
            var house = db.Households.Find(householdId);
            var flag = house.Users.Any(u => u.Id == userId);
            return (flag);
        }

        public void AddUserToHousehold(string userId, int householdId)
        {
            if (!IsUserOnHousehold(userId, householdId))
            {
                Household house = db.Households.Find(householdId); var newUser = db.Users.Find(userId);

                house.Users.Add(newUser); db.SaveChanges();
            }
        }

        public ICollection<Household> ListUserHouseholds(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var households = user.Households.ToList();
            return (households);
        }

        public void RemoveUserFromHousehold(string userId, int projectId)
        {
            if (IsUserOnHousehold(userId, projectId))
            {
                Household proj = db.Households.Find(projectId);
                var delUser = db.Users.Find(userId);
                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified;
            }
        }
        public ICollection<ApplicationUser> UsersOnHousehold(int projectId)
        {
            return db.Households.Find(projectId).Users;
        }

        public ICollection<ApplicationUser> UsersNotOnHousehold(int projectId)
        {
            return db.Users.Where(u => u.Households.All(p => p.Id != projectId)).ToList();
        }
    }
}