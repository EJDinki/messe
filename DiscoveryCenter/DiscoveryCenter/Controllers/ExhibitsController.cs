using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DiscoveryCenter.Models;
using System.IO;

namespace DiscoveryCenter.Controllers
{
    public class ExhibitsController : Controller
    {
        private SurveyContext db = new SurveyContext();

        // GET: Exhibits
        public ActionResult Index()
        {
            return View(db.Exhibits.ToList());
        }

        // GET: Exhibits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exhibit exhibit = db.Exhibits.Find(id);
            if (exhibit == null)
            {
                return HttpNotFound();
            }
            return View(exhibit);
        }

        // GET: Exhibits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exhibits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Image")] Exhibit exhibit)
        {
            string imagePartial = "/Content/images/exhibits/";
            string filename = null;

            if(exhibit.Image != null)
                filename = Path.GetFileName(exhibit.Image.FileName);
            
            //Save partial path to use as src
            exhibit.ImageLocation = imagePartial + filename;

            Exhibit existing = (from e in db.Exhibits where e.ImageLocation == exhibit.ImageLocation select e).FirstOrDefault();

            if(existing != null && filename!=null)
                ModelState.AddModelError("Image", "An image already exists with the file name chosen. Please rename this file or delete the previous.");

            exhibit = (from e in db.Exhibits where e.Name == exhibit.Name select e).FirstOrDefault();

            if (existing != null)
                ModelState.AddModelError("Name", "An exhibt already exists with the name chosen. Please the exhibit.");
            
            if (ModelState.IsValid)
            {
                exhibit.CreateDate = DateTime.Now;
                exhibit.LastModifiedDate = DateTime.Now;
           
                //Use full path to save to server
                if(filename !=null)
                    exhibit.Image.SaveAs(Path.Combine(Server.MapPath("~/Content/images/exhibits"), filename));
                
                db.Exhibits.Add(exhibit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exhibit);
        }

        // GET: Exhibits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exhibit exhibit = db.Exhibits.Find(id);
            if (exhibit == null)
            {
                return HttpNotFound();
            }
            return View(exhibit);
        }

        // POST: Exhibits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Exhibit exhibit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exhibit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exhibit);
        }

        // GET: Exhibits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exhibit exhibit = db.Exhibits.Find(id);
            if (exhibit == null)
            {
                return HttpNotFound();
            }
            return View(exhibit);
        }

        // POST: Exhibits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exhibit exhibit = db.Exhibits.Find(id);
            db.Exhibits.Remove(exhibit);
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
