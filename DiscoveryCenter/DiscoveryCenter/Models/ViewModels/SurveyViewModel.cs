using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class SurveyViewModel
    {
        public List<QuestionViewModel> QuestionModels { get; set; }
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string SurveyDescription { get; set; }

        public Theme Theme { get; set; }
    }
}