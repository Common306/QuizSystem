using System;
using System.Collections.Generic;

namespace QuizSystemWeb.Models
{
    public partial class TakeQuiz
    {
        public TakeQuiz()
        {
            TakeAnswers = new HashSet<TakeAnswer>();
        }

        public int TakeQuizId { get; set; }
        public int? UserId { get; set; }
        public int? QuizId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public double? Score { get; set; }

        public virtual Quiz? Quiz { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<TakeAnswer> TakeAnswers { get; set; }
    }
}
