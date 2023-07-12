using QuizSystemWeb.Models;

namespace QuizSystemWeb.Dto.Response
{
    public class UserDtoResponse
    {
        public List<User> Users { get; set; } = null!;
        public int Total { get; set; }
        public int? Page { get; set; }
        public int PageSize { get; set; }

    }
}
