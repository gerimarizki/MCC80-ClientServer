using API.DTOs.Roles;
using FluentValidation;

namespace API.Utilities.Validations.Roles
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleValidator()
        {
            RuleFor(role => role.Name)
           .NotEmpty();
        }
    }
}
