using DevFreela.Application.Commands.CreateUser;
using FluentValidation;
using System.Text.RegularExpressions;

namespace DevFreela.Application.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress();

            RuleFor(u => u.Password)
                .Must(ValidPassword)
                .WithMessage("Password must be at least 8-character long, and contain a number, " +
                    "a lower case letter, uppercase letter, and a special character.");

            RuleFor(u => u.FullName)
                .NotNull()
                .NotEmpty();
        }

        private bool ValidPassword(string password)
        {
            var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");
            return regex.IsMatch(password);
        }
    }
}
