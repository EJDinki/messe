namespace DiscoveryCenter.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DiscoveryCenter.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<DiscoveryCenter.Models.SurveyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DiscoveryCenter.Models.SurveyContext context)
        {
            //  This method will be called after migrating to the latest version.
            Survey survey = new Survey();
            Question question = new Question();
            Question question2 = new Question();
            Answer answer = new Answer();
            Answer answer2 = new Answer();
            Submission submission = new Submission();

            question.Text = "Do you like this application?";
            question.Choices = "Yes;No";
            question.Type = Question.QuestionType.MultipleChoice;

            question2.Text = "Tell me about your life.";
            question2.Choices = "";
            question2.Type = Question.QuestionType.ShortAnswer;

            answer.Value = "Yes";
            answer.Question = question;
            answer.QuestionId = question.Id;

            answer2.Value = "There is nothing to tell";
            answer2.Question = question2;
            answer2.QuestionId = question2.Id;

            question.Answers = new System.Collections.Generic.List<Answer>();
            question2.Answers = new System.Collections.Generic.List<Answer>();
            question.Answers.Add(answer);
            question2.Answers.Add(answer2);

            survey.CreateDate = DateTime.Now;
            survey.Name = "Test Survey";
            survey.Questions = new System.Collections.Generic.List<Question>();
            survey.Questions.Add(question);
            survey.Questions.Add(question2);

            submission.Answers = new System.Collections.Generic.List<Answer>();
            submission.Answers.Add(answer);
            submission.Answers.Add(answer2);
            submission.ParentSurvey = survey;
            submission.Timestamp = DateTime.Now;



            context.Surveys.AddOrUpdate(survey);
            context.Questions.AddOrUpdate(question);
            context.Questions.AddOrUpdate(question2);
            context.Answers.AddOrUpdate(answer);
            context.Answers.AddOrUpdate(answer2);
            context.Submissions.AddOrUpdate(submission);

            context.SaveChanges();

        }
    }
}
