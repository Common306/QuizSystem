using Microsoft.AspNetCore.Mvc;

namespace QuizSystemWeb.Controllers
{
    public class QuestionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
