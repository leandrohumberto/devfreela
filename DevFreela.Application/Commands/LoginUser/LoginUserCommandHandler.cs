using DevFreela.Application.ViewModels;
using DevFreela.Core.Exceptions;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using MediatR;

namespace DevFreela.Application.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public LoginUserCommandHandler(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Utilizar o mesmo algoritmo para criar o hash da senha
            var passwordHash = _authService.ComputeSha256Hash(request.Password);

            // Buscar no meu banco de dados um User que tenha meu e-mail e a minha senha em formato hash
            var user = await _userRepository.GetByEmailAndPasswordAsync(request.Email, passwordHash, cancellationToken);

            // Se não existir, erro no login
            if (user == null)
            {
                throw new LoginFailException(request.Email, request.Password,
                    $"Incorrect {nameof(request.Email)} or {nameof(request.Password)}");
            }
            
            // Se existir, gero o token usando os dados do usuário
            var token = _authService.GenerateJwtToken(email: user.Email, role: user.Role);
            return new LoginUserViewModel(user.Email, token);
        }
    }
}
