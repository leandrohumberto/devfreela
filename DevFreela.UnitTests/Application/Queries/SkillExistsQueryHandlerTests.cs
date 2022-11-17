using DevFreela.Application.Queries.SkillExists;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Queries
{
    public class SkillExistsQueryHandlerTests
    {
        [Fact]
        public async void SkillIdExists_Executed_ReturnTrue()
        {
            // Arrange
            var id = 0;
            var skills = new List<Skill>
            {
                new Skill("Skill 01"),
                new Skill("Skill 02"),
            };

            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.ExistsAsync(id, CancellationToken.None).Result)
                .Returns(skills.Any(p => p.Id == id));

            var query = new SkillExistsQuery(id);
            var handler = new SkillExistsQueryHandler(mock.Object);

            // Act
            var exists = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(exists);
            mock.Verify(mock => mock.ExistsAsync(id, CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }


        [Fact]
        public async void SkillIdDoesNotExist_Executed_ReturnFalse()
        {
            // Arrange
            var id = 1;
            var skills = new List<Skill>
            {
                new Skill("Skill 01"),
                new Skill("Skill 02"),
            };

            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.ExistsAsync(id, CancellationToken.None).Result)
                .Returns(skills.Any(p => p.Id == id));

            var query = new SkillExistsQuery(id);
            var handler = new SkillExistsQueryHandler(mock.Object);

            // Act
            var exists = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(exists);
            mock.Verify(mock => mock.ExistsAsync(id, CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
