using API.Contracts;
using API.DTOs.Register;
using API.Repositories;
using FluentValidation;
using Microsoft.Win32;

namespace API.Utilities.Validations.Register
{
    public class RegisterAccountValidator : AbstractValidator<RegisterDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        public RegisterAccountValidator(IEmployeeRepository employeeRepository)
        {

            _employeeRepository = employeeRepository;

            RuleFor(register => register.FirstName)
                .NotEmpty().WithMessage("FirstName is required");
            RuleFor(register => register.LastName)
           .NotEmpty().WithMessage("LastName is required");

            RuleFor(register => register.BirthDate)
                .NotEmpty().WithMessage("BirthDate is required")
                .LessThanOrEqualTo(DateTime.Now.AddYears(-10));

            RuleFor(register => register.Gender)
                .NotNull()
                .IsInEnum();

            RuleFor(register => register.HiringDate)
                .NotEmpty().WithMessage("Hiring Date is required");

            RuleFor(register => register.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is mot valid")
            .Must(IsDuplicateValue).WithMessage("Email already exist");


            RuleFor(register => register.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required")
                .MaximumLength(13)
                .MinimumLength(11)
                .Matches(@"^+[0-9]")
                .Must(IsDuplicateValue).WithMessage("Phone Number alreadt exist");

            RuleFor(register => register.Major)
                      .NotEmpty();
            RuleFor(register => register.Degree)
                .NotEmpty();
            RuleFor(register => register.GPA)
                .NotEmpty();
            RuleFor(register => register.UniversityCode)
                .NotEmpty();
            RuleFor(register => register.UniversityName)
              .NotEmpty();

            RuleFor(register => register.Password)
              .NotEmpty().WithMessage("Password is required")
              .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{8,}$");
            RuleFor(register => register.ConfirmPassword)
             .Equal(register => register.Password).WithMessage("Password Correct")
             .WithMessage("Passwords do not match");


        }

        private bool IsDuplicateValue(string arg)
        {
            return _employeeRepository.IsNotExist(arg);
        }
    }
}
