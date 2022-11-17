using DevFreela.Core.Entities;
using DevFreela.Core.Enums;

namespace DevFreela.UnitTests.Core.Entities
{

    public class ProjectTests
    {
        [Fact]
        public void InputDataisOk_ProjectInstantiated_ProjectCreated()
        {
            //
            // Arrange
            var title = "Title";
            var description = "Description";
            var idFreelancer = 1;
            var idClient = 2;
            var totalCost = 100.0M;

            //
            // Act
            var project = new Project(title: "Title",
                                      description: "Description",
                                      idFreelancer: 1,
                                      idClient: 2,
                                      totalCost: 100.0M);

            //
            // Assert
            Assert.Equal(ProjectStatusEnum.Created, project.Status);
            Assert.Equal(title, project.Title);
            Assert.Equal(description, project.Description);
            Assert.Equal(idFreelancer, project.IdFreelancer);
            Assert.Equal(idClient, project.IdClient);
            Assert.Equal(totalCost, project.TotalCost);
            Assert.True(project.CreatedAt >= DateTime.Today);
            Assert.Null(project.StartedAt);
            Assert.Null(project.FinishedAt);
        }

        [Fact]
        public void InputDataIsNotOk_ProjectInstantiated_ThrowException()
        {
            //
            // Arrange

            //
            // Act

            //
            //Assert
            Assert.ThrowsAny<Exception>(() => new Project(string.Empty, "Description", 1, 2, 100.0M));
            Assert.ThrowsAny<Exception>(() => new Project("Title", string.Empty, 1, 2, 100.0M));
            Assert.ThrowsAny<Exception>(() => new Project(string.Empty, string.Empty, 1, 2, 100.0M));
        }

        [Fact]
        public void InputDataIsOk_Updated_ProjectUpdated()
        {
            //
            // Arrange
            var project = new Project(title: "Title",
                                      description: "Description",
                                      idFreelancer: 1,
                                      idClient: 2,
                                      totalCost: 100.0M);

            var newTitle = "New Title";
            var newDescription = "New Description";
            var newTotalCost = 200.0M;
            var status = project.Status;

            //
            // Act
            project.Update(newTitle, newDescription, newTotalCost);

            //
            //Assert
            Assert.Equal(project.Title, newTitle);
            Assert.Equal(project.Description, newDescription);
            Assert.Equal(project.TotalCost, newTotalCost);
            Assert.Equal(status, project.Status);
        }

        [Fact]
        public void InputDataIsNotOk_Updated_ThrowException()
        {
            //
            // Arrange
            var project = new Project(title: "Title",
                                      description: "Description",
                                      idFreelancer: 1,
                                      idClient: 2,
                                      totalCost: 100.0M);

            //
            // Act

            //
            //Assert
            Assert.ThrowsAny<Exception>(() => project.Update(string.Empty, "New Description", 200.0M));
            Assert.ThrowsAny<Exception>(() => project.Update("New Title", string.Empty, 200.0M));
            Assert.ThrowsAny<Exception>(() => project.Update(string.Empty, string.Empty, 200.0M));
        }

        [Fact]
        public void ProjectCreated_Started_StatusStarted()
        {
            //
            // Arrange
            var project = new Project(title: "Title",
                                      description: "Description",
                                      idFreelancer: 1,
                                      idClient: 2,
                                      totalCost: 100.0M);

            Assert.Equal(ProjectStatusEnum.Created, project.Status);
            Assert.Null(project.StartedAt);
            Assert.NotNull(project.Title);
            Assert.NotNull(project.Description);

            //
            // Act
            project.Started();

            //
            //Assert
            Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
            Assert.NotNull(project.StartedAt);
        }

        [Fact]
        public void ProjectInProgress_Cancelled_StatusCancelled()
        {
            //
            // Arrange
            var project = new Project(title: "Title",
                                      description: "Description",
                                      idFreelancer: 1,
                                      idClient: 2,
                                      totalCost: 100.0M);

            Assert.Equal(ProjectStatusEnum.Created, project.Status);
            Assert.Null(project.StartedAt);
            Assert.NotNull(project.Title);
            Assert.NotNull(project.Description);
            project.Started();
            Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
            Assert.NotNull(project.StartedAt);

            //
            // Act
            project.Cancel();

            //
            //Assert
            Assert.Equal(ProjectStatusEnum.Cancelled, project.Status);
        }
    }
}
