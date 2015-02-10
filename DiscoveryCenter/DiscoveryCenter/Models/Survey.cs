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

        
        [Display(Name = "Survey Name")]
        [Required(ErrorMessage = "A survey name is required", AllowEmptyStrings = false)]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        
        public virtual List<Question> Questions { get; set; }

        [ScriptIgnore]
        public virtual List<Submission> Submissions { get; set; }

    }
}