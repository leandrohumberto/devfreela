using DevFreela.Application.InputModels;
using DevFreela.Application.ViewModels;

namespace DevFreela.Application.Services.Interfaces
{
    public interface ISkillService
    {
        IEnumerable<SkillViewModel> GetAll();

        SkillViewModel GetById(int id);

        int? Create(CreateSkillInputModel inputModel);

        void Update(int id, UpdateSkillInputModel inputModel);

        void Delete(int id);
    }
}
