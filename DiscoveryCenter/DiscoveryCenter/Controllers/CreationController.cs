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
using System.IO;

namespace DiscoveryCenter.Controllers
{
    
    public class CreationController : Controller
    {
        private SurveyContext db = new SurveyContext();
        private static readonly int surveyPerPage = 10;
        private static readonly string choiceImagePartial = "/Content/images/choiceImage/";

        [Authorize]
        public PartialViewResult RefreshChoices(int typeIndex, string choices, string images)
        {   
            var listChoices = new List<Choice>();
            var listText = choices.Split(';');
            var listImage = images.Split(';');

            if ((Question.QuestionType)typeIndex == Question.QuestionType.Slider)
            {
                //Ugly magic number, but slider hard coded to have 3 choices
                for (int i = 0; i < 3; i++)
                {
                    Choice c = new Choice();

                    if(i < listText.Length)
                        c.Text = listText[i];

                    if (i < listImage.Length)
                        c.ImageName = listImage[i];

                    listChoices.Add(c);
                }
            }
            else
            {
            
                //If not slider, Choices should be based on the number of choices previously filled out
            for(int i=0; i < listText.Length; i ++)
            {
                Choice c = new Choice();
                c.Text = listText[i];

                if(i < listImage.Length)
                    c.ImageName = listImage[i];

                listChoices.Add(c);
            }
        }
            return PartialView("_ChoicesInputGroup", new Question() { Type = (Question.QuestionType)typeIndex, Choices = listChoices });
        }

        [Authorize]
        public PartialViewResult BlankChoiceBox(string value, string imageName ,bool allowDelete)
        {
            string guid = Guid.NewGuid().ToString();
            string nameAndId = String.Format("Questions[{0}].Choice[{1}]", guid, guid);
            string imgId = String.Format("Questions[{0}].Img[{1}]", guid, guid);

            List<SelectListItem> storedImageNames = new List<SelectListItem>();
            storedImageNames.Add(new SelectListItem() { Text = "None", Value = "" });

            foreach(string image in Directory.GetFiles(Server.MapPath("~"+choiceImagePartial)))
            {
                storedImageNames.Add(new SelectListItem() { Text = Path.GetFileName(image), Value = choiceImagePartial + Path.GetFileName(image) });
            }
            System.Diagnostics.Debug.WriteLine("test");
            return PartialView("_ChoiceBox", new ChoiceBoxViewModel() { 
                NameAndId = nameAndId,
                ImgChoiceId = imgId,
                Value = value,
                AvailableImages=storedImageNames ,
                AllowDelete = allowDelete });
        }

        [Authorize]
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
                foreach (var choice in question.Choices)
                {
                    choice.ParentQuestion = question;
                    db.Choices.Add(choice);
                }
                db.Questions.Add(question);
            }
            db.SaveChanges();

            return RedirectToAction("index");
        }
        
        [Authorize]
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
            int nonRatingSurveys = (from x in db.Surveys where x.IsRatingSurvey == false select x).Count();
            int numPages = (nonRatingSurveys / surveyPerPage);

            numPages += (nonRatingSurveys % surveyPerPage > 0 || numPages == 0) ? 1 : 0;

            

            //Handle out of bounds cases
            if (id < 0)
                id = 0;
            else if (id >= numPages)
                id = numPages - 1;

            var currentPageList = (from s in db.Surveys where s.IsRatingSurvey == false orderby s.Name select s).Skip(id * surveyPerPage).Take(surveyPerPage).ToList();              
            Tuple<IEnumerable<Survey>, int, int> tuple = new Tuple<IEnumerable<Survey>, int, int>(currentPageList, numPages, id);

            return View(tuple);
        }

        [Authorize]
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

        [Authorize]
        public ActionResult Create()
        {
            return View("Edit", MakeCreationEditVM(new Survey()));
        }

        // POST: Creation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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

       [Authorize]
        public ActionResult Edit(int? id)
        {
            Session.Add("questionIndex", 0);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            
            if (survey == null)
            {
                return HttpNotFound();
            }
            survey.Questions = survey.Questions.OrderBy(q => q.IndexInSurvey).ToList();


            return View(MakeCreationEditVM(survey));
        }

        public CreationEditViewModel MakeCreationEditVM(Survey survey)
        {
            List<SelectListItem> themes = new List<SelectListItem>(); ;
            db.Themes.ToList().ForEach(t => themes.Add(new SelectListItem() { Value = t.Id.ToString(), Text = t.Name }));

            return new CreationEditViewModel() { Survey = survey, Themes = themes };
        }

        // POST: Creation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
                    var choices = question.Choices;

                    if (choices.Count == 0)
                    {
                        ModelState.AddModelError(String.Format("Questions[{0}].Choices", question.IndexInSurvey),
                                                 String.Format("Question {0} must have at least one choice.", question.IndexInSurvey));
                    }

                    for (int i = 0; i < choices.Count; i++)
                        if (String.IsNullOrWhiteSpace(choices[i].Text))
                        {
                            ModelState.AddModelError(String.Format("Questions[{0}].Choices", question.IndexInSurvey),
                                String.Format("Choice {0} for question {1} is blank.", i+1, question.IndexInSurvey));
                            break;
                        }

                    var hasDuplicates = choices.GroupBy(c => c.Text).Where(g => g.Count() > 1).Count() >= 1;
                    if(hasDuplicates)
                    {
                        ModelState.AddModelError(String.Format("Questions[{0}].Choices", question.IndexInSurvey),
                                String.Format("Question {0} contains duplicate choices", question.IndexInSurvey));
                    }
                }
                else if (question.Type == Question.QuestionType.Spinner)
                {
                    if(question.MaxSelectedChoices < question.MinSelect)
                    {
                        ModelState.AddModelError(String.Format("Questions[{0}].Range", question.IndexInSurvey), String.Format("The Max must be >= Min for question {0}.", question.IndexInSurvey));
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
                oldVersion.Description = survey.Description;
                oldVersion.ThemeId = survey.ThemeId;
                oldVersion.LastModifiedDate = DateTime.Now;
                List<Question> deleteList = new List<Question>();
                foreach (Question q in oldVersion.Questions)
                {
                    Question match = (from s in survey.Questions where s.Id == q.Id select s).SingleOrDefault();

                    //Load answers into context in case question is deleted and they are orphaned.
                    List<Answer> allAnswers = (from a in db.Answers where a.QuestionId == q.Id select a).ToList();

                    if (match == null)
                    {
                        deleteList.Add(q);
                    }
                    else
                    {
                        q.Text = match.Text;
                        q.Type = match.Type;
                        q.Choices = new List<Choice>();
                        for(int i=0; i < match.Choices.Count; i++)
                        {
                            //If the old version has no match, the choice was added in edit. Add to db.
                            if (i < q.Choices.Count)
                            {
                                q.Choices[i].Text = match.Choices[i].Text;
                                q.Choices[i].ImageName = match.Choices[i].ImageName;
                            }
                            else
                            {
                                Choice addedChoice = new Choice()
                                {
                                    ParentQuestion = q,
                                    Text = match.Choices[i].Text,
                                    ImageName = match.Choices[i].ImageName
                                };
                                db.Choices.Add(addedChoice);
                            }
                        }
                        q.MaxSelectedChoices = match.MaxSelectedChoices;
                        q.MinSelect = match.MinSelect;
                        q.IndexInSurvey = match.IndexInSurvey;
                    }
                }

                foreach (Question delete in deleteList)
                {
                    List<Choice> allChoices = (from c in db.Choices where c.QuestionID == delete.Id select c).ToList();
                    foreach (var choice in allChoices)
                    {
                        db.Set<Choice>().Remove(choice);
                    }
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

        [Authorize]
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
            ViewBag.DeleteObject = survey.Name + " survey";
            return View("DeleteConfirm");
        }

        // POST: Creation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
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
