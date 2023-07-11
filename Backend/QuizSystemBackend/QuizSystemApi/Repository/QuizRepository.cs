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
        public List<Quiz> GetAll()
        {
            return QuizDao.GetAll();
        }
        public Quiz Get(int id, User user)
        {
            return QuizDao.Get(id, user);
        }
        public Quiz Get(int id)
        {
            return QuizDao.Get(id);
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
        public List<TakeQuiz> ListResults(int id, User user)
        {
            return QuizDao.ListResults(id, user);
        }
        public List<TakeAnswer> ReviewQuiz(int id, User user)
        {
            return QuizDao.ReviewQuiz(id, user);
        }

    }
}
