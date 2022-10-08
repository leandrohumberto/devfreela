namespace DevFreela.Application.InputModels
{
    public class CreateSkillInputModel
    {
        public CreateSkillInputModel(string description, bool disabled)
        {
            Description = description;
            Disabled = disabled;
        }

        public string Description { get; private set; }

        public bool Disabled { get; private set; }
    }
}