using DevFreela.Core.Enums;

namespace DevFreela.Application.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public ProjectDetailsViewModel(int id, string title, string description, ProjectStatusEnum status, DateTime? startedAt,
            DateTime? finishedAt, string clientFullName, string freelancerFullName, decimal totalCost,
            IEnumerable<CommentViewModel> comments)
        {
            Id = id;
            Title = title;
            Description = description;
            StartedAt = startedAt;
            FinishedAt = finishedAt;
            ClientFullName = clientFullName;
            FreelancerFullName = freelancerFullName;
            TotalCost = totalCost;

            Comments = Enumerable.Empty<CommentViewModel>();
            foreach (var comment in comments) Comments = Comments.Append(comment);
            Status = status;
        }

        public int Id { get; private set; }
        
        public string Title { get; private set; }

        public string Description { get; private set; }

        public ProjectStatusEnum Status { get; private set; }

        public DateTime? StartedAt { get; private set; }

        public DateTime? FinishedAt { get; private set; }

        public string? ClientFullName { get; private set; }
        
        public string? FreelancerFullName { get; private set; }

        public decimal TotalCost { get; private set; }

        public IEnumerable<CommentViewModel> Comments { get; private set; }
    }
}
