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
    public class ProjectCommandsController : Controller
    {
        private readonly ITCompanyContext _context;

        public ProjectCommandsController(ITCompanyContext context)
        {
            _context = context;
        }

        // GET: ProjectCommands
        public async Task<IActionResult> Index()
        {
            var iTCompanyContext = _context.ProjectCommands.Include(p => p.Employee).Include(p => p.Project); 
            return View(await iTCompanyContext.ToListAsync());
        }

        // GET: ProjectCommands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectCommand = await _context.ProjectCommands
                .Include(p => p.Employee)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (projectCommand == null)
            {
                return NotFound();
            }

            return View(projectCommand);
        }

        // GET: ProjectCommands/Create
        public IActionResult Create(int? id)
        {
            ViewData["EmployeeID"] = new SelectList(_context.Users.Where(x=>(x.RoleID != _context.Roles.First(x=>(x.Name.Equals("Клієнт") || x.Name.Equals("Директор") || x.Name.Equals("HR Manager"))).ID) && !_context.ProjectCommands.Where(x => x.ProjectID == id && x.EmployeeID != x.ID).Select(x=>x.EmployeeID).Contains(x.ID)), "ID", "FullName");
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID", (int)id);
            return View();
        }

        // POST: ProjectCommands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,ProjectID,isPrime")] ProjectCommand projectCommand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectCommand);
                await _context.SaveChangesAsync();
                return Redirect("/Projects/Details/"+projectCommand.ProjectID);
            }
            ViewData["EmployeeID"] = new SelectList(_context.Users, "ID", "ID", projectCommand.EmployeeID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID", projectCommand.ProjectID);
            return View(projectCommand);
        }

        // GET: ProjectCommands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectCommand = await _context.ProjectCommands.FindAsync(id);
            if (projectCommand == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Users, "ID", "ID", projectCommand.EmployeeID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID", projectCommand.ProjectID);
            return View(projectCommand);
        }

        // POST: ProjectCommands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,EmployeeID,ProjectID,isPrime")] ProjectCommand projectCommand)
        {
            if (id != projectCommand.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectCommand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectCommandExists(projectCommand.ID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Users, "ID", "ID", projectCommand.EmployeeID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID", projectCommand.ProjectID);
            return View(projectCommand);
        }

        // GET: ProjectCommands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectCommand = await _context.ProjectCommands
                .Include(p => p.Employee)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (projectCommand == null)
            {
                return NotFound();
            }

            return View(projectCommand);
        }

        // POST: ProjectCommands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectCommand = await _context.ProjectCommands.FindAsync(id);
            _context.ProjectCommands.Remove(projectCommand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectCommandExists(int id)
        {
            return _context.ProjectCommands.Any(e => e.ID == id);
        }
    }
}
