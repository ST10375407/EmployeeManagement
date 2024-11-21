using FluentValidation;

namespace EmployeeManagement.Models
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.EmployeeNo).NotEmpty().WithMessage("Employee number is required.");
            RuleFor(e => e.LectureName).NotEmpty().WithMessage("Lecture name is required.");
            RuleFor(e => e.LectureSurname).NotEmpty().WithMessage("Lecture surname is required.");

            // Claim validation rules
            RuleFor(e => e.NumberOfHours)
                .LessThanOrEqualTo(40).WithMessage("Cannot approve claim with more than 40 hours.");
            RuleFor(e => e.HourlyRate)
                .GreaterThanOrEqualTo(15).WithMessage("Hourly rate must be at least 15.");
        }
    }
}
