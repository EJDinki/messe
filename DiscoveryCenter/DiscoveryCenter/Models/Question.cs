using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoveryCenter.Models
{
    public class Question
    {

        public enum QuestionType
        {
            MultipleChoice,
            ShortAnswer,
            Slider
        }

        public string Text { get; set; }
        public int Id { get; set; }

        public QuestionType Type { get; set; }


        [ForeignKey("ParentSurvey")]
        public int SurveyID { get; set; }
        public Survey ParentSurvey { get; set; }
        public string Choices { get; set; }
        public virtual List<Answer> Answers { get; set; }
    }
}