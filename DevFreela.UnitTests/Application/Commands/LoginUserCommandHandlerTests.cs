using DevFreela.Application.Commands.LoginUser;
using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class LoginUserCommandHandlerTests
    {
        [Fact]
        public async void ValidEmailAndPassword_Executed_ReturnLoginUserViewModel()
        {
            //
            // Arrange
            var user = new User("Name", "mail@mail.com",
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<RoleEnum>());
            var authServiceMock = new Mock<IAuthService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            authServiceMock
                .Setup(mock => mock.ComputeSha256Hash(It.IsAny<string>()))
                .Returns(It.IsAny<string>());
            authServiceMock
                .Setup(mock => mock.GenerateJwtToken(It.IsAny<string>(), It.IsAny<RoleEnum>()))
                .Returns(It.IsAny<string>());
            userRepositoryMock
                .Setup(mock => mock.GetByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(),
                    CancellationToken.None).Result)
                .Returns(user);
            var command = new LoginUserCommand(It.IsAny<string>(), It.IsAny<string>());
            var handler = new LoginUserCommandHandler(authServiceMock.Object, userRepositoryMock.Object);

            //
            // Act
            var loginUserViewModel = await handler.Handle(command, CancellationToken.None);

            //
            // Assert
            Assert.NotNull(loginUserViewModel);
            Assert.True(loginUserViewModel.Email == user.Email);
            authServiceMock
                .Verify(mock => mock.ComputeSha256Hash(It.IsAny<string>()), Times.Once);
            authServiceMock
                .Verify(mock => mock.GenerateJwtToken(It.IsAny<string>(), It.IsAny<RoleEnum>()), Times.Once);
            userRepositoryMock
                .Verify(mock => mock.GetByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(),
                    CancellationToken.None).Result, Times.Once);
            authServiceMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void InvalidEmailAndPassword_Executed_ThrowException()
        {
            //
            // Arrange
            var authServiceMock = new Mock<IAuthService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            authServiceMock
                .Setup(mock => mock.ComputeSha256Hash(It.IsAny<string>()))
                .Returns(It.IsAny<string>());
            authServiceMock
                .Setup(mock => mock.GenerateJwtToken(It.IsAny<string>(), It.IsAny<RoleEnum>()))
                .Returns(It.IsAny<string>());
            userRepositoryMock
                .Setup(mock => mock.GetByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(),
                    CancellationToken.None).Result)
                .Returns(It.IsAny<User>());
            var command = new LoginUserCommand(It.IsAny<string>(), It.IsAny<string>());
            var handler = new LoginUserCommandHandler(authServiceMock.Object, userRepositoryMock.Object);

            //
            // Act

            //
            // Assert
            Assert.ThrowsAny<Exception>(() => handler.Handle(command, CancellationToken.None).Result);
            authServiceMock
                .Verify(mock => mock.ComputeSha256Hash(It.IsAny<string>()), Times.Once);
            authServiceMock
                .Verify(mock => mock.GenerateJwtToken(It.IsAny<string>(), It.IsAny<RoleEnum>()), Times.Never);
            userRepositoryMock
                .Verify(mock => mock.GetByEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>(),
                    CancellationToken.None).Result, Times.Once);
            authServiceMock.VerifyNoOtherCalls();
            userRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
