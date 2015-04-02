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
using System.Drawing;

namespace DiscoveryCenter.Controllers
{
    public class ExhibitsController : Controller
    {
        private SurveyContext db = new SurveyContext();
        private static readonly string exhibitImagePartial = "/Content/images/exhibits/";
        private static readonly int exhibitsPerPage = 8;

        // GET: Exhibits
        public ActionResult Index(int id = 0)
        {
            int numPages = (db.Exhibits.Count() / exhibitsPerPage) + 1;

            //Handle out of bounds cases
            if (id < 0)
                id = 0;
            else if (id >= numPages)
                id = numPages - 1;

            var currentPageList = (from s in db.Exhibits orderby s.Name select s).Skip(id * exhibitsPerPage).Take(exhibitsPerPage).ToList();
            Tuple<IEnumerable<Exhibit>, int, int> tuple = new Tuple<IEnumerable<Exhibit>, int, int>(currentPageList, numPages, id);

            return View(tuple);
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
        public ActionResult Create(Exhibit exhibit)
        {


            if (exhibit.Image != null)
                exhibit.ImageLocation = exhibitImagePartial + Path.GetFileName(exhibit.Image.FileName);

            if (exhibit.ShowcaseImage != null)
                exhibit.ShowcaseImageLocation = exhibitImagePartial + Path.GetFileName(exhibit.ShowcaseImage.FileName);

            if (!ValidateExhibit(exhibit))
                return View(exhibit);

            exhibit.CreateDate = DateTime.Now;
            exhibit.LastModifiedDate = DateTime.Now;

            //Use full path to save to server
            if(exhibit.ImageLocation !=null)
                exhibit.Image.SaveAs(Path.Combine(Server.MapPath("~/Content/images/exhibits"), Path.GetFileName(exhibit.Image.FileName)));

            if (exhibit.ShowcaseImageLocation != null)
                exhibit.ShowcaseImage.SaveAs(Path.Combine(Server.MapPath("~/Content/images/exhibits"), Path.GetFileName(exhibit.ShowcaseImage.FileName)));
                
            db.Exhibits.Add(exhibit);
            db.SaveChanges();
            return RedirectToAction("Index");    
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
        public ActionResult Edit(Exhibit exhibit)
        {
            if (exhibit.Image != null)
                exhibit.ImageLocation = exhibitImagePartial + Path.GetFileName(exhibit.Image.FileName);

            if (exhibit.ShowcaseImage != null)
                exhibit.ShowcaseImageLocation = exhibitImagePartial + Path.GetFileName(exhibit.ShowcaseImage.FileName);
                

            if (!ValidateExhibit(exhibit, true))
            {
                return View(exhibit);
            }
            
            if(exhibit.Image != null)
                exhibit.Image.SaveAs(Path.Combine(Server.MapPath("~/Content/images/exhibits"), Path.GetFileName(exhibit.Image.FileName)));

            if (exhibit.ShowcaseImageLocation != null)
                exhibit.ShowcaseImage.SaveAs(Path.Combine(Server.MapPath("~/Content/images/exhibits"), Path.GetFileName(exhibit.ShowcaseImage.FileName)));
              
            exhibit.LastModifiedDate = DateTime.Now;
            db.Entry(exhibit).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

  
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

        private bool ValidateExhibit(Exhibit ex, bool isEdit = false)
        {
            string filename = null;
            bool passed = true;

            if (ex.Image != null)
            {
                filename = Path.GetFileName(ex.Image.FileName);

                try
                {
                    Image.FromStream(ex.Image.InputStream);
                }
                catch
                {
                    ModelState.AddModelError("Image", "The file uploaded was not a valid image format.");
                    passed = false;
                }
            }

            Exhibit existing = null;
            
            if(!isEdit)
                existing = (from e in db.Exhibits where e.ImageLocation == ex.ImageLocation select e).FirstOrDefault();

            if (existing != null && ex.ImageLocation != null)
            {
                ModelState.AddModelError("Image", "An image already exists with the file name chosen. Please rename this file or delete the previous.");
                passed = false;
            }

            if (!isEdit)
                existing = (from e in db.Exhibits where e.Name == ex.Name select e).FirstOrDefault();

            if (existing != null)
            {
                ModelState.AddModelError("Name", "An exhibt already exists with the name chosen. Please the exhibit.");
                passed = false;
            }

            return passed;
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
