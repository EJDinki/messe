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


        /*
         *Currently ugly code just meant to test my knowledge.
         *Need to store previously selected answers in an Answer in the HttpSession and call them up.
         *Probably should break this behemoth of a method into smaller chunks soon. Bad code smell.
         */
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
        }

        //public ActionResult MultipleChoice(int id)
        //{

        //    return View(((Survey)Session["Survey"]).Questions[id]);
        //}

        //public ActionResult ShortAnswer(int id)
        //{

        //    return View(((Survey)Session["Survey"]).Questions[id]);
        //}



    }
}