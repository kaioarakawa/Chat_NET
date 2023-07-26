using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        public readonly ApplicationDbContext _context;
        public readonly UserManager<AppUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
			//var currentUser = await _userManager.GetUsersForClaimAsync();


			return View(users);
        }


		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}
	}
}
