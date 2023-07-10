using QuizSystemWeb.Models;
using System.Text.Json.Serialization;

namespace QuizSystemWeb.Dto.Response
{
    public class ReviewQuizDtoResponse
    {
        public TakeQuiz TakeQuiz { get; set; }
        public Quiz Quiz { get; set; }

        public double? TotalScore { get; set; }
        public List<Question> Questions { get; set; }
        public IDictionary<int, bool> ReviewAnswer { get; set; } = new Dictionary<int, bool>();
        public IDictionary<int, double?> QuestionScore { get; set; } = new Dictionary<int, double?>();

        public bool CheckAnswer(Answer trueAnswer, TakeAnswer checkAnswer)
        {
            if (checkAnswer is null) return false;

            return trueAnswer.IsCorrect == true && trueAnswer.AnswerId == checkAnswer.AnswerId;
        }
    }
}
