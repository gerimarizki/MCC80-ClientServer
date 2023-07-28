using API.DTOs.Login;
using FluentValidation;

namespace API.Utilities.Validations.Login
{
    public class LoginAccountValidator : AbstractValidator<LoginDto>
    {
        public LoginAccountValidator()
        {
            RuleFor(Login => Login.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress();
            RuleFor(Login => Login.Password)
                .NotEmpty().WithMessage("Password is required")
                .Matches(@"^(?=.*[0-9])(?=.*[A-Z]).{8,}$")
                .WithMessage("Password invalid! Passwords must have at least 1 upper case and 1 number");
        }
    }
}
