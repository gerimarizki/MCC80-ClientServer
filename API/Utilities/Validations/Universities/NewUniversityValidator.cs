using API.DTOs.Universities;
using FluentValidation;

namespace API.Utilities.Validations.Universities
{
    public class NewUniversityValidator : AbstractValidator<NewUniversityDto>
    {
        public NewUniversityValidator()
        {
            RuleFor(university => university.Name)
                .NotEmpty();
            RuleFor(university => university.Code)
                .NotEmpty();
        }
    }
}
