using QuizSystemApi.Dao;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Repository
{
    public class QuizRepository : IQuizRepository
    {
        public List<Quiz> GetAll()
        {
            return QuizDao.GetAll();
        }
        public Quiz Get(int id)
        {
            return QuizDao.Get(id);
        }
        public Quiz Create(Quiz quiz)
        {
            return QuizDao.Create(quiz);
        }
        public Quiz Update(int id, Quiz quiz)
        {
            return QuizDao.Update(id, quiz);
        }
        public bool Delete(int id)
        {
            return QuizDao.Delete(id);
        }
        
    }
}
