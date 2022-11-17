using DevFreela.Application.Queries.SkillExists;
using DevFreela.Application.Queries.UserExists;
using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using DevFreela.Core.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.UnitTests.Application.Queries
{
    public class UserExistsQueryHandlerTests
    {
        [Fact]
        public async void UserIdExists_Executed_ReturnTrue()
        {
            // Arrange
            var id = 0;
            var users = new List<User>
            {
                new User("User 01", "mail@mail.com", DateTime.Today.AddYears(-20), "123", RoleEnum.Client),
                new User("User 02", "mail@mail.com", DateTime.Today.AddYears(-30), "123", RoleEnum.Freelancer),
            };

            var mock = new Mock<IUserRepository>();
            mock.Setup(mock => mock.ExistsAsync(id, CancellationToken.None).Result)
                .Returns(users.Any(p => p.Id == id));

            var query = new UserExistsQuery(id);
            var handler = new UserExistsQueryHandler(mock.Object);

            // Act
            var exists = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(exists);
            mock.Verify(mock => mock.ExistsAsync(id, CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void UserIdDoesNotExist_Executed_ReturnTrue()
        {
            // Arrange
            var id = 1;
            var users = new List<User>
            {
                new User("User 01", "mail@mail.com", DateTime.Today.AddYears(-20), "123", RoleEnum.Client),
                new User("User 02", "mail@mail.com", DateTime.Today.AddYears(-30), "123", RoleEnum.Freelancer),
            };

            var mock = new Mock<IUserRepository>();
            mock.Setup(mock => mock.ExistsAsync(id, CancellationToken.None).Result)
                .Returns(users.Any(p => p.Id == id));

            var query = new UserExistsQuery(id);
            var handler = new UserExistsQueryHandler(mock.Object);

            // Act
            var exists = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(exists);
            mock.Verify(mock => mock.ExistsAsync(id, CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
