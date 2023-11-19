using DevFreela.Core.IntegrationEvents;
using DevFreela.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DevFreela.Application.Consumers
{
    public class PaymentApprovedConsumer : BackgroundService
    {
        public const string PAYMENT_APPROVED_QUEUE = "PaymentsApproved";
        public readonly IConnection _connection;
        public readonly IModel _channel;
        public readonly IServiceProvider _serviceProvider;

        public PaymentApprovedConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: PAYMENT_APPROVED_QUEUE,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var paymentApprovedBytes = eventArgs.Body.ToArray();
                var paymentApprovedJson = Encoding.UTF8.GetString(paymentApprovedBytes);
                var paymentApprovedIntegrationEvent = JsonSerializer.Deserialize<PaymentApprovedIntegrationEvent>(paymentApprovedJson);

                if (paymentApprovedIntegrationEvent != null)
                {
                    await FinishProject(paymentApprovedIntegrationEvent.IdProject);
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(
                queue: PAYMENT_APPROVED_QUEUE,
                autoAck: false,
                consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task FinishProject(int idProject, CancellationToken cancellationToken = default)
        {
            using var scope = _serviceProvider.CreateScope();
            var projectRepository = scope.ServiceProvider.GetRequiredService<IProjectRepository>();

            if (projectRepository != null && await projectRepository.ExistsAsync(idProject, CancellationToken.None))
            {
                var project = await projectRepository.GetByIdAsync(idProject, cancellationToken);
                project?.Finish();
                await projectRepository.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // TODO: Publicar mensagem em uma fila de erros
            }
        }
    }
}
