using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.DeleteSkill
{
    public class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand, Unit>
    {
        private readonly ISkillRepository _repository;

        public DeleteSkillCommandHandler(ISkillRepository repository) => _repository = repository;

        public async Task<Unit> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
        {
            var skill = await _repository.GetByIdAsync(request.Id, cancellationToken);
            skill.Disable();
            await _repository.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
