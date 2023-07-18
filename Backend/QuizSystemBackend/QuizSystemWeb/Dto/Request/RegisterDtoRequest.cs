namespace QuizSystemWeb.Dto.Request
{
    public class RegisterDtoRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
    }
}
