using API.DTOs.Accounts;
using FluentValidation;


namespace API.Utilities.Validations.Accounts
{
    public class UpdateAccountValidator : AbstractValidator<NewAccountDto>
    {
        public UpdateAccountValidator()
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
