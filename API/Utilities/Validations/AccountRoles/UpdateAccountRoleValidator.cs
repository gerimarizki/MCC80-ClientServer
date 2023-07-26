using API.DTOs.AccountRoles;
using FluentValidation;

namespace API.Utilities.Validations.AccountRoles
{
    public class UpdateAccountRoleValidator : AbstractValidator<UpdateAccountRoleDto>
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
