using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizSystemApi.Dto.Request;
using QuizSystemApi.Models;

namespace QuizSystemWeb.Controllers
{
    public class UsersController : Controller
    {
        //private readonly DBContext _context;

        //public UsersController(DBContext context)
        //{
        //    _context = context;
        //}

        //// GET: Users
        //public async Task<IActionResult> Index()
        //{
        //    var dBContext = _context.Users.Include(u => u.Role);
        //    return View(await dBContext.ToListAsync());
        //}
        private readonly HttpClient client = null;
        private string url = "";

        public UsersController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            url = "https://localhost:7049/api/";
        }

        public async Task<IActionResult> Teachers()
        {
            //var dBContext = _context.Users.Include(u => u.Role);
            return View();
        }
        
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterDtoRequest user)
        {
            try
            {
                JsonContent content = JsonContent.Create(user);
                var response = await client.PostAsync($"{url}register", content);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    //var user = JsonSerializer.Deserialize<User>(json, options);
                    //if (user.Role.RoleDesc == "user")
                    //{
                    //    HttpContext.Session.SetInt32("id", user.UserId);
                    //    HttpContext.Session.SetString("name", user.FirstName + user.LastName);
                    //    HttpContext.Session.SetString("role", user.Role.RoleDesc);
                    //    return Redirect("./users/edit/" + user.UserId);
                    //}
                    //else if (user.Role.RoleDesc == "admin")
                    //{
                    //    HttpContext.Session.SetInt32("id", user.UserId);
                    //    HttpContext.Session.SetString("name", user.FirstName + user.LastName);
                    //    HttpContext.Session.SetString("role", user.Role.RoleDesc);
                    //    return RedirectToAction("Index", "Users");
                    //}
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Users/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .Include(u => u.Role)
        //        .FirstOrDefaultAsync(m => m.UserId == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// GET: Users/Create
        //public IActionResult Create()
        //{
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
        //    return View();
        //}

        //// POST: Users/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UserId,Username,Password,RoleId,FullName,PhoneNumber,CreateAt,UpdateAt,IsEnable")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", user.RoleId);
        //    return View(user);
        //}

        //// GET: Users/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", user.RoleId);
        //    return View(user);
        //}

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password,RoleId,FullName,PhoneNumber,CreateAt,UpdateAt,IsEnable")] User user)
        //{
        //    if (id != user.UserId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.UserId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", user.RoleId);
        //    return View(user);
        //}

        //// GET: Users/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .Include(u => u.Role)
        //        .FirstOrDefaultAsync(m => m.UserId == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Users == null)
        //    {
        //        return Problem("Entity set 'DBContext.Users'  is null.");
        //    }
        //    var user = await _context.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _context.Users.Remove(user);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(int id)
        //{
        //  return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        //}
    }
}
