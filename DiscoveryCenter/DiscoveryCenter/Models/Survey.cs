using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DiscoveryCenter.Models
{
    public class Survey
    {
        public Survey()
        {
            Questions = new List<Question>();
            Submissions = new List<Submission>();
        }
        public int Id { get; set; }

        [Required]
        [Display(Name = "Survey Name")]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        
        public virtual List<Question> Questions { get; set; }

        [ScriptIgnore]
        public virtual List<Submission> Submissions { get; set; }
    }
}