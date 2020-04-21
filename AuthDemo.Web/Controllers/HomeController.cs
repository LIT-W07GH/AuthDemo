using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AuthDemo.Data;
using Microsoft.AspNetCore.Mvc;
using AuthDemo.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=AuthDemo;Integrated Security=true;";


        public IActionResult Index()
        {
            var vm = new HomePageViewModel
            {
                IsLoggedIn = User.Identity.IsAuthenticated //tells us if current user is logged in
            };
            return View(vm);
        }

        [Authorize]
        public IActionResult Secret()
        {
            var db = new UserDb(_connectionString);

            //User.Identity.Name will always return the value placed into the cookie
            //when this user was logged in. Something like this:
            /* var claims = new List<Claim>
            {
                new Claim("user", email) <--- this value, the email, is the User.Identity.Name
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            */
            var user = db.GetByEmail(User.Identity.Name);
            return View(user);
        }
    }
}
