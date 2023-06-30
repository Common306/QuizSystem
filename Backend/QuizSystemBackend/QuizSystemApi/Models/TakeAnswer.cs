using System;
using System.Collections.Generic;

namespace QuizSystemApi.Models
{
    public partial class TakeAnswer
    {
        public int TakeQuizId { get; set; }
        public int AnswerId { get; set; }
        public int? QuestionId { get; set; }

        public virtual Answer Answer { get; set; } = null!;
        public virtual TakeQuiz TakeQuiz { get; set; } = null!;
    }
}
