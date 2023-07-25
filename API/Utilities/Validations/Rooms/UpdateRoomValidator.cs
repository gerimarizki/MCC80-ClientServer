﻿using API.DTOs.Rooms;
using FluentValidation;

namespace API.Utilities.Validations.Rooms
{
    public class UpdateRoomValidator : AbstractValidator<NewRoomDto>
    {
        public UpdateRoomValidator()
        {
            RuleFor(room => room.Name)
                .NotEmpty();
            RuleFor(room => room.Floor)
                .NotEmpty();
            RuleFor(room => room.Capacity)
                .NotEmpty();
        }
    }
}