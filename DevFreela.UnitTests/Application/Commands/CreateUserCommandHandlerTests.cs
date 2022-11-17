using DevFreela.Application.Commands.CreateUser;
using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class CreateUserCommandHandlerTests
    {
        [Fact]
        public async void UserWithoutSkills_Executed_ReturnUserId()
        {
            //
            // Arrange

            // Instantiate mocks
            var userRespositoryMock = new Mock<IUserRepository>();
            var skillRespositoryMock = new Mock<ISkillRepository>();
            var authServiceMock = new Mock<IAuthService>();

            // Setup: user with the same email does not exist
            userRespositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result)
                .Returns(false);

            // Setup: add user
            userRespositoryMock
                .Setup(mock => mock.AddAsync(It.IsAny<User>(), CancellationToken.None));

            // Setup: save changes
            userRespositoryMock
                .Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            // Setup: compute SHA256 password
            authServiceMock
                .Setup(mock => mock.ComputeSha256Hash(It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            // Command
            var command = new CreateUserCommand(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<RoleEnum>(),
                Enumerable.Empty<int>());

            // Handler
            var handler = new CreateUserCommandHandler(userRespositoryMock.Object,
                skillRespositoryMock.Object, authServiceMock.Object);

            //
            // Act

            var id = await handler.Handle(command, CancellationToken.None);

            //
            // Assert
            Assert.True(id >= 0);
            userRespositoryMock.Verify(
                mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result,
                Times.Once);
            userRespositoryMock
                .Verify(mock => mock.AddAsync(It.IsAny<User>(), CancellationToken.None),
                Times.Once);
            userRespositoryMock
                .Verify(mock => mock.SaveChangesAsync(CancellationToken.None),
                Times.Once);
            authServiceMock
                .Verify(mock => mock.ComputeSha256Hash(It.IsAny<string>()),
                Times.Once);
            userRespositoryMock.VerifyNoOtherCalls();
            skillRespositoryMock.VerifyNoOtherCalls();
            authServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UserWithValidSkills_Executed_ReturnUserId()
        {
            //
            // Arrange

            // Instantiate mocks
            var userRespositoryMock = new Mock<IUserRepository>();
            var skillRespositoryMock = new Mock<ISkillRepository>();
            var authServiceMock = new Mock<IAuthService>();

            // Setup: user with the same email does not exist
            userRespositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result)
                .Returns(false);

            // Setup: add user
            userRespositoryMock
                .Setup(mock => mock.AddAsync(It.IsAny<User>(), CancellationToken.None));

            // Setup: save changes
            userRespositoryMock
                .Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            // Setup: skill id exists
            skillRespositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(true);

            // Setup: compute SHA256 password
            authServiceMock
                .Setup(mock => mock.ComputeSha256Hash(It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            // Command
            var idSkills = Enumerable.Empty<int>().Append(It.IsAny<int>()).Append(It.IsAny<int>());
            var command = new CreateUserCommand(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<RoleEnum>(),
                idSkills);

            // Handler
            var handler = new CreateUserCommandHandler(userRespositoryMock.Object,
                skillRespositoryMock.Object, authServiceMock.Object);

            //
            // Act

            var id = await handler.Handle(command, CancellationToken.None);

            //
            // Assert
            Assert.True(id >= 0);
            userRespositoryMock.Verify(
                mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result,
                Times.Once);
            userRespositoryMock
                .Verify(mock => mock.AddAsync(It.IsAny<User>(), CancellationToken.None),
                Times.Once);
            userRespositoryMock
                .Verify(mock => mock.SaveChangesAsync(CancellationToken.None),
                Times.Exactly(2)); // First call for saving the new user and the second one for saving the related skills
            skillRespositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Exactly(idSkills.Count()));
            authServiceMock
                .Verify(mock => mock.ComputeSha256Hash(It.IsAny<string>()),
                Times.Once);
            userRespositoryMock.VerifyNoOtherCalls();
            skillRespositoryMock.VerifyNoOtherCalls();
            authServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UserWithoutValidSkills_Executed_ReturnUserId()
        {
            //
            // Arrange

            // Instantiate mocks
            var userRespositoryMock = new Mock<IUserRepository>();
            var skillRespositoryMock = new Mock<ISkillRepository>();
            var authServiceMock = new Mock<IAuthService>();

            // Setup: user with the same email does not exist
            userRespositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result)
                .Returns(false);

            // Setup: add user
            userRespositoryMock
                .Setup(mock => mock.AddAsync(It.IsAny<User>(), CancellationToken.None));

            // Setup: save changes
            userRespositoryMock
                .Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            // Setup: skill id does not exist
            skillRespositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(false);

            // Setup: compute SHA256 password
            authServiceMock
                .Setup(mock => mock.ComputeSha256Hash(It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            // Command
            var idSkills = Enumerable.Empty<int>().Append(It.IsAny<int>()).Append(It.IsAny<int>());
            var command = new CreateUserCommand(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<RoleEnum>(),
                idSkills);

            // Handler
            var handler = new CreateUserCommandHandler(userRespositoryMock.Object,
                skillRespositoryMock.Object, authServiceMock.Object);

            //
            // Act

            var id = await handler.Handle(command, CancellationToken.None);

            //
            // Assert
            Assert.True(id >= 0);
            userRespositoryMock.Verify(
                mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result,
                Times.Once);
            userRespositoryMock
                .Verify(mock => mock.AddAsync(It.IsAny<User>(), CancellationToken.None),
                Times.Once);
            userRespositoryMock
                .Verify(mock => mock.SaveChangesAsync(CancellationToken.None),
                Times.Once); // Only one call for saving the user, no calls for the skills
            skillRespositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Exactly(idSkills.Count()));
            authServiceMock
                .Verify(mock => mock.ComputeSha256Hash(It.IsAny<string>()),
                Times.Once);
            userRespositoryMock.VerifyNoOtherCalls();
            skillRespositoryMock.VerifyNoOtherCalls();
            authServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void EmailAlreadyExists_Executed_ThrowException()
        {
            //
            // Arrange

            // Instantiate mocks
            var userRespositoryMock = new Mock<IUserRepository>();
            var skillRespositoryMock = new Mock<ISkillRepository>();
            var authServiceMock = new Mock<IAuthService>();

            // Setup: user with the same email exists
            userRespositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result)
                .Returns(true);

            // Setup: add user
            userRespositoryMock
                .Setup(mock => mock.AddAsync(It.IsAny<User>(), CancellationToken.None));

            // Setup: save changes
            userRespositoryMock
                .Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            // Setup: skill id exists
            skillRespositoryMock
                .Setup(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(It.IsAny<bool>());

            // Setup: compute SHA256 password
            authServiceMock
                .Setup(mock => mock.ComputeSha256Hash(It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            // Command
            var command = new CreateUserCommand(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<RoleEnum>(),
                It.IsAny<IEnumerable<int>>());

            // Handler
            var handler = new CreateUserCommandHandler(userRespositoryMock.Object,
                skillRespositoryMock.Object, authServiceMock.Object);

            //
            // Act

            // No actions needed

            //
            // Assert
            Assert.ThrowsAny<Exception>(() => handler.Handle(command, CancellationToken.None).Result);
            userRespositoryMock.Verify(
                mock => mock.ExistsAsync(It.IsAny<string>(), CancellationToken.None).Result,
                Times.Once);
            userRespositoryMock
                .Verify(mock => mock.AddAsync(It.IsAny<User>(), CancellationToken.None),
                Times.Never);
            userRespositoryMock
                .Verify(mock => mock.SaveChangesAsync(CancellationToken.None),
                Times.Never);
            skillRespositoryMock
                .Verify(mock => mock.ExistsAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Never);
            authServiceMock
                .Verify(mock => mock.ComputeSha256Hash(It.IsAny<string>()),
                Times.Never);
            userRespositoryMock.VerifyNoOtherCalls();
            skillRespositoryMock.VerifyNoOtherCalls();
            authServiceMock.VerifyNoOtherCalls();
        }
    }
}
