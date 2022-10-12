using MediatR;

namespace DevFreela.Application.Queries.ProjectExists
{
    public class ProjectExistsQuery : IRequest<bool>
    {
        public ProjectExistsQuery(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
