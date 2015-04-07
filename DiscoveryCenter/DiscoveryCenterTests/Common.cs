using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DiscoveryCenter.Models;
using DiscoveryCenterTests.ObjectRepository;
using ArtOfTest.WebAii.TestTemplates;

namespace DiscoveryCenterTests
{
    public class Common
    {
        //public static readonly string BaseUrl = "http://museumsurvey.somee.com";
        public static readonly string BaseUrl = "http://localhost:19509";
        /// <summary>
        /// Adds a survey with 1 of every question type to the database.
        /// The suvey has a Guid as its name for uniqueness
        /// </summary>
        /// <returns>The survey that was added</returns>
        public static Survey AddSurveyToDB()
        {
            Survey theSurvey = null;
            Choice choice1 = new Choice();
            choice1.Text = "m1Choice";
            Choice choice2 = new Choice();
            choice2.Text = "m1Choice2";

            Choice choice3 = new Choice();
            choice3.Text = "mMChoice";
            Choice choice4 = new Choice();
            choice4.Text = "mMChoice2";

            Choice choice5 = new Choice();
            choice5.Text = "sChoice";
            Choice choice6 = new Choice();
            choice6.Text = "sChoice2";
            Choice choice7 = new Choice();
            choice7.Text = "sChoice3";



            using (SurveyContext dbContext = new SurveyContext())
            {
                theSurvey = new Survey()
                {
                    Name = Guid.NewGuid().ToString(),
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Description = "The survey to use in functional unit testing"
                };

                Question sAnswer = new Question()
                {
                    Text = "This is a short answer.",
                    Type = Question.QuestionType.ShortAnswer,
                    IndexInSurvey = 1,
                    ParentSurvey = theSurvey
                };

                Question mChoose1 = new Question()
                {
                    Text = "This is a multiple choice choose one.",
                    Type = Question.QuestionType.MultipleChoiceChooseOne,
                    IndexInSurvey = 2,
                    ParentSurvey = theSurvey,
                    Choices = new List<Choice>() {choice1,choice2}
                };

                Question mChooseM = new Question()
                {
                    Text = "This is a multiple choice choose many.",
                    Type = Question.QuestionType.MultipleChoiceChooseMany,
                    IndexInSurvey = 3,
                    ParentSurvey = theSurvey,
                    Choices = new List<Choice>() { choice3, choice4 }
                };

                Question sSlider = new Question()
                {
                    Text = "This is a slider.",
                    Type = Question.QuestionType.Slider,
                    IndexInSurvey = 4,
                    ParentSurvey = theSurvey,
                    Choices = new List<Choice>() { choice5, choice6, choice7 }
                };

                Question sExhibit = new Question()
                {
                    Text = "This is a Select Exhibit question.",
                    Type = Question.QuestionType.ExhibitsChooseMany,
                    IndexInSurvey = 5,
                    ParentSurvey = theSurvey,
                    MaxSelectedChoices = 4
                };

                theSurvey.Questions = new List<Question>();
                theSurvey.Questions.Add(sAnswer);
                theSurvey.Questions.Add(mChoose1);
                theSurvey.Questions.Add(mChooseM);
                theSurvey.Questions.Add(sSlider);
                theSurvey.Questions.Add(sExhibit);

                dbContext.Surveys.Add(theSurvey);
                dbContext.SaveChanges();
            }
            return theSurvey;
        }


        /// <summary>
        /// Each test that interacts with a survey will add its own survey to not be dependant on other tests.
        /// This leads to a bloated db. Truncate the tables at the end of tests when needed.
        /// </summary>
        public static void TruncateDbTables()
        {
            using(SurveyContext dbContext = new SurveyContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {

                        dbContext.Database.ExecuteSqlCommand("DELETE FROM [Answers]");
                        dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Answers, RESEED, 1)");

                        dbContext.Database.ExecuteSqlCommand("DELETE FROM [Submissions]");
                        dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Submissions, RESEED, 1)");

                        dbContext.Database.ExecuteSqlCommand("DELETE FROM [Questions]");
                        dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Questions, RESEED, 1)");

                        dbContext.Database.ExecuteSqlCommand("DELETE FROM [Surveys]");
                        dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Surveys, RESEED, 1)");
                        
                        dbContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        Console.Error.WriteLine("Table truncation failed with error: " + ex.Message);
                        transaction.Rollback();
                    }
                }          
            }
        }

        public static void LogIn(BaseTest test, string username = "admin" , string password ="admin101" , bool rememberMe = false)
        {
            LoginPage lPage = new LoginPage(test);

            lPage.Username.Text = username;
            lPage.Password.Text = password;

            if (rememberMe)
                lPage.RememberMe.Check(true, true);

            lPage.LogIn.Click();
        }
    }
}
