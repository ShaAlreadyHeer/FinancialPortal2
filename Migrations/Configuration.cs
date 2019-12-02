namespace FinancialPortal.Migrations
{
    using FinancialPortal.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FinancialPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FinancialPortal.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            #region Role Creation
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "HeadOfHousehold"))
            {
                roleManager.Create(new IdentityRole { Name = "HeadOfHousehold" });
            }
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }
            #endregion

            #region User Creation
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "shaheerahmed23@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "shaheerahmed23@gmail.com",
                    Email = "shaheerahmed23@gmail.com",
                    FirstName = "Shaheer",
                    LastName = "Ahmed",
                    DisplayName = "ShaAlreadyHeer",
                    AvatarPath = "~/Avatars/default_user.png",
                }, "Abc&123!");
            }
            if (!context.Users.Any(u => u.Email == "hoh@mailinator.com"))
            { 
                userManager.Create(new ApplicationUser
                {
                    UserName = "hoh@mailinator.com",
                    Email = "hoh@mailinator.com",
                    FirstName = "Head",
                    LastName = "Household",
                    DisplayName = "HeadOfHousehold",
                    AvatarPath = "~/Avatars/default_user.png",
                }, "Abc&123!");
            }
            if (!context.Users.Any(u => u.Email == "member@mailinator.com"))
            { 
                userManager.Create(new ApplicationUser
                {
                    UserName = "member@mailinator.com",
                    Email = "member@mailinator.com",
                    FirstName = "Member",
                    LastName = "Guy",
                    DisplayName = "Might Guy",
                    AvatarPath = "~/Avatars/default_user.png",
                }, "Abc&123!");
            }
            #endregion



            #region Assign Roles 
            var userId = userManager.FindByEmail("shaheerahmed23@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");
            userId = userManager.FindByEmail("hoh@mailinator.com").Id;
            userManager.AddToRole(userId, "HeadOfHousehold");
            userId = userManager.FindByEmail("member@mailinator.com").Id;
            userManager.AddToRole(userId, "Member");
            #endregion
        }
    }
}
