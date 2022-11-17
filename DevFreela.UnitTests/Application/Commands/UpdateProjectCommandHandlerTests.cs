using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class UpdateProjectCommandHandlerTests
    {
        [Fact]
        public async void InputDataIsOk_Execute_UpdateProject()
        {
            //
            // Arrange
            var project = new Project("Title", "Description",
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>());
            var mock = new Mock<IProjectRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(project);
            mock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));
            var command = new UpdateProjectCommand("New Title", "New Description",
                It.IsAny<decimal>());
            var handler = new UpdateProjectCommandHandler(mock.Object);

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
        public void ProjectIdDoesNotExist_Execute_ThrowException()
        {
            //
            // Arrange
            var mock = new Mock<IProjectRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Throws(new Exception());
            mock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));
            var command = new UpdateProjectCommand(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<decimal>());
            var handler = new UpdateProjectCommandHandler(mock.Object);

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
