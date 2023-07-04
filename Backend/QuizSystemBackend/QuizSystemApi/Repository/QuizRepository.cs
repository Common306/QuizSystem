using QuizSystemApi.Dao;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Repository
{
    public class QuizRepository : IQuizRepository
    {
        public List<Quiz> GetAll(User user)
        {
            return QuizDao.GetAll(user);
        }
        public Quiz Get(int id, User user)
        {
            return QuizDao.Get(id, user);
        }
        public Quiz Create(Quiz quiz)
        {
            return QuizDao.Create(quiz);
        }
        public Quiz Update(int id, Quiz quiz, User user)
        {
            return QuizDao.Update(id, quiz, user);
        }
        public bool Delete(int id, User user)
        {
            return QuizDao.Delete(id, user);
        }
        
    }
}
