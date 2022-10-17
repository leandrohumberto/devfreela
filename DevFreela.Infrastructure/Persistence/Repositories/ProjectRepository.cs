using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DevFreelaDbContext _dbContext;

        public ProjectRepository(DevFreelaDbContext dbContext) => _dbContext = dbContext;

        public async Task AddAsync(Project project, CancellationToken cancellationToken)
        {
            _ = await _dbContext.Projects.AddAsync(project, cancellationToken);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
            => await _dbContext.Projects.AnyAsync(p => p.Id == id, cancellationToken);

        public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken)
            => await _dbContext.Projects.ToListAsync(cancellationToken);

        public async Task<Project> GetByIdAsync(int id, CancellationToken cancellationToken)
            => await _dbContext.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Include(p => p.Comments)
                .ThenInclude(p => p.User)
                .SingleAsync(p => p.Id == id, cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
