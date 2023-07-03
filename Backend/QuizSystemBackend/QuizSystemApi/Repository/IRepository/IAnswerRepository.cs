using QuizSystemApi.Models;

namespace QuizSystemApi.Repository.IRepository
{
    public interface IAnswerRepository
    {
        public List<Answer> GetListByQuestionId(int questionId);
        public List<Answer> CreateListAnswer(List<Answer> answers);
    }
}
