using FinancialPortal.Helpers;
using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FinancialPortal.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HelperClass helper = new HelperClass();




        //GET: Admin
        public ActionResult ManageRoles()
        {
            var users1 = db.Users;
            ViewBag.UserIds = new MultiSelectList(users1, "Id", "Email");
            ViewBag.Role = new SelectList(db.Roles, "Name", "Name");
            var users = new List<ManageRoleViewModel>();
            foreach (var user in db.Users.ToList())
            {
                users.Add(new ManageRoleViewModel
                {
                    UserName = $"{user.LastName},{user.FirstName}",
                    RoleName = helper.ListUserRoles(user.Id).FirstOrDefault()
                });
            }
            return View(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string role)
        {
            foreach (var userId in userIds)
            {
                var userRole = helper.ListUserRoles(userId).FirstOrDefault();
                if (userRole != null)
                {
                    helper.RemoveUserFromRole(userId, userRole);
                }
            }

            if (!string.IsNullOrEmpty(role))
            {
                foreach (var userId in userIds)
                {
                    helper.AddUserToRole(userId, role);
                }
            }
            return RedirectToAction("manageroles", "admin");
        }

        public ActionResult ManageHouseholdUsers()
        {
            ViewBag.Households = new MultiSelectList(db.Households, "Id", "HouseholdName");
            ViewBag.Developers = new MultiSelectList(helper.UsersInRole("Developer"), "Id", "Email").ToList();
            ViewBag.Submitters = new MultiSelectList(helper.UsersInRole("Submitter"), "Id", "Email").ToList();
            ViewBag.VBUsers = new SelectList(db.Users, "Id", "Email");

            if (User.IsInRole("Admin"))
            {
                ViewBag.ProjectManagerId = new SelectList(helper.UsersInRole("HeadOfHousehold"), "Id", "Email");
            }

            //Create View Model for User's Projects
            var myData = new List<UserHouseholdListViewModel>();
            UserHouseholdListViewModel userVm = null;
            foreach (var user in db.Users.ToList())
            {
                userVm = new UserHouseholdListViewModel
                {
                    Name = $"{user.LastName},{user.FirstName}",
                    HouseholdNames = helper.ListUserHouseholds(user.Id).Select(p => p.Name).ToList()
                };

                if (userVm.HouseholdNames.Count() == 0)
                    userVm.HouseholdNames.Add("N/A");

                myData.Add(userVm);
            }
            return View(myData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageHouseholdUsers(List<int> households, string HeadOfHouseholdId)
        {
            //Remove users from selected projects
            if (households != null)
            {
                foreach (var householdId in households)
                {
                    foreach (var user in helper.UsersOnHousehold(householdId).ToList())
                    {
                        helper.RemoveUserFromHousehold(user.Id, householdId);
                    }
                    if (!string.IsNullOrEmpty(HeadOfHouseholdId))
                    {
                        helper.AddUserToHousehold(HeadOfHouseholdId, householdId);
                    }
                }
            }
            return RedirectToAction("ManageHouseholdUsers");
        }
    }
}