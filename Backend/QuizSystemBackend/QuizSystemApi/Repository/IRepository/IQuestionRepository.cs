using QuizSystemApi.Models;

namespace QuizSystemApi.Repository.IRepository
{
    public interface IQuestionRepository
    {
        public List<Question> GetListByQuizId(int quizId, User user);
        public Question Create(Question question);
        public Question Update(int id, Question question, User user);
        public bool Delete(int id, User user);
    }
}
