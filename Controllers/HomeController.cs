using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DailyMarker.Models;

using Microsoft.AspNetCore.Identity;

using DailyMarker.Data;

namespace DailyMarker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, 
                            UserManager<IdentityUser> userManager,
                            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public string Test()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null && userId.Length > 0)
            {
                UserAccount user;
                var userAccounts = from u in _context.UserAccounts
                                   select u;
                userAccounts = userAccounts.Where(u => u.UserId.Equals(userId));
                if (userAccounts.Any())
                    user = userAccounts.First();
                else
                { // Create User & TableTask
                    user = new UserAccount { UserId = userId };
                    _context.Add(user);
                    _context.SaveChanges();

                    var tabletask = new TableTask
                    {
                        UserAccount = user,
                        UserAccountId = user.Id,
                        DailyTasks = new List<DailyTask>()
                    };

                    _context.Add(tabletask);

                    user.TableTask = tabletask;
                    user.TableTaskId = tabletask.Id;

                    _context.Update(user);

                    _context.SaveChanges();
                }
                return user.UserId;
            } else
            {
                return "Not Logged in";
            }
        }

        public IActionResult Index()
        {
            return View();
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
