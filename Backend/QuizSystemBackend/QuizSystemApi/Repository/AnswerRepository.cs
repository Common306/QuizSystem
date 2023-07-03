using QuizSystemApi.Dao;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Repository
{
    public class AnswerRepository : IAnswerRepository
    {
        public List<Answer> GetListByQuestionId(int questionId)
        {
            return AnswerDao.GetListByQuestionId(questionId);
        }
        public List<Answer> CreateListAnswer(List<Answer> answers)
        {
            return AnswerDao.CreateListAnswer(answers);
        }
    }
}
