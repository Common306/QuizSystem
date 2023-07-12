using QuizSystemWeb.Models;

namespace QuizSystemWeb.Dto.Response
{
    public class QuizDtoResponse
    {
        public List<Quiz> Quizzes { get; set; } = null!;
        public int Total { get; set; }
        public int? Page { get; set; }
        public int PageSize { get; set; }
    }
}
