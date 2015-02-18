using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DiscoveryCenter.Models
{
    public class Question
    {
        public Question()
        {
            Choices = "";
        }
        public enum QuestionType
        {
            MultipleChoiceChooseOne = 0,
            MultipleChoiceChooseMany = 1,
            ShortAnswer = 2,
            Slider = 3,
            ExhibitsChooseMany = 4
        }

        [Required(ErrorMessage = "Question text is required", AllowEmptyStrings = false)]
        public string Text { get; set; }
        public int Id { get; set; }

        public QuestionType Type { get; set; }


        [ForeignKey("ParentSurvey")]
        public int? SurveyID { get; set; }

        [ScriptIgnore]
        public Survey ParentSurvey { get; set; }

        [Required(AllowEmptyStrings=true)]
        public string Choices { get; set; }

        [ScriptIgnore]
        public virtual List<Answer> Answers { get; set; }

        public int IndexInSurvey { get; set; }
    }
}