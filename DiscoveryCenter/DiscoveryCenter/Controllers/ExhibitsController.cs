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
using System.Net.Http;
using System.Net.Http.Headers;

namespace DiscoveryCenter.Controllers
{
    [Authorize]
    public class ExhibitsController : Controller
    {
        private SurveyContext db = new SurveyContext();
        private static readonly int exhibitsPerPage = 8;

        // GET: Exhibits
        public ActionResult Index(int id = 0)
        {
            int numPages = (db.Exhibits.Count() / exhibitsPerPage);
            numPages += (db.Exhibits.Count() % exhibitsPerPage > 0 || numPages == 0) ? 1 : 0;

            //Handle out of bounds cases
            if (id < 0)
                id = 0;
            else if (id >= numPages)
                id = numPages - 1;

            var currentPageList = (from s in db.Exhibits orderby s.Name select s).Skip(id * exhibitsPerPage).Take(exhibitsPerPage).ToList();
            Tuple<IEnumerable<Exhibit>, int, int> tuple = new Tuple<IEnumerable<Exhibit>, int, int>(currentPageList, numPages, id);

            return View(tuple);
        }

        /// <summary>
        /// Gets the rating survey based on exhibit id, then redirects to the survey report for the specified exhibit ratings.
        /// </summary>
        /// <param name="id">Exhibit id</param>
        /// <returns></returns>
        public ActionResult ViewExhibitRating(int id)
        {
            int? reportId = (from e in db.Exhibits where e.Id == id select e.RatingSurveyID).FirstOrDefault();

            return RedirectToAction("Index", "Report", new { id=reportId });
        }

        /// <summary>
        /// Gets the QR code for the First page of the Exhibit rating survey based on exhibit id.
        /// </summary>
        /// <param name="id">Exhibit id</param>
        /// <returns>QRCode for url</returns>
        public FileResult DownloadQRCode(int id)
        {
            int? reportId = (from e in db.Exhibits where e.Id == id select e.RatingSurveyID).FirstOrDefault();
            FileContentResult result;
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            string parameters = "/Home/Survey/"+reportId;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync("https://api.qrserver.com/v1/create-qr-code/?size=150x150&data="+
                                                               baseUrl+parameters).Result;
                byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
                result = new FileContentResult(bytes, "image/png");
            }
          
            return result;
        }

        // GET: Exhibits/Create
        public ActionResult Create()
        {
            return View("Edit", new Exhibit());
        }

        public static Survey CreateRatingSurvey(Exhibit exhibit, SurveyContext db, int id = -1)
        {
            Survey RateThisExhibit = null;

            if (id != -1)
                RateThisExhibit = db.Surveys.Find(id);

            if (RateThisExhibit == null)
            {
                RateThisExhibit = new Survey()
                {
                    IsRatingSurvey = true,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Theme = (from t in db.Themes where t.Name == "Mobile" select t).SingleOrDefault(),
                    Description = "Give a rating to the " + exhibit.Name + " exhibit!",
                    Name = "Rate " + exhibit.Name
                };

                RateThisExhibit.Id = id;

                Question theQuestion =
               new Question()
               {
                   Text = "Did you enjoy the " + exhibit.Name + " exhibit?",
                   ParentSurvey = RateThisExhibit,
                   Type = Question.QuestionType.Slider,

               };



                List<Choice> choices = new List<Choice>()
            {
                new Choice()
                {
                    Text = "Didn't like it.",
                    ImageName = ""
                },

                new Choice()
                {
                    Text = "Liked it.",
                    ImageName = ""
                },
                
                new Choice()
                {
                    Text = "Loved it!",
                    ImageName = ""
                }
            };
                db.Questions.Add(theQuestion);

                theQuestion.Choices = choices;
            }
     
            return RateThisExhibit;
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
            if (!ValidateExhibit(exhibit, true))
            {
                return View(exhibit);
            }

            //if new exhibit
            Exhibit oldVersion = db.Exhibits.Find(exhibit.Id);
            if (oldVersion == null)
            {
                exhibit.CreateDate = DateTime.Now;
                exhibit.LastModifiedDate = DateTime.Now;
                exhibit.ImageLocation = exhibit.BrokenWorkaround;
                exhibit.RatingSurvey = ExhibitsController.CreateRatingSurvey(exhibit, db);
                db.Exhibits.Add(exhibit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                oldVersion.ImageLocation = exhibit.BrokenWorkaround;
                oldVersion.LastModifiedDate = DateTime.Now;
                oldVersion.Name = exhibit.Name;
                oldVersion.Description = exhibit.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
            ViewBag.DeleteObject = exhibit.Name + " exhibit";
            return View("DeleteConfirm");
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
            bool passed = true;

            Exhibit existing = null;
            
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
