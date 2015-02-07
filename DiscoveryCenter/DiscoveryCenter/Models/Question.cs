using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace DiscoveryCenter.Models
{
    public class Question
    {

        public enum QuestionType
        {
            MultipleChoiceChooseOne,
            MultipleChoiceChooseMany,
            ShortAnswer,
            Slider
        }

        public string Text { get; set; }
        public int Id { get; set; }

        public QuestionType Type { get; set; }


        [ForeignKey("ParentSurvey")]
        public int SurveyID { get; set; }

        [ScriptIgnore]
        public Survey ParentSurvey { get; set; }
        public string Choices { get; set; }

        [ScriptIgnore]
        public virtual List<Answer> Answers { get; set; }

        public int IndexInSurvey { get; set; }
    }
}