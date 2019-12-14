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
    public class DailyTask_TaskDateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyTask_TaskDateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailyTask_TaskDate
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DailyTask_TaskDates.Include(d => d.DailyTask).Include(d => d.TaskDate);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DailyTask_TaskDate/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyTask_TaskDate = await _context.DailyTask_TaskDates
                .Include(d => d.DailyTask)
                .Include(d => d.TaskDate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyTask_TaskDate == null)
            {
                return NotFound();
            }

            return View(dailyTask_TaskDate);
        }

        // GET: DailyTask_TaskDate/Create
        public IActionResult Create()
        {
            ViewData["DailyTaskId"] = new SelectList(_context.DailyTasks, "Id", "Id");
            ViewData["TaskDateId"] = new SelectList(_context.TaskDates, "Id", "Id");
            return View();
        }

        // POST: DailyTask_TaskDate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DailyTaskId,TaskDateId")] DailyTask_TaskDate dailyTask_TaskDate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyTask_TaskDate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DailyTaskId"] = new SelectList(_context.DailyTasks, "Id", "Id", dailyTask_TaskDate.DailyTaskId);
            ViewData["TaskDateId"] = new SelectList(_context.TaskDates, "Id", "Id", dailyTask_TaskDate.TaskDateId);
            return View(dailyTask_TaskDate);
        }

        // GET: DailyTask_TaskDate/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyTask_TaskDate = await _context.DailyTask_TaskDates.FindAsync(id);
            if (dailyTask_TaskDate == null)
            {
                return NotFound();
            }
            ViewData["DailyTaskId"] = new SelectList(_context.DailyTasks, "Id", "Id", dailyTask_TaskDate.DailyTaskId);
            ViewData["TaskDateId"] = new SelectList(_context.TaskDates, "Id", "Id", dailyTask_TaskDate.TaskDateId);
            return View(dailyTask_TaskDate);
        }

        // POST: DailyTask_TaskDate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DailyTaskId,TaskDateId")] DailyTask_TaskDate dailyTask_TaskDate)
        {
            if (id != dailyTask_TaskDate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyTask_TaskDate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyTask_TaskDateExists(dailyTask_TaskDate.Id))
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
            ViewData["DailyTaskId"] = new SelectList(_context.DailyTasks, "Id", "Id", dailyTask_TaskDate.DailyTaskId);
            ViewData["TaskDateId"] = new SelectList(_context.TaskDates, "Id", "Id", dailyTask_TaskDate.TaskDateId);
            return View(dailyTask_TaskDate);
        }

        // GET: DailyTask_TaskDate/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyTask_TaskDate = await _context.DailyTask_TaskDates
                .Include(d => d.DailyTask)
                .Include(d => d.TaskDate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyTask_TaskDate == null)
            {
                return NotFound();
            }

            return View(dailyTask_TaskDate);
        }

        // POST: DailyTask_TaskDate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyTask_TaskDate = await _context.DailyTask_TaskDates.FindAsync(id);
            _context.DailyTask_TaskDates.Remove(dailyTask_TaskDate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyTask_TaskDateExists(int id)
        {
            return _context.DailyTask_TaskDates.Any(e => e.Id == id);
        }
    }
}
