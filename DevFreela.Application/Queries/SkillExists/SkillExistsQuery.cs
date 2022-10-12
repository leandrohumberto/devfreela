using MediatR;

namespace DevFreela.Application.Queries.SkillExists
{
    public class SkillExistsQuery : IRequest<bool>
    {
        public SkillExistsQuery(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
