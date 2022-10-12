using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly DevFreelaDbContext _dbContext;

        public CreateUserCommandHandler(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.FullName, request.Email, request.BirthDate.Date);

            _dbContext.Users.Add(user);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);

            if (request.Skills != null)
            {
                foreach (var idSkill in request.Skills)
                {
                    if (await _dbContext.Skills.AnyAsync(s => s.Id == idSkill, cancellationToken))
                    {
                        var skill = await _dbContext.Skills.SingleAsync(s => s.Id == idSkill, cancellationToken);
                        var userSkill = new UserSkill(user.Id, skill.Id);
                        user.UserSkills.Add(userSkill);
                    }
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
