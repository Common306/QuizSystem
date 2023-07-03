using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Models;
using QuizSystemApi.Repository;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/question")]
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
            List<Question> questions = _questionRepository.GetListByQuizId(quizId);
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
            Question res = _questionRepository.Update(id, question);
            return Ok(res);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            bool isSuccess = _questionRepository.Delete(id);
            if (!isSuccess)
            {
                return BadRequest("Question is not exist");
            }
            return Ok("Delete successfully!");
        }
    }
}
