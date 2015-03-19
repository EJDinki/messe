﻿using System;
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

        public PartialViewResult RefreshChoices(int typeIndex, string choices)
        {
            return PartialView("_ChoicesInputGroup", new Question() { Type = (Question.QuestionType)typeIndex, Choices = choices });
        }

        public PartialViewResult BlankChoiceBox(string value, bool allowDelete)
        {
            string guid = Guid.NewGuid().ToString();
            string nameAndId = String.Format("Questions[{0}].Choice[{1}]", guid, guid);
            return PartialView("_ChoiceBox", new ChoiceBoxViewModel() { NameAndId = nameAndId, Value = value, AllowDelete = allowDelete });
        }

        public ActionResult Duplicate(int id)
        {
            Survey survey = db.Surveys.Find(id);
            
            if (survey == null)
                return HttpNotFound();

            do
            {
                survey.Name = survey.Name + " - Copy";
            }
            while (db.Surveys.Where(q => q.Name == survey.Name).SingleOrDefault() != null);

            var questions = survey.Questions;

            db.Surveys.Add(survey);

            foreach (var question in questions)
            {
                question.SurveyID = survey.Id;
                db.Questions.Add(question);
            }
            db.SaveChanges();

            return RedirectToAction("index");
        }
        public ViewResult BlankQuestionRow(int id)
        {
            Survey survey = (from s in db.Surveys where s.Id == id select s).FirstOrDefault();
            
            Question q;
            if(survey != null)
                q = new Question() { SurveyID = survey.Id };
            else
                q = new Question();

            return View("Question", q);
        }
        // GET: Creation
        public ActionResult Index(int id = 0)
        {
            int numPages = (db.Surveys.Count() / 10) + 1;

            //Handle out of bounds cases
            if (id < 0)
                id = 0;
            else if (id >= numPages)
                id = numPages - 1;

            var currentPageList = (from s in db.Surveys orderby s.Id select s).Skip(id * 10).Take(10).ToList();              
            Tuple<IEnumerable<Survey>, int, int> tuple = new Tuple<IEnumerable<Survey>, int, int>(currentPageList, numPages, id);

            return View(tuple);
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
            return View("Edit", MakeCreationEditVM(new Survey()));
        }

        // POST: Creation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([ModelBinder(typeof(SurveyModelBinder))] CreationEditViewModel cevm)
        {
            Survey survey = cevm.Survey;

            if (ModelState.IsValid)
            {
                survey.LastModifiedDate = DateTime.Now;
                db.Surveys.Add(survey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Edit", MakeCreationEditVM(survey));
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


            return View(MakeCreationEditVM(survey));
        }

        public CreationEditViewModel MakeCreationEditVM(Survey survey)
        {
            List<SelectListItem> themes = new List<SelectListItem>(); ;
            db.Themes.ToList().ForEach(t => themes.Add(new SelectListItem() { Value = t.Id.ToString(), Text = t.Name.Value }));

            return new CreationEditViewModel() { Survey = survey, Themes = themes };
        }

        // POST: Creation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([ModelBinder(typeof(SurveyModelBinder))] CreationEditViewModel cevm)
        {
            Survey survey = cevm.Survey;

            //--------------------------Validation----------------------------
            //clear errors
            //foreach (var key in ModelState.Keys)
             //   ModelState[key].Errors.Clear();

            //check if there are any questions
            if (!survey.Questions.Any())
                ModelState.AddModelError("Questions", "A survey requires at least one question.");

            foreach (var question in survey.Questions)
            {
                //check if choices are invalid
                if (question.Type != Question.QuestionType.ShortAnswer &&
                    question.Type != Question.QuestionType.ExhibitsChooseMany &&
                    question.Type != Question.QuestionType.Spinner)
                {
                    var choices = question.Choices.Split(';');
                    for (int i = 0; i < choices.Length; i++)
                        if (String.IsNullOrWhiteSpace(choices[i]))
                        {
                            ModelState.AddModelError(String.Format("Questions[{0}].Choices", question.IndexInSurvey, i),
                                String.Format("Choice {0} for question {1} is blank.", i+1, question.IndexInSurvey));
                            break;
                        }
                }

                //check if question text is invalid
                if (String.IsNullOrWhiteSpace(question.Text))
                    ModelState.AddModelError(String.Format("Questions[{0}].Text", question.IndexInSurvey), String.Format("Question text for question {0} is blank.", question.IndexInSurvey));
            }

            //-------------------------Update Survey----------------------------

            if (!ModelState.IsValid)
                return View(MakeCreationEditVM(survey));

            Survey oldVersion = (from s in db.Surveys where s.Id == survey.Id select s).SingleOrDefault();
            if (oldVersion == null)
            {
                survey.LastModifiedDate = DateTime.Now;
                db.Surveys.Add(survey);
            }
            else
            {
                oldVersion.Name = survey.Name;
                oldVersion.ThemeId = survey.ThemeId;
                oldVersion.LastModifiedDate = DateTime.Now;
                List<Question> deleteList = new List<Question>();
                foreach (Question q in oldVersion.Questions)
                {
                    Question match = (from s in survey.Questions where s.Id == q.Id select s).SingleOrDefault();

                    if (match == null)
                    {
                        deleteList.Add(q);
                    }
                    else
                    {
                        q.Text = match.Text;
                        q.Type = match.Type;
                        q.Choices = match.Choices;
                        q.MaxSelectedChoices = match.MaxSelectedChoices;
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
            //Include Questions & Submissions in context in case they are orphaned.
            Survey survey = db.Set<Survey>()
                .Include(m => m.Questions)
                .Include(m => m.Submissions)
                .FirstOrDefault(m => m.Id == id);

            db.Set<Survey>().Remove(survey);
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
