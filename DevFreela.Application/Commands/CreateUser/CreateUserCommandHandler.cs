using DevFreela.Core.Entities;
using DevFreela.Core.Exceptions;
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
            if (await _userRepository.ExistsAsync(command.Email, cancellationToken))
            {
                throw new InvalidUserEmailException(command.Email, "Email already exists.");
            }

            var passwordHash = _authService.ComputeSha256Hash(command.Password);
            var user = new User(command.FullName, command.Email, command.BirthDate.Date,
                passwordHash, command.Role);

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            if (command.Skills != null)
            {
                var idSkills = command.Skills.Where(id => _skillRepository.ExistsAsync(id, cancellationToken).Result).ToList();

                if (idSkills.Count > 0)
                {
                    foreach (var idSkill in idSkills)
                    {
                        user.UserSkills.Add(new UserSkill(user.Id, idSkill));
                    }

                    await _userRepository.SaveChangesAsync(cancellationToken);
                }
            }

            return user.Id;
        }
    }
}
