using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoard.Data.EF;

namespace JobBoard.UI.MVC.Controllers
{
    public class UserDetailsController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        // GET: UserDetails
        public ActionResult Index()
        {
            return View(db.UserDetails.ToList());
        }

        // GET: UserDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // GET: UserDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,ResumeFileName")] UserDetail userDetail
            , HttpPostedFileBase resumeFile)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                if (resumeFile != null)
                {
                    fileName = Guid.NewGuid() + ".pdf";

                    string path = Server.MapPath("~/Content/Resumes/");
                    resumeFile.SaveAs(path + fileName);
                    userDetail.ResumeFileName = fileName;
                }
                
                
                   
                }

                db.UserDetails.Add(userDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

         

        // GET: UserDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,ResumeFileName")] UserDetail userDetail,
            HttpPostedFileBase resumeFile)
        {
            if (ModelState.IsValid)
            {
                if (resumeFile != null)
                {
                    string path = Server.MapPath("~/Content/Resumes/");

                    if (userDetail.ResumeFileName != null)
                    {
                        FileInfo file = new FileInfo(path + userDetail.ResumeFileName);
                        file.Delete();
                    }
                    string fileName = Guid.NewGuid() + ".pdf";

                    resumeFile.SaveAs(path + fileName);
                    userDetail.ResumeFileName = fileName;


                }

                db.Entry(userDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userDetail);
        }

        // GET: UserDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserDetail userDetail = db.UserDetails.Find(id);
            db.UserDetails.Remove(userDetail);
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
