using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DiscoveryCenter.Models;
using System.Drawing;

namespace DiscoveryCenter.Controllers
{
    // [Authorize]
    public class CreationController : Controller
    {
        private SurveyContext db = new SurveyContext();

        public ViewResult BlankQuestionRow(int id, int surveyId)
        {
            Survey survey = (from s in db.Surveys where s.Id == surveyId select s).FirstOrDefault();
            
            Question q;
            if(survey != null)
                q = new Question() { SurveyID = survey.Id };
            else
                q = new Question();

            return View("Question", new Tuple<int,Question>(id , q));
        }
        // GET: Creation
        public ActionResult Index()
        {
            return View(db.Surveys.ToList());
        }

        // GET: Creation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // GET: Creation/Create
        public ActionResult Create()
        {
            return View("Edit", new Survey());
        }

        // POST: Creation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([ModelBinder(typeof(SurveyModelBinder))] Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Surveys.Add(survey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(survey);
        }

        // GET: Creation/Edit/5
        public ActionResult Edit(int? id)
        {
            Session.Add("questionIndex", 0);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            survey.Questions = survey.Questions.OrderBy(q => q.IndexInSurvey).ToList();
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // POST: Creation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([ModelBinder(typeof(SurveyModelBinder))] Survey survey)
        {
            if (String.IsNullOrEmpty(survey.Name))
                ModelState.AddModelError("Name", "Name is invalid");
            else if(!survey.Questions.Any())
                ModelState.AddModelError("Questions", "Survey requires at least one question");

            if (ModelState.IsValid)
            {
                Survey oldVersion = (from s in db.Surveys where s.Id == survey.Id select s).SingleOrDefault();
                if(oldVersion == null)
                    db.Surveys.Add(survey);
                else {
                    oldVersion.Name = survey.Name;
                    oldVersion.CreateDate = DateTime.Now;
                    List<Question> deleteList = new List<Question>();
                    foreach(Question q in oldVersion.Questions)
                    {
                       Question match = (from s in survey.Questions where s.Id == q.Id select s).SingleOrDefault();

                       if(match == null)
                       {
                           deleteList.Add(q);
                       }
                       else
                       {
                           q.Text = match.Text;
                           q.Type = match.Type;
                           q.Choices = match.Choices;
                           q.IndexInSurvey = match.IndexInSurvey;
                       }
                    }

                   foreach (Question delete in deleteList)
                   {
                       db.Set(typeof(Question)).Remove(delete);
                   }

                   foreach (Question newID in survey.Questions)
                   {
                       if (newID.Id == 0)
                           oldVersion.Questions.Add(newID);
                   }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(survey);
        }

        // GET: Creation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // POST: Creation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Survey survey = db.Surveys.Find(id);
            db.Surveys.Remove(survey);
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
