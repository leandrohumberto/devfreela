namespace DevFreela.Core.Entities
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class UserSkill
    {
        public UserSkill(int idUser, int idSkill)
        {
            IdUser = idUser;
            IdSkill = idSkill;
        }

        public int IdUser { get; private set; }

        public int IdSkill { get; private set; }

        public Skill Skill { get; private set; }

        public User User { get; private set; }
    }
}