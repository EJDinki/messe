using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class ChoiceBoxViewModel
    {
        public string NameAndId { get; set; }
        public string Value { get; set; }

        public bool AllowDelete { get; set; }
    }
}