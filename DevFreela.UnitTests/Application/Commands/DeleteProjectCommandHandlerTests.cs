using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class DeleteProjectCommandHandlerTests
    {
        [Fact]
        public async void ProjectIdExists_Executed_DeleteProject()
        {
            //
            // Arrange
            var project = new Project("Title", "Description",
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>());
            var mock = new Mock<IProjectRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(project);
            mock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));
            var command = new DeleteProjectCommand(It.IsAny<int>());
            var handler = new DeleteProjectCommandHandler(mock.Object);

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
        public void ProjectIdDoesNotExist_Executed_ThrowException()
        {
            //
            // Arrange
            var mock = new Mock<IProjectRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Throws(new Exception());
            mock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));
            var command = new DeleteProjectCommand(It.IsAny<int>());
            var handler = new DeleteProjectCommandHandler(mock.Object);

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
