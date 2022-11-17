using DevFreela.Application.Queries.GetSkillById;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Queries
{
    public class GetSkillByIdQueryHandlerTests
    {
        [Fact]
        public async void SkillIdExists_Executed_ReturnSkillViewModel()
        {
            // Arrange
            var skill = new Skill("Description");
            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(skill);
            var query = new GetSkillByIdQuery(It.IsAny<int>());
            var handler = new GetSkillByIdQueryHandler(mock.Object);

            // Act
            var skillViewModel = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(skillViewModel);
            Assert.True(skillViewModel.Id >= 0);
            Assert.Equal(skill.Description, skillViewModel.Description);
            mock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SkillIdDoesNotExist_Executed_ThrowException()
        {
            // Arrange
            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(It.IsAny<Skill>());
            var query = new GetSkillByIdQuery(It.IsAny<int>());
            var handler = new GetSkillByIdQueryHandler(mock.Object);

            // Act

            // Assert
            Assert.ThrowsAny<Exception>(() => handler.Handle(query, CancellationToken.None).Result);
            mock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
