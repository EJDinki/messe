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
            try
            {
                AccountDbContext identityContext = new AccountDbContext();
                var manager = new UserManager<Account>(new UserStore<Account>(identityContext));
                var user1 = new Account { Id = "1", UserName = "admin" };
                IdentityResult res = manager.Create(user1, "admin101");
                identityContext.SaveChanges();
            }
            catch (Exception ex) { }

            //--------------------Themes------------------------
            Theme adultTheme = new Theme() { Id = 1, Name = "Adult", CssBundleName = "~/bundles/AdultSurvey", JsBundleName = "~/Content/AdultSurvey" };
            Theme childTheme = new Theme() { Id = 2, Name = "Child", CssBundleName = "~/bundles/ChildSurvey", JsBundleName = "~/Content/ChildSurvey" };
            context.Themes.Add(adultTheme);
            context.Themes.Add(childTheme);
            context.SaveChanges();


            //--------------------Surveys------------------------
            new List<Survey>
            {
                new Survey() 
                { 
                    Id=1,
                    Description = "This survey will only take 5 minutes to complete.",
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    ThemeId = 1,
                    Name = "Adult", 

                },
                new Survey() 
                { 
                    Id=2,
                    Description = "This survey will only take 5 minutes to complete.",
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    ThemeId = 2,
                    Name = "Child", 
                }

            }.ForEach(survey => context.Surveys.AddOrUpdate(survey));
            context.SaveChanges();

            new List<Question>
                {
                    //Adult Questions
                    new Question()
                    {
                        Id = 1,
                        IndexInSurvey = 1,
                        Text = "Where did you hear about 'The Discovery Center of the Southern Tier'? Check all that apply.",
                        Type = Question.QuestionType.MultipleChoiceChooseMany,
                        Choices = "Advertisement (Posters, Flyers);Internet;TV;Newspaper;Family or Friends",
                        SurveyID = 1
                    } ,
                    new Question()
                    {
                        Id = 2,
                        IndexInSurvey = 2,
                        Text = "What is your race/ethnicity? Check all that apply",
                        Type = Question.QuestionType.MultipleChoiceChooseMany,
                        Choices = "Black or African American;Caucasian;Hispanic/Latino;American Indian;Asian;Other",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 3,
                        IndexInSurvey = 3,
                        Text = "What is your approximate average household income",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "$0-$24,999;$25,000-$49,999;$75,000-$99,999;$100,000 and above",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 4,
                        IndexInSurvey = 4,
                        Text = "Are you a Discovery Center of the Southern Tier member?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Yes;No",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 5,
                        IndexInSurvey = 5,
                        Text = "How many times a year do you visit The Discovery Center?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "2-3 times;4-6 times;7-10 times;More than 10 times",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 6,
                        IndexInSurvey = 6,
                        Text = "What are the ages of the children with you on your visit? Please check all that apply.",
                        Type = Question.QuestionType.MultipleChoiceChooseMany,
                        Choices = "0-1;2-4;5-6;7-8;9-12;13 and older",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 7,
                        IndexInSurvey = 7,
                        Text = "What was your child's favorite exhibit at The Discovery Center( Please choose only 4)?",
                        Type = Question.QuestionType.ExhibitsChooseMany,
                        //Choices are obtained by reading the Exhibits from the database
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 8,
                        IndexInSurvey = 8,
                        Text = "Do you attend our special events?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Yes;No",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 9,
                        IndexInSurvey = 9,
                        Text = "If yes, which events have you attended?",
                        Type = Question.QuestionType.ShortAnswer,
                        Choices = "",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 10,
                        IndexInSurvey = 10,
                        Text = "Are there programs that you would like that are not currently offered?",
                        Type = Question.QuestionType.ShortAnswer,
                        Choices = "",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 11,
                        IndexInSurvey = 11,
                        Text = "How friendly was the staff?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Not Friendly;Slightly Friendly;Friendly;Very Friendly",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 12,
                        IndexInSurvey = 12,
                        Text = "How helpful was the staff?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Not Helpful;Slightly Helpful;Helpful;Very Helpful",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 13,
                        IndexInSurvey = 13,
                        Text = "Please describe your experience with the staff?",
                        Type = Question.QuestionType.ShortAnswer,
                        Choices = "",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 14,
                        IndexInSurvey = 14,
                        Text = "How well is the museum maintained?",
                        Type = Question.QuestionType.Slider,
                        Choices = "Poor;Neutral;Excellent",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 15,
                        IndexInSurvey = 15,
                        Text = "How would you rate the museum's cleanliness?",
                        Type = Question.QuestionType.Slider,
                        Choices = "Poor;Neutral;Excellent",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 16,
                        IndexInSurvey = 16,
                        Text = "How would you rate the museum's educational value?",
                        Type = Question.QuestionType.Slider,
                        Choices = "Poor;Neutral;Excellent",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 17,
                        IndexInSurvey = 17,
                        Text = "How would you rate the museum's engagement with children?",
                        Type = Question.QuestionType.Slider,
                        Choices = "Poor;Neutral;Excellent",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 18,
                        IndexInSurvey = 18,
                        Text = "Did you enjoy your visit to The Discovery Center?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Yes;No",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 19,
                        IndexInSurvey = 19,
                        Text = "Will you visit The Discovery Center again?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Yes;No",
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 20,
                        IndexInSurvey = 20,
                        Text = "Please leave any other comments about the museum?",
                        Type = Question.QuestionType.ShortAnswer,
                        Choices = "",
                        SurveyID = 1
                    },



                    //Child Questions
                    new Question()
                    {
                        Id = 21,
                        IndexInSurvey = 1,
                        Text = "How old are you?",
                        Type = Question.QuestionType.Spinner,
                        Choices = "",
                        SurveyID = 2
                    },
                    new Question()
                    {
                        Id = 22,
                        IndexInSurvey = 2,
                        Text = "What are your 3 favorite exhibits?",
                        MaxSelectedChoices = 3,
                        Type = Question.QuestionType.ExhibitsChooseMany,
                        Choices = "",
                        SurveyID = 2
                    },
                    new Question()
                    {
                        Id = 23,
                        IndexInSurvey = 3,
                        Text = "Are you a boy or a girl?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Boy;Girl",
                        SurveyID = 2
                    },
                    new Question()
                    {
                        Id = 24,
                        IndexInSurvey = 4,
                        Text = "Are you from rochester?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = "Yes;No",
                        SurveyID = 2
                    }
                    
                }.ForEach(question => context.Questions.AddOrUpdate(question));
            context.SaveChanges();

            new List<Exhibit>
            {
                new Exhibit() 
                { 
                    Id=1,
                    Name = "Fire Station"

                },
                new Exhibit() 
                { 
                    Id=2,
                    Name = "Widgets & Gadgets"

                },
                new Exhibit() 
                { 
                    Id=3,
                    Name = "Take Flight"

                },
                new Exhibit() 
                { 
                    Id=4,
                    Name = "Hospital"

                },
                new Exhibit() 
                { 
                    Id=5,
                    Name = "Dental"

                },
                new Exhibit() 
                { 
                    Id=6,
                    Name = "Power of Play"

                },
                new Exhibit() 
                { 
                    Id=7,
                    Name = "Weis Markets"

                },
                new Exhibit() 
                { 
                    Id=8,
                    Name = "Eco Kids"

                },
                new Exhibit() 
                { 
                    Id=9,
                    Name = "Music Alley"

                },
                new Exhibit() 
                { 
                    Id=10,
                    Name = "Chenango Canal Alley"

                },
                new Exhibit() 
                { 
                    Id=11,
                    Name = "Susquhanna Meets The Chesapeake"

                },
                new Exhibit() 
                { 
                    Id=12,
                    Name = "Studio 60"

                },
                new Exhibit() 
                { 
                    Id=13,
                    Name = "Action News"

                },
                new Exhibit() 
                { 
                    Id=14,
                    Name = "You've Got Game"

                }
            }.ForEach(exhibit => context.Exhibits.AddOrUpdate(exhibit));
            context.SaveChanges();
        }
    }
}
