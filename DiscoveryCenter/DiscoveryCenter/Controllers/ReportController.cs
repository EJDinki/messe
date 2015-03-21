using DiscoveryCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ActionResult ExportToCSV(int id = 0, bool exportRawData = true)
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

                string csv = ConvertSurveyToCSV(survey, exportRawData);
                string fileName = survey.Name + "-" + DateTime.Today.ToString("MM/dd/yyyy") + ".csv";
                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", fileName);
            }
        }

        private string ConvertSurveyToCSV(Survey survey, bool exportRawData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\"Export for Survey: " + survey.Name + "\"\n");
            builder.Append("Date: " + DateTime.Today.ToString("MM/dd/yyyy") + "\n");
            builder.Append("Number of Questions: " + survey.Questions.Count + "\n");
            builder.Append("Number of Submissions: " + survey.Submissions.Count + "\n");
            builder.Append("\n");

            foreach (var question in survey.Questions.OrderBy(q => q.IndexInSurvey))
            {
                // TODO handle exhibits better or make exhibits save name instead of ID
                builder.Append("Question Number: " + question.IndexInSurvey + "\n");
                builder.Append("Question Type: " + question.Type + "\n");
                builder.Append("\"Question Text: " + question.Text + "\"\n");
                if (question.Answers.Count == 0)
                {
                    builder.Append("There are no submitted answers for this question");
                }
                else
                {
                    switch (question.Type)
                    {
                        case Question.QuestionType.MultipleChoiceChooseMany:
                        case Question.QuestionType.MultipleChoiceChooseOne:
                        case Question.QuestionType.Slider:
                            builder.Append("\"Question Choices (At Time of Export): " + question.Choices + "\"\n");
                            goto case Question.QuestionType.ExhibitsChooseMany;
                        case Question.QuestionType.ExhibitsChooseMany:
                            ConvertAnswersToCondensedCSV(question, builder);
                            builder.Append("\n");
                            goto default;
                        case Question.QuestionType.Spinner:
                        case Question.QuestionType.ShortAnswer:
                        default:
                            if (exportRawData)
                                ConvertAnswersToCSV(question, builder);
                            break;
                    }
                }
                builder.Append("\n");
            }

            return builder.ToString(); 
        }
        private void ConvertAnswersToCondensedCSV(Question question, StringBuilder builder)
        {
            var Counts = new Dictionary<string, int>();
            foreach (var answer in question.Answers)
            {
                if (!Counts.ContainsKey(answer.Value))
                    Counts.Add(answer.Value, 1);
                else
                    Counts[answer.Value]++;
            }
            builder.Append("Choice,Number of Selections\n");
            foreach (KeyValuePair<string, int> entry in Counts)
            {
                builder.Append("\"" + entry.Key + "\"," + entry.Value + "\n");
            }
        }

        private void ConvertAnswersToCSV(Question question, StringBuilder builder)
        {
            builder.Append("Raw Data Below\n");
            builder.Append("Date,Answer\n");
            foreach (var answer in question.Answers)
            {
                builder.Append(answer.Submission.Timestamp + ",");
                builder.Append("\"" + answer.Value + "\"\n");
            }
        }
    }
}