using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.Commands.UpdateSkill;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class UpdateSkillCommandHandlerTests
    {
        [Fact]
        public async void InputDataIsOk_Execute_UpdateSkill()
        {
            //
            // Arrange
            var skill = new Skill("Description");
            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(skill);
            mock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));
            var command = new UpdateSkillCommand("New Description", false);
            command.SetId(It.IsAny<int>());
            var handler = new UpdateSkillCommandHandler(mock.Object);

            //
            // Act
            _ = await handler.Handle(command, CancellationToken.None);

            //
            // Assert
            mock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            mock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SkillIdDoesNotExist_Execute_ThrowException()
        {
            //
            // Arrange
            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Throws(new Exception());
            mock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));
            var command = new UpdateSkillCommand(It.IsAny<string>(), It.IsAny<bool>());
            var handler = new UpdateSkillCommandHandler(mock.Object);

            //
            // Act

            //
            // Assert
            _ = Assert.ThrowsAny<Exception>(() => handler.Handle(command, CancellationToken.None).Result);
            mock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            mock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Never);
            mock.VerifyNoOtherCalls();
        }
    }
}
