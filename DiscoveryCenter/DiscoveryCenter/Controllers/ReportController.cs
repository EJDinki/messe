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
                    report = new ReportViewModel();
                    report.Id = question.Id;
                    report.Text = question.Text;

                    reports.Reports.Add(report);
                }
            }

            return View(reports);
        }

        public ActionResult Details(int id)//question id
        {
            ReportViewModel report = new ReportViewModel();
            using(SurveyContext db = new SurveyContext())
            {
                Question question = db.Questions.Find(id);
                
                report = new ReportViewModel();
                report.Id = question.Id;
                report.Type = question.Type;
                report.QuestionIndex = question.IndexInSurvey;
                report.Text = question.Text;

                if (report.Type == Question.QuestionType.ShortAnswer)
                {
                    report.ShortAnswers = new List<String>();
                    question.Answers.ForEach(a => report.ShortAnswers.Add(a.Value));
                    return View("ShortAnswerReport", report);
                }
                else
                {
                    foreach (var answer in question.Answers)
                    {
                        if (!report.Counts.ContainsKey(answer.Value))
                            report.Counts.Add(answer.Value, 1);
                        else
                            report.Counts[answer.Value]++;
                    }
                    var entries = report.Counts.Select(d =>
                    string.Format("[\"{0}\", {1}]", d.Key, string.Join(",", d.Value)));
                    report.ChartJSON = "[" + string.Join(",", entries) + "]";
                    return View("ChartReport", report);
                }
            }
        }

        public ActionResult ExportToCSV(int id = 0, bool exportRawData = true, string dateRadio = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            // Strings for radio
            string date = "date";
            string noDate = "noDate";

            if (id <= 0)
            {
                return new HttpNotFoundResult();
            }

            if (dateRadio != date && dateRadio != noDate)
            {
                return new HttpNotFoundResult();
            }

            using (SurveyContext db = new SurveyContext())
            {
                string csv;
                Survey survey = db.Surveys.Find(id);
                if (survey == null)
                    return HttpNotFound();

                if (dateRadio == date)
                {
                    if (startDate == null || endDate == null)
                    {
                        return new HttpNotFoundResult();
                    }
                    else
                    {
                        // Export using dates
                        // Date will default to DateTime at 12AM of that day (since user only enters a date)
                        // To make the end date inclusive, we will change it from 01/01/1111 12:00am to 01/01/1111 11:59pm
                        DateTime modifiedEnd = endDate.Value;
                        modifiedEnd = modifiedEnd.AddDays(1).AddSeconds(-1);
                        csv = ConvertSurveyToCSV(survey, exportRawData, startDate.Value, modifiedEnd);
                    }
                }
                else
                {
                    // Export without dates
                    csv = ConvertSurveyToCSV(survey, exportRawData);
                }

                string fileName = survey.Name + "-" + DateTime.Today.ToString("MM/dd/yyyy") + ".csv";
                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", fileName);
            }
        }

        /*
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
        */

        private string ConvertSurveyToCSV(Survey survey, bool exportRawData, DateTime? startDate = null, DateTime? endDate = null)
        {
            bool useDates = (startDate != null && endDate != null);
            StringBuilder builder = new StringBuilder();
            builder.Append("\"Export for Survey: " + survey.Name + "\"\n");
            builder.Append("Date: " + DateTime.Today.ToString("MM/dd/yyyy") + "\n");

            if (useDates)
                builder.Append("Only exporting between Start Date: " + startDate + " and End Date: " + endDate + "\n");

            builder.Append("Number of Questions: " + survey.Questions.Count + "\n");

            if (useDates)
                builder.Append("Number of Submissions: " + CalcNumSubmissions(survey, startDate.Value, endDate.Value) + "\n");
            else
                builder.Append("Number of Submissions: " + survey.Submissions.Count + "\n");

            builder.Append("\n");

            foreach (var question in survey.Questions.OrderBy(q => q.IndexInSurvey))
            {
                // TODO handle exhibits better or make exhibits save name instead of ID
                builder.Append("------------------------------\n");
                builder.Append("Question Number: " + question.IndexInSurvey + "\n");
                builder.Append("Question Type: " + question.Type + "\n");
                builder.Append("\"Question Text: " + question.Text + "\"\n");
                if (question.Answers.Count == 0)
                {
                    builder.Append("There are no submitted answers for this question\n");
                }
                else
                {
                    switch (question.Type)
                    {
                        case Question.QuestionType.MultipleChoiceChooseMany:
                        case Question.QuestionType.MultipleChoiceChooseOne:
                        case Question.QuestionType.Slider:
                            ConvertChoicesToCSV(question, builder);
                            goto case Question.QuestionType.ExhibitsChooseMany;
                        case Question.QuestionType.ExhibitsChooseMany:
                            builder.Append("\n");
                            builder.Append("Summarized Data Below\n");
                            if (useDates)
                                ConvertAnswersToCondensedCSV(question, builder, startDate.Value, endDate.Value);
                            else
                                ConvertAnswersToCondensedCSV(question, builder);
                            goto default;
                        case Question.QuestionType.Spinner:
                        case Question.QuestionType.ShortAnswer:
                        default:
                            if (exportRawData)
                                builder.Append("\n");
                                builder.Append("Raw Data Below\n");
                                if (useDates)
                                    ConvertAnswersToCSV(question, builder, startDate.Value, endDate.Value);
                                else
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

        private void ConvertAnswersToCondensedCSV(Question question, StringBuilder builder, DateTime startDate, DateTime endDate)
        {
            var Counts = new Dictionary<string, int>();
            foreach (var answer in question.Answers)
            {
                if (answer.Submission.Timestamp >= startDate && answer.Submission.Timestamp <= endDate)
                {
                    if (!Counts.ContainsKey(answer.Value))
                        Counts.Add(answer.Value, 1);
                    else
                        Counts[answer.Value]++;
                }
            }
            builder.Append("Choice,Number of Selections\n");
            foreach (KeyValuePair<string, int> entry in Counts)
            {
                builder.Append("\"" + entry.Key + "\"," + entry.Value + "\n");
            }
        }

        private void ConvertAnswersToCSV(Question question, StringBuilder builder)
        {
            builder.Append("Date,Answer\n");
            foreach (var answer in question.Answers)
            {
                builder.Append(answer.Submission.Timestamp + ",");
                builder.Append("\"" + answer.Value + "\"\n");
            }
        }
        private void ConvertAnswersToCSV(Question question, StringBuilder builder, DateTime startDate, DateTime endDate)
        {
            builder.Append("Date,Answer\n");
            foreach (var answer in question.Answers)
            {
                if (answer.Submission.Timestamp >= startDate && answer.Submission.Timestamp <= endDate)
                {
                    builder.Append(answer.Submission.Timestamp + ",");
                    builder.Append("\"" + answer.Value + "\"\n");
                }
            }
        }
        private void ConvertChoicesToCSV(Question question, StringBuilder builder)
        {
            String choices = "";
            foreach (var choice in question.Choices)
            {
                if (choices != "") choices = choices + ",";
                choices = choices + "\"" + choice.Text + "\"" ;
            }
            builder.Append("Question Choices (At Time of Export):," + choices + "\n");
        }

        private int CalcNumSubmissions(Survey survey, DateTime startDate, DateTime endDate)
        {
            int count = 0;
            foreach (var sub in survey.Submissions)
            {
                if (sub.Timestamp >= startDate && sub.Timestamp <= endDate)
                    count++;
            }
            return count;
        }
    }
}