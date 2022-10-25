using DevFreela.Application.Commands.LoginUser;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress();

            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}
