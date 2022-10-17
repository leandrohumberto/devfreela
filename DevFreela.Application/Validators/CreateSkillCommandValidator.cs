using DevFreela.Application.Commands.CreateSkill;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
    {
        public CreateSkillCommandValidator()
        {
            RuleFor(s => s.Description)
                .NotNull()
                .NotEmpty();

            var descriptionMaximumLength = 20;
            RuleFor(s => s.Description)
                .MaximumLength(descriptionMaximumLength);
        }
    }
}
