using System;
using System.Collections.Generic;

namespace QuizSystemWeb.Models
{
    public partial class Answer
    {
        public Answer()
        {
            TakeAnswers = new HashSet<TakeAnswer>();
        }

        public int AnswerId { get; set; }
        public string? Content { get; set; }
        public bool? IsCorrect { get; set; }
        public bool? IsActive { get; set; }
        public int? QuestionId { get; set; }

        public virtual Question? Question { get; set; }
        public virtual ICollection<TakeAnswer> TakeAnswers { get; set; }
    }
}
