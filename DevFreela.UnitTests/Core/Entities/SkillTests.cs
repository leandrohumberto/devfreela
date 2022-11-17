using DevFreela.Core.Entities;
namespace DevFreela.UnitTests.Core.Entities
{
    public class SkillTests
    {
        [Fact]
        public void DataIsOk_SkillInstantiated_Status_DisabledFalse()
        {
            //
            // Arrange

            //
            // Act
            var skill = new Skill("Description");

            //
            // Assert
            Assert.True(skill.CreatedAt >= DateTime.Today);
            Assert.False(skill.Disabled);
        }

        [Fact]
        public void DataIsOk_Updated_SkillUpdated()
        {
            //
            // Arrange
            var skill = new Skill("Description");
            var previousDescription = skill.Description;
            var previousDisabled = skill.Disabled;

            //
            // Act
            skill.Update("New Description", true);
            var newDescription = skill.Description;
            var newDisabled = skill.Disabled;

            //
            // Assert
            Assert.NotEqual(previousDescription, newDescription);
            Assert.NotEqual(previousDisabled, newDisabled);
        }

        [Fact]
        public void SkillCreated_ChangedToEnabled_DisabledFalse()
        {
            //
            // Arrange
            var skill = new Skill("Description");

            //
            // Act
            skill.Update("New Description", false);
            skill.Enable();

            //
            // Assert
            Assert.False(skill.Disabled);
        }

        [Fact]
        public void SkillCreated_ChangedToDisabled_DisabledTrue()
        {
            //
            // Arrange
            var skill = new Skill("Description");

            //
            // Act
            skill.Update("New Description", true);
            skill.Disable();

            //
            // Assert
            Assert.True(skill.Disabled);
        }
    }
}
