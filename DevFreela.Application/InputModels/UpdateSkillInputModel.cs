namespace DevFreela.Application.InputModels
{
    public class UpdateSkillInputModel
    {
        public UpdateSkillInputModel(string description, bool disabled)
        {
            Description = description;
            Disabled = disabled;
        }

        public int Id { get; private set; }

        public string Description { get; private set; }

        public bool Disabled { get; private set; }
    }
}