using Xunit;
using FluentAssertions;
using EmployeeManagement.Controllers;
using EmployeeManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace RunTest.ControllerTests
{
    public class AdminControllerTests : IDisposable // Implement IDisposable to clean up the in-memory database
    {
        private readonly AdminController _controller;
        private readonly ApplicationDbContext _context;

        public AdminControllerTests()
        {
            // Create an in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Create a new context instance using the in-memory database
            _context = new ApplicationDbContext(options);

            // Seed the database with test data
            SeedDatabase();

            // Initialize the controller with the in-memory context
            _controller = new AdminController(_context);
        }

        private void SeedDatabase()
        {
            // Seed the in-memory database with fake employees
            _context.Employees.AddRange(new List<Employee>
            {
                new Employee { Id = 1, LectureName = "John", LectureSurname = "Doe", EmployeeNo = "EMP001", IsApproved = "Pending" },
                new Employee { Id = 2, LectureName = "Jane", LectureSurname = "Smith", EmployeeNo = "EMP002", IsApproved = "Pending" }
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task Index_ShouldReturnViewResult_WithListOfEmployees()
        {
            // Act: Call the Index action
            var result = await _controller.Index();

            // Assert: Check that the result is a ViewResult and contains the list of employees
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.Model.Should().BeAssignableTo<List<Employee>>();
            var model = viewResult.Model as List<Employee>;
            model.Should().HaveCount(2);
            model.First().LectureName.Should().Be("John");
        }

        [Fact]
        public void Details_ValidId_ShouldUpdateEmployeeAndRedirect()
        {
            // Arrange
            int id = 1;

            // Act
            var result = _controller.Details(id);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Index");

            // Verify the employee's IsApproved status was updated
            var updatedEmployee = _context.Employees.Find(id);
            updatedEmployee.Should().NotBeNull();
            updatedEmployee.IsApproved.Should().Be("Approved");
        }

        [Fact]
        public void Deapproved_ValidId_ShouldUpdateEmployeeAndRedirect()
        {
            // Arrange
            int id = 1;

            // Act
            var result = _controller.Deapproved(id);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("Index");

            // Verify the employee's IsApproved status was updated
            var updatedEmployee = _context.Employees.Find(id);
            updatedEmployee.Should().NotBeNull();
            updatedEmployee.IsApproved.Should().Be("Disapproved");
        }

        // Implement IDisposable to clean up the in-memory database
        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Clean up the database after tests
            _context.Dispose();
        }
    }
}
