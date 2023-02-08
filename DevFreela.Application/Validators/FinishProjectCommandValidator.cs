using DevFreela.Application.Commands.FinishProject;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class FinishProjectCommandValidator : AbstractValidator<FinishProjectCommand>
    {
        public FinishProjectCommandValidator()
        {
            RuleFor(s => s.CreditCardNumber)
                .CreditCard();

            RuleFor(s => s.Cvv)
                .Matches("^[0-9]{3}$");

            RuleFor(s => s.ExpiresAt)
                .Matches("^[0-9]{6}$");

            RuleFor(s => s.FullName)
                .NotEmpty()
                .NotNull();
        }
    }
}
