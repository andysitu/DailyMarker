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
    public class DailyTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyTasksController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: DailyTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DailyTasks.Include(d => d.TableTask);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DailyTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyTask = await _context.DailyTasks
                .Include(d => d.TableTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyTask == null)
            {
                return NotFound();
            }

            return View(dailyTask);
        }

        // GET: DailyTasks/Create
        public IActionResult Create()
        {
            ViewData["TableTaskId"] = new SelectList(_context.TableTasks, "Id", "Id");
            return View();
        }

        // POST: DailyTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,TableTaskId")] DailyTask dailyTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TableTaskId"] = new SelectList(_context.TableTasks, "Id", "Id", dailyTask.TableTaskId);
            return View(dailyTask);
        }

        // GET: DailyTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyTask = await _context.DailyTasks.FindAsync(id);
            if (dailyTask == null)
            {
                return NotFound();
            }
            ViewData["TableTaskId"] = new SelectList(_context.TableTasks, "Id", "Id", dailyTask.TableTaskId);
            return View(dailyTask);
        }

        // POST: DailyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,TableTaskId")] DailyTask dailyTask)
        {
            if (id != dailyTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyTaskExists(dailyTask.Id))
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
            ViewData["TableTaskId"] = new SelectList(_context.TableTasks, "Id", "Id", dailyTask.TableTaskId);
            return View(dailyTask);
        }

        // GET: DailyTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyTask = await _context.DailyTasks
                .Include(d => d.TableTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyTask == null)
            {
                return NotFound();
            }

            return View(dailyTask);
        }

        // POST: DailyTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyTask = await _context.DailyTasks.FindAsync(id);
            _context.DailyTasks.Remove(dailyTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyTaskExists(int id)
        {
            return _context.DailyTasks.Any(e => e.Id == id);
        }
    }
}
