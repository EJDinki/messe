using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class MultipleChoiceQuestionViewModel
    {
        public String SurveyName;
        public bool AllowMultiple;
        public List<String> Choices;
        public String Question;
    }
}