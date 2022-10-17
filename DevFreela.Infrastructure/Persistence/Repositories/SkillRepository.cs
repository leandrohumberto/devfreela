using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly DevFreelaDbContext _dbContext;

        public SkillRepository(DevFreelaDbContext dbContext) => _dbContext = dbContext;

        public async Task AddAsync(Skill skill, CancellationToken cancellationToken = default)
        {
            _dbContext.Skills.Add(skill);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _dbContext.Skills.AnyAsync(p => p.Id == id, cancellationToken);

        public async Task<bool> ExistsAsync(string description, CancellationToken cancellationToken = default)
            => await _dbContext.Skills.AnyAsync(s => s.Description == description, cancellationToken);

        public async Task<IEnumerable<Skill>> GetAllAsync(CancellationToken cancellationToken)
            => await _dbContext.Skills.ToListAsync(cancellationToken);

        public async Task<Skill> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _dbContext.Skills.Where(s => s.Id == id).SingleAsync(cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
