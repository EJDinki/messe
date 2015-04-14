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
        public  ActionResult Survey(int id = 1)
        {
            SurveyViewModel model = null;
            Survey survey;
            Session["startTime"] = DateTime.Now;
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
                            mS.Choices = question.Choices;
                            mS.MaxSelectedChoices = question.MaxSelectedChoices;
                            model.QuestionModels.Add(mS);
                            break;
                        case (Question.QuestionType.Slider):
                            SliderViewModel s = new SliderViewModel();
                            s.QuestionId = question.Id;
                            s.Answer = "";
                            s.Question = question.Text;
                            s.Type = question.Type;
                            s.Choices = question.Choices;
                            model.QuestionModels.Add(s);
                            break;
                        case(Question.QuestionType.MultipleChoiceChooseOne):
                            MultipleChoiceViewModel mC = new MultipleChoiceViewModel();
                            mC.QuestionId = question.Id;
                            mC.Answer = "";
                            mC.Question = question.Text;
                            mC.Type = question.Type;
                            mC.Choices = question.Choices;
                            model.QuestionModels.Add(mC);
                            break;
                        case(Question.QuestionType.ExhibitsChooseMany):
                            MultipleSelectViewModel eM = new MultipleSelectViewModel();
                            eM.QuestionId = question.Id;
                            eM.Answer = "";
                            eM.Question = question.Text;
                            eM.Type = question.Type;                       
                            eM.MaxSelectedChoices = question.MaxSelectedChoices;
                            eM.Choices = new List<Choice>();

                            foreach (Exhibit ex in dbContext.Exhibits)
                            {
                                eM.Choices.Add(new Choice() { Text = ex.Name, ImageName = ex.ImageLocation });
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
            DateTime endingTime = DateTime.Now;
            int totalTime = (int) (endingTime - DateTime.Parse(Session["startTime"].ToString())).TotalSeconds;
            ThankYouViewModel ret = new ThankYouViewModel();

            using (SurveyContext db = new SurveyContext())
            {
                Submission submission = new Submission() { Timestamp = endingTime, SurveyId = model.SurveyId, CompletionTime = totalTime};
                db.Submissions.Add(submission);

                foreach (var qmodel in model.QuestionModels)
                {
                    Answer answer;
                    if (qmodel is MultipleSelectViewModel)
                    {
                        foreach (var selection in ((MultipleSelectViewModel)qmodel).Choices.Where(c => c.IsSelected))
                        {
                            answer = new Answer() { QuestionId = qmodel.QuestionId, Value = selection.Text, Submission = submission };
                            db.Answers.Add(answer);
                        }
                    }
                    else if (qmodel.Type == Question.QuestionType.ExhibitsChooseMany)
                    {
                        foreach (string ex in qmodel.Answer.Split(';'))
                        {
                            if (ex != null && ex.Length != 0)
                            {
                                answer = new Answer() { QuestionId = qmodel.QuestionId, Value = ex.Trim(), Submission = submission };
                                db.Answers.Add(answer);
                            }
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
                ret.Muted = model.Muted;
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

        public ActionResult PreviewTheme(int id = 1)
        {
            SurveyViewModel model = null;

            using (SurveyContext db = new SurveyContext())
            {
                model = new SurveyViewModel();
                model.SurveyName = "Test Survey";
                model.SurveyDescription = "This is a test survey to preview the page styling";
                model.Theme = db.Themes.Find(id);
            }

            return View("Index", model);
        }

    }
}