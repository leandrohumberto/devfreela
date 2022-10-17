using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.StartProject
{
    public class StartProjectCommandHandler : IRequestHandler<StartProjectCommand, Unit>
    {

        private readonly IProjectRepository _repository;

        public StartProjectCommandHandler(IProjectRepository repository) => _repository = repository;

        public async Task<Unit> Handle(StartProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);
            project.Started();
            await _repository.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
