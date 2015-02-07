using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DiscoveryCenter.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        
        public virtual List<Question> Questions { get; set; }

        [ScriptIgnore]
        public virtual List<Submission> Submissions { get; set; }
    }
}