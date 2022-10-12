namespace DevFreela.Application.ViewModels
{
    public class SkillViewModel
    {
        public SkillViewModel(int id, string description, bool? disabled)
        {
            Id = id;
            Description = description;
            Disabled = disabled;
        }

        public int Id { get; private set; }
        public string Description { get; private set; }
        public bool? Disabled { get; private set; }
    }
}