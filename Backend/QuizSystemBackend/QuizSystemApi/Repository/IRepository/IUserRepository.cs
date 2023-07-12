using QuizSystemApi.Dto.Request;
using QuizSystemApi.Models;

namespace QuizSystemApi.Repository.IRepository
{
    public interface IUserRepository
    {
        public User? Login(LoginDtoRequest request);
        public User? Register(RegisterDtoRequest request);
        public List<User> GetAll(string? search, int? page);
        public User Get(int id);
        public User Create(User user);
        public User Update(int id, User user);
        public bool Delete(int id);
        public int Total(string search);
    }
}
