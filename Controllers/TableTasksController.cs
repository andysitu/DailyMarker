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
    public class TableTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TableTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TableTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TableTasks.Include(t => t.UserAccount);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TableTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tableTask = await _context.TableTasks
                .Include(t => t.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tableTask == null)
            {
                return NotFound();
            }

            return View(tableTask);
        }

        // GET: TableTasks/Create
        public IActionResult Create()
        {
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "Id");
            return View();
        }

        // POST: TableTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserAccountId")] TableTask tableTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tableTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "Id", tableTask.UserAccountId);
            return View(tableTask);
        }

        // GET: TableTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tableTask = await _context.TableTasks.FindAsync(id);
            if (tableTask == null)
            {
                return NotFound();
            }
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "Id", tableTask.UserAccountId);
            return View(tableTask);
        }

        // POST: TableTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserAccountId")] TableTask tableTask)
        {
            if (id != tableTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tableTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableTaskExists(tableTask.Id))
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
            ViewData["UserAccountId"] = new SelectList(_context.UserAccounts, "Id", "Id", tableTask.UserAccountId);
            return View(tableTask);
        }

        // GET: TableTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tableTask = await _context.TableTasks
                .Include(t => t.UserAccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tableTask == null)
            {
                return NotFound();
            }

            return View(tableTask);
        }

        // POST: TableTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tableTask = await _context.TableTasks.FindAsync(id);
            _context.TableTasks.Remove(tableTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableTaskExists(int id)
        {
            return _context.TableTasks.Any(e => e.Id == id);
        }
    }
}
