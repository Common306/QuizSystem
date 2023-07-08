using QuizSystemApi.Models;

namespace QuizSystemApi.Repository.IRepository
{
    public interface IQuizRepository
    {
        public List<Quiz> GetAll(User user);
        public List<Quiz> GetAllForStudent(User user);
        public Quiz Get(int id, User user);
        public Quiz Create(Quiz quiz);
        public Quiz Update(int id, Quiz quiz, User user);
        public bool Delete(int id, User user);
    }
}
