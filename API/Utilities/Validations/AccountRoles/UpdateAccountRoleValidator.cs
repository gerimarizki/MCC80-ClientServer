using API.DTOs.AccountRoles;
using FluentValidation;

namespace API.Utilities.Validations.AccountRoles
{
    public class UpdateAccountRoleValidator : AbstractValidator<NewAccountRoleDto>
    {
        public UpdateAccountRoleValidator()
        {
            RuleFor(AccountRole => AccountRole.AccountGuid)
                .NotEmpty();
            RuleFor(AccountRole => AccountRole.RoleGuid)
                .NotEmpty();
        }
    }
}
