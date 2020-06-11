using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoard.Data.EF;
using Microsoft.AspNet.Identity;

namespace JobBoard.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class LocationsController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        [Authorize(Roles = "Admin, Manager")]
        // GET: Locations
        public ActionResult Index()
        {
            var locations = db.Locations.Include(l => l.UserDetail);
            return View(locations.ToList());
        }

        [Authorize(Roles = "Admin")]
        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        [Authorize(Roles = "Admin")]
        // GET: Locations/Create
        public ActionResult Create()
        {

            ////retrieve all users
            //var allUsers = db.AspNetUsers;

            ////role selection
            //var managerRole = db.AspNetRoles.Where(r => r.Name == "Admin");

            ////create a list to hold all users in that role
            //var managers = new List<UserDetail>();

            ////loop through all users in that role
            //foreach (var a in allUsers)
            //{
            //    foreach (var r in managerRole)
            //    {
            //        //if the user is found as a value in the roles nav, add to list\
            //        if (r.AspNetUsers.Contains(a))
            //        {
            //            managers.Add(a);
            //        }
            //    }
            //}

            var manager = db.Locations;
            var userDetails = db.UserDetails;
            var details = new List<UserDetail>();
            foreach (var item in manager)
            {
                foreach (var ud in userDetails)
                {
                    if (item.ManagerId == ud.UserId)
                    {
                        details.Add(ud);
                    }
                }

            }
            ViewBag.ManagerId = new SelectList(details, "UserId", "FullName");


            //ViewBag.ManagerId = new SelectList(managers, "Id", "FullName");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationId,StoreNumber,City,State,ManagerId")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ManagerId = User.Identity.GetUserId();
            ViewBag.ManagerId = new SelectList(db.UserDetails, "UserId", "FullName", location.ManagerId);
            return View(location);
        }

        [Authorize(Roles = "Admin, Manager")]
        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            var currentUser = User.Identity.GetUserId();
            if (User.IsInRole("Manager"))
            {


                var manager = db.UserDetails.Where(ud => ud.UserId == currentUser);
                ViewBag.ManagerId = new SelectList(manager, "UserId", "FullName");
                return View(location);
            }
            else
            {
                var manager = db.Locations;
                var userDetails = db.UserDetails;
                var details = new List<UserDetail>();
                foreach (var item in manager)
                {
                    foreach (var ud in userDetails)
                    {
                        if (item.ManagerId == ud.UserId)
                        {
                            details.Add(ud);
                        }
                    }

                }
                ViewBag.ManagerId = new SelectList(details, "UserId", "FullName");
                return View(location);
            }

        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit([Bind(Include = "LocationId,StoreNumber,City,State,ManagerId")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(db.UserDetails, "UserId", "FirstName", location.ManagerId);
            return View(location);
        }

        // GET: Locations/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index");
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
