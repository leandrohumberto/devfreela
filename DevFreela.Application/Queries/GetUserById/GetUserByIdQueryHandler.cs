using DevFreela.Application.ViewModels;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailViewModel>
    {
        private readonly IUserRepository _repository;

        public GetUserByIdQueryHandler(IUserRepository repository) => _repository = repository;

        public async Task<UserDetailViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

            var skillViewModel = new List<SkillViewModel>();
            if (user.UserSkills != null)
            {
                foreach (var userSkill in user.UserSkills)
                {
                    skillViewModel.Add(new SkillViewModel(userSkill.Skill.Id, userSkill.Skill.Description, userSkill.Skill.Disabled));
                }
            }

            return new UserDetailViewModel(user.FullName, user.Email, user.BirthDate, user.Role, user.Active, skillViewModel);
        }
    }
}
