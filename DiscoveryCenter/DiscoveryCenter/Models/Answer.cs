using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoveryCenter.Models
{
    public class Answer
    {
        public Answer() { }
        public Answer(Question q, string val = null)
        {
            Question = q;
            QuestionId = q.Id;
            Value = val;
        }

        public int Id { get; set; }

        [ForeignKey("Question")]
        public int? QuestionId { get; set; }
        public Question Question { get; set; }

        public string Value { get; set; }

        [ForeignKey("Submission")]
        public int SubmissionId { get; set; }

        public virtual Submission Submission { get; set; }
    }
}