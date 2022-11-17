using DevFreela.Application.Queries.GetAllSkills;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.UnitTests.Application.Queries
{
    public class GetAllSkillsQueryHandlerTests
    {
        [Fact]
        public async void FourSkillsExist_Executed_ReturnThreeSkillViewModels()
        {
            // Arrange
            var skills = new List<Skill>
            {
                new Skill("A"),
                new Skill("B"),
                new Skill("C"),
                new Skill("D"),
            };
            var mock = new Mock<ISkillRepository>();
            mock.Setup(mock => mock.GetAllAsync(CancellationToken.None).Result).Returns(skills);
            var query = new GetAllSkillsQuery();
            var handler = new GetAllSkillsQueryHandler(mock.Object);

            // Act
            var skillViewModels = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(skillViewModels);
            Assert.NotEmpty(skillViewModels);
            Assert.Equal(skills.Count, skillViewModels.Count());
            mock.Verify(mock => mock.GetAllAsync(CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void NoSkillExists_Executed_ReturnZeroSkillViewModels()
        {
            // Arrange
            var mock = new Mock<ISkillRepository>();
            var skills = Enumerable.Empty<Skill>();
            mock.Setup(mock => mock.GetAllAsync(CancellationToken.None).Result).Returns(skills);
            var query = new GetAllSkillsQuery();
            var handler = new GetAllSkillsQueryHandler(mock.Object);

            // Act
            var skillViewModels = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(skillViewModels);
            Assert.NotNull(skillViewModels);
            Assert.Equal(skills.Count(), skillViewModels.Count());
            mock.Verify(mock => mock.GetAllAsync(CancellationToken.None).Result, Times.Once);
            mock.VerifyNoOtherCalls();
        }
    }
}
