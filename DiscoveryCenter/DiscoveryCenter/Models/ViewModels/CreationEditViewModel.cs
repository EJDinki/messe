using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscoveryCenter.Models
{
    public class CreationEditViewModel
    {
        public Survey Survey { get; set; }

        public IEnumerable<SelectListItem> Themes { get; set; }

    }
}