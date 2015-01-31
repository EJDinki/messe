using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{

    public class ViewModel
    {
        public string Question { get; set; }
        public int QuestionId { get; set; }

        public string Answer { get; set; }

        public Question.QuestionType Type { get; set; }
        
    }

    public class MultipleChoiceViewModel : ViewModel
    {
        public List<String> Choices { get; set; }    
    }

    public class MultipleSelectViewModel : MultipleChoiceViewModel
    {
        public List<Selection> Options { get; set; }
    }

    public class Selection
    {
        public bool IsSelected;
        public string text;
    }
}