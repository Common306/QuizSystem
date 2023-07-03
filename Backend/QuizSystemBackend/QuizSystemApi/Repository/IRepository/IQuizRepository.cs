using QuizSystemApi.Models;

namespace QuizSystemApi.Repository.IRepository
{
    public interface IQuizRepository
    {
        public List<Quiz> GetAll();
        public Quiz Get(int id);
        public Quiz Create(Quiz quiz);
        public Quiz Update(int id, Quiz quiz);
        public bool Delete(int id);
    }
}
