using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;

using DailyMarker.Data;

using DailyMarker.Models;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailyMarker.Controllers
{
    public class MainController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager; 

        public MainController(UserManager<IdentityUser> userManager,
                            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        private UserAccount GetUser()
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
                return user;
            }
            else
            {
                return null;
            }
        }

        private List<DailyTask> GetTasks()
        {
            UserAccount u = GetUser();
            if (u != null)
            {
                var tabletasks = from t in _context.TableTasks
                                 select t;

                tabletasks = tabletasks.Where(tt => tt.UserAccountId.Equals(u.Id));
                TableTask tt = tabletasks.First();
                _context.Entry(tt).Collection(p => p.DailyTasks).Load();

                return tt.DailyTasks;
            }
            else
                return null;
        }

        public string Test()
        {
            string s = "";
            UserAccount u = GetUser();
            if (u != null)
            {
                var options = new JsonSerializerOptions
                {
                    //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                var tasks = GetTasks();

                List<string> tasks_name = new List<string>();
                foreach (DailyTask d in tasks)
                {
                    tasks_name.Add(d.name);
                }

                var test = JsonSerializer.Serialize(tasks_name, tasks_name.GetType(), options);
                return test;
            }
            return s;
        }

        // GET: Main
        public ActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            
            return View();
        }

        // GET: Main/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Main/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Main/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Main/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Main/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Main/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Main/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}