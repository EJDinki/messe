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
            catch (Exception) { }

            //--------------------Themes------------------------
            Theme adultTheme = new Theme() 
            { 
                Id = 1, 
                Name = ThemeName.Adult, 
                CssFileName = "AdultSurvey.css", 
                JsFileName = "AdultSurvey.js",
            };
            Theme childTheme = new Theme() 
            { 
                Id = 2,
                Name = ThemeName.Child,
                CssFileName = "ChildSurvey.css",
                JsFileName = "ChildSurvey.js",
            };
            context.Themes.AddOrUpdate(adultTheme);
            context.Themes.AddOrUpdate(childTheme);
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
                        Choices = new List<Choice>(){
                            new Choice(){Text="Advertisement (Posters, Flyers)", QuestionID=1},
                            new Choice(){Text="Internet", QuestionID=1},
                            new Choice(){Text="TV", QuestionID=1},
                            new Choice(){Text="Newspaper", QuestionID=1},
                            new Choice(){Text="Family or Friends", QuestionID=1}
                        },
                        SurveyID = 1
                    } ,

                    new Question()
                    {
                        Id = 2,
                        IndexInSurvey = 2,
                        Text = "What is your approximate average household income",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = new List<Choice>(){
                            new Choice(){Text="$0-$24,999", QuestionID=2},
                            new Choice(){Text="$25,000-$49,999", QuestionID=2},
                            new Choice(){Text="75,000-$99,999", QuestionID=2},
                            new Choice(){Text="$100,000 and above", QuestionID=2}
                        },
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 3,
                        IndexInSurvey = 3,
                        Text = "What was your child's favorite exhibit at The Discovery Center( Please choose only 4)?",
                        Type = Question.QuestionType.ExhibitsChooseMany,
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 4,
                        IndexInSurvey = 4,
                        Text = "If yes, which events have you attended?",
                        Type = Question.QuestionType.ShortAnswer,
                        SurveyID = 1
                    },
                    new Question()
                    {
                        Id = 5,
                        IndexInSurvey = 5,
                        Text = "How well is the museum maintained?",
                        Type = Question.QuestionType.Slider,
                        Choices = new List<Choice>(){
                            new Choice(){Text="Poor", QuestionID=5},
                            new Choice(){Text="Nuetral", QuestionID=5},
                            new Choice(){Text="Excellent", QuestionID=5}
                        },
                        SurveyID = 1
                    },

                    //Child Questions
                    new Question()
                    {
                        Id = 6,
                        IndexInSurvey = 1,
                        Text = "How old are you?",
                        Type = Question.QuestionType.Spinner,
                        SurveyID = 2
                    },
                    new Question()
                    {
                        Id = 7,
                        IndexInSurvey = 2,
                        Text = "What are your 3 favorite exhibits?",
                        MaxSelectedChoices = 3,
                        Type = Question.QuestionType.ExhibitsChooseMany,
                        SurveyID = 2
                    },
                    new Question()
                    {
                        Id = 8,
                        IndexInSurvey = 3,
                        Text = "Are you a boy or a girl?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = new List<Choice>(){
                            new Choice(){Text="Boy", QuestionID=8},
                            new Choice(){Text="Girl", QuestionID=8}
                        },
                        SurveyID = 2
                    },
                    new Question()
                    {
                        Id = 9,
                        IndexInSurvey = 4,
                        Text = "Are you from rochester?",
                        Type = Question.QuestionType.MultipleChoiceChooseOne,
                        Choices = new List<Choice>(){
                            new Choice(){Text="Yes", QuestionID=9},
                            new Choice(){Text="No", QuestionID=9}
                        },
                        SurveyID = 2
                    }
                    
                }.ForEach(question => context.Questions.AddOrUpdate(question));
            context.SaveChanges();

            new List<Exhibit>
            {
                new Exhibit() 
                { 
                    Id=1,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Fire Station"

                },
                new Exhibit() 
                { 
                    Id=2,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Widgets & Gadgets"

                },
                new Exhibit() 
                { 
                    Id=3,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Take Flight"

                },
                new Exhibit() 
                { 
                    Id=4,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Hospital"

                },
                new Exhibit() 
                { 
                    Id=5,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Dental"

                },
                new Exhibit() 
                { 
                    Id=6,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Power of Play"

                },
                new Exhibit() 
                { 
                    Id=7,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Weis Markets"

                },
                new Exhibit() 
                { 
                    Id=8,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Eco Kids"

                },
                new Exhibit() 
                { 
                    Id=9,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Music Alley"

                },
                new Exhibit() 
                { 
                    Id=10,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Chenango Canal Alley"

                },
                new Exhibit() 
                { 
                    Id=11,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Susquhanna Meets The Chesapeake"

                },
                new Exhibit() 
                { 
                    Id=12,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Studio 60"

                },
                new Exhibit() 
                { 
                    Id=13,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "Action News"

                },
                new Exhibit() 
                { 
                    Id=14,
                    CreateDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    Name = "You've Got Game"

                }
            }.ForEach(exhibit => context.Exhibits.AddOrUpdate(exhibit));
            context.SaveChanges();
        }
    }
}
