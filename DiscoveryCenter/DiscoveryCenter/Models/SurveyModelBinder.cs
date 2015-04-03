using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscoveryCenter.Models
{
    public class SurveyModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            if (bindingContext.ModelType == typeof(SurveyViewModel))
            {
                SurveyViewModel svm = BindSVM(request);
                
                base.BindModel(controllerContext, bindingContext);
                return svm;
            }
            else if (bindingContext.ModelType == typeof(CreationEditViewModel))
            {

                CreationEditViewModel cevm = BindCreationEditViewModel(request);
               
                base.BindModel(controllerContext, bindingContext);
                return cevm;
            }
            else
            {
                return base.BindModel(controllerContext, bindingContext);
            }
        }

        private CreationEditViewModel BindCreationEditViewModel(HttpRequestBase request)
        {
            int sId = Convert.ToInt32(request.Form.Get("Survey.Id"));
            string sName = request.Form.Get("Survey.Name");
            string sDesc = request.Form.Get("Survey.Description");
            int themeId = Convert.ToInt32(request.Form.Get("Survey.ThemeId"));
            List<Question> questions = new List<Question>();

            Question question = null;
            Choice currentChoice = null;
            List<Choice> choices = new List<Choice>();

            foreach (string key in request.Form.Keys)
            {

                if (key.Contains("].Id"))
                {
                    if(question != null)
                    {
                        //if (choices.Any() && String.IsNullOrWhiteSpace(question.Choices))
                        //    question.Choices = String.Join(";", choices);
                        question.Choices = choices;
                        questions.Add(question);
                    }
                    question = new Question();
                    choices = new List<Choice>();
                    question.Id = Convert.ToInt32(request.Form.Get(key));
                }
                else if (key.Contains(".Text"))
                {
                    if(question == null || question.Text != null)
                    {
                        if (question != null)
                        {
                            //if (choices.Any() && String.IsNullOrWhiteSpace(question.Choices))
                            //    question.Choices = String.Join(";", choices);

                            questions.Add(question);
                        }
                        question = new Question();
                        choices = new List<Choice>();
                    }
                    question.Text = request.Form.Get(key);
                }
                else if (key.Contains(".Type"))
                {
                    question.Type = (Question.QuestionType)Enum.Parse(typeof(Question.QuestionType), request.Form.Get(key));
                }
                else if (key.Contains("IndexInSurvey"))
                {
                    question.IndexInSurvey = Convert.ToInt32(request.Form.Get(key));
                }
                else if (key.Contains("MaxSelectedChoices"))
                {
                    question.MaxSelectedChoices = Convert.ToInt32(request.Form.Get(key));
                }
                else if(key.Contains(".Choice"))
                {
                    currentChoice = new Choice(){Text=request.Form.Get(key), IsSelected=false, ParentQuestion=question};
                    
                    
                }
                else if (key.Contains(".Img"))
                {
                    currentChoice.ImageName = request.Form.Get(key);
                    choices.Add(currentChoice);
                }
                else if (key.Contains(".Choices"))
                {
                    Console.WriteLine(request.Form.Get(key));
                    //question.Choices = request.Form.Get(key);
                } 
            }

            if (question != null && question.Text != null)
            {
                //if (choices.Any() && String.IsNullOrWhiteSpace(question.Choices))
                //    question.Choices = String.Join(";", choices);
                question.Choices = choices;
                questions.Add(question);
            }

            Survey survey = new Survey()
            {
                Name = sName,
                Id = sId,
                Description = sDesc,
                Questions = questions,
                CreateDate = DateTime.Now,
                ThemeId = themeId
            };

            return new CreationEditViewModel() { Survey = survey, Themes = null };
        }

        private SurveyViewModel BindSVM(HttpRequestBase request)
        {
            List<QuestionViewModel> models = new List<QuestionViewModel>();

            int sId = Convert.ToInt32(request.Form.Get("SurveyId"));
            string sDesc = "";
            string question = "";
            int questionId = -1;
            string ans = "";
            bool isSelected = false;
            string selectionText = "";
            Question.QuestionType type;
            QuestionViewModel model;
            List<Choice> qChoices = new List<Choice>();

            foreach (string key in request.Form.Keys)
            {

                if (key.Contains(".QuestionId"))
                {
                    questionId = Convert.ToInt32(request.Form.Get(key));
                }
                else if (key.Contains(".Question"))
                {
                    question = request.Form.Get(key);
                }
                else if (key.Contains(".Answer"))
                {
                    ans = request.Form.Get(key);
                }
                else if (key.Contains(".text"))
                {
                    selectionText = request.Form.Get(key);
                }
                else if (key.Contains(".IsSelected"))
                {
                    isSelected = Boolean.Parse(request.Form.Get(key).Split(',')[0]);
                    qChoices.Add(new Choice() { Text = selectionText, IsSelected = isSelected });
                }
                else if (key.Contains(".Type"))
                {
                    type = (Question.QuestionType)Enum.Parse(typeof(Question.QuestionType), request.Form.Get(key));
                    switch (type)
                    {
                        case (Question.QuestionType.MultipleChoiceChooseMany):
                            model = new MultipleSelectViewModel();
                            model.Question = question;
                            model.QuestionId = questionId;
                            ((MultipleChoiceViewModel)model).Choices = qChoices;
                            model.Type = type;
                            model.Answer = "";
                            models.Add(model);
                            break;
                        default:
                            model = new QuestionViewModel();
                            model.Question = question;
                            model.QuestionId = questionId;
                            model.Type = type;
                            model.Answer = ans;
                            models.Add(model);
                            ans = "";
                            break;
                    }
                    qChoices = new List<Choice>();
                }
            }

            return new SurveyViewModel
            {
                SurveyId = sId,
                SurveyDescription = sDesc,
                QuestionModels = models
            };
        }

    }
}