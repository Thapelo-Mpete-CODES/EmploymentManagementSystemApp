using Employee_Management_System.Data;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index(string departmentName, int? minAge)
        {
            var employees = _context.Employees
                .Include(e => e.Department)
                .Where(e => e.IsActive); // Only show active

            if (!string.IsNullOrEmpty(departmentName))
            {
                employees = employees.Where(e => e.Department.Name.Contains(departmentName));
            }

            if (minAge.HasValue)
            {
                employees = employees.Where(e => e.Age > minAge.Value);
            }

            ViewBag.Departments = new SelectList(await _context.Departments.ToListAsync(), "Name", "Name");
            return View(await employees.ToListAsync());
        }

        // GET: Employee/Create
        public async Task<IActionResult> Create()
        {
            ViewData["DepartmentID"] = new SelectList(await _context.Departments.ToListAsync(), "ID", "Name");
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.IsActive = true;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Name", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || !employee.IsActive) return NotFound();

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Name", employee.DepartmentID);
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.ID == id)) return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "Name", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.ID == id && m.IsActive);

            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST: Employee/Delete/5 (Soft Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = false; // Soft delete
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
