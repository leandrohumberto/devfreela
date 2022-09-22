using DevFreela.Application.ViewModels;

namespace DevFreela.Application.Services.Interfaces
{
    public interface ISkillService
    {
        IEnumerable<SkillViewModel> GetAll();
    }
}
