using DevFreela.Application.Queries.ProjectExists;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Queries
{
    public class ProjectExistsQueryHandlerTests
    {
        [Fact]
        public async void ProjectIdExists_Executed_ReturnTrue()
        {
            // Arrange
            var id = 0;
            var projects = new List<Project>
            {
                new Project("Project 01", "Description 01", 1, 2, 1000),
                new Project("Project 02", "Description 02", 1, 2, 1000),
            };

            var mock = new Mock<IProjectRepository>();
            mock.Setup(mock => mock.ExistsAsync(id, CancellationToken.None).Result)
                .Returns(projects.Any(p => p.Id == id));

            var query = new ProjectExistsQuery(id);
            var handler = new ProjectExistsQueryHandler(mock.Object);

            // Act
            var exists = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(exists);
            mock.Verify(mock => mock.ExistsAsync(id, CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }


        [Fact]
        public async void ProjectIdDoesNotExist_Executed_ReturnFalse()
        {
            // Arrange
            var id = 1;
            var projects = new List<Project>
            {
                new Project("Project 01", "Description 01", 1, 2, 1000),
                new Project("Project 02", "Description 02", 1, 2, 1000),
            };

            var mock = new Mock<IProjectRepository>();
            mock.Setup(mock => mock.ExistsAsync(id, CancellationToken.None).Result)
                .Returns(projects.Any(p => p.Id == id));

            var query = new ProjectExistsQuery(id);
            var handler = new ProjectExistsQueryHandler(mock.Object);

            // Act
            var exists = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(exists);
            mock.Verify(mock => mock.ExistsAsync(id, CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
