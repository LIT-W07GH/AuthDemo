using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthDemo.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=AuthDemo;Integrated Security=true;";

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var db = new UserDb(_connectionString);
            db.AddUser(user, password);
            return Redirect("/account/signup");
        }

        public IActionResult Login()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Message = TempData["Error"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new UserDb(_connectionString);
            var user = db.Login(email, password);
            if (user == null)
            {
                TempData["Error"] = "Invalid login!";
                return Redirect("/account/login");
            }

            //this code logs in the current user!
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/secret");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}