using DiscoveryCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscoveryCenter.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index(int id = 0)
        {
            if (id <= 0)
            {
                return RedirectToAction("Index", "Creation");
            }

            ReportsViewModel reports = new ReportsViewModel();
            ReportViewModel report;

            using(SurveyContext db = new SurveyContext())
            {
                Survey survey = db.Surveys.Find(id);
                if (survey == null)
                    return HttpNotFound();

                reports.SurveyName = survey.Name;
                reports.SurveyId = survey.Id;

                foreach (var question in survey.Questions.OrderBy(q => q.IndexInSurvey))
                {
                    switch (question.Type)
                    {
                        case Question.QuestionType.ShortAnswer:
                            break;
                        default:
                            report = new ReportViewModel();
                            report.Type = question.Type;
                            report.QuestionIndex = question.IndexInSurvey;
                            report.Text = question.Text;

                            foreach (var answer in question.Answers)
                            {
                                if (!report.Counts.ContainsKey(answer.Value))
                                    report.Counts.Add(answer.Value, 1);
                                else
                                    report.Counts[answer.Value]++;
                            }

                            reports.Reports.Add(report);
                            break;
                    }
                }
            }

            return View(reports);
        }

        //public FileContentResult ExportToCSV(int id = 0)
        public ActionResult ExportToCSV(int id = 0)
        {
            if (id <= 0)
            {
                return new HttpNotFoundResult();
            }

            using (SurveyContext db = new SurveyContext())
            {
                Survey survey = db.Surveys.Find(id);
                if (survey == null)
                    return HttpNotFound();

                string csv = ConvertSurveyToCSV(survey);
                string fileName = survey.Name + "-" + DateTime.Today.ToString("MM/dd/yyyy");
                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", fileName);
            }
        }

        private string ConvertSurveyToCSV(Survey survey)
        {
            // TODO convert the survey into a string
            return "test1, test2, test3";
        }
    }
}