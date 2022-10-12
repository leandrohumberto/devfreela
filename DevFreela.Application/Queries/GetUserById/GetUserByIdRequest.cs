using DevFreela.Application.ViewModels;
using MediatR;

namespace DevFreela.Application.Queries.GetUserById
{
    public class GetUserByIdRequest : IRequest<UserDetailViewModel>
    {
        public GetUserByIdRequest(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
