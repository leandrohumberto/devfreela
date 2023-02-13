using DevFreela.Core.Enums;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace DevFreela.Core.Entities
{
    public class Project : BaseEntity
    {
        public Project(string title, string description, int idClient, int idFreelancer, decimal totalCost)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or empty.", nameof(title));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException($"'{nameof(description)}' cannot be null or empty.", nameof(description));
            }

            Title = title;
            Description = description;
            IdClient = idClient;
            IdFreelancer = idFreelancer;
            TotalCost = totalCost;

            CreatedAt = DateTime.Now;
            Status = ProjectStatusEnum.Created;
            Comments = new List<ProjectComment>();
        }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public int IdClient { get; private set; }

        public User Client { get; private set; }

        public int IdFreelancer { get; private set; }

        public User Freelancer { get; private set; }

        public decimal TotalCost { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? StartedAt { get; private set; }

        public DateTime? FinishedAt { get; private set; }

        public ProjectStatusEnum Status { get; private set; }

        public List<ProjectComment> Comments { get; private set; }

        public void Cancel()
        {
            if (Status == ProjectStatusEnum.InProgress || Status == ProjectStatusEnum.Suspended)
            {
                Status = ProjectStatusEnum.Cancelled;
            }
        }

        public void Finish()
        {
            if (Status == ProjectStatusEnum.PaymentPending)
            {
                Status = ProjectStatusEnum.Finished;
                FinishedAt = DateTime.Now;
            }
        }

        public void Started()
        {
            if (Status == ProjectStatusEnum.Created)
            {
                Status = ProjectStatusEnum.InProgress;
                StartedAt = DateTime.Now;
            }
        }

        public void SetPaymentPending()
        {
            Status = ProjectStatusEnum.PaymentPending;
            FinishedAt = null;
        }

        public void Update(string title, string description, decimal totalCost)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or empty.", nameof(title));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException($"'{nameof(description)}' cannot be null or empty.", nameof(description));
            }

            Title = title;
            Description = description;
            TotalCost = totalCost;
        }
    }
}