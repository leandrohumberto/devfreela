using DevFreela.Application.Commands.UpdateSkill;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class UpdateSkillCommandValidator : AbstractValidator<UpdateSkillCommand>
    {
        public UpdateSkillCommandValidator()
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
