using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.UpdateSkill
{
    public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, Unit>
    {
        private readonly ISkillRepository _repository;

        public UpdateSkillCommandHandler(ISkillRepository repository) => _repository = repository;

        public async Task<Unit> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
        {
            var skill = await _repository.GetByIdAsync(request.Id, cancellationToken);
            skill.Update(request.Description, request.Disabled);
            await _repository.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
