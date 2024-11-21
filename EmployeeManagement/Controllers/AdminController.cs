using EmployeeManagement.Data;
using EmployeeManagement.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Employee> _employeeValidator;

        // Constructor to inject dependencies
        public AdminController(ApplicationDbContext context, IValidator<Employee> employeeValidator)
        {
            _context = context;
            _employeeValidator = employeeValidator;
        }

        // GET: AdminController
        public async Task<ActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: AdminController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
                if (employee != null)
                {
                    // Validate the employee claim before approving
                    var validationResult = _employeeValidator.Validate(employee);
                    if (!validationResult.IsValid)
                    {
                        foreach (var failure in validationResult.Errors)
                        {
                            ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                        }
                        return View(employee); // Show the employee details with validation errors
                    }

                    // If validation passes, approve the claim
                    employee.IsApproved = "Approved";
                    CalculateFinalPayment(employee); // Automatically calculate the final payment
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Deapproved/5
        public async Task<ActionResult> Deapproved(int id)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
                if (employee != null)
                {
                    employee.IsApproved = "Disapproved";
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Helper method to calculate final payment based on hours worked and hourly rate
        private void CalculateFinalPayment(Employee employee)
        {
            // Calculate the final payment: NumberOfHours * HourlyRate
            employee.FinalPayment = employee.NumberOfHours * employee.HourlyRate;
        }
    }
}
