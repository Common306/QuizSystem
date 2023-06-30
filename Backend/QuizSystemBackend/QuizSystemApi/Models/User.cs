using System;
using System.Collections.Generic;

namespace QuizSystemApi.Models
{
    public partial class User
    {
        public User()
        {
            Quizzes = new HashSet<Quiz>();
            TakeQuizzes = new HashSet<TakeQuiz>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? IsEnable { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<TakeQuiz> TakeQuizzes { get; set; }
    }
}
