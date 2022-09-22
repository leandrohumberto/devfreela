namespace DevFreela.Application.InputModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class NewProjectInputModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int IdClient { get; set; }

        public int IdFreelancer { get; set; }

        public decimal TotalCost { get; set; }
    }
}
