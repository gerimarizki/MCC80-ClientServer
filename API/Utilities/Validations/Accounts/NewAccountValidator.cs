using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class NewAccountValidator : AbstractValidator<NewAccountDto>
    {
        public NewAccountValidator()
        {
            RuleFor(Account => Account.Password)
                .NotEmpty();
            RuleFor(Account => Account.OTP)
                .NotEmpty();
            RuleFor(Account => Account.ExpiredDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now.AddYears(10));

        }
    }
}
