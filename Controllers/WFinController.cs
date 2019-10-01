using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using DailyMarker.Data;
using DailyMarker.Models;

using DailyMarker.ViewModel;

namespace DailyMarker.Controllers
{
    public class TaskViewModel
    {
        public string year { get; set; }
    }
    public class WFinController : Controller
    {
        private readonly TaskContext _context;
        public WFinController(TaskContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            DateTime dateObj = new DateTime();

            int count = 0;

            List<TaskData> taskList = new List<TaskData>();
            string s = "";
            var tasks = _context.BasicTasks;
            foreach (BasicTask t in tasks)
            {
                count++;

                taskList.Add(new TaskData
                {
                    Name = t.Name,
                    TaskID = t.Id
                });

                await _context.Entry(t).Collection(_t => _t.TaskDateTasks).LoadAsync();
                TaskDateTask tt = t.TaskDateTasks.First();

                await _context.Entry(tt).Reference(x => x.TaskDate).LoadAsync();

                //s += "Name " + t.Name + ": " + tt.TaskDate.TDate.ToString();
            }
            ViewData["taskCount"] = count;
            ViewData["tasks"] = taskList;
            //return View(taskList);
            return View();
        }

        [HttpPost]
        public IActionResult GetData([FromBody]TaskViewModel tvm)
        {
            var data = new { Name = "test", Number = 40, Year = tvm.year, };
            return Json(data);
        }

        public IActionResult GetTasks(int year, int month)
        {
            var data = new { year = year, month = month };
            return Json(data);
        }
    }
}