using DevFreela.Application.Commands.CreateComment;
using DevFreela.Core.Entities;
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
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(true);
            projectRepositoryMock
                .Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(project);
            projectRepositoryMock
                .Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            userRepositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
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
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            projectRepositoryMock
                .Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            projectRepositoryMock
                .Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
            userRepositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            projectRepositoryMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UserDoesNotExist_Executed_DoNotAddComment()
        {
            //
            // Arrange

            var projectRepositoryMock = new Mock<IProjectRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();

            projectRepositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(It.IsAny<bool>());
            projectRepositoryMock
                .Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result);
            projectRepositoryMock
                .Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            userRepositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(false);

            var command = new CreateCommentCommand(It.IsAny<string>(), It.IsAny<int>());
            command.SetIdProject(It.IsAny<int>());

            var handler = new CreateCommentCommandHandler(projectRepositoryMock.Object,
                userRepositoryMock.Object);

            //
            // Act

            _ = await handler.Handle(command, CancellationToken.None);

            //
            // Assert

            projectRepositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result);
            projectRepositoryMock
                .Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Never);
            projectRepositoryMock
                .Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Never);
            userRepositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            projectRepositoryMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ProjectDoesNotExist_Executed_DoNotAddComment()
        {
            //
            // Arrange

            var projectRepositoryMock = new Mock<IProjectRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();

            projectRepositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(false);
            projectRepositoryMock
                .Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result);
            projectRepositoryMock
                .Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            userRepositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(It.IsAny<bool>());

            var command = new CreateCommentCommand(It.IsAny<string>(), It.IsAny<int>());
            command.SetIdProject(It.IsAny<int>());

            var handler = new CreateCommentCommandHandler(projectRepositoryMock.Object,
                userRepositoryMock.Object);

            //
            // Act

            _ = await handler.Handle(command, CancellationToken.None);

            //
            // Assert

            projectRepositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            projectRepositoryMock
                .Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Never);
            projectRepositoryMock
                .Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Never);
            userRepositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result);
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
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(false);
            projectRepositoryMock
                .Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result);
            projectRepositoryMock
                .Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            userRepositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(false);

            var command = new CreateCommentCommand(It.IsAny<string>(), It.IsAny<int>());
            command.SetIdProject(It.IsAny<int>());

            var handler = new CreateCommentCommandHandler(projectRepositoryMock.Object,
                userRepositoryMock.Object);

            //
            // Act

            _ = await handler.Handle(command, CancellationToken.None);

            //
            // Assert

            projectRepositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            projectRepositoryMock
                .Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Never);
            projectRepositoryMock
                .Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Never);
            userRepositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            projectRepositoryMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
