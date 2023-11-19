using DevFreela.Application.Commands.CreateProject;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
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

            RuleFor(p => p.IdClient)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.IdFreelancer)
                .NotEmpty()
                .NotNull();
        }
    }
}
