using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Queries.UserExists
{
    public class UserExistsQueryHandler : IRequestHandler<UserExistsQuery, bool>
    {
        private readonly IUserRepository _repository;

        public UserExistsQueryHandler(IUserRepository repository) => _repository = repository;

        public async Task<bool> Handle(UserExistsQuery request, CancellationToken cancellationToken)
            => await _repository.ExistsAsync(request.Id, cancellationToken);
    }
}
