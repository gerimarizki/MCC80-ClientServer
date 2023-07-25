using API.DTOs.Roles;
using FluentValidation;

namespace API.Utilities.Validations.Roles
{
    public class UpdateRoleValidator : AbstractValidator<NewRoleDto>
    {
        public UpdateRoleValidator()
        {
            RuleFor(role => role.Name)
           .NotEmpty();
        }
    }
}
