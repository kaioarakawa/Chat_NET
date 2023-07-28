using ChatApp.Data;
using ChatApp.Hubs;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Diagnostics;

namespace ChatApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext  _context;
        public readonly UserManager<AppUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

		//public async Task<IActionResult> Index()
		//{
		//    var currentUser = await _userManager.GetUserAsync(User);
		//    if(User.Identity.IsAuthenticated)
		//    {
		//        ViewBag.CurretUserName = currentUser.UserName;
		//    }
		//    var messages = await _context.Messages.ToListAsync();
		//    return View(messages);
		//}

		public async Task<IActionResult> Index()
		{
			var sender = await _userManager.GetUserAsync(User);

            var friends = _context.Friends.Where(x => x.UserId == sender.Id || x.UserFriendId == sender.Id).Select(x => x.UserFriendId).ToList();

			var users = _userManager.Users.Where(x => x.Id != sender.Id && friends.Contains(x.Id));

			return View(users);
		}

		public async Task<IActionResult> ChatUser(AppUser user)
		{
			try
			{
				var currentUser = await _userManager.GetUserAsync(User);
				var friend = _userManager.Users.Where(x => x.Id.Equals(user.Id)).FirstOrDefault();

				var relation = _context.Friends.Where(x => x.UserId == currentUser.Id && x.UserFriendId == friend.Id).FirstOrDefault();

				if (relation == null)
				{
					throw new Exception("Usuarios ainda não são amigos");
				}

				if (User.Identity.IsAuthenticated)
				{
					ViewBag.CurretUserName = currentUser.UserName;
				}
				 
				var messages = _context.Messages.Where(x => (x.UserID == currentUser.Id && x.ToUserId == friend.Id) || (x.ToUserId == currentUser.Id && x.UserID == friend.Id));

				currentUser.Messages.AddRange(messages);
				currentUser.Friends.Add(relation);

				return View(@$"Chat", currentUser);
			}
			catch (Exception)
			{
				return Error();
				throw;
			}
		}

		public async Task<IActionResult> Create(Message message)
        {
            try
            {
                message.Username = User.Identity.Name;
                var sender = await _userManager.GetUserAsync(User);
                //message.UserID = sender.Id;
                //message.ToUserId = sender.Id;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return Error();
                throw;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}