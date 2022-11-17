using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Unit>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public CreateCommentCommandHandler(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var projectExists = await _projectRepository.ExistsAsync(request.IdProject, cancellationToken);
            var userExists = await _userRepository.ExistsAsync(request.IdUser, cancellationToken);

            if (projectExists && userExists)
            {
                var project = await _projectRepository.GetByIdAsync(request.IdProject, cancellationToken);
                var comment = new ProjectComment(request.Content, request.IdProject, request.IdUser);
                project.Comments.Add(comment);
                await _projectRepository.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
