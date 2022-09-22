using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;

namespace DevFreela.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly DevFreelaDbContext _dbContext;

        public UserService(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Create(CreateUserInputModel model)
        {
            var user = new User(model.FullName, model.Email, model.BirthDate);
            _dbContext.Users.Add(user);
            return user.Id;
        }

        public UserDetailViewModel GetById(int id)
        {
            var user = _dbContext.Users.SingleOrDefault(p => p.Id == id);

#pragma warning disable CS8603 // Possible null reference return.
            return user == null
                ? null
                : new UserDetailViewModel(user.FullName, user.Email, user.BirthDate, user.Active);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
