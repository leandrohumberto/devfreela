using MediatR;

namespace DevFreela.Application.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<Unit>
    {
        public CreateCommentCommand(string content, int idUser)
        {
            Content = content;
            IdUser = idUser;
        }

        public int IdProject { get; private set; }

        public string Content { get; private set; }

        public int IdUser { get; private set; }

        public void SetIdProject(int id) => IdProject = id;
    }
}
