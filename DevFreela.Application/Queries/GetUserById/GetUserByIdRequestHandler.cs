using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetUserById
{
    public class GetUserByIdRequestHandler : IRequestHandler<GetUserByIdRequest, UserDetailViewModel>
    {
        private readonly DevFreelaDbContext _dbContext;

        public GetUserByIdRequestHandler(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDetailViewModel> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(p => p.UserSkills.Where(s => s.Skill.Disabled != true))
                .ThenInclude(p => p.Skill)
                .SingleAsync(p => p.Id == request.Id, cancellationToken);

            var skillViewModel = new List<SkillViewModel>();
            if (user.UserSkills != null)
            {
                foreach (var userSkill in user.UserSkills)
                {
                    skillViewModel.Add(new SkillViewModel(userSkill.Skill.Id, userSkill.Skill.Description, userSkill.Skill.Disabled));
                }
            }

            return new UserDetailViewModel(user.FullName, user.Email, user.BirthDate, user.Active, skillViewModel);
        }
    }
}
