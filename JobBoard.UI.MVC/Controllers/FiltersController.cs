using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoard.Data.EF;
using PagedList;

namespace JobBoard.UI.MVC.Controllers
{
    public class FiltersController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        // GET: Filters
        public ActionResult OpenPositionFilter()
        {
            
            var openPositions = db.OpenPositions.Include(o => o.Location).Include(o => o.Position);
            return View(openPositions.ToList());
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
