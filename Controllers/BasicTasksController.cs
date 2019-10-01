using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DailyMarker.Data;
using DailyMarker.Models;

namespace DailyMarker.Controllers
{
    public class BasicTasksController : Controller
    {
        private readonly TaskContext _context;

        public BasicTasksController(TaskContext context)
        {
            _context = context;
        }

        // GET: BasicTasks
        public async Task<IActionResult> Index()
        {

            return View(await _context.BasicTasks.ToListAsync());
        }

        // GET: BasicTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicTask = await _context.BasicTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basicTask == null)
            {
                return NotFound();
            }

            return View(basicTask);
        }

        // GET: BasicTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BasicTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Note")] BasicTask basicTask)
        {
            if (ModelState.IsValid)
            {
                DateTime todayDate = DateTime.Today;

                var dates = _context.TaskDates
                                    .Where(td => td.TDate == todayDate);
                TaskDate taskdate;
                if (!dates.Any())
                {
                    taskdate = new TaskDate { TDate = todayDate };
                    _context.Add(taskdate);
                }
                else
                {
                    taskdate = dates.First();
                }

                TaskDateTask tdt = new TaskDateTask { TaskDate = taskdate, BasicTask = basicTask };
                _context.TaskDateTask.Add(tdt);

                taskdate.TaskDateTasks.Add(tdt);
                basicTask.TaskDateTasks.Add(tdt);

                _context.Add(basicTask);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "WFin");
            }
            return View(basicTask);
        }

        // GET: BasicTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicTask = await _context.BasicTasks.FindAsync(id);
            if (basicTask == null)
            {
                return NotFound();
            }
            return View(basicTask);
        }

        // POST: BasicTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Note")] BasicTask basicTask)
        {
            if (id != basicTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basicTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasicTaskExists(basicTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(basicTask);
        }

        // GET: BasicTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basicTask = await _context.BasicTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basicTask == null)
            {
                return NotFound();
            }

            return View(basicTask);
        }

        // POST: BasicTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basicTask = await _context.BasicTasks.FindAsync(id);
            _context.BasicTasks.Remove(basicTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BasicTaskExists(int id)
        {
            return _context.BasicTasks.Any(e => e.Id == id);
        }
    }
}
