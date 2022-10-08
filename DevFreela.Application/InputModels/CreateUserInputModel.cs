namespace DevFreela.Application.InputModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class CreateUserInputModel
    {
        public string FullName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public List<int>? Skills { get; set; }

    }
}