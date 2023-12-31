﻿using API.DTOs.Universities;
using FluentValidation;

namespace API.Utilities.Validations.Universities
{
    public class UpdateUniversityValidator : AbstractValidator<UpdateUniversityDto>
    {
        public UpdateUniversityValidator()
        {
            RuleFor(university => university.Name)
                .NotEmpty();
            RuleFor(university => university.Code)
                .NotEmpty();
        }
    }
}
