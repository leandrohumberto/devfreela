using DevFreela.Application.Queries.GetUserById;
using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Queries
{
    public class GetUserByIdQueryHandlerTests
    {
        [Fact]
        public async void UserIdExists_Executed_ReturnUserDetailViewModel()
        {
            // Arrange
            var user = new User("User Name",
                                "mail@mail.com",
                                DateTime.Today,
                                "123",
                                RoleEnum.Client);

            var mock = new Mock<IUserRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(user);
            var query = new GetUserByIdQuery(It.IsAny<int>());
            var handler = new GetUserByIdQueryHandler(mock.Object);

            // Act
            var userDetailViewModel = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(userDetailViewModel);
            Assert.Equal(user.FullName, userDetailViewModel.FullName);
            Assert.Equal(user.Email, userDetailViewModel.Email);
            Assert.Equal(user.BirthDate, userDetailViewModel.BirthDate);
            Assert.Equal(user.Role, userDetailViewModel.Role);
            mock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public void UserIdDoesNotExist_Executed_ThrowException()
        {
            // Arrange
            var mock = new Mock<IUserRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(It.IsAny<User>());
            var query = new GetUserByIdQuery(It.IsAny<int>());
            var handler = new GetUserByIdQueryHandler(mock.Object);

            // Act

            // Assert
            Assert.ThrowsAny<Exception>(() => handler.Handle(query, CancellationToken.None).Result);
            mock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
