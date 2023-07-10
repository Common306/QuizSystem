using QuizSystemApi.Dto.Response;
using QuizSystemApi.Models;

namespace QuizSystemApi.Repository.IRepository
{
    public interface IQuizRepository
    {
        public List<Quiz> GetAll(User user);
        public Quiz Get(int id, User user);
        public Quiz Create(Quiz quiz);
        public Quiz Update(int id, Quiz quiz, User user);
        public bool Delete(int id, User user);
        public List<TakeQuiz> ListResults(int id, User user);
        public ReviewQuizDtoResponse ReviewQuiz(int id, User user);
    }
}
