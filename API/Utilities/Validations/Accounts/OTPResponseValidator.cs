using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class OTPResponseValidator : AbstractValidator<OtpResponseDto>
    {
        public OTPResponseValidator()
        {
            RuleFor(register => register.Email)
             .NotEmpty().WithMessage("Email is required");
            RuleFor(Account => Account.Otp)
                .NotEmpty().WithMessage("Email is required");


        }
    }
}
