using DevFreela.Application.ViewModels;
using MediatR;

namespace DevFreela.Application.Queries.GetSkillById
{
    public class GetSkillByIdQuery : IRequest<SkillViewModel>
    {
        public GetSkillByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
