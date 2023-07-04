using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace QuizSystemApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            List<User> users = _userRepository.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            User? user = _userRepository.Get(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            User res = _userRepository.Create(user);
            if (res == null)
            {
                return BadRequest("User has been exist");
            }
            return Ok(res);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, User user)
        {
            User res = _userRepository.Update(id, user);
            if (res == null)
            {
                return BadRequest();
            }
            return Ok(res);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            bool isSuccess = _userRepository.Delete(id);
            if(!isSuccess)
            {
                return BadRequest("User is not exist");
            }
            return Ok("Delete successfully!");
        }
    }
}
