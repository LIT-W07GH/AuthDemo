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

/*Create an application where users can post simple ads for others to see. An add consists of a Title, Phone number and description. To create an ad, a user must be logged in. Here are the basic pages you'll need:

Home Page - on this page, display all the ads in the system. If the current user is logged in, and some of the ads belong to them, those ads should also have a delete button that gives that user the abilitiy to delete that ad.

New Ad - (Put a link to this page up in the nav bar using the layout page) - When this page is accessed, if the current user isn't logged in, they should be redirected to a login page. If the are logged in, they should be shown a form where they can create a new ad. 

My Account (this should be shown in the nav bar using the layout as well - however, this link should only be shown if the current user is logged in) - on this page, display all the ads that the current user has created as well as a delete button for each one.

Finally, in the nav bar on top, if the current user is logged in, display a Logout link. If they're NOT logged in, display a link called Login.

*/
