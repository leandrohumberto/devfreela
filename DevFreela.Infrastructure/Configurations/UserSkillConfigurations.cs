using DevFreela.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFreela.Infrastructure.Configurations
{
    internal class UserSkillConfigurations : IEntityTypeConfiguration<UserSkill>
    {
        public void Configure(EntityTypeBuilder<UserSkill> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(p => p.Skill)
                .WithOne()
                .HasForeignKey<UserSkill>(p => p.IdSkill)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
