using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITCompany.Data;
using ITCompany.Models;

namespace ITCompany.Controllers
{
    public class TaskCommandsController : Controller
    {
        private readonly ITCompanyContext _context;

        public TaskCommandsController(ITCompanyContext context)
        {
            _context = context;
        }

        // GET: TaskCommands
        public async Task<IActionResult> Index()
        {
            var iTCompanyContext = _context.TaskCommands.Include(t => t.Employee).Include(t => t.Task);
            return View(await iTCompanyContext.ToListAsync()); 
        }

        // GET: TaskCommands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskCommand = await _context.TaskCommands
                .Include(t => t.Employee)
                .Include(t => t.Task)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taskCommand == null)
            {
                return NotFound();
            }

            return View(taskCommand);
        }

        // GET: TaskCommands/Create
        public IActionResult Create(int? id)
        {
            var task = _context.Tasks.First(x => x.ID == id);
            var users = _context.Users.Where(x => _context.ProjectCommands.Where(x => x.ProjectID == task.ProjectID).Select(x => x.EmployeeID).Contains(x.ID) &&
            !_context.TaskCommands.Where(x=>x.TaskID == id).Select(x=>x.EmployeeID).Contains(x.ID));
            ViewData["EmployeeID"] = new SelectList(users, "ID", "FullName");
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "ID", (int)id);
            return View();
        }

        // POST: TaskCommands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,TaskID")] TaskCommand taskCommand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskCommand);
                await _context.SaveChangesAsync();
                return Redirect("/Tasks/Details/"+taskCommand.TaskID);
            }
            ViewData["EmployeeID"] = new SelectList(_context.Users, "ID", "ID", taskCommand.EmployeeID);
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "ID", taskCommand.TaskID);
            return View(taskCommand);
        }

        // GET: TaskCommands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskCommand = await _context.TaskCommands.FindAsync(id);
            if (taskCommand == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Users, "ID", "ID", taskCommand.EmployeeID);
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "ID", taskCommand.TaskID);
            return View(taskCommand);
        }

        // POST: TaskCommands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,EmployeeID,TaskID")] TaskCommand taskCommand)
        {
            if (id != taskCommand.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskCommand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskCommandExists(taskCommand.ID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Users, "ID", "ID", taskCommand.EmployeeID);
            ViewData["TaskID"] = new SelectList(_context.Tasks, "ID", "ID", taskCommand.TaskID);
            return View(taskCommand);
        }

        // GET: TaskCommands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskCommand = await _context.TaskCommands
                .Include(t => t.Employee)
                .Include(t => t.Task)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taskCommand == null)
            {
                return NotFound();
            }

            return View(taskCommand);
        }

        // POST: TaskCommands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskCommand = await _context.TaskCommands.FindAsync(id);
            _context.TaskCommands.Remove(taskCommand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskCommandExists(int id)
        {
            return _context.TaskCommands.Any(e => e.ID == id);
        }
    }
}
