using DevFreela.Application.InputModels;
using DevFreela.Application.ViewModels;

namespace DevFreela.Application.Services.Interfaces
{
    public interface IUserService
    {
        public UserDetailViewModel GetById(int id);

        public int Create(CreateUserInputModel model);
    }
}
