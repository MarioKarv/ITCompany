using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITCompany.Data;
using ITCompany.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ITCompany.Controllers
{
    public class UsersController : Controller
    {
        private readonly ITCompanyContext _context;
        private IHostingEnvironment Environment;

        public UsersController(ITCompanyContext context, IHostingEnvironment _environment)
        {
            _context = context;
            Environment = _environment;        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var iTCompanyContext = _context.Users.Include(u => u.Role);
            return View(await iTCompanyContext.ToListAsync());
        } 


        // GET: Clients
        public async Task<IActionResult> Clients()
        {
            var iTCompanyContext = _context.Users.Include(u => u.Role).Where(x=>x.RoleID == _context.Roles.First(x=>x.Name.Equals("Клієнт")).ID);
            return View("Index", await iTCompanyContext.ToListAsync());
        }
        // GET: Employees
        public async Task<IActionResult> Employees()
        {
            var iTCompanyContext = _context.Users.Include(u => u.Role).Where(x => x.RoleID != _context.Roles.First(x => x.Name.Equals("Клієнт")).ID);
            return View("Index", await iTCompanyContext.ToListAsync());
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.My = true;
                var currentUser = _context.Users
                    .Include(x => x.Role)
                    .Include(x => x.Projects)
                    .Include(u => u.ProjectCommands)
                    .Include(x => x.TaskCommands)
                    .First(x => x.Login.Equals(HttpContext.User.Identity.Name));
                foreach (Project p in currentUser.Projects)
                {
                    p.ProjectType = _context.ProjectTypes.First(x => x.ID == p.ProjectTypeID);
                }
                foreach (ProjectCommand pc in currentUser.ProjectCommands)
                {
                    pc.Project = _context.Projects.Include(x=>x.ProjectType).Include(x => x.Tasks).Include(x=>x.ProjectCommands).First(x => x.ID == pc.ProjectID);
                }
                foreach (TaskCommand tc in currentUser.TaskCommands)
                {
                    tc.Task = _context.Tasks.Include(x => x.Project).First(x => x.ID == tc.TaskID);
                }

                return View(currentUser);
            }

            ViewBag.My = false;
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(x => x.Projects)
                    .Include(u => u.ProjectCommands)
                    .Include(x => x.TaskCommands)
                .FirstOrDefaultAsync(m => m.ID == id);

            foreach (Project p in user.Projects)
            {
                p.ProjectType = _context.ProjectTypes.First(x => x.ID == p.ProjectTypeID);
            }
            foreach (ProjectCommand pc in user.ProjectCommands)
            {
                pc.Project = _context.Projects.Include(x => x.ProjectType).Include(x=>x.Tasks).Include(x => x.ProjectCommands).First(x => x.ID == pc.ProjectID);
            }
            foreach (TaskCommand tc in user.TaskCommands)
            {
                tc.Task = _context.Tasks.Include(x=>x.Project).First(x => x.ID == tc.TaskID);
            }

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Login,Password,FirstName,LastName,RoleID")] User user, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();

                string wwwPath = this.Environment.WebRootPath;
                string contentPath = this.Environment.ContentRootPath;

                User user2 = _context.Users.First(x => x.Login.Equals(user.Login));

                string path = Path.Combine(this.Environment.WebRootPath, "Images/Users/" + user2.ID);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(file.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                user2.Avatar = "/Images/Users/" + user2.ID + "/" + fileName;

                _context.Users.Update(user2);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "Name", user.RoleID);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "Name", user.RoleID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Login,Password,FirstName,LastName,Avatar,RoleID")] User user, IFormFile file)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path = Path.Combine(this.Environment.WebRootPath, "Images/Users/" + user.ID);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string fileName = Path.GetFileName(file.FileName);
                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        user.Avatar = "/Images/Users/" + user.ID + "/" + fileName;
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("~/Users/Details/" + user.ID);
            }
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "Name", user.RoleID);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }
    }
}
