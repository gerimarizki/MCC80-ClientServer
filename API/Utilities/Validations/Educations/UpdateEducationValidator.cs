using API.DTOs.Educations;
using FluentValidation;

namespace API.Utilities.Validations.Educations
{
    public class UpdateEducationValidator : AbstractValidator<UpdateEducationDto>
    {
        public UpdateEducationValidator()
        {
            RuleFor(education => education.Major)
                .NotEmpty();
            RuleFor(education => education.Degree)
                .NotEmpty();
            RuleFor(education => education.GPA)
                .NotEmpty();
            RuleFor(education => education.UniversityGuid)
                .NotEmpty();
        }
    }
}
