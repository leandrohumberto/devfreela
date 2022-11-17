using DevFreela.Application.Commands.DeleteSkill;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class DeleteSkillCommandHandlerTests
    {
        [Fact]
        public async void SkillIdExists_Executed_DeleteSkill()
        {
            //
            // Arrange
            var skill = new Skill("Skill");
            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(skill);
            mock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));
            var command = new DeleteSkillCommand(It.IsAny<int>());
            var handler = new DeleteSkillCommandHandler(mock.Object);

            //
            // Act

            _ = await handler.Handle(command, CancellationToken.None);

            //
            // Assert

            mock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
            mock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SkillIdDoesNotExist_Executed_ThrowException()
        {
            //
            // Arrange
            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Throws(new Exception());
            mock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));
            var command = new DeleteSkillCommand(It.IsAny<int>());
            var handler = new DeleteSkillCommandHandler(mock.Object);

            //
            // Act

            //
            // Assert
            Assert.ThrowsAny<Exception>(() => handler.Handle(command, CancellationToken.None).Result);
            mock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            mock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Never);
            mock.VerifyNoOtherCalls();
        }
    }
}
