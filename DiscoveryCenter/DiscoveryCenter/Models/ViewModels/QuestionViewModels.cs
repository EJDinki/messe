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
        public List<String> Choices { get; set; }    
    }

    public class MultipleSelectViewModel : MultipleChoiceViewModel
    {
        public List<Selection> Options { get; set; }
    }

    public class SliderViewModel : QuestionViewModel
    {
        public List<String> Choices { get; set; }
    }

    public class Selection
    {
        public bool IsSelected;
        public string text;
        public string image;
    }
}