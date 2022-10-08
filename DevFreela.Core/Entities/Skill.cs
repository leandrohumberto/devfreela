namespace DevFreela.Core.Entities
{
    public class Skill : BaseEntity
    {
        public Skill(string description)
        {
            Description = description;
            CreatedAt = DateTime.Now;
            UserSkills = new List<UserSkill>();
            Enable();
        }

        public string Description { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public List<UserSkill> UserSkills { get; private set; }

        public bool? Disabled { get; private set; }

        public void Update(string description, bool disabled)
        {
            Description = description;
            if (disabled) Disable(); else Enable();
        }

        public void Disable()
        {
            Disabled = true;
        }

        public void Enable()
        {
            Disabled = false;
        }
    }
}
