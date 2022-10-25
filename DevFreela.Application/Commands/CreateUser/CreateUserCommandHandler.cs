using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using MediatR;

namespace DevFreela.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(IUserRepository userRepository, ISkillRepository skillRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _skillRepository = skillRepository;
            _authService = authService;
        }

        public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var passwordHash = _authService.ComputeSha256Hash(command.Password);
            var user = new User(command.FullName, command.Email, command.BirthDate.Date,
                passwordHash, command.Role);

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            if (command.Skills != null)
            {
                foreach (var idSkill in command.Skills)
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
