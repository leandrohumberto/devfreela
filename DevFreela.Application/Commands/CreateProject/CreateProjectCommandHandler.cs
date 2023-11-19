using DevFreela.Core.Entities;
using DevFreela.Core.Exceptions;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IProjectRepository _repository;
        private readonly IUserRepository _userRepository;

        public CreateProjectCommandHandler(IProjectRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(request.IdClient, cancellationToken))
            {
                throw new InvalidUserException(request.IdClient, $"User not found. Id: {request.IdClient}");
            }

            if (!await _userRepository.ExistsAsync(request.IdFreelancer, cancellationToken))
            {
                throw new InvalidUserException(request.IdFreelancer, $"User not found. Id: {request.IdFreelancer}");
            }

            var project = new Project(request.Title,
                request.Description,
                request.IdClient,
                request.IdFreelancer,
                request.TotalCost);

            await _repository.AddAsync(project, cancellationToken);

            return project.Id;
        }
    }
}
