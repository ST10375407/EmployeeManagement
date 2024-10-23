using EmployeeManagement.Data; // Update this with your actual namespace
using EmployeeManagement.Models; // Update this with your actual namespace
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, IFormFile? supportingDocument)
        {
            if (ModelState.IsValid)
            {
                // Handle the file upload if a file was submitted
                if (supportingDocument != null)
                {
                    // Set the directory path for file storage
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Ensure the directory exists
                    Directory.CreateDirectory(uploadsFolder);

                    // Generate a unique file name to prevent conflicts
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(supportingDocument.FileName);

                    // Get the full path to save the file
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Copy the uploaded file to the target location
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await supportingDocument.CopyToAsync(fileStream);
                    }

                    // Set the file path in the model
                    employee.SupportingDocument = uniqueFileName;
                }

                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee, IFormFile? supportingDocument)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle the file upload if a new file was submitted
                    if (supportingDocument != null)
                    {
                        // Set the directory path for file storage
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                        // Ensure the directory exists
                        Directory.CreateDirectory(uploadsFolder);

                        // Generate a unique file name to prevent conflicts
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(supportingDocument.FileName);

                        // Get the full path to save the file
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Copy the uploaded file to the target location
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await supportingDocument.CopyToAsync(fileStream);
                        }

                        // Set the file path in the model
                        employee.SupportingDocument = uniqueFileName;
                    }

                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
