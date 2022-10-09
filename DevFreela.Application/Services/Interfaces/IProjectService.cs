using DevFreela.Application.InputModels;
using DevFreela.Application.ViewModels;

namespace DevFreela.Application.Services.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectViewModel> GetAll(string query);
        
        ProjectDetailsViewModel GetById(int id);

        int Create(NewProjectInputModel inputModel);

        void Update(int id, UpdateProjectInputModel inputModel);

        void Delete(int id);

        void CreateComment(int id, CreateCommentInputModel inputModel);

        void Start(int id);

        void Finish(int id);
    }
}
