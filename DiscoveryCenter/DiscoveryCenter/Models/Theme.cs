using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class Theme
    {
        [Key]
        public int Id { get; set; }
        public String Name;
        public String CssBundleName;
        public String JsBundleName;

        public virtual List<Survey> Surveys { get; set; }
    }
}