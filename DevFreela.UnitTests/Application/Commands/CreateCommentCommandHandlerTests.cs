using DevFreela.Application.Commands.CreateComment;
using DevFreela.Core.Entities;
using DevFreela.Core.Exceptions;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class CreateCommentCommandHandlerTests
    {
        [Fact]
        public async void UserExistsAndProjectExists_Executed_AddComment()
        {
            //
            // Arrange
            var project = new Project("Project", "Description",
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>());

            var projectRepositoryMock = new Mock<IProjectRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();

            projectRepositoryMock
                .Setup(pr => pr.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(true);
            projectRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(project);
            projectRepositoryMock
                .Setup(pr => pr.SaveChangesAsync(CancellationToken.None));

            userRepositoryMock
                .Setup(ur => ur.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(true);

            var command = new CreateCommentCommand(It.IsAny<string>(), It.IsAny<int>());
            command.SetIdProject(It.IsAny<int>());

            var handler = new CreateCommentCommandHandler(projectRepositoryMock.Object,
                userRepositoryMock.Object);

            //
            // Act
            _ = await handler.Handle(command, CancellationToken.None);

            //
            // Assert
            Assert.True(project.Comments.Count >= 1);
            projectRepositoryMock
                .Verify(pr => pr.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            projectRepositoryMock
                .Verify(pr => pr.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            projectRepositoryMock
                .Verify(pr => pr.SaveChangesAsync(CancellationToken.None), Times.Once);
            userRepositoryMock
                .Verify(ur => ur.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            projectRepositoryMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UserDoesNotExist_Executed_ThrowException()
        {
            //
            // Arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();

            projectRepositoryMock
                .Setup(pr => pr.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(true);
            projectRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result);
            projectRepositoryMock
                .Setup(pr => pr.SaveChangesAsync(CancellationToken.None));

            userRepositoryMock
                .Setup(ur => ur.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(false);

            var command = new CreateCommentCommand(It.IsAny<string>(), It.IsAny<int>());
            command.SetIdProject(It.IsAny<int>());

            var handler = new CreateCommentCommandHandler(projectRepositoryMock.Object,
                userRepositoryMock.Object);

            //
            // Act

            //
            // Assert
            await Assert.ThrowsAnyAsync<InvalidUserException>(() => handler.Handle(command, CancellationToken.None));
            projectRepositoryMock
                .Verify(pr => pr.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.AtMost(1));
            projectRepositoryMock
                .Verify(pr => pr.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Never);
            projectRepositoryMock
                .Verify(pr => pr.SaveChangesAsync(CancellationToken.None), Times.Never);
            userRepositoryMock
                .Verify(ur => ur.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            projectRepositoryMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ProjectDoesNotExist_Executed_ThrowException()
        {
            //
            // Arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();

            projectRepositoryMock
                .Setup(pr => pr.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(false);
            projectRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result);
            projectRepositoryMock
                .Setup(pr => pr.SaveChangesAsync(CancellationToken.None));

            userRepositoryMock
                .Setup(ur => ur.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(true);

            var command = new CreateCommentCommand(It.IsAny<string>(), It.IsAny<int>());
            command.SetIdProject(It.IsAny<int>());

            var handler = new CreateCommentCommandHandler(projectRepositoryMock.Object,
                userRepositoryMock.Object);

            //
            // Act

            //
            // Assert
            await Assert.ThrowsAnyAsync<InvalidProjectException>(() => handler.Handle(command, CancellationToken.None));
            projectRepositoryMock
                .Verify(pr => pr.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            projectRepositoryMock
                .Verify(pr => pr.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Never);
            projectRepositoryMock
                .Verify(pr => pr.SaveChangesAsync(CancellationToken.None), Times.Never);
            userRepositoryMock
                .Verify(ur => ur.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.AtMost(1));
            projectRepositoryMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UserDoesNotExistAndProjectDoesNotExist_Executed_DoNotAddComment()
        {
            //
            // Arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();

            projectRepositoryMock
                .Setup(pr => pr.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(false);
            projectRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result);
            projectRepositoryMock
                .Setup(pr => pr.SaveChangesAsync(CancellationToken.None));

            userRepositoryMock
                .Setup(ur => ur.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(false);

            var command = new CreateCommentCommand(It.IsAny<string>(), It.IsAny<int>());
            command.SetIdProject(It.IsAny<int>());

            var handler = new CreateCommentCommandHandler(projectRepositoryMock.Object,
                userRepositoryMock.Object);

            //
            // Act

            //
            // Assert
            await Assert.ThrowsAnyAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            projectRepositoryMock
                .Verify(pr => pr.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.AtMost(1));
            projectRepositoryMock
                .Verify(pr => pr.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Never);
            projectRepositoryMock
                .Verify(ur => ur.SaveChangesAsync(CancellationToken.None), Times.Never);
            userRepositoryMock
                .Verify(ur => ur.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.AtMost(1));
            projectRepositoryMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
