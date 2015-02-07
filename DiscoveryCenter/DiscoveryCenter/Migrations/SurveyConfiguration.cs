namespace DiscoveryCenter.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DiscoveryCenter.Models;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class SurveyConfiguration : DbMigrationsConfiguration<DiscoveryCenter.Models.SurveyContext>
    {
        public SurveyConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DiscoveryCenter.Models.SurveyContext context)
        {
            //Use this to debug this method
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            //accounts
            AccountDbContext identityContext = new AccountDbContext();
            var manager = new UserManager<Account>(new UserStore<Account>(identityContext));
            var user1 = new Account { Id = "1", UserName = "admin"};
            IdentityResult res = manager.Create(user1, "admin101");
            identityContext.SaveChanges();

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
                        Id = 1,
                        Text = "Where did you hear about 'The Discovery Center of the Southern Tier'? Check all that apply.",
                        Type = Question.QuestionType.MultipleChoiceChooseMany,
                        Choices = "Advertisement (Posters, Flyers);Internet;TV;Newspaper;Family or Friends",
                        SurveyID = 1
                    } ,
                    new Question()
                    {
                        Id = 2,
                        Text = "What is your race/ethnicity? Check all that apply",
                        Type = Question.QuestionType.MultipleChoiceChooseMany,
                        Choices = "Black or African American;Caucasian;Hispanic/Latino;American Indian;Asian;Other",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 3,
                        Text = "What is your approximate average household income",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "$0-$24,999;$25,000-$49,999;$75,000-$99,999;$100,000 and above",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 4,
                        Text = "Are you a Discovery Center of the Southern Tier member?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Yes;No",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 5,
                        Text = "How many times a year do you visit The Discovery Center?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "2-3 times;4-6 times;7-10 times;More than 10 times",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 6,
                        Text = "What are the ages of the children with you on your visit? Please check all that apply.",
                        Type = Question.QuestionType.MultipleChoiceChooseMany,
                        Choices = "0-1;2-4;5-6;7-8;9-12;13 and older",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 7,
                        Text = "What was your child's favorite exhibit at The Discovery Center( Please choose only 4)?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "**list exhibits!!**",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 8,
                        Text = "Do you attend our special events?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Yes;NO",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 9,
                        Text = "If yes, which events have you attended?",
                        Type = Question.QuestionType.ShortAnswer,
                        Choices = "",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 10,
                        Text = "Are there programs that you would like that are not currently offered?",
                        Type = Question.QuestionType.ShortAnswer,
                        Choices = "",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 11,
                        Text = "How friendly was the staff?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Not Friendly;Slightly Friendly;Friendly;Very Friendly",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 12,
                        Text = "How helpful was the staff?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Not Helpful;Slightly Helpful;Helpful;Very Helpful",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 13,
                        Text = "Please describe your experience with the staff?",
                        Type = Question.QuestionType.ShortAnswer,
                        Choices = "",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 14,
                        Text = "How well is the museum maintained?",
                        Type = Question.QuestionType.Slider,
                        Choices = "Poor;Excellent",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 15,
                        Text = "How would you rate the museum's cleanliness?",
                        Type = Question.QuestionType.Slider,
                        Choices = "Poor;Excellent",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 16,
                        Text = "How would you rate the museum's educational value?",
                        Type = Question.QuestionType.Slider,
                        Choices = "Poor;Excellent",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 17,
                        Text = "How would you rate the museum's engagement with children?",
                        Type = Question.QuestionType.Slider,
                        Choices = "Poor;Excellent",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 18,
                        Text = "Did you enjoy your visit to The Discovery Center?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Yes;No",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 19,
                        Text = "Will you visit The Discovery Center again?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Yes;No",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 20,
                        Text = "Please leave any other comments about the museum?",
                        Type = Question.QuestionType.ShortAnswer,
                        Choices = "",
                        SurveyID = 1
                    }
                }.ForEach(question => context.Questions.AddOrUpdate(question));
            context.SaveChanges();
        }
    }
}
