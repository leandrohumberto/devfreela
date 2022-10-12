using MediatR;

namespace DevFreela.Application.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest<Unit>
    {
        public UpdateProjectCommand(string title, string description, decimal totalCost)
        {
            Title = title;
            Description = description;
            TotalCost = totalCost;
        }

        public int Id { get; private set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal TotalCost { get; set; }

        public void SetId(int id) => Id = id;
    }
}
