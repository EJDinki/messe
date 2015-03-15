using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Survey Description")]
        [Required(ErrorMessage = "A survey description is required", AllowEmptyStrings = false)]
        public string Description { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime LastModifiedDate { get; set; }

        [ForeignKey("Theme")]
        public int ThemeId { get; set; }
        [ScriptIgnore]
        public virtual Theme Theme { get; set; }

        
        public virtual List<Question> Questions { get; set; }

        [ScriptIgnore]
        public virtual List<Submission> Submissions { get; set; }

    }
}