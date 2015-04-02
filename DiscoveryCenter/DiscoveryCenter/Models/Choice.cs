using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DiscoveryCenter.Models
{
    public class Choice
    {
        public Choice()
        {
        }

        [Required(ErrorMessage = "Choice text is required", AllowEmptyStrings = false)]
        public string Text { get; set; }

        public string ImageName { get; set; }

        public int Id { get; set; }

        [ForeignKey("ParentQuestion")]
        public int? QuestionID { get; set; }

        [ScriptIgnore]
        public Question ParentQuestion { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

    }
}