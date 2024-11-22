using EmployeeManagement.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers

{

    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: AdminController
        public async Task<ActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(x => x.Id == id);
                if (employee != null)
                {
                    employee.IsApproved = "Approved";
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Deapproved(int id)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(x => x.Id == id);
                if (employee != null)
                {
                    employee.IsApproved = "Disapproved";
                    _context.SaveChanges();
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
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(x => x.Id == id);
                if (employee != null)
                {
                    employee.IsApproved = "Approved";
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(x => x.Id == id);
                if (employee != null)
                {
                    employee.IsApproved = "Approved";
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}