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
    public class TasksController : Controller
    {
        private readonly ITCompanyContext _context;

        public TasksController(ITCompanyContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var iTCompanyContext = _context.Tasks.Include(t => t.Project);
            return View(await iTCompanyContext.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        { 
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.TaskCommands)
                .FirstOrDefaultAsync(m => m.ID == id);

            var currentUser = _context.Users.First(x => x.Login.Equals(HttpContext.User.Identity.Name));

            ViewBag.isPrime = false;
            var project = _context.Projects.Include(x=>x.ProjectCommands).First(x=>x.ID == task.ProjectID);


            foreach (ProjectCommand pc in project.ProjectCommands)
            {
                pc.Employee = _context.Users.Include(x => x.Role).First(x => x.ID == pc.EmployeeID);
                if (pc.EmployeeID == currentUser.ID && pc.isPrime)
                    ViewBag.isPrime = true;

            }

            foreach (TaskCommand tc in task.TaskCommands)
            {
                tc.Employee = _context.Users.Include(x=>x.Role).First(x=>x.ID == tc.EmployeeID);
            }
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create(int? id)
        {
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID",(int)id);
            return View();
        }

        public IActionResult Finish(int? id)
        {
            var task = _context.Tasks.Include(x => x.Project).Include(x => x.Project.Tasks).First(x => x.ID == (int)id);

            task.FactDate = DateTime.Now;

            _context.Tasks.Update(task);
            _context.SaveChanges();

            var project = task.Project;
            project.Percent = (int)(((double)project.Tasks.Where(x=>x.FactDate.ToString() != "01.01.0001 0:00:00").Count() / (double)project.Tasks.Count() ) * 100);

            if (project.Percent == 100)
                project.EndDate = DateTime.Now;

            _context.Projects.Update(project);
            _context.SaveChanges();

            return Redirect("/Projects/Details/"+project.ID);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StartDate,EndDate,Description,Coeffecient,ProjectID")] ITCompany.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();

                var task2 = _context.Tasks.Include(x=>x.Project).Include(x => x.Project.Tasks).First(x=>x.ID == task.ID);

                var project = task2.Project;
                
                project.Percent = (int)(((double)project.Tasks.Where(x => x.FactDate.ToString() != "01.01.0001 0:00:00").Count() / (double)project.Tasks.Count()) * 100);

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                return Redirect("/Projects/Details/"+task.ProjectID);
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID", task.ProjectID);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID", task.ProjectID);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,StartDate,EndDate,FactDate,Description,Coeffecient,ProjectID")] ITCompany.Models.Task task)
        {
            if (id != task.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.ID))
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
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ID", "ID", task.ProjectID);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.ID == id);
        }
    }
}
