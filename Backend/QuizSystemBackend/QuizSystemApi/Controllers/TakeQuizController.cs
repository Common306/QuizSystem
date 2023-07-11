using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Dao;
using QuizSystemApi.Models;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/takeQuiz")]
    public class TakeQuizController : Controller
    {
        [HttpGet]
        public IActionResult GetList()
        {
            List<TakeQuiz> takes = TakeQuizDao.GetAll();
            return Ok(takes);
        }

        [HttpPost]
        public IActionResult Create(TakeQuiz take)
        {
            TakeQuiz t = TakeQuizDao.Create(take);
            return Ok(t);
        }
    }
}
