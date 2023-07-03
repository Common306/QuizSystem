using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/answer")]
    public class AnswerController : Controller
    {

        private readonly IAnswerRepository _answerRepository;

        public AnswerController(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }
        [HttpGet]
        public IActionResult GetList(int questionId)
        {
            List<Answer> answers = _answerRepository.GetListByQuestionId(questionId);
            return Ok(answers);
        }

        [HttpPost]
        public IActionResult Create(List<Answer> answers)
        {
            List<Answer> res = _answerRepository.CreateListAnswer(answers);
            return Ok(res);
        }
    }
}
