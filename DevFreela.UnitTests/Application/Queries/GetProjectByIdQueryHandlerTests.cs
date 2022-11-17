using DevFreela.Application.Queries.GetProjectById;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Queries
{
    public class GetProjectByIdQueryHandlerTests
    {
        [Fact]
        public async void ProjectIdExists_Executed_ReturnProjectDetailsViewModel()
        {
            // Arrange
            var project = new Project("Project A", "Description", 1, 2, 1000);
            var mock = new Mock<IProjectRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result).Returns(project);
            var query = new GetProjectByIdQuery(It.IsAny<int>());
            var handler = new GetProjectByIdQueryHandler(mock.Object);

            // Act
            var projectViewModel = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(projectViewModel);
            Assert.True(projectViewModel.Id >= 0);
            Assert.Equal(project.Title, projectViewModel.Title);
            Assert.Equal(project.Description, projectViewModel.Description);
            Assert.Equal(project.TotalCost, projectViewModel.TotalCost);
            Assert.Equal(project.Status, projectViewModel.Status);
            mock.Verify(p => p.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ProjectIdDoesNotExist_Executed_ThrowException()
        {
            // Arrange
            var mock = new Mock<IProjectRepository>();
            _ = mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(It.IsAny<Project>());

            var query = new GetProjectByIdQuery(It.IsAny<int>());
            var handler = new GetProjectByIdQueryHandler(mock.Object);

            // Act

            // Assert
            Assert.ThrowsAny<Exception>(() => handler.Handle(query, CancellationToken.None).Result);
            mock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
