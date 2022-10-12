using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Queries.GetProjectById
{
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailsViewModel>
    {
        private readonly DevFreelaDbContext _dbContext;

        public GetProjectByIdQueryHandler(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectDetailsViewModel> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _dbContext.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Include(p => p.Comments)
                .ThenInclude(p => p.User)
                .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            return project == null
                ? null
                : new ProjectDetailsViewModel(
                    project.Id,
                    project.Title,
                    project.Description,
                    project.StartedAt,
                    project.FinishedAt,
                    project.Client?.FullName,
                    project.Freelancer?.FullName,
                    project.TotalCost,
                    project.Comments.Select(c => new CommentViewModel(c.Content, c.User.FullName)));
        }
    }
}
