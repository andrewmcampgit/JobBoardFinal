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


                var managerLocation = db.Locations.Where(l => l.ManagerId == userID).FirstOrDefault();
                var manLocID = managerLocation.LocationId;
                var openPositions = db.OpenPositions.Where(o => o.LocationId == manLocID).ToList();

                List<int> OPList = new List<int>();

                List<int> manAppIds = new List<int>();

                foreach (var item in openPositions)
                {
                    var Id = openPositions.Select(o => o.OpenPositionId).FirstOrDefault();
                    OPList.Add(Id);
                    var application = db.Applications.Select(o => o.OpenPositionId).FirstOrDefault();
                    if (Id == application)
                    {
                        manAppIds.Add(application);
                    }

                }
                List<Application> apps = new List<Application>();
                foreach (var item in manAppIds)
                {
                    foreach (var a in db.Applications)
                    {
                        
                        if (item == a.ApplicationId)
                        {
                            apps.Add(a);
                        }
                    }

                }

                return View(apps);
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
            ViewBag.OpenPositionId = new SelectList(db.OpenPositions, "OpenPositionId", "OpenPositionId", application.OpenPositionId);
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "Id", application.UserId);
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
