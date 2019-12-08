using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Extentions;
using FinancialPortal.Helpers;
using FinancialPortal.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HelperClass roleHelper = new HelperClass();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Include(h => h.OwnerUser).FirstOrDefault(h => h.Id == id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Greeting,Created")] Household household)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                household.OwnerUserId = User.Identity.GetUserId();
                household.Created = DateTime.Now;
                user.Household = household;
                db.Households.Add(household);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Greeting,Created")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> LeaveAsync()
        {
            var userId = User.Identity.GetUserId();

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var user = db.Users.Find(userId);

            switch(myRole)
            {
                case "HeadOfHousehold":

                    var inhabitants = db.Users.Where(u => u.HouseholdId == user.HouseholdId).Count();
                    if (inhabitants > 1)
                    {
                        TempData["Message"] = $"You are unable to leave the Household at this time as there are still" +
                            $" <b> {inhabitants} ";
                        return RedirectToAction("ExitDenied");
                    }
                    user.HouseholdId = null;
                    db.SaveChanges();

                    roleHelper.RemoveUserFromRole(userId, "HeadOfHousehold");
                    await ControllerContext.HttpContext.RefreshAuthentication(user);

                    return RedirectToAction("Index", "Home");

                case "Member":
                default:
                    user.HouseholdId = null;
                    db.SaveChanges();

                    roleHelper.RemoveUserFromRole(userId, "Member");
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                    return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ExitDenied()
        {
            return View();
        }

        public ActionResult AppointSuccessor()
        {
            var userId = User.Identity.GetUserId();
            var myHouseholdId = db.Users.Find(userId).HouseholdId ?? 0;

            if (myHouseholdId == 0)
                return RedirectToAction("Index", "Home");

            var members = db.Users.Where(u => u.HouseholdId == myHouseholdId && u.Id == userId);
            ViewBag.NewHoh = new SelectList(members, "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AppointSuccessorAsync(string newHoh)
        {
            if (string.IsNullOrEmpty(newHoh))
                return RedirectToAction("Index", "Home");

            var me = db.Users.Find(User.Identity.GetUserId());
            me.Household = null;
            db.SaveChanges();

            roleHelper.RemoveUserFromRole(me.Id, "HeadOfHousehold");
            await ControllerContext.HttpContext.RefreshAuthentication(me);

            roleHelper.RemoveUserFromRole(newHoh, "Member");
            roleHelper.AddUserToRole(newHoh, "HeadOfHousehold");

            return RedirectToAction("Index", "Home");

        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
