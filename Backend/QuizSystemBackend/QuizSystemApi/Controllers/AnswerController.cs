using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;
using System.Data;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/answer")]
    [Authorize(Roles = "Admin, Teacher")]
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

        [HttpPut]
        public IActionResult Update(List<Answer> answers)
        {
            List<Answer> res = _answerRepository.UpdateListAnswer(answers);
            return Ok(res);
        }
    }
}
