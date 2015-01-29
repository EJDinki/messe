namespace DiscoveryCenter.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DiscoveryCenter.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<DiscoveryCenter.Models.SurveyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DiscoveryCenter.Models.SurveyContext context)
        {
            ////  This method will be called after migrating to the latest version.
            //Survey survey = new Survey();
            //Question question = new Question();
            //Question question2 = new Question();
            //Answer answer = new Answer();
            //Answer answer2 = new Answer();
            //Submission submission = new Submission();

            //question.Text = "Do you like this application?";
            //question.Choices = "Yes;No";
            //question.Type = Question.QuestionType.MultipleChoiceChooseOne;

            //question2.Text = "Tell me about your life.";
            //question2.Choices = "";
            //question2.Type = Question.QuestionType.ShortAnswer;

            //answer.Value = "Yes";
            //answer.Question = question;
            //answer.QuestionId = question.Id;

            //answer2.Value = "There is nothing to tell";
            //answer2.Question = question2;
            //answer2.QuestionId = question2.Id;

            //question.Answers = new System.Collections.Generic.List<Answer>();
            //question2.Answers = new System.Collections.Generic.List<Answer>();
            //question.Answers.Add(answer);
            //question2.Answers.Add(answer2);

            //survey.CreateDate = DateTime.Now;
            //survey.Name = "Test Survey";
            //survey.Questions = new System.Collections.Generic.List<Question>();
            //survey.Questions.Add(question);
            //survey.Questions.Add(question2);

            //submission.Answers = new System.Collections.Generic.List<Answer>();
            //submission.Answers.Add(answer);
            //submission.Answers.Add(answer2);
            //submission.ParentSurvey = survey;
            //submission.Timestamp = DateTime.Now;



            //context.Surveys.AddOrUpdate(survey);
            //context.Questions.AddOrUpdate(question);
            //context.Questions.AddOrUpdate(question2);
            //context.Answers.AddOrUpdate(answer);
            //context.Answers.AddOrUpdate(answer2);
            //context.Submissions.AddOrUpdate(submission);

            //context.SaveChanges();

            new List<Survey>
            {
                new Survey() 
                { 
                    Id=1,
                    CreateDate = DateTime.Now,
                    Name = "Adult", 
                    
                }
            }.ForEach(survey => context.Surveys.Add(survey));
            context.SaveChanges();


            new List<Question>
            {
                new Question()
                {
                    Text = "Where did you hear about 'The Discovery Center of the Southern Tier'? Check all that apply.",
                    Type = Question.QuestionType.MultipleChoiceChooseMany,
                    Choices = "Advertisement (Posters, Flyers);Internet;TV;Newspaper;Family or Friends",
                    SurveyID = 1
                } ,
                new Question()
                {
                    Text = "What is your race/ethnicity? Check all that apply",
                    Type = Question.QuestionType.MultipleChoiceChooseMany,
                    Choices = "Black or African American;Caucasian;Hispanic/Latino;American Indian;Asian;Other",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "What is your approximate average household income",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    Choices = "$0-$24,999;$25,000-$49,999;$75,000-$99,999;$100,000 and above",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "Are you a Discovery Center of the Southern Tier member?",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    Choices = "Yes;No",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "How many times a year do you visit The Discovery Center?",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    Choices = "2-3 times;4-6 times;7-10 times;More than 10 times",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "What are the ages of the children with you on your visit? Please check all that apply.",
                    Type = Question.QuestionType.MultipleChoiceChooseMany,
                    Choices = "0-1;2-4;5-6;7-8;9-12;13 and older",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "What was your child's favorite exhibit at The Discovery Center( Please choose only 4)?",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    Choices = "**list exhibits!!**",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "Do you attend our special events?",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    Choices = "Yes;NO",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "If yes, which events have you attended?",
                    Type = Question.QuestionType.ShortAnswer,
                    Choices = "",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "Are there programs that you would like that are not currently offered?",
                    Type = Question.QuestionType.ShortAnswer,
                    Choices = "",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "How friendly was the staff?",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    Choices = "Not Friendly;Slightly Friendly;Friendly;Very Friendly",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "How helpful was the staff?",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    Choices = "Not Helpful;Slightly Helpful;Helpful;Very Helpful",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "Please describe your experience with the staff?",
                    Type = Question.QuestionType.ShortAnswer,
                    Choices = "",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "How well is the museum maintained?",
                    Type = Question.QuestionType.Slider,
                    Choices = "Poor;Excellent",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "How would you rate the museum's cleanliness?",
                    Type = Question.QuestionType.Slider,
                    Choices = "Poor;Excellent",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "How would you rate the museum's educational value?",
                    Type = Question.QuestionType.Slider,
                    Choices = "Poor;Excellent",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "How would you rate the museum's engagement with children?",
                    Type = Question.QuestionType.Slider,
                    Choices = "Poor;Excellent",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "Did you enjoy your visit to The Discovery Center?",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    Choices = "Yes;No",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "Will you visit The Discovery Center again?",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    Choices = "Yes;No",
                    SurveyID = 1
                },
                new Question()
                {
                    Text = "Please leave any other comments about the museum?",
                    Type = Question.QuestionType.ShortAnswer,
                    Choices = "",
                    SurveyID = 1
                }
            }.ForEach(question => context.Questions.Add(question));
            context.SaveChanges();
        }
    }
}
