using MediatR;

namespace DevFreela.Application.Commands.CreateSkill
{
    public class CreateSkillCommand : IRequest<int?>
    {
        public CreateSkillCommand(string description, bool disabled)
        {
            Description = description;
            Disabled = disabled;
        }

        public string Description { get; private set; }

        public bool Disabled { get; private set; }
    }
}
