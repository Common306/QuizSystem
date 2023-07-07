using Microsoft.AspNetCore.Mvc;
using QuizSystemWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace QuizSystemWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient client = null;
        private string UserApiUrl = "https://localhost:7049/api/user";
        public UserController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task<IActionResult> Teachers()
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(UserApiUrl);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                List<User>? users = JsonSerializer.Deserialize<List<User>>(strData, options);
                return View(users.Where(u => u.RoleId == 2).ToList());
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }
    }
}
