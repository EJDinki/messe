using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{

    public class QuestionViewModel
    {
        public string Question { get; set; }
        public int QuestionId { get; set; }

        public string Answer { get; set; }

        public Question.QuestionType Type { get; set; }
        
    }

    public class MultipleChoiceViewModel : QuestionViewModel
    {
        public List<Choice> Choices { get; set; }    
    }

    public class SpinnerViewModel : QuestionViewModel
    {
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public int StartValue { get; set; }
    }

    public class MultipleSelectViewModel : MultipleChoiceViewModel
    {
        public int MaxSelectedChoices { get; set; }
    }

    public class SliderViewModel : MultipleChoiceViewModel
    {
    }
}