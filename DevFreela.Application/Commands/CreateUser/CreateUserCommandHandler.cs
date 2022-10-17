using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISkillRepository _skillRepository;

        public CreateUserCommandHandler(IUserRepository userRepository, ISkillRepository skillRepository)
        {
            _userRepository = userRepository;
            _skillRepository = skillRepository;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.FullName, request.Email, request.BirthDate.Date);

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            if (request.Skills != null)
            {
                foreach (var idSkill in request.Skills)
                {
                    if (await _skillRepository.ExistsAsync(idSkill, cancellationToken))
                    {
                        var skill = await _skillRepository.GetByIdAsync(idSkill, cancellationToken);
                        var userSkill = new UserSkill(user.Id, skill.Id);
                        user.UserSkills.Add(userSkill);
                    }
                }
            }

            await _userRepository.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
