using DevFreela.Core.Entities;

namespace DevFreela.Core.Repositories
{
    public interface ISkillRepository
    {
        Task AddAsync(Skill skill, CancellationToken cancellationToken = default);
        Task<IEnumerable<Skill>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Skill> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(string description, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
