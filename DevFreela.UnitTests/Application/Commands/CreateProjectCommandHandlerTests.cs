using DevFreela.Application.Commands.CreateProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Exceptions;
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
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var command = new CreateProjectCommand(
                "Project Name 01",
                "Project Description 01",
                It.IsAny<int>(),
                It.IsAny<int>(),
                10000M);

            userRepositoryMock.Setup(r => r.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(true);

            var handler = new CreateProjectCommandHandler(projectRepositoryMock.Object, userRepositoryMock.Object);

            // Act
            var id = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(id >= 0);
            projectRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>(), CancellationToken.None), Times.Once);
            userRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<int>(), CancellationToken.None), Times.Exactly(2));
            projectRepositoryMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        public async Task UserDoesNotExist_Executed_ThrowException(int idClient, int idFreelancer)
        {
            // Arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var command = new CreateProjectCommand(
                title: "Project Name 01",
                description: "Project Description 01",
                idClient: idClient,
                idFreelancer: idFreelancer,
                totalCost: 10000M);

            userRepositoryMock.Setup(r => r.ExistsAsync(1, CancellationToken.None).Result)
                .Returns(false);
            userRepositoryMock.Setup(r => r.ExistsAsync(2, CancellationToken.None).Result)
                .Returns(true);

            var handler = new CreateProjectCommandHandler(projectRepositoryMock.Object, userRepositoryMock.Object);

            // Act

            // Assert
            await Assert.ThrowsAnyAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
            projectRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>(), CancellationToken.None), Times.Never);
            userRepositoryMock.Verify(r => r.ExistsAsync(It.IsAny<int>(), CancellationToken.None), Times.AtLeastOnce);
            projectRepositoryMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
