using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizSystemApi;
using QuizSystemWeb.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace QuizSystemWeb.Controllers
{
    [Route("question")]
    public class QuestionController : Controller
    {
        private readonly HttpClient client = null;
        private string QuizApiUrl = "https://localhost:7049/api/quiz";
        private string QuestionApiUrl = "https://localhost:7049/api/question";
        private string AnswerApiUrl = "https://localhost:7049/api/answer";
        private string TakeQuizApiUrl = "https://localhost:7049/api/takeQuiz";
        public QuestionController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        [Route("index")]
        public async Task<IActionResult> Index(int quizid)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(QuizApiUrl + "/" + quizid);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // get quiz
                Quiz? quiz = JsonSerializer.Deserialize<Quiz>(strData, options);

                //get list question of a quiz
                response = await client.GetAsync(QuestionApiUrl + "?quizId=" + quizid);
                strData = await response.Content.ReadAsStringAsync();
                List<Question>? questions = JsonSerializer.Deserialize<List<Question>>(strData, options);
                foreach (Question q in questions)
                {
                    response = await client.GetAsync(AnswerApiUrl + "?questionId=" + q.QuestionId);
                    strData = await response.Content.ReadAsStringAsync();
                    List<Answer>? answers = JsonSerializer.Deserialize<List<Answer>>(strData, options);
                    q.Answers = answers;
                }
                quiz.Questions = questions;

                return View(quiz);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("create")]
        public async void CreateQuestion(int quizId, string questionContent, int questionScore, string[] answersMark, string[] answers)
        {

            Question question = new Question
            {
                Content = questionContent,
                IsActive = true,
                Score = questionScore,
                MultipleChoice = true,
                QuizId = quizId,
            };

            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    throw new AuthenticationException("You don't have permission");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // create a question
                string jsonData = JsonSerializer.Serialize(question);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(QuestionApiUrl, content);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Question? newQuestion = JsonSerializer.Deserialize<Question>(strData, options);

                // create list answer of a question
                List<Answer> listAnswer = new List<Answer>();
                for (int i = 0; i < answersMark.Length; i++)
                {
                    Answer ans = new Answer
                    {
                        Content = answers[i],
                        IsCorrect = Convert.ToBoolean(answersMark[i]),
                        IsActive = true,
                        QuestionId = newQuestion.QuestionId
                    };
                    listAnswer.Add(ans);
                }

                string jsonData2 = JsonSerializer.Serialize(listAnswer);
                var content2 = new StringContent(jsonData2, Encoding.UTF8, "application/json");
                HttpResponseMessage response2 = await client.PostAsync(AnswerApiUrl, content2);
                string strData2 = await response2.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                throw new AuthenticationException("You dont have permisstion");
            }
        }

        [HttpPost]
        [Route("edit")]

        public async void EditQuestion(int quizId, int questionId, string questionContent, int questionScore, string[] answersMark, string[] answers, int[] answersId)
        {
            Question question = new Question()
            {
                QuestionId = questionId,
                Content = questionContent,
                Score = questionScore,
                MultipleChoice = true,
                QuizId = quizId,
                IsActive = true
            };

            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    throw new AuthenticationException("You don't have permission");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // edit a question
                string jsonData = JsonSerializer.Serialize(question);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(QuestionApiUrl + "/" + questionId, content);

                //edit list answer
                List<Answer> listAnswer = new List<Answer>();
                for (int i = 0; i < answers.Length; i++)
                {
                    Answer a = new Answer
                    {
                        AnswerId = answersId[i],
                        Content = answers[i],
                        IsCorrect = answersMark[i] == "true" ? true : false,
                        IsActive = true,
                        QuestionId = questionId
                    };
                    listAnswer.Add(a);
                }

                jsonData = JsonSerializer.Serialize(listAnswer);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                response = await client.PutAsync(AnswerApiUrl, content);

            }
            catch (Exception ex)
            {
                throw new AuthenticationException("You dont have permisstion");
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

                HttpResponseMessage response = await client.DeleteAsync(QuestionApiUrl + "/" + id);
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("You dont have permisstion");
            }
        }

        [Route("doQuiz")]
        public async Task<IActionResult> DoQuiz(int quizid)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(QuizApiUrl + "/student/" + quizid);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // get quiz
                Quiz? quiz = JsonSerializer.Deserialize<Quiz>(strData, options);

                //get list question of a quiz
                response = await client.GetAsync(QuestionApiUrl + "/student?quizId=" + quizid);
                strData = await response.Content.ReadAsStringAsync();
                List<Question>? questions = JsonSerializer.Deserialize<List<Question>>(strData, options);
                foreach (Question q in questions)
                {
                    response = await client.GetAsync(AnswerApiUrl + "/student?questionId=" + q.QuestionId);
                    strData = await response.Content.ReadAsStringAsync();
                    List<Answer>? answers = JsonSerializer.Deserialize<List<Answer>>(strData, options);
                    q.Answers = answers;
                }
                quiz.Questions = questions;

                return View(quiz);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("SubmitQuiz")]

        public async Task<IActionResult> SubmitQuiz(int quizId, DateTime startAt)
        {
            string? token = Request.Cookies["Token"];
            User user = GetUserFromToken(token);
            if (token == null)
            {
                return Unauthorized();
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            double score = 0;
            var listQuestion = Request.Form;
            HttpResponseMessage response = await client.GetAsync(QuestionApiUrl + "/quizKey?quizId=" + quizId);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Question>? keyQuestions = JsonSerializer.Deserialize<List<Question>>(strData, options);
            foreach (Question key in keyQuestions)
            {
                bool flag = true;
                foreach (var k in key.Answers)
                {
                    if (!listQuestion[key.QuestionId.ToString()].ToList().Contains(k.AnswerId + "") || listQuestion[key.QuestionId.ToString()].ToList().Count() != key.Answers.Count())
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    score += double.Parse(key.Score.ToString().AsSpan());
                }
            }
            var end = DateTime.Now;
            TakeQuiz takeQuiz = new TakeQuiz();
            takeQuiz.UserId = user.UserId;
            takeQuiz.QuizId = quizId;
            takeQuiz.StartAt = startAt; 
            takeQuiz.EndAt = end;
            takeQuiz.Score = score;
            string jsonData = JsonSerializer.Serialize(takeQuiz);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            response = await client.PostAsync(TakeQuizApiUrl, content);
            strData = await response.Content.ReadAsStringAsync();

            return RedirectToAction("Quizzes", "Quiz");
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
