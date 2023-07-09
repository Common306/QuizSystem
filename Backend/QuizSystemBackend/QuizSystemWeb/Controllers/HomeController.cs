using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Dto.Request;
using QuizSystemApi.Dto.Response;
using QuizSystemWeb.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
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
		[Route("/login")]
		public IActionResult Login()
		{
			return View();
		}

        [Route("/logout")]
        public IActionResult Logout()
        {
			Response.Cookies.Delete("Token");
            return RedirectToAction("Login", "Home");
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
				return RedirectToAction("Index", "User");
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
					return RedirectToAction("Login", "Home");
				}
			}
			catch
			{
				return RedirectToAction("Index", "Home");
			}
		}
    }
}