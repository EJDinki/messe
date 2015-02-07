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
                model.QuestionModels = new List<QuestionViewModel>();
                model.SurveyId = survey.Id;
                model.SurveyName = survey.Name;

                
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
            return View(model);
        }

        [HttpPost]
        public ActionResult Survey([ModelBinder(typeof(SurveyModelBinder))]SurveyViewModel model)
        {
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
                    else if (!String.IsNullOrWhiteSpace(qmodel.Answer))
                    {
                        answer = new Answer() { QuestionId = qmodel.QuestionId, Value = qmodel.Answer, Submission = submission };
                        db.Answers.Add(answer);
                    }
                }

                db.SaveChanges();
            }
            return Redirect("/");
        }

        public ActionResult Index()
        {
            return View();
        }
      
    }
}