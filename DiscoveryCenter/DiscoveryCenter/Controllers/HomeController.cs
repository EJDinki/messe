﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiscoveryCenter.Models;

namespace DiscoveryCenter.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public  ActionResult Survey(int id = 1)
        {
            SurveyViewModel model = null;
            Survey survey;

            using(SurveyContext dbContext = new SurveyContext())
            {
                survey = (from s in dbContext.Surveys where s.Id == id select s).Single();
                model = new SurveyViewModel();
                model.QuestionModels = new List<QuestionViewModel>();
                model.SurveyId = survey.Id;
                model.SurveyName = survey.Name;
                model.Theme = survey.Theme;
                
                foreach(var question in survey.Questions.OrderBy(x => x.IndexInSurvey))
                {
                    switch (question.Type)
                    {
                        case(Question.QuestionType.MultipleChoiceChooseMany):
                            MultipleSelectViewModel mS = new MultipleSelectViewModel();
                            mS.QuestionId = question.Id;
                            mS.Answer = "";
                            mS.Question = question.Text;
                            mS.Type = question.Type;
                            mS.Choices = question.Choices.Split(';').ToList();
                            mS.MaxSelectedChoices = question.MaxSelectedChoices;
                            mS.Options = new List<Selection>();

                            foreach(string choice in mS.Choices)
                            {
                                Selection select = new Selection();
                                select.IsSelected = false;
                                select.text = choice;

                                mS.Options.Add(select);
                            }

                            model.QuestionModels.Add(mS);
                            break;
                        case (Question.QuestionType.Slider):
                            SliderViewModel s = new SliderViewModel();
                            s.QuestionId = question.Id;
                            s.Answer = "";
                            s.Question = question.Text;
                            s.Type = question.Type;
                            s.Choices = question.Choices.Split(';').ToList();
                            model.QuestionModels.Add(s);
                            break;
                        case(Question.QuestionType.MultipleChoiceChooseOne):
                            MultipleChoiceViewModel mC = new MultipleChoiceViewModel();
                            mC.QuestionId = question.Id;
                            mC.Answer = "";
                            mC.Question = question.Text;
                            mC.Type = question.Type;
                            mC.Choices = question.Choices.Split(';').ToList();
                            model.QuestionModels.Add(mC);
                            break;
                        case(Question.QuestionType.ExhibitsChooseMany):
                            MultipleSelectViewModel eM = new MultipleSelectViewModel();
                            eM.QuestionId = question.Id;
                            eM.Answer = "";
                            eM.Question = question.Text;
                            eM.Type = question.Type;
                            eM.MaxSelectedChoices = question.MaxSelectedChoices;
                            eM.Choices = (from e in dbContext.Exhibits select e.Name).ToList();
                            eM.Options = new List<Selection>();

                            foreach(string choice in eM.Choices)
                            {
                                Selection select = new Selection();
                                select.IsSelected = false;
                                select.text = choice;
                                // TODO Don't hardcode this
                                select.image = "http://placekitten.com/g/64/64";

                                eM.Options.Add(select);
                            }

                            model.QuestionModels.Add(eM);
                            break;
                        default:
                            QuestionViewModel m = new QuestionViewModel();
                            m.QuestionId = question.Id;
                            m.Answer = "";
                            m.Question = question.Text;
                            m.Type = question.Type;
                            model.QuestionModels.Add(m);
                            break;
                    }
                }
            }
            return View("Survey", model);
        }

        [HttpPost]
        public ActionResult Survey([ModelBinder(typeof(SurveyModelBinder))]SurveyViewModel model)
        {
            ThankYouViewModel ret = new ThankYouViewModel();

            using (SurveyContext db = new SurveyContext())
            {
                Submission submission = new Submission() { Timestamp = DateTime.Now, SurveyId = model.SurveyId };
                db.Submissions.Add(submission);

                foreach (var qmodel in model.QuestionModels)
                {
                    Answer answer;
                    if (qmodel is MultipleSelectViewModel)
                    {
                        foreach (var selection in ((MultipleSelectViewModel)qmodel).Options.Where(c => c.IsSelected))
                        {
                            answer = new Answer() { QuestionId = qmodel.QuestionId, Value = selection.text, Submission = submission };
                            db.Answers.Add(answer);
                        }
                    }
                    else if (qmodel.Type == Question.QuestionType.ExhibitsChooseMany)
                    {
                        string exhibitIds = "";

                        //change exhibit name to Id
                        foreach (string name in qmodel.Answer.Split(';'))
                        {
                            string trimmed = name.Trim();
                            int? exhibit = (from e in db.Exhibits where e.Name.Contains(trimmed) select e.Id).FirstOrDefault();

                            if (exhibit != null)
                                exhibitIds += exhibit + ";";
                            else
                                throw new NullReferenceException("Could not find exhibit with name: " + trimmed);
                        }

                        //Create new Answer for each selected exhibit Id
                        foreach (string ex in exhibitIds.Split(';'))
                        {
                            answer = new Answer() {QuestionId=qmodel.QuestionId, Value=ex, Submission = submission };
                            db.Answers.Add(answer);
                        }
                    }
                    else if (!String.IsNullOrWhiteSpace(qmodel.Answer))
                    {
                        answer = new Answer() { QuestionId = qmodel.QuestionId, Value = qmodel.Answer, Submission = submission };
                        db.Answers.Add(answer);
                    }
                }

                db.SaveChanges();

                ret.Theme = db.Surveys.Find(model.SurveyId).Theme;
                ret.SurveyId = model.SurveyId;
            }

            return View("ThankYou", ret);
        }

        public ActionResult Index(int id = 1)
        {
            SurveyViewModel model = null;

            using (SurveyContext dbContext = new SurveyContext())
            {
                Survey survey = (from s in dbContext.Surveys where s.Id == id select s).Single();
                model = new SurveyViewModel();
                model.SurveyName = survey.Name;
                model.SurveyDescription = survey.Description;
                model.SurveyId = id;
                model.Theme = survey.Theme;
            }
            return View("Index", model);
        }
      
    }
}