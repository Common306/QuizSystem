using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizSystemApi.Dto.Request;
using QuizSystemApi.Dto.Response;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public LoginController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginDtoRequest request)
        {
            User? user = _userRepository.Login(request);
            if(user == null)
            {
                return Unauthorized();
            }

            var tokenStr = GenerateJwt(user);
            LoginDtoResponse response = new LoginDtoResponse
            {
                Token = tokenStr
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterDtoRequest request)
        {
            User? userRegister = _userRepository.Register(request);
            if(userRegister == null)
            {
                return Unauthorized("Account has been exist");
            }
            var tokenStr = GenerateJwt(userRegister);
            RegisterDtoResponse response = new RegisterDtoResponse
            {
                Token = tokenStr
            };
            return Ok(response);
        }

        private string GenerateJwt(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Username", user.Username),
                new Claim("RoleId", user.RoleId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim("Fullname", user.FullName ?? ""),
                new Claim("PhoneNumber", user.PhoneNumber ?? ""),
                new Claim("CreateAt", user.CreateAt.ToString() ?? ""),
                new Claim("UpdateAt", user.UpdateAt.ToString() ?? ""),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken.ToString();
        }
    }
}
