using EmployeeManagement.Controllers;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class EmployeesControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            // Create a new in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed some test data
            SeedDatabase();

            // Initialize the controller with the in-memory context
            _controller = new EmployeesController(_context, null); // Passing null for IWebHostEnvironment
        }

        private void SeedDatabase()
        {
            _context.Employees.AddRange(new List<Employee>
            {
                new Employee { Id = 1, LectureName = "John", LectureSurname = "Doe", EmployeeNo = "E001", IsApproved = "Approved" },
                new Employee { Id = 2, LectureName = "Jane", LectureSurname = "Smith", EmployeeNo = "E002", IsApproved = null }
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task Index_Returns_ViewResult_With_Employees()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Employee>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Create_ValidEmployee_Redirects_To_Index()
        {
            // Arrange
            var newEmployee = new Employee
            {
                LectureName = "Alice",
                LectureSurname = "Johnson",
                EmployeeNo = "E003",
                ContactDetails = "alice@example.com",
                Model = "Model A",
                Program = "Program 1",
                NumberOfHours = 20,
                HourlyRate = 50,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            // Act
            var result = await _controller.Create(newEmployee, null); // Null for IFormFile

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(3, _context.Employees.Count()); // Check if the new employee was added
        }

        [Fact]
        public async Task Edit_ValidEmployee_Redirects_To_Index()
        {
            // Arrange
            var existingEmployee = await _context.Employees.FindAsync(1);
            existingEmployee.ContactDetails = "updated@example.com";

            // Act
            var result = await _controller.Edit(existingEmployee.Id, existingEmployee, null); // Null for IFormFile

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("updated@example.com", (await _context.Employees.FindAsync(1)).ContactDetails); // Check if the contact details were updated
        }

        [Fact]
        public async Task DeleteConfirmed_Removes_Employee_And_Redirects()
        {
            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(1, _context.Employees.Count()); // Check if the employee was removed
        }
    }
}
