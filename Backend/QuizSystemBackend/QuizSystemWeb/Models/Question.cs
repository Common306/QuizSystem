using System;
using System.Collections.Generic;

namespace QuizSystemWeb.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public string? Content { get; set; }
        public double? Score { get; set; }
        public bool? MultipleChoice { get; set; }
        public int? QuizId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Quiz? Quiz { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
