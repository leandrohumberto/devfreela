using DevFreela.Application.ViewModels;
using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Queries.GetProjectById
{
#pragma warning disable CS8604 // Possible null reference argument.
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailsViewModel>
    {
        private readonly IProjectRepository _repository;

        public GetProjectByIdQueryHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectDetailsViewModel> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);

            return new ProjectDetailsViewModel(
                    project.Id,
                    project.Title,
                    project.Description,
                    project.Status,
                    project.StartedAt,
                    project.FinishedAt,
                    project.Client?.FullName,
                    project.Freelancer?.FullName,
                    project.TotalCost,
                    project.Comments.Select(c => new CommentViewModel(c.Content, c.User.FullName)));
        }
    }
}
