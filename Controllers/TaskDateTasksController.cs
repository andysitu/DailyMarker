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
    public class TaskDateTasksController : Controller
    {
        private readonly TaskContext _context;

        public TaskDateTasksController(TaskContext context)
        {
            _context = context;
        }

        // GET: TaskDateTasks
        public async Task<IActionResult> Index()
        {
            var taskContext = _context.TaskDateTask.Include(t => t.BasicTask).Include(t => t.TaskDate);
            return View(await taskContext.ToListAsync());
        }

        // GET: TaskDateTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskDateTask = await _context.TaskDateTask
                .Include(t => t.BasicTask)
                .Include(t => t.TaskDate)
                .FirstOrDefaultAsync(m => m.BasicTaskId == id);
            if (taskDateTask == null)
            {
                return NotFound();
            }

            return View(taskDateTask);
        }

        // GET: TaskDateTasks/Create
        public IActionResult Create()
        {
            ViewData["BasicTaskId"] = new SelectList(_context.BasicTasks, "Id", "Id");
            ViewData["TaskDateId"] = new SelectList(_context.TaskDates, "Id", "Id");
            return View();
        }

        // POST: TaskDateTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BasicTaskId,TaskDateId")] TaskDateTask taskDateTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskDateTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BasicTaskId"] = new SelectList(_context.BasicTasks, "Id", "Id", taskDateTask.BasicTaskId);
            ViewData["TaskDateId"] = new SelectList(_context.TaskDates, "Id", "Id", taskDateTask.TaskDateId);
            return View(taskDateTask);
        }

        // GET: TaskDateTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskDateTask = await _context.TaskDateTask.FindAsync(id);
            if (taskDateTask == null)
            {
                return NotFound();
            }
            ViewData["BasicTaskId"] = new SelectList(_context.BasicTasks, "Id", "Id", taskDateTask.BasicTaskId);
            ViewData["TaskDateId"] = new SelectList(_context.TaskDates, "Id", "Id", taskDateTask.TaskDateId);
            return View(taskDateTask);
        }

        // POST: TaskDateTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BasicTaskId,TaskDateId")] TaskDateTask taskDateTask)
        {
            if (id != taskDateTask.BasicTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskDateTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskDateTaskExists(taskDateTask.BasicTaskId))
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
            ViewData["BasicTaskId"] = new SelectList(_context.BasicTasks, "Id", "Id", taskDateTask.BasicTaskId);
            ViewData["TaskDateId"] = new SelectList(_context.TaskDates, "Id", "Id", taskDateTask.TaskDateId);
            return View(taskDateTask);
        }

        // GET: TaskDateTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskDateTask = await _context.TaskDateTask
                .Include(t => t.BasicTask)
                .Include(t => t.TaskDate)
                .FirstOrDefaultAsync(m => m.BasicTaskId == id);
            if (taskDateTask == null)
            {
                return NotFound();
            }

            return View(taskDateTask);
        }

        // POST: TaskDateTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskDateTask = await _context.TaskDateTask.FindAsync(id);
            _context.TaskDateTask.Remove(taskDateTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskDateTaskExists(int id)
        {
            return _context.TaskDateTask.Any(e => e.BasicTaskId == id);
        }
    }
}
