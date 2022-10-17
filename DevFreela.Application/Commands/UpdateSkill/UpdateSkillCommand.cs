using MediatR;

namespace DevFreela.Application.Commands.UpdateSkill
{
    public class UpdateSkillCommand : IRequest<Unit>
    {
        public UpdateSkillCommand(string description, bool disabled)
        {
            Description = description;
            Disabled = disabled;
        }

        public int Id { get; private set; }

        public string Description { get; set; }

        public bool Disabled { get; set; }

        public void SetId(int id) => Id = id;
    }
}
