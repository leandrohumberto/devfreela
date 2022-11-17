using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Queries
{
    public class GetAllProjectsQueryHandlerTests
    {
        [Fact] // Notação do xUnit para identificar os métodos que fazem parte da suite de testes
        public async void ThreeProjectsExist_Executed_ReturnThreeProjectViewModels()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project("Project Name 01", "Project Description 01", 1, 2, 10000M),
                new Project("Project Name 01", "Project Description 02", 1, 2, 20000M),
                new Project("Project Name 02", "Project Description 03", 1, 2, 30000M),
            };

            var mock = new Mock<IProjectRepository>();
            _ = mock.Setup(mock => mock.GetAllAsync(CancellationToken.None).Result).Returns(projects);
            var query = new GetAllProjectsQuery("");
            var handler = new GetAllProjectsQueryHandler(mock.Object);

            // Act
            var viewModels = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(viewModels);
            Assert.NotEmpty(viewModels);
            Assert.Equal(viewModels.Count(), projects.Count);
            mock.Verify(mock => mock.GetAllAsync(CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void NoProjectExists_Executed_ReturnZeroProjectViewModels()
        {
            // Arrange
            var projects = Enumerable.Empty<Project>();
            var mock = new Mock<IProjectRepository>();
            mock.Setup(mock => mock.GetAllAsync(CancellationToken.None).Result).Returns(projects);
            var query = new GetAllProjectsQuery("");
            var handler = new GetAllProjectsQueryHandler(mock.Object);

            // Act
            var viewModels = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(viewModels); // Tem que devolver um enumerable vazio
            Assert.Empty(viewModels);
            Assert.Equal(viewModels.Count(), projects.Count());
            mock.Verify(mock => mock.GetAllAsync(CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
