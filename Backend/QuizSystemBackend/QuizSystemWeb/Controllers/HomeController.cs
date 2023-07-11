using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizSystemWeb.Dto.Request;
using QuizSystemWeb.Dto.Response;
using QuizSystemWeb.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Json;

namespace QuizSystemWeb.Controllers
{
	[Route("home")]
    public class HomeController : Controller
    {
		private readonly HttpClient client = null;
		
		public HomeController()
        {
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
		}
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

		[Route("/login")]
		public IActionResult Login()
		{
			return View();
		}

        [Route("/logout")]
        public IActionResult Logout()
        {
			Response.Cookies.Delete("Token");
            return RedirectToAction("Index", "Home");
        }

        [Route("/register")]
		public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
			LoginDtoRequest req = new LoginDtoRequest
			{
				Username = username,
				Password = password
			};
			string jsonData = JsonSerializer.Serialize(req);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			string LoginApiUrl = "https://localhost:7049/api/login";
			HttpResponseMessage response = await client.PostAsync(LoginApiUrl, content);
			if((int)response.StatusCode == StatusCodes.Status200OK)
			{
				string strData = await response.Content.ReadAsStringAsync();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				LoginDtoResponse? token = JsonSerializer.Deserialize<LoginDtoResponse>(strData, options);
				// luu token trong cookie
				var cookieOptions = new CookieOptions
				{
					HttpOnly = true,
					Expires = DateTime.Now.AddDays(1),
				};
				Response.Cookies.Append("Token", token.Token, cookieOptions);

                // redirect đến trang tuỳ thuộc vào role của user
                User user = GetUserFromToken(token.Token);
                if(user.RoleId == 1)
                {
                    return RedirectToAction("Index", "User");
                } else if(user.RoleId == 2)
                {
                    return RedirectToAction("Index", "Quiz");
                } else
                {
                    return RedirectToAction("Quizzes", "Quiz");
                }
            } else
			{
				return View();
			}
        }

		[HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register(RegisterDtoRequest request)
		{
			try
			{
				JsonContent content = JsonContent.Create(request);
				var response = await client.PostAsync("https://localhost:7049/api/register", content);
				if (response.IsSuccessStatusCode)
				{
					string json = await response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					};
					return RedirectToAction("Login", "Home");
				}
				else
				{
                    ViewBag.Message = "Account has been existed!!!";
                    return View();
				}
			}
			catch
			{
				return RedirectToAction("Index", "Home");
			}
		}


        public User GetUserFromToken(string token)
        {
            var _config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Issuer"]
            };

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                string userIdClaim = claimsPrincipal.FindFirst("UserId").Value;
                string userUsernameClaim = claimsPrincipal.FindFirst("Username").Value;
                string userRoleIdClaim = claimsPrincipal.FindFirst("RoleId").Value;
                string userRoleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role).Value;
                string userFullNameClaim = claimsPrincipal.FindFirst("Fullname").Value;
                string userPhoneNumberClaim = claimsPrincipal.FindFirst("PhoneNumber").Value;
                string? userCreateAtClaim = claimsPrincipal.FindFirst("CreateAt").Value;
                string userUpdateAtClaim = claimsPrincipal.FindFirst("UpdateAt").Value;

                if (userIdClaim != null)
                {
                    User user = new User
                    {
                        UserId = Convert.ToInt32(userIdClaim),
                        Username = userUsernameClaim,
                        Password = "",
                        RoleId = Convert.ToInt32(userRoleIdClaim),
                        FullName = userFullNameClaim,
                        PhoneNumber = userPhoneNumberClaim,
                        CreateAt = userCreateAtClaim != "" ? Convert.ToDateTime(userCreateAtClaim) : null,
                        UpdateAt = userUpdateAtClaim != "" ? Convert.ToDateTime(userUpdateAtClaim) : null,
                        IsEnable = true
                    };
                    Role role = new Role
                    {
                        RoleId = Convert.ToInt32(userRoleIdClaim),
                        RoleName = userRoleClaim
                    };
                    user.Role = role;
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}