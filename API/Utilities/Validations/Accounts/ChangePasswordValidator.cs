using API.Contracts;
using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        public ChangePasswordValidator(IEmployeeRepository employeeRepository) 
        {
            _employeeRepository = employeeRepository;

            RuleFor(Change => Change.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
            RuleFor(Change => Change.OTP)
               .NotEmpty().WithMessage("OTP is required");  
            
            RuleFor(Change => Change.NewPassword)
               .NotEmpty().WithMessage("Password is required")
              .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{8,}$");
            RuleFor(Change => Change.ConfirmPassword)
                .Equal(Change => Change.NewPassword).WithMessage("Password Correct")
             .WithMessage("Passwords do not match");
        }
    }
}
