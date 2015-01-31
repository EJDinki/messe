using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiscoveryCenter.Models;

namespace DiscoveryCenter.Controllers
{
    public class HomeController : Controller
    {
        private SurveyContext db = new SurveyContext();

        [HttpGet]
        public  ActionResult Survey()
        {
            SurveyViewModel model = null;
            using(SurveyContext dbContext = new SurveyContext())
            {
                Survey survey = (from s in dbContext.Surveys where s.Id == 1 select s).Single();
                model = new SurveyViewModel();
                model.QuestionModels = new List<ViewModel>();
                model.SurveyId = survey.Id;
                model.SurveyName = survey.Name;

                for(int i = 0; i < survey.Questions.Count; i++)
                {
                    switch(survey.Questions[i].Type)
                    {
                        case(Question.QuestionType.MultipleChoiceChooseMany):
                            MultipleSelectViewModel mS = new MultipleSelectViewModel();
                            mS.QuestionId = survey.Questions[i].Id;
                            mS.Answer = "";
                            mS.Question = survey.Questions[i].Text;
                            mS.Type = survey.Questions[i].Type;
                            mS.Choices = survey.Questions[i].Choices.Split(';').ToList();
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
                        case(Question.QuestionType.MultipleChoiceChooseOne):
                            MultipleChoiceViewModel mC = new MultipleChoiceViewModel();
                            mC.QuestionId = survey.Questions[i].Id;
                            mC.Answer = "";
                            mC.Question = survey.Questions[i].Text;
                            mC.Type = survey.Questions[i].Type;
                            mC.Choices = survey.Questions[i].Choices.Split(';').ToList();
                            model.QuestionModels.Add(mC);
                            break;
                        default:
                            ViewModel m = new ViewModel();
                            m.QuestionId = survey.Questions[i].Id;
                            m.Answer = "";
                            m.Question = survey.Questions[i].Text;
                            m.Type = survey.Questions[i].Type;
                            model.QuestionModels.Add(m);
                            break;
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Survey([ModelBinder(typeof(SurveyModelBinder))]SurveyViewModel model)
        {
            Submission submission = new Submission() { Timestamp = DateTime.Now, SurveyId = model.SurveyId };
            db.Submissions.Add(submission);

            foreach(var qmodel in model.QuestionModels)
            {
                Answer answer;
                if(qmodel is MultipleSelectViewModel)
                {
                    foreach(var selection in ((MultipleSelectViewModel)qmodel).Options.Where(c => c.IsSelected))
                    {
                        answer = new Answer() { QuestionId = qmodel.QuestionId, Value = selection.text, Submission=submission};
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

            return Redirect("/");
        }

        public ActionResult Index()
        {
            return RedirectToAction("Survey");
        }
        /*
         *Currently ugly code just meant to test my knowledge.
         *Need to store previously selected answers in an Answer in the HttpSession and call them up.
         *Probably should break this behemoth of a method into smaller chunks soon. Bad code smell.
         */
        /*
        public PartialViewResult Index(int id = 0)
        {
            if (Session.Count == 0)
            {
                Session.Add("QuestionIndex", 0);
                Session.Add("NumQuestions", 0);
                Session.Add("Survey", null);
            }

            Models.Survey survey = null;
            Models.Question question = null;
            using (Models.SurveyContext aContext = new Models.SurveyContext())
            {
                
                survey = (from s in aContext.Surveys where s.Id == 1 select s).Single();
                Session["Survey"] = survey;
                Session["NumQuestions"] = survey.Questions.Count;

                if (id < 0)
                    id = 0;
                else if (id >= ViewBag.NumQuestions)
                    id = (int)Session["NumQuestions"] - 1;

                question = survey.Questions[id];

                Session["QuestionIndex"] = id;

                switch (question.Type)
                {
                    case (Question.QuestionType.ShortAnswer):
                        return PartialView("ShortAnswer", question);
                    case (Question.QuestionType.MultipleChoiceChooseOne):
                        return PartialView("MultipleChoice", 
                            new MultipleChoiceQuestionViewModel()
                            { 
                                SurveyName = survey.Name,
                                AllowMultiple = false,
                                Choices= question.Choices.Split(';').ToList<string>(),
                                Question=question.Text
                            });
                    case (Question.QuestionType.MultipleChoiceChooseMany):
                        return PartialView("MultipleChoice",
                            new MultipleChoiceQuestionViewModel()
                            {
                                SurveyName = survey.Name,
                                AllowMultiple = true,
                                Choices = question.Choices.Split(';').ToList<string>(),
                                Question = question.Text
                            });
                    default:
                        return PartialView(question);
                }
            }
        }*/
    }
}