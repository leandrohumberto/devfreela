using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Commands.UpdateSkill
{
#pragma warning disable CS8604 // Possible null reference argument.
    public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, Unit>
    {
        private readonly DevFreelaDbContext _dbContext;

        public UpdateSkillCommandHandler(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
        {
            var hasRepetitions = await _dbContext.Skills.AnyAsync(s => s.Id != request.Id && s.Description == request.Description, cancellationToken);

            if (!hasRepetitions)
            {
                var skill = _dbContext.Skills.Where(s => s.Id == request.Id).First();
                skill?.Update(request.Description, request.Disabled);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
