using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using JobBoard.Data.EF;
using System.Collections;

namespace JobBoard.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    public class ApplicationsController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        [Authorize(Roles = "Admin, Manager")]
        // GET: Applications
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var applications = db.Applications.Include(a => a.OpenPosition).Include(a => a.UserDetail).Include(a => a.ApplicationStatu);
                return View(applications.ToList());
            }
            else
            {
                var userID = User.Identity.GetUserId();
                var managerLocation = db.Locations.Where(l => l.ManagerId == userID);

                // create list of openpositions to return (start empty)
                List<OpenPosition> myLocationsOPs = new List<OpenPosition>();
                //create list of my location's Applications to return (start empty)
                List<Application> mylocationsApps = new List<Application>();
                //loop through each mgr location and get all openPositions for that location, then add each of those to openPositions
                foreach (var loc in managerLocation)
                {
                    var thisLocOpenPositions = loc.OpenPositions;
                    foreach (OpenPosition op in thisLocOpenPositions)
                    {
                        myLocationsOPs.Add(op);
                    }
                }
                //loop through each of the mgr's OpenPositions to get all applications for that OP, then
                //add each of those applications.
                foreach (OpenPosition op in myLocationsOPs)
                {
                    var thisOPsApps = op.Applications;
                    foreach (Application app in thisOPsApps)
                    {
                        mylocationsApps.Add(app);
                    }
                }
                var applications = mylocationsApps
                    .OrderByDescending(a => a.ApplicationDate)
                    .ThenBy(a => a.UserId).ToList();

                return View(applications);
                

                
            }


        }
        [Authorize(Roles = "Employee")]
        public ActionResult MyApplications()
        {
            var userId = User.Identity.GetUserId();
            var applications = db.Applications.Where(u => u.UserId == userId).Include(a => a.OpenPosition).Include(a => a.UserDetail).Include(a => a.ApplicationStatu);
            return View(applications.ToList());
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        // GET: Applications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }
        [Authorize(Roles = "Admin")]
        // GET: Applications/Create
        public ActionResult Create()
        {
            ViewBag.OpenPositionId = new SelectList(db.OpenPositions, "OpenPositionId", "OpenPositionId");
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName");
            ViewBag.ApplicationStatusId = new SelectList(db.ApplicationStatus, "ApplicationStatusId", "StatusName");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicationId,UserId,OpenPositionId,ApplicationDate,ManagerNotes,ApplicationStatusId,ResumeFilename")] Application application)
        {
            if (ModelState.IsValid)
            {

                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OpenPositionId = new SelectList(db.OpenPositions, "OpenPositionId", "OpenPositionId", application.OpenPositionId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", application.UserId);
            ViewBag.ApplicationStatusId = new SelectList(db.ApplicationStatus, "ApplicationStatusId", "StatusName", application.ApplicationStatusId);
            return View(application);
        }

        public ActionResult singleClickApply(string userId, int openPositionId, int applicationStatusId, string resumeFileName, Application application)
        {
            userId = User.Identity.GetUserId();

            applicationStatusId = 1;
            resumeFileName = db.UserDetails.Where(r => r.UserId == userId).Select(u => u.ResumeFileName).FirstOrDefault();

            application = new Application()
            {
                UserId = userId,
                OpenPositionId = openPositionId,
                ApplicationDate = DateTime.Now,
                ApplicationStatusId = applicationStatusId,
                ResumeFilename = resumeFileName

            };

            db.Applications.Add(application);
            db.SaveChanges();
            ViewBag.OpenPositionId = new SelectList(db.OpenPositions, "OpenPositionId", "OpenPositionId", application.OpenPositionId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", application.UserId);
            ViewBag.ApplicationStatusId = new SelectList(db.ApplicationStatus, "ApplicationStatusId", "StatusName", application.ApplicationStatusId);
            return RedirectToAction("MyApplications");


        }
        [Authorize(Roles = "Manager")]
        // GET: Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.OpenPositionId = new SelectList(db.OpenPositions, "OpenPositionId", "OpenPositionId", application.OpenPositionId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "Id", application.UserId);
            ViewBag.ApplicationStatusId = new SelectList(db.ApplicationStatus, "ApplicationStatusId", "StatusName", application.ApplicationStatusId);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationId,UserId,OpenPositionId,ApplicationDate,ManagerNotes,ApplicationStatusId,ResumeFilename")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OpenPositionId = application.OpenPositionId;
            ViewBag.UserId = application.UserId;
            ViewBag.ApplicationStatusId = new SelectList(db.ApplicationStatus, "ApplicationStatusId", "StatusName", application.ApplicationStatusId);
            return View(application);
        }
        [Authorize(Roles = "Admin")]
        // GET: Applications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }
        [Authorize(Roles = "Admin")]
        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
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
