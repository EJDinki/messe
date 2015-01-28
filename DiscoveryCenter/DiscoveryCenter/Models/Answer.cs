using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoveryCenter.Models
{
    public class Answer
    {

        public int Id { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string Value { get; set; }
    }
}