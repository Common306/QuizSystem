using QuizSystemApi.Dao;
using QuizSystemApi.Dto.Response;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Repository
{
    public class QuizRepository : IQuizRepository
    {
        public List<Quiz> GetAll(User user, string? search, int? page)
        {
            return QuizDao.GetAll(user, search, page);
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
        public List<TakeQuiz> ListResults(int id, User user, string? search, int? page)
        {
            return QuizDao.ListResults(id, user, search, page);
        }
        public ReviewQuizDtoResponse ReviewQuiz(int id, User user)
        {
            return QuizDao.ReviewQuiz(id, user);
        }
        public int Total(User user,string? search)
        {
            return QuizDao.Total(user, search);
        }
        public int TotalQuizResult(int id, User user, string? search)
        {
            return QuizDao.TotalQuizResult(id, user, search);
        }
    }
}
