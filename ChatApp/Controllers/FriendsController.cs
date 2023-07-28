using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Linq;

namespace ChatApp.Controllers
{
	[Authorize]
	public class FriendsController : Controller
	{
		public readonly ApplicationDbContext _context;
		public readonly UserManager<AppUser> _userManager;

		public FriendsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
			_context = context;
			_userManager = userManager;
		}

        public async Task<IActionResult> Index()
		{
			var sender = await _userManager.GetUserAsync(User);
			var friends = _context.Friends.Where(x => x.UserId == sender.Id || x.UserFriendId == sender.Id).Select(x => x.UserFriendId).ToList();

			var users = _userManager.Users.Where(x => x.Id != sender.Id && !friends.Contains(x.Id));

			return View(users);
		}

		[HttpGet]  
		public IActionResult Create()
		{
			return View();
		}

		public async Task<IActionResult> Create(AppUser user)
		{
			try
			{
				var sender = await _userManager.GetUserAsync(User);
				var friend = _userManager.Users.Where(x => x.Id.Equals(user.Id)).FirstOrDefault();

				List<Friends> friends = new List<Friends>();

				friends.Add(new Friends()
				{
					User = sender,
					UserFriend = friend,
					IsConfirmed = false,
				});

                friends.Add(new Friends()
                {
                    User = friend,
                    UserFriend = sender,
                    IsConfirmed = false,
                });

                await _context.Friends.AddRangeAsync(friends);
                await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			catch (Exception)
			{
				return Error();

				throw;
			}
		}

		public async Task<IActionResult> Delete(AppUser user)
		{
			try
			{
				
				var sender = await _userManager.GetUserAsync(User);
				var friend = _userManager.Users.Where(x => x.Id.Equals(user.Id)).FirstOrDefault();
				var friendRelation = _context.Friends.Where(x => (x.UserId == sender.Id && x.UserFriendId == friend.Id) || (x.UserId == friend.Id && x.UserFriendId == sender.Id));

				if(friendRelation != null)
				{
					_context.RemoveRange(friendRelation);
					await _context.SaveChangesAsync();
				}
				else
				{
					throw new Exception("Amigo não encontrado");
				}
				
				return RedirectToAction("List");
			}
			catch (Exception)
			{
				return Error();

				throw;
			}
		}

		public async Task<IActionResult> List()
		{
			var user = await _userManager.GetUserAsync(User);
            var friends = _context.Friends.Where(x => x.UserId == user.Id || x.UserFriendId == user.Id).Select(x => x.UserFriendId).ToList();
            var userFriends = _userManager.Users.Where(x => friends.Contains(x.Id));

			return View(userFriends);
		}

		public async Task<IActionResult> Details(int id)
		{
			var query = from m in _context.Friends
						join ff in _context.Users on
							new { m.UserFriend.Id } equals new { ff.Id }
						join ft in _context.Users on
							new { m.User.Id } equals new { ft.Id }
						where m.ID == id
						select m;

			return View(query.Single());
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
