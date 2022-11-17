using DevFreela.Application.Commands.CreateProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class CreateProjectCommandHandlerTests
    {
        [Fact]
        public async Task InputDataIsOk_Executed_ReturnProjectId()
        {
            // Arrange
            var mock = new Mock<IProjectRepository>();
            var command = new CreateProjectCommand(
                "Project Name 01",
                "Project Description 01",
                1,
                2,
                10000M);
            var handler = new CreateProjectCommandHandler(mock.Object);

            // Act
            var id = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(id >= 0);
            mock.Verify(mock => mock.AddAsync(It.IsAny<Project>(), CancellationToken.None), Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
