using QuizSystemApi.Models;

namespace QuizSystemApi.Repository.IRepository
{
    public interface IQuestionRepository
    {
        public List<Question> GetListByQuizId(int quizId);
        public Question Create(Question question);
        public Question Update(int id, Question question);
        public bool Delete(int id);
    }
}
