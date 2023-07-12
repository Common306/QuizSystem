using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizSystemApi.Dto.Response;
using QuizSystemApi.Models;
using QuizSystemApi.Repository;
using QuizSystemApi.Repository.IRepository;
using System.Data;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/quiz")]
    [Authorize(Roles = "Admin, Teacher")]
    public class QuizController : Controller
    {
        private readonly IQuizRepository _quizRepository;

        public QuizController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [HttpGet]
        public IActionResult GetAll(string? search, int? page) {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            List<Quiz> quizzes = _quizRepository.GetAll(user, search, page);
            int total = _quizRepository.Total(search);
            return Ok(new
            {
                Quizzes = quizzes,
                Total = total,
                Page = page,
                PageSize = 10
            });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            Quiz quiz = _quizRepository.Get(id, user);
            return Ok(quiz);
        }

        [HttpPost]
        public IActionResult Create(Quiz quiz)
        {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            quiz.CreatorId = user.UserId;
            Quiz res = _quizRepository.Create(quiz);
            return Ok(res);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, Quiz quiz)
        {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            Quiz res = _quizRepository.Update(id, quiz, user);
            return Ok(res);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            bool isSuccess = _quizRepository.Delete(id, user);
            if (!isSuccess)
            {
                return BadRequest("Quiz is not exist");
            }
            return Ok("Delete successfully!");
        }

        [HttpGet]
        [Route("results/{id}")]
        public IActionResult Results(int id, string? search, int? page)
        {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            List<TakeQuiz> list = _quizRepository.ListResults(id, user, search, page);
            int total = _quizRepository.TotalQuizResult(id, search);
            return Ok(new
            {
                TakeQuizzes = list,
                Total = total,
                Page = page,
                PageSize = 10
            });
        }

        [HttpGet]
        [Route("review/{id}")]
        public IActionResult ReviewQuiz(int id) {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            ReviewQuizDtoResponse result = _quizRepository.ReviewQuiz(id, user);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("student")]
        public IActionResult GetAllForStudent()
        {
            List<Quiz> quizzes = _quizRepository.GetAll();
            return Ok(quizzes);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("student/{id}")]
        public IActionResult GetForStudent(int id)
        {
            Quiz quiz = _quizRepository.Get(id);
            return Ok(quiz);
        }
    }
}
