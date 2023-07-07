using System;
using System.Collections.Generic;

namespace QuizSystemWeb.Models

{
    public partial class Quiz
    {
        public Quiz()
        {
            //Questions = new HashSet<Question>();
            //TakeQuizzes = new HashSet<TakeQuiz>();
        }

        public int QuizId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsPublish { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public double? PassScore { get; set; }
        public string? QuizCode { get; set; }
        public int? CreatorId { get; set; }

        public virtual User? Creator { get; set; }
        //public virtual ICollection<Question>? Questions { get; set; }
        //public virtual ICollection<TakeQuiz>? TakeQuizzes { get; set; }
    }
}
