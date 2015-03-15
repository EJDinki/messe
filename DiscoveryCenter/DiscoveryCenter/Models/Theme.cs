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

        public string Name { get; set; }

        public string CssBundleName { get; set; }

        public string JsBundleName { get; set; }

    }
}