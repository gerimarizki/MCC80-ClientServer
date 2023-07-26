using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class ForgotPasswordOTPValidator : AbstractValidator<ForgotPasswordOTPDto>
    {
        public ForgotPasswordOTPValidator()
        {
            RuleFor(forgot => forgot.Email)
                .NotEmpty().WithMessage("Email is required");
        }
    }
}
