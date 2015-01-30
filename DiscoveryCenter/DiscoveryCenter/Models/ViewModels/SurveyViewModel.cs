using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class SurveyViewModel
    {

        public string SurveyName {get; set;}
        public List<Question> questions { get; set; }
        public Dictionary<int, List<Option>> options { get; set; }

        public List<string> answer { get; set; }

        public class Option
        {
            public Option ()
            {
                
            }

            public Option (int question, bool selected = false)
            {
                questionId = question;
                isSelected = selected;
            }
            public int questionId { get; set; }
            public string text { get; set; }
            public bool isSelected { get; set; }

        }
    }
}