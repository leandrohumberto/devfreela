using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DevFreelaDbContext _dbContext;

        public UserRepository(DevFreelaDbContext dbContext) => _dbContext = dbContext;

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
            => await _dbContext.Users.AddAsync(user, cancellationToken);

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
            => await _dbContext.Users.AnyAsync(p => p.Id == id, cancellationToken);

        public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
            => await _dbContext.Users.AnyAsync(p => p.Email == email, cancellationToken);

        public async Task<User?> GetByEmailAndPasswordAsync(string email, string passwordHash, CancellationToken cancellationToken)
            => await _dbContext.Users.SingleOrDefaultAsync(p => p.Email == email && p.Password == passwordHash, cancellationToken);

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _dbContext.Users
                .Include(p => p.UserSkills.Where(s => s.Skill.Disabled != true))
                .ThenInclude(p => p.Skill)
                .SingleAsync(p => p.Id == id, cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
