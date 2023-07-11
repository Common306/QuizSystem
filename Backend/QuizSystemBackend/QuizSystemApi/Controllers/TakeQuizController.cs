using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Dao;
using QuizSystemApi.Models;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/takeQuiz")]
    public class TakeQuizController : Controller
    {
        [HttpPost]
        public IActionResult Create(TakeQuiz take)
        {
            TakeQuiz t = TakeQuizDao.Create(take);
            return Ok(t);
        }
    }
}
