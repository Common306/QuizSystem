using Microsoft.AspNetCore.Mvc;
using QuizSystemWeb.Models;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;

namespace QuizSystemWeb.Controllers
{
    [Route("user")]
	public class UserController : Controller
	{
		private readonly HttpClient client = null;
		private string UserApiUrl = "https://localhost:7049/api/user";
		public UserController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
		}
        [HttpGet]
        [Route("index")]
		public async Task<IActionResult> Index()
		{ 
			try
			{
				string? token = Request.Cookies["Token"];
				if (token == null)
				{
					return Unauthorized();
				}
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.GetAsync(UserApiUrl);
				string strData = await response.Content.ReadAsStringAsync();

				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				List<User>? users = JsonSerializer.Deserialize<List<User>>(strData, options);
				return View(users);
			} catch (Exception ex)
			{
				return Unauthorized();
			}
			
		}

        [HttpPost]
        [Route("create")]
		public async Task<IActionResult> Create(User user)
		{
            user.CreateAt = DateTime.Now;
            if(user.IsEnable == null)
            {
                user.IsEnable = false;
            }
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string jsonData = JsonSerializer.Serialize(user);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(UserApiUrl, content);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

		[HttpPost]
        [Route("edit")]
		public async Task<IActionResult> Edit(User user)
		{
            user.UpdateAt = DateTime.Now;
            if (user.IsEnable == null)
            {
                user.IsEnable = false;
            }
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    return Unauthorized();
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string jsonData = JsonSerializer.Serialize(user);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(UserApiUrl + "/" + user.UserId, content);
                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async void Delete(int id)
        {
            try
            {
                string? token = Request.Cookies["Token"];
                if (token == null)
                {
                    throw new AuthenticationException("You don't have permission");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.DeleteAsync(UserApiUrl + "/" + id);
                string strData = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("You don't have permission");
            }
        }
	}
}
