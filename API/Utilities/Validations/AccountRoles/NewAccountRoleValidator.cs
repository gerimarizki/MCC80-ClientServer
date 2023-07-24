using API.DTOs.AccountRoles;
using FluentValidation;

namespace API.Utilities.Validations.AccountRoles
{
    public class NewAccountRoleValidator : AbstractValidator<NewAccountRoleDto>
    {
        public NewAccountRoleValidator()
        {
            RuleFor(AccountRole => AccountRole.AccountGuid)
                .NotEmpty();
            RuleFor(AccountRole => AccountRole.RoleGuid)
                .NotEmpty();
        }
    }
}
