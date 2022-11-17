using DevFreela.Application.Commands.CreateSkill;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class CreateSkillCommandHandlerTests
    {
        [Fact]
        public async Task NewSkill_Executed_ReturnSkillId()
        {
            // Arrange
            var skillName = "Skill 01";
            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.ExistsAsync(skillName, CancellationToken.None).Result)
                .Returns(false);
            mock.Setup(mock => mock.AddAsync(It.IsAny<Skill>(), CancellationToken.None));
            var command = new CreateSkillCommand(skillName, false);
            var handler = new CreateSkillCommandHandler(mock.Object);

            // Act
            var id = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(id);
            Assert.True(id.HasValue);
            Assert.True(id.Value >= 0);
            mock.Verify(mock => mock.ExistsAsync(skillName, CancellationToken.None).Result, Times.Once);
            mock.Verify(mock => mock.AddAsync(It.IsAny<Skill>(), CancellationToken.None), Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SkillExists_Executed_ReturnNull()
        {
            // Arrange
            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result)
                .Returns(true);
            var command = new CreateSkillCommand(It.IsAny<string>(), false);
            var handler = new CreateSkillCommandHandler(mock.Object);

            // Act
            var id = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(id);
            Assert.False(id.HasValue);
            mock.Verify(mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
