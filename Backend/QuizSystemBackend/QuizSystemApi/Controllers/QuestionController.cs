using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Models;
using QuizSystemApi.Repository;
using QuizSystemApi.Repository.IRepository;
using System.Data;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/question")]
    [Authorize(Roles = "Admin, Teacher")]
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        [HttpGet]
        public IActionResult GetListByQuizId(int quizId)
        {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            List<Question> questions = _questionRepository.GetListByQuizId(quizId, user);
            return Ok(questions);
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Route("quizKey")]
        public IActionResult GetKeyOfQuiz(int quizId)
        {
            List<Question> questions = _questionRepository.GetKeyOfQuiz(quizId);
            return Ok(questions);
        }

        [HttpPost]
        public IActionResult Create(Question question)
        {
            Question res = _questionRepository.Create(question);
            return Ok(res);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, Question question)
        {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            Question res = _questionRepository.Update(id, question, user);
            return Ok(res);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            User user = TokenHelper.GetUserFromToken(HttpContext);
            bool isSuccess = _questionRepository.Delete(id, user);
            if (!isSuccess)
            {
                return BadRequest("Question is not exist");
            }
            return Ok("Delete successfully!");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("student")]
        public IActionResult GetListByQuizIdForStudent(int quizId)
        {
            List<Question> questions = _questionRepository.GetListByQuizId(quizId);
            return Ok(questions);
        }
    }
}
