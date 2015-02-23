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
        public static readonly string BaseUrl = "http://museumsurvey.somee.com";

        /// <summary>
        /// Adds a survey with 1 of every question type to the database.
        /// The suvey has a Guid as its name for uniqueness
        /// </summary>
        /// <returns>The survey that was added</returns>
        public static Survey AddSurveyToDB()
        {
            Survey theSurvey = null;
            using (SurveyContext dbContext = new SurveyContext())
            {
                theSurvey = new Survey()
                {
                    Name = Guid.NewGuid().ToString(),
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
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
                    Choices = "m1Choice;m1Choice2"
                };

                Question mChooseM = new Question()
                {
                    Text = "This is a multiple choice choose many.",
                    Type = Question.QuestionType.MultipleChoiceChooseMany,
                    IndexInSurvey = 3,
                    ParentSurvey = theSurvey,
                    Choices = "mMChoice;mMChoice2"
                };

                Question sSlider = new Question()
                {
                    Text = "This is a slider.",
                    Type = Question.QuestionType.Slider,
                    IndexInSurvey = 4,
                    ParentSurvey = theSurvey,
                    Choices = "sChoice;sChoice2;sChoice3"
                };

                theSurvey.Questions = new List<Question>();
                theSurvey.Questions.Add(sAnswer);
                theSurvey.Questions.Add(mChoose1);
                theSurvey.Questions.Add(mChooseM);
                theSurvey.Questions.Add(sSlider);

                dbContext.Surveys.Add(theSurvey);
                dbContext.SaveChanges();
            }
            return theSurvey;
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
