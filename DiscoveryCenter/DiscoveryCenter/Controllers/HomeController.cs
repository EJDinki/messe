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
        [HttpGet]
        public  ActionResult Survey()
        {
            SurveyViewModel model = null;
            using(SurveyContext dbContext = new SurveyContext())
            {
                Survey survey = (from s in dbContext.Surveys where s.Id == 1 select s).Single();
                model = new SurveyViewModel();
                model.questions = survey.Questions;
                model.options = new Dictionary<int,List<SurveyViewModel.Option>>();

                for(int i = 0; i < survey.Questions.Count; i++)
                {
                    model.answer.Add("");
                    model.options.Add(i, new List<SurveyViewModel.Option>());
                    if(survey.Questions[i].Type == Question.QuestionType.MultipleChoiceChooseMany ||
                       survey.Questions[i].Type == Question.QuestionType.MultipleChoiceChooseOne)
                    {
                        foreach(string s in survey.Questions[i].Choices.Split(';'))
                        {
                            SurveyViewModel.Option option = new SurveyViewModel.Option(survey.Questions[i].Id);
                            option.text = s;
                            model.options[i].Add(option);
                        }
                    }
                    else
                    {
                        model.options[i].Add(new SurveyViewModel.Option(survey.Questions[i].Id, true));
                    }
                    
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Survey(SurveyViewModel model)
        {
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