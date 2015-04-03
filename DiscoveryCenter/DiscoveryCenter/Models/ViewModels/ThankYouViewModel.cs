using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class ThankYouViewModel
    {
        public int SurveyId { get; set; }
        public Theme Theme { get; set; }

        public bool Muted { get; set; }
    }
}