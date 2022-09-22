﻿using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;

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

            return project.Id;
        }

        public void CreateComment(CreateCommentInputModel inputModel)
        {
            var comment = new ProjectComment(
                inputModel.Content,
                inputModel.IdProject,
                inputModel.IdUser);

            _dbContext.Comments.Add(comment);
        }

        public void Delete(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
            project?.Cancel();
        }

        public void Finish(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
            project?.Finish();
        }

        public IEnumerable<ProjectViewModel> GetAll(string query)
        {
            var projects = _dbContext.Projects;
            return projects.Select(p => new ProjectViewModel(p.Id, p.Title, p.CreatedAt));
        }

        public ProjectDetailsViewModel GetById(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);

#pragma warning disable CS8603 // Possible null reference return.
            return project == null
                ? null
                : new ProjectDetailsViewModel(project.Id, project.Title, project.Description,
                project.StartedAt, project.FinishedAt);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void Start(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
            project?.Started();
        }

        public void Update(UpdateProjectInputModel inputModel)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == inputModel.Id);
#pragma warning disable CS8604 // Possible null reference argument.
            project?.Update(inputModel.Title, inputModel.Description, inputModel.TotalCost);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}