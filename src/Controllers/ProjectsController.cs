using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectManagerContext _context;

        public ProjectsController(ProjectManagerContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index(string sortOrder, int? priorityFilter)
        {
            ViewBag.priorityFilter = priorityFilter;
            ViewBag.priorityList = new SelectList(new int[] { 1, 2, 3, 4, 5 });

            IQueryable<Project> projects;
            projects = priorityFilter == null ?
                projects = _context.Projects.Select(s => s)
                    .Include(p => p.Manager) :
                projects = _context.Projects.Select(s => s)
                    .Include(p => p.Manager)
                    .Where(s => s.Priority == priorityFilter);

            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PrioritySortParm = sortOrder == "priority" ? "priority_desc" : "priority";
            ViewBag.ClientSortParm = sortOrder == "client" ? "client_desc" : "client";
            ViewBag.ManagerSortParm = sortOrder == "manager" ? "manager_desc" : "manager";
            ViewBag.StartSortParm = sortOrder == "start" ? "start_desc" : "start";
            ViewBag.EndSortParm = sortOrder == "end" ? "end_desc" : "end";

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.ProjectName);
                    break;
                case "priority":
                    projects = projects.OrderBy(s => s.Priority);
                    break;
                case "priority_desc":
                    projects = projects.OrderByDescending(s => s.Priority);
                    break;
                case "client":
                    projects = projects.OrderBy(s => s.ClientName);
                    break;
                case "client_desc":
                    projects = projects.OrderByDescending(s => s.ClientName);
                    break;
                case "manager":
                    projects = projects.OrderBy(s => s.Manager);
                    break;
                case "manager_desc":
                    projects = projects.OrderByDescending(s => s.Manager);
                    break;
                case "start":
                    projects = projects.OrderBy(s => s.StartDate);
                    break;
                case "start_desc":
                    projects = projects.OrderByDescending(s => s.StartDate);
                    break;
                case "end":
                    projects = projects.OrderBy(s => s.EndDate);
                    break;
                case "end_desc":
                    projects = projects.OrderByDescending(s => s.EndDate);
                    break;
                default:
                    projects = projects.OrderBy(s => s.ProjectName);
                    break;
            }

            return View(await projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Staffs, "Id", "Name");
            ViewBag.Staffs = _context.Staffs.ToList();
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,ClientName,ManagerId,StartDate,EndDate,Priority")] Project project, int[] selectedStaffs)
        {
            if (ModelState.IsValid)
            {
                foreach (var employee in _context.Staffs.Where(s => selectedStaffs.Contains(s.Id)))
                {
                    project.Employees.Add(employee);
                }
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.Staffs, "Id", "Name", project.ManagerId);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Staffs, "Id", "Name", project.ManagerId);

            ViewBag.Staffs = _context.Staffs.ToList();

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectName,ClientName,ManagerId,StartDate,EndDate,Priority")] Project project, int[] selectedStaffs)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newProject = await _context.Projects
                        .Include(p => p.Manager)
                        .Include(p => p.Employees)
                        .FirstOrDefaultAsync(m => m.Id == id);
                    newProject.ProjectName = project.ProjectName;
                    newProject.ManagerId = project.ManagerId;
                    newProject.ClientName = project.ClientName;
                    newProject.StartDate = project.StartDate;
                    newProject.EndDate = project.EndDate;
                    newProject.Priority = project.Priority;
                    newProject.Employees.Clear();
                    foreach (var employee in _context.Staffs.Where(s => selectedStaffs.Contains(s.Id)))
                    {
                        newProject.Employees.Add(employee);
                    }
                    _context.Update(newProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            ViewData["ManagerId"] = new SelectList(_context.Staffs, "Id", "Name", project.ManagerId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ProjectManagerContext.Projects'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
          return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
