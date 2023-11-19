using DevFreela.Application.Commands.CreateComment;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(c => c.Content)
                .NotNull()
                .NotEmpty();

            RuleFor(c => c.IdUser)
                .NotNull()
                .NotEmpty();
        }
    }
}
