using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.StartProject
{
    public class StartProjectCommandHandler : IRequestHandler<StartProjectCommand, Unit>
    {
        private readonly DevFreelaDbContext _dbContext;

        public StartProjectCommandHandler(DevFreelaDbContext dbContext) => _dbContext = dbContext;

        public async Task<Unit> Handle(StartProjectCommand request, CancellationToken cancellationToken)
        {
            var project = _dbContext.Projects.Single(p => p.Id == request.Id);
            project?.Started();
            _ = await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
