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
using Microsoft.EntityFrameworkCore;

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

        private TableTask GetTable()
        {
            UserAccount u = GetUser();
            if (u != null)
            {
                TableTask tt = _context.TableTasks.
                    Where(tt => tt.UserAccountId.Equals(u.Id)).First();

                _context.Entry(tt).Collection(p => p.DailyTasks).Load();

                return tt;
            }
            else
                return null;
        }

        private List<DailyTask> GetTasks()
        {
            UserAccount u = GetUser();
            if (u != null)
            {
                var tt = GetTable();

                _context.Entry(tt).Collection(p => p.DailyTasks).Load();
                return tt.DailyTasks;
            }
            else
                return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_TaskName_Ajax(
            [FromForm] string task_name, [FromForm] int task_id)
        {
            if (task_name != null && task_name.Length > 0
                && !String.IsNullOrEmpty(task_id.ToString()))
            {
                UserAccount u = GetUser();
                if (u != null)
                {
                    var tt = GetTable();

                    var task = _context.DailyTasks.First(
                        d => d.TableTaskId == tt.Id &&
                            d.Id == task_id
                        );
                    task.name = task_name;
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
            }
            return Json(new { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete_Task_Ajax(
            [FromForm] int task_id)
        {
            if (!String.IsNullOrEmpty(task_id.ToString()))
            {
                UserAccount u = GetUser();
                if (u != null)
                {
                    var tt = GetTable();

                    var task = _context.DailyTasks.First(
                        d => d.TableTaskId == tt.Id &&
                            d.Id == task_id
                        );
                    _context.DailyTasks.Remove(task);
                    await _context.SaveChangesAsync();
                }
            }
            return Json(new { Delete = "delete" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add_Task_Ajax([FromForm] string task_name)
        {
            if (task_name != null && task_name.Length > 0)
            {
                var tt = GetTable();
                var task = new DailyTask
                {
                    name = task_name,
                    TableTaskId = tt.Id
                };
                _context.Add(task);
                await _context.SaveChangesAsync();

                return Json(new { Name = task_name, Id =  task.Id});
            }
            return Json(new { });            
        }

        // Gets Dictionary of the tasks asychronously only
        public string Get_Tasks_Json(int year, int month)
        {
            UserAccount u = GetUser();
            if (u != null)
            {
                var options = new JsonSerializerOptions
                {
                    //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                var tasks = GetTasks();

                if (String.IsNullOrEmpty(year.ToString()) || 
                    String.IsNullOrEmpty(month.ToString()))
                {
                    DateTime date = DateTime.Now;
                    year = date.Year;
                    month = date.Month;
                }
                
                var firstDay = new DateTime(year, month, 1);
                var lastDay = firstDay.AddMonths(1).AddDays(-1);

                Dictionary<string, Dictionary<string, string>> tasks_dictionary =
                    new Dictionary<string, Dictionary<string, string>>();

                int task_id;
                Dictionary<string, string> task_dict;
                foreach (DailyTask t in tasks)
                {
                    task_id = t.Id;
                    task_dict = new Dictionary<string, string>();
                    _context.DailyTask_TaskDates.Where(
                        dt_t => dt_t.DailyTaskId == task_id &&
                        dt_t.TaskDate.TDate >= firstDay &&
                        dt_t.TaskDate.TDate <= lastDay).Include(td_td=>td_td.TaskDate).Load();
                    string task_dates = "";
                    if (t.DailyTask_TaskDates != null)
                    {
                        foreach (DailyTask_TaskDate dt_t in t.DailyTask_TaskDates)
                        {
                            _context.TaskDates.Where(td => dt_t.TaskDateId == td.Id).Load();
                            TaskDate td = dt_t.TaskDate;
                            task_dates += td.TDate.ToString("yyyy-MM-dd") + "_";
                        }
                        task_dates.TrimEnd('_');
                    }

                    task_dict.Add("name", t.name);
                    task_dict.Add("date_string", task_dates);
                    tasks_dictionary.Add(t.Id.ToString(), task_dict);
                }
                var tasks_json = JsonSerializer.Serialize(tasks_dictionary, tasks_dictionary.GetType(), options);
                return tasks_json;
            }
            return "{}";
        }

        public class TaskDateRequestModel
        {
            public int task_id { get; set; }
            public int day { get; set; }
            public int month { get; set; }
            public int year { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Set_Taskdate_Ajax([FromBody] TaskDateRequestModel t)
            public string Set_Taskdate_Ajax([FromBody] TaskDateRequestModel t)
        {
            if (t != null)
            {
                UserAccount u = GetUser();

                int task_id = t.task_id;

                // Check that the task belongs to user
                var task = _context.DailyTasks.Include(
                            t=>t.TableTask
                        ).First(
                            t => t.Id == task_id);
                if (task.TableTask.UserAccountId != u.Id)
                {
                    return "";
                } 

                var d = new DateTime(t.year, t.month, t.day);
                var td = _context.TaskDates.
                    FirstOrDefault(dt => dt.TDate == d);

                if (td == null)
                {
                    td = new TaskDate { TDate = d };
                    _context.Add(td);
                    _context.SaveChanges();
                }

                var dt_td = _context.DailyTask_TaskDates.
                    FirstOrDefault(dt => dt.DailyTaskId == task_id &&
                        dt.TaskDateId == td.Id
                    );

                if (dt_td == null)
                {
                    dt_td = new DailyTask_TaskDate
                    {
                        DailyTaskId = task_id,
                        //DailyTask = task,
                        TaskDateId = td.Id,
                        //TaskDate = td
                    };
                    _context.Add(dt_td);
                    _context.SaveChanges();
                } else  // Remove
                {
                    _context.DailyTask_TaskDates.Remove(dt_td);
                    _context.SaveChanges();
                }

                return t.task_id.ToString();
            }
            return "";
        }

        // GET: Main
        public ActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            return View();
        }

    }
}