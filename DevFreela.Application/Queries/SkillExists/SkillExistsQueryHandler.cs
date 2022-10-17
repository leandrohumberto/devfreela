using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Queries.SkillExists
{
    public class SkillExistsQueryHandler : IRequestHandler<SkillExistsQuery, bool>
    {
        private readonly ISkillRepository _repository;

        public SkillExistsQueryHandler(ISkillRepository repository) => _repository = repository;

        public async Task<bool> Handle(SkillExistsQuery request, CancellationToken cancellationToken)
            => await _repository.ExistsAsync(request.Id, cancellationToken);
    }
}
