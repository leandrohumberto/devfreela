using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
    {

        private readonly IProjectRepository _repository;

        public DeleteProjectCommandHandler(IProjectRepository repository) => _repository = repository;

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);

            project?.Cancel();
            await _repository.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
