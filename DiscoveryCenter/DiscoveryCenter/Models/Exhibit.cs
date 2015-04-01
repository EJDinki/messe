using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;


namespace DiscoveryCenter.Models
{
    public class Exhibit
    {
        [Display(Name = "Exhibit Name")]
        [Required(ErrorMessage = "An exhibit name is required", AllowEmptyStrings = false)]
        public string Name { get; set; }

        public int Id { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime LastModifiedDate { get; set; }

        public string ImageLocation { get; set; }

        [NotMapped]
        public HttpPostedFileBase Image { get; set; }
    }
}