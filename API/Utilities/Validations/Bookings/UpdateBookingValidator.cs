﻿using API.DTOs.Bookings;
using FluentValidation;

namespace API.Utilities.Validations.Bookings
{
    public class UpdateBookingValidator : AbstractValidator<NewBookingDto>
    {
        public UpdateBookingValidator()
        {
            RuleFor(booking => booking.StartDate)
                .NotEmpty();
            RuleFor(booking => booking.EndDate)
                .NotEmpty();
            RuleFor(booking => booking.Status)
                .NotNull()
                .IsInEnum();
            RuleFor(booking => booking.Remarks)
                .NotEmpty();
            RuleFor(booking => booking.EmployeeGuid)
                .NotEmpty();
            RuleFor(booking => booking.RoomGuid)
                .NotEmpty();
        }

    }
}
