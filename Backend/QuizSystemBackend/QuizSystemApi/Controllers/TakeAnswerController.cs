using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Dao;
using QuizSystemApi.Models;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/takeAnswer")]
    public class TakeAnswerController : Controller
    {
        [HttpPost]
        public IActionResult Create(List<TakeAnswer> takes)
        {
            List<TakeAnswer> t = TakeAnswerDao.Create(takes);
            return Ok(t);
        }
    }
}
