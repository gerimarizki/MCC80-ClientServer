using API.DTOs.Login;
using FluentValidation;

namespace API.Utilities.Validations.Login
{
    public class LoginAccountValidator : AbstractValidator<LoginDto>
    {
        public LoginAccountValidator()
        {
            RuleFor(Login => Login.Email)
                .NotEmpty().WithMessage("Email is required");
            RuleFor(Login => Login.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
