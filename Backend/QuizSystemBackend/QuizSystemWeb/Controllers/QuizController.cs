using Microsoft.AspNetCore.Mvc;
using QuizSystemWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace QuizSystemWeb.Controllers
{
    public class QuizController : Controller
    {
        private readonly HttpClient client = null;
        private string QuizApiUrl = "https://localhost:7049/api/quiz";
        public QuizController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(QuizApiUrl);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                List<Quiz>? quizes = JsonSerializer.Deserialize<List<Quiz>>(strData, options);
                return View(quizes);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }
    }
}
