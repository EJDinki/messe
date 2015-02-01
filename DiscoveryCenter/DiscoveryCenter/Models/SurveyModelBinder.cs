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
            if (bindingContext.ModelType == typeof(SurveyViewModel))
            {
                HttpRequestBase request = controllerContext.HttpContext.Request;

                List<ViewModel> models = new List<ViewModel>();

                int sId = Convert.ToInt32(request.Form.Get("SurveyId"));
                string question="";
                int questionId = -1;
                string ans = "";
                List<Selection> options = new List<Selection>();
                bool isSelected = false;
                string selectionText = "";
                Question.QuestionType type;
                ViewModel model;

                foreach (string key in request.Form.Keys)
                {
                    if(key.Contains(".QuestionId"))
                    {
                        questionId = Convert.ToInt32(request.Form.Get(key));
                    }
                    else if(key.Contains(".Question"))
                    {
                        options = new List<Selection>();
                        question = request.Form.Get(key);
                    }
                    else if(key.Contains(".Answer"))
                    {
                        ans = request.Form.Get(key);
                    }
                    else if(key.Contains(".text"))
                    {
                        selectionText = request.Form.Get(key);
                    }
                    else if(key.Contains(".IsSelected"))
                    {
                        isSelected = Boolean.Parse(request.Form.Get(key).Split(',')[0]);
                        Selection select = new Selection();
                        select.text = selectionText;
                        select.IsSelected = isSelected;
                        options.Add(select);
                    }
                    else if(key.Contains(".Type"))
                    {
                        type = (Question.QuestionType)Enum.Parse(typeof(Question.QuestionType), request.Form.Get(key));
                        switch(type)
                        {
                            case(Question.QuestionType.MultipleChoiceChooseMany):
                                model = new MultipleSelectViewModel();
                                model.Question = question;
                                model.QuestionId = questionId;
                                model.Type = type;
                                model.Answer = "";
                                ((MultipleSelectViewModel)model).Options = options;
                                models.Add(model);
                                break;
                            default:
                                model = new ViewModel();
                                model.Question = question;
                                model.QuestionId = questionId;
                                model.Type = type;
                                model.Answer = ans;
                                models.Add(model);
                                break;
                        }
                    }
                }

                return new SurveyViewModel
                {
                    SurveyId = sId,
                    QuestionModels = models
                };

                //// call the default model binder this new binding context
                //return base.BindModel(controllerContext, newBindingContext);
            }
            else
            {
                return base.BindModel(controllerContext, bindingContext);
            }
        }

    }
}