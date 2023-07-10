using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using QuizSystemWeb.Dto.Response;
using QuizSystemWeb.Models;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;

namespace QuizSystemWeb.Controllers
{
    [Route("quiz")]
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
        [Route("index")]
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
                List<Quiz>? quizzes = JsonSerializer.Deserialize<List<Quiz>>(strData, options);
                return View(quizzes);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(QuizApiUrl + "/" + id);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Quiz? quiz = JsonSerializer.Deserialize<Quiz>(strData, options);
                return View(quiz);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string jsonData = JsonSerializer.Serialize(quiz);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(QuizApiUrl, content);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return RedirectToAction("Index", "Quiz");
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(Quiz quiz)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string jsonData = JsonSerializer.Serialize(quiz);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(QuizApiUrl + "/" + quiz.QuizId, content);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return RedirectToAction("Index", "Quiz");
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async void Delete(int id)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    throw new AuthenticationException("You don't have permission");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.DeleteAsync(QuizApiUrl + "/" + id);
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("You don't have permission");
            }
        }

        [HttpGet]
        [Route("results/{id}")]
        public async Task<IActionResult> Results(int id)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(QuizApiUrl + "/results/" + id);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                List<TakeQuiz>? quiz = JsonSerializer.Deserialize<List<TakeQuiz>>(strData, options);
                ViewBag.QuizId = id;
                return View(quiz);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("review/{id}")]
        public async Task<IActionResult> Review(int id)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(QuizApiUrl + "/review/" + id);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                ReviewQuizDtoResponse? review = JsonSerializer.Deserialize<ReviewQuizDtoResponse>(strData, options);
                return View(review);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }
    }
}
