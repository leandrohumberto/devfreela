namespace DevFreela.Core.Entities
{
    public class UserSkill : BaseEntity
    {
        public int IdUser { get; private set; }

        public int IdSkill { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Skill Skill { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}