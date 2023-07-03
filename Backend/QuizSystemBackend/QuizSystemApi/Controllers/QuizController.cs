using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Models;
using QuizSystemApi.Repository;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/quiz")]
    public class QuizController : Controller
    {
        private readonly IQuizRepository _quizRepository;

        public QuizController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [HttpGet]
        public IActionResult GetAll() {
            List<Quiz> quizzes = _quizRepository.GetAll();
            return Ok(quizzes);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            Quiz quiz = _quizRepository.Get(id);
            return Ok(quiz);
        }

        [HttpPost]
        public IActionResult Create(Quiz quiz)
        {
            Quiz res = _quizRepository.Create(quiz);
            return Ok(res);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, Quiz quiz)
        {
            Quiz res = _quizRepository.Update(id, quiz);
            return Ok(res);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            bool isSuccess = _quizRepository.Delete(id);
            if (!isSuccess)
            {
                return BadRequest("Quiz is not exist");
            }
            return Ok("Delete successfully!");
        }
    }
}
