using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.UserExists
{
    public class UserExistsQueryHandler : IRequestHandler<UserExistsQuery, bool>
    {
        private readonly DevFreelaDbContext _dbContext;

        public UserExistsQueryHandler(DevFreelaDbContext dbContext) => _dbContext = dbContext;

        public async Task<bool> Handle(UserExistsQuery request, CancellationToken cancellationToken)
            => await _dbContext.Users.AnyAsync(p => p.Id == request.Id, cancellationToken);
    }
}
