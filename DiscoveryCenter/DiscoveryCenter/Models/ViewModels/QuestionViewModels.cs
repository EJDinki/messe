using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    /**
     * 
     * This Model is under review to be deleted.
     * */
    [System.Obsolete("Under review for removal. Please use SurveyViewModel")]
    public class MultipleChoiceQuestionViewModel
    {
        public bool AllowMultiple;
        public List<String> Choices;
        public String Question;
    }
}