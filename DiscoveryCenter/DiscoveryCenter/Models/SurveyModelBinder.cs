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
                return BindSVM(request);

            }
            else if (bindingContext.ModelType == typeof(Survey))
            {
                return BindSurvey(request);
            }
            else
            {
                return base.BindModel(controllerContext, bindingContext);
            }
        }

        private Survey BindSurvey(HttpRequestBase request)
        {
            int sId = Convert.ToInt32(request.Form.Get("Id"));
            string sName = request.Form.Get("Name");
            List<Question> questions = new List<Question>();

            //string question = "";
            //int questionId = -1;
            //string choices = "";
            //int indexInSurvey = 0;

            Question question = null;
            List<string> choices = new List<string>();

            Question.QuestionType type = Question.QuestionType.ShortAnswer;

            foreach (string key in request.Form.Keys)
            {

                if (key.Contains(".Id"))
                {
                    if(question != null)
                    {
                        if (choices.Any() && question.Choices == null)
                            question.Choices = String.Join(";", choices);
                        questions.Add(question);
                    }
                    question = new Question();
                    choices = new List<string>();
                    question.Id = Convert.ToInt32(request.Form.Get(key));
                }
                else if (key.Contains(".Text"))
                {
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
                else if(key.Contains(".Choice"))
                {
                    choices.Add(request.Form.Get(key));
                }
                else if (key.Contains(".Choices"))
                {
                    question.Choices = request.Form.Get(key);
                }
                
            }

            return new Survey()
            {
                Name = sName,
                Id = sId,
                Questions = questions,
                CreateDate = DateTime.Now
            };
        }

        private SurveyViewModel BindSVM(HttpRequestBase request)
        {
            List<QuestionViewModel> models = new List<QuestionViewModel>();

            int sId = Convert.ToInt32(request.Form.Get("SurveyId"));
            string question = "";
            int questionId = -1;
            string ans = "";
            List<Selection> options = new List<Selection>();
            bool isSelected = false;
            string selectionText = "";
            Question.QuestionType type;
            QuestionViewModel model;

            foreach (string key in request.Form.Keys)
            {

                if (key.Contains(".QuestionId"))
                {
                    questionId = Convert.ToInt32(request.Form.Get(key));
                }
                else if (key.Contains(".Question"))
                {
                    options = new List<Selection>();
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
                    Selection select = new Selection();
                    select.text = selectionText;
                    select.IsSelected = isSelected;
                    options.Add(select);
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
                            model.Type = type;
                            model.Answer = "";
                            ((MultipleSelectViewModel)model).Options = options;
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
                }
            }

            return new SurveyViewModel
            {
                SurveyId = sId,
                QuestionModels = models
            };
        }

    }
}