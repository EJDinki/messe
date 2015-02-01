using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class SurveyViewModel
    {
        public List<ViewModel> QuestionModels { get; set; }
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }

    }
}