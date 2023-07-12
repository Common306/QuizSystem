using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using QuizSystemWeb.Models;
using QuizSystemWeb.Dto.Response;
using QuizSystemWeb.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace QuizSystemWeb.Controllers
{
    [Route("quiz")]
    public class QuizController : Controller
    {
        private readonly HttpClient client = null;
        private string QuizApiUrl = "https://localhost:7049/api/quiz";
        private string TakeQuizApiUrl = "https://localhost:7049/api/takeQuiz";
        public QuizController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        [Route("index")]
        public async Task<IActionResult> Index(string? search, int? page)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                if (search != null && page != null)
                {
                    QuizApiUrl += "?search=" + search + "&page=" + page;
                }
                else if (search != null)
                {
                    QuizApiUrl += "?search=" + search;
                }
                else if (page != null)
                {
                    QuizApiUrl += "?page=" + page;
                }
                HttpResponseMessage response = await client.GetAsync(QuizApiUrl);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                QuizDtoResponse? res = JsonSerializer.Deserialize<QuizDtoResponse>(strData, options);
                ViewData["ValueSearch"] = search;
                ViewData["Page"] = res.Page;
                ViewData["Total"] = res.Total;
                ViewData["PageSize"] = res.PageSize;
                ViewData["CurrentPage"] = (page == null) ? 1 : page;
                return View(res.Quizzes);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }

        }
        [Route("quizzes")]
        public async Task<IActionResult> Quizzes()
        {
            try
            {
                string? token = Request.Cookies["Token"];
                User user = GetUserFromToken(token);
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(QuizApiUrl + "/student");
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                List<Quiz>? quizzes = JsonSerializer.Deserialize<List<Quiz>>(strData, options);
                response = await client.GetAsync(TakeQuizApiUrl);
                strData = await response.Content.ReadAsStringAsync();
                List<TakeQuiz>? takeQuizzes = JsonSerializer.Deserialize<List<TakeQuiz>>(strData, options);
                ViewBag.takeQuizzes = takeQuizzes;
                ViewBag.user = user;
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
                if(quiz.IsPublish == null)
                {
                    quiz.IsPublish = false;
                }
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
                if (quiz.IsPublish == null)
                {
                    quiz.IsPublish = false;
                }
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
        public async Task<IActionResult> Results(int id, string? search, int? page)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (search != null && page != null)
                {
                    QuizApiUrl += "/results/" + id + "?search=" + search + "&page=" + page;
                }
                else if (search != null)
                {
                    QuizApiUrl += "/results/" + id + "?search=" + search;
                }
                else if (page != null)
                {
                    QuizApiUrl += "/results/" + id + "?page=" + page;
                } else
                {
                    QuizApiUrl += "/results/" + id;
                }
                HttpResponseMessage response = await client.GetAsync(QuizApiUrl);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                QuizResultsDtoResponse? res = JsonSerializer.Deserialize<QuizResultsDtoResponse>(strData, options);
                ViewBag.QuizId = id;
                ViewData["ValueSearch"] = search;
                ViewData["Page"] = res.Page;
                ViewData["Total"] = res.Total;
                ViewData["PageSize"] = res.PageSize;
                ViewData["CurrentPage"] = (page == null) ? 1 : page;
                return View(res.TakeQuizzes);
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
            if (token == null)
            {
                return new User();
            }
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
                return new User();
            }
            catch (Exception ex)
            {
                return new User();
            }
        }
    }
}
