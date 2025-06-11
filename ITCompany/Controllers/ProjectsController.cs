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
    public class ProjectsController : Controller
    {
        private readonly ITCompanyContext _context;

        public ProjectsController(ITCompanyContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var iTCompanyContext = _context.Projects.Include(p => p.ProjectType).Include(x=>x.ProjectCommands).Include(x => x.Tasks);
            return View(await iTCompanyContext.ToListAsync()); 
        }

        public async Task<IActionResult> MyProjects()
        {
            var currentUser = _context.Users.Include(x=>x.Role).First(x => x.Login.Equals(HttpContext.User.Identity.Name));
            if (currentUser.Role.Name.Equals("Клієнт"))
            {
                var iTCompanyContext = _context.Projects.Include(p => p.ProjectType).Include(x => x.ProjectCommands).Include(x => x.Tasks).Where(x => x.ClientID == currentUser.ID);
                return View("Index", await iTCompanyContext.ToListAsync());
            }
            else
            {
                var iTCompanyContext = _context.Projects.Include(x=>x.ProjectCommands).Include(p => p.ProjectType).Include(x => x.ProjectCommands).Include(x => x.Tasks).Where(x => x.ProjectCommands.Select(x=>x.EmployeeID).Contains(currentUser.ID));
                return View("Index", await iTCompanyContext.ToListAsync());
            }
            
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectType)
                .Include(p => p.ProjectCommands)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(m => m.ID == id);


            var currentUser = _context.Users.First(x => x.Login.Equals(HttpContext.User.Identity.Name));

            ViewBag.isPrime = false; 
            if(project.ProjectCommands.FirstOrDefault(x=>x.EmployeeID == currentUser.ID) != null)
            {
                ViewBag.isPrime = project.ProjectCommands.FirstOrDefault(x => x.EmployeeID == currentUser.ID).isPrime;
            }

            foreach(ProjectCommand pc in project.ProjectCommands)
            {
                pc.Employee = _context.Users.Include(x=>x.Role).First(x=>x.ID == pc.EmployeeID);
            }



            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public async Task<IActionResult> Accept(int? id, DateTime EndDate, int Price)
        {

            var project = await _context.Projects
                .Include(p => p.ProjectType)
                .FirstOrDefaultAsync(m => m.ID == id);

            project.StartDate = DateTime.Now;
            project.EndDate = EndDate;
            project.Price = Price;

            _context.Projects.Update(project);
            _context.SaveChanges();
            return Redirect("/Projects/Details/"+project.ID);
        }

        public async Task<IActionResult> Reject(int? id)
        {

            var project = await _context.Projects
                .Include(p => p.ProjectType)
                .FirstOrDefaultAsync(m => m.ID == id);

            project.StartDate = DateTime.Now;
            project.EndDate = DateTime.Now;

            _context.Projects.Update(project);
            _context.SaveChanges();
            return Redirect("/Projects/Details/" + project.ID);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewData["ProjectTypeID"] = new SelectList(_context.ProjectTypes, "ID", "Name");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Price,ProjectTypeID")] Project project)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _context.Users.First(x => x.Login.Equals(HttpContext.User.Identity.Name));
                project.ClientID = currentUser.ID;
                _context.Add(project);
                await _context.SaveChangesAsync();
                return Redirect("/Projects/MyProjects");
            }
            ViewData["ProjectTypeID"] = new SelectList(_context.ProjectTypes, "ID", "ID", project.ProjectTypeID);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["ProjectTypeID"] = new SelectList(_context.ProjectTypes, "ID", "Name", project.ProjectTypeID);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,StartDate,EndDate,Price,ProjectTypeID")] Project project)
        {
            if (id != project.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = _context.Users.Include(x => x.Role).First(x => x.Login.Equals(HttpContext.User.Identity.Name));

                    project.Client = currentUser;
                    project.ClientID = currentUser.ID;

                    _context.Projects.Update(project);
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ID))
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
            ViewData["ProjectTypeID"] = new SelectList(_context.ProjectTypes, "ID", "ID", project.ProjectTypeID);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectType)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ID == id);
        }
    }
}
