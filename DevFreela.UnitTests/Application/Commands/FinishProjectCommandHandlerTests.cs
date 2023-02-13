using DevFreela.Application.Commands.FinishProject;
using DevFreela.Core.DTOs;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using Moq;

namespace DevFreela.UnitTests.Application.Commands
{
    public class FinishProjectCommandHandlerTests
    {
        [Fact]
        public async void ProjectIdExists_Executed_FinishProject()
        {
            //
            // Arrange
            var project = new Project("Title", "Description",
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>());

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Returns(project);
            projectRepositoryMock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(mock => mock.ProcessPayment(It.IsAny<PaymentInfoDTO>()));

            var command = new FinishProjectCommand(
                It.IsAny<int>(),
                "9999999999999999",
                "999",
                "9999",
                "FULL NAME");
            var handler = new FinishProjectCommandHandler(projectRepositoryMock.Object, paymentServiceMock.Object);

            //
            // Act

            _ = await handler.Handle(command, CancellationToken.None);

            //
            // Assert

            projectRepositoryMock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
            projectRepositoryMock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            paymentServiceMock.Verify(mock => mock.ProcessPayment(It.IsAny<PaymentInfoDTO>()),
                Times.Once);
            projectRepositoryMock.VerifyNoOtherCalls();
            paymentServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ProjectIdDoesNotExist_Executed_ThrowException()
        {
            //
            // Arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result)
                .Throws(new Exception());
            projectRepositoryMock.Setup(mock => mock.SaveChangesAsync(CancellationToken.None));

            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(mock => mock.ProcessPayment(It.IsAny<PaymentInfoDTO>()));

            var command = new FinishProjectCommand(
                It.IsAny<int>(),
                "9999999999999999",
                "999",
                "9999",
                "FULL NAME");
            var handler = new FinishProjectCommandHandler(projectRepositoryMock.Object, paymentServiceMock.Object);

            //
            // Act

            //
            // Assert
            Assert.ThrowsAny<Exception>(() => handler.Handle(command, CancellationToken.None).Result);
            projectRepositoryMock.Verify(mock => mock.GetByIdAsync(It.IsAny<int>(), CancellationToken.None).Result,
                Times.Once);
            projectRepositoryMock.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Never);
            paymentServiceMock.Verify(mock => mock.ProcessPayment(It.IsAny<PaymentInfoDTO>()),
                Times.Never);
            projectRepositoryMock.VerifyNoOtherCalls();
            paymentServiceMock.VerifyNoOtherCalls();
        }
    }
}
