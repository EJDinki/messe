using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Web.Mvc;


namespace DiscoveryCenter.Models
{
    public class Exhibit
    {
        [Display(Name = "Exhibit Name")]
        [Required(ErrorMessage = "An exhibit name is required", AllowEmptyStrings = false)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime LastModifiedDate { get; set; }

        public string ImageLocation { get; set; }


        [ForeignKey("RatingSurvey")]
        public int? RatingSurveyID { get; set; }

        [ScriptIgnore]
        public Survey RatingSurvey { get; set; }

        /// <summary>
        /// Please help. This is used because on the Exhibit edit page, the ImageLocation wasn't being bound to the model.
        /// Even though the same code from the creation page was used and it was bound correctly there. If we have time before release,
        /// fix this HACK workaround.
        /// </summary>
        [NotMapped]
        public string BrokenWorkaround { get; set; }
    }
}