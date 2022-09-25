using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly DevFreelaDbContext _dbContext;

        public ProjectService(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Create(NewProjectInputModel inputModel)
        {
            var project = new Project(inputModel.Title,
                inputModel.Description,
                inputModel.IdClient,
                inputModel.IdFreelancer,
                inputModel.TotalCost);

            _dbContext.Projects.Add(project);
            _dbContext.SaveChanges();

            return project.Id;
        }

        public void CreateComment(CreateCommentInputModel inputModel)
        {
            var comment = new ProjectComment(
                inputModel.Content,
                inputModel.IdProject,
                inputModel.IdUser);

            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
            project?.Cancel();
            _dbContext.SaveChanges();
        }

        public void Finish(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
            project?.Finish();
            _dbContext.SaveChanges();
        }

        public IEnumerable<ProjectViewModel> GetAll(string query)
        {
            var projects = _dbContext.Projects;
            return projects.Select(p => new ProjectViewModel(p.Id, p.Title, p.CreatedAt));
        }

        public ProjectDetailsViewModel GetById(int id)
        {
            var project = _dbContext.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .SingleOrDefault(p => p.Id == id);

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.
            return project == null
                ? null
                : new ProjectDetailsViewModel(
                    project.Id,
                    project.Title,
                    project.Description,
                    project.StartedAt,
                    project.FinishedAt,
                    project.Client?.FullName,
                    project.Freelancer?.FullName);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void Start(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
            project?.Started();
            _dbContext.SaveChanges();
        }

        public void Update(UpdateProjectInputModel inputModel)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == inputModel.Id);
#pragma warning disable CS8604 // Possible null reference argument.
            project?.Update(inputModel.Title, inputModel.Description, inputModel.TotalCost);
#pragma warning restore CS8604 // Possible null reference argument.
            _dbContext.SaveChanges();
        }
    }
}
