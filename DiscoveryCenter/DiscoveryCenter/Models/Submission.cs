using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoveryCenter.Models
{
    public class Submission
    {
        public int Id { get; set; }

        [ForeignKey("ParentSurvey")]
        public int? SurveyId { get; set; }
        public int CompletionTime { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual Survey ParentSurvey { get; set; }

        public virtual List<Answer> Answers {get; set;}
    }
}