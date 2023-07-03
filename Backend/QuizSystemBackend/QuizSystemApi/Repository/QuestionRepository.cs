using QuizSystemApi.Dao;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        public List<Question> GetListByQuizId(int quizId)
        {
            return QuestionDao.GetListByQuizId(quizId);
        }

        public Question Create(Question question)
        {
            return QuestionDao.Create(question);
        }

        public Question Update(int id, Question question)
        {
            return QuestionDao.Update(id, question);
        }

        public bool Delete(int id)
        {
            return QuestionDao.Delete(id);
        }
    }
}
