using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace DiscoveryCenter.Models
{
    public class Exhibit
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}