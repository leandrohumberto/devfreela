﻿using DevFreela.Application.Commands.UpdateProject;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Validators
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(p => p.Description)
                .NotEmpty()
                .NotNull();

            var descriptionMaximumLength = 255;
            RuleFor(p => p.Description)
                .NotEmpty()
                .NotNull()
                .MaximumLength(descriptionMaximumLength);

            RuleFor(p => p.Title)
                .NotEmpty()
                .NotNull();

            var titleMaximumLength = 30;
            RuleFor(p => p.Title)
                .MaximumLength(titleMaximumLength);

            RuleFor(p => p.TotalCost)
                .GreaterThan(0M);
        }
    }
}
