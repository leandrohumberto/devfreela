using DevFreela.Core.Services;
using RabbitMQ.Client;

namespace DevFreela.Infrastructure.MessageBus
{
    public class MessageBusService : IMessageBusService
    {
        private readonly ConnectionFactory _connectionFactory;

        public MessageBusService()
        {
            /* 
             * No caso de uma conexão com um servidor externo, cadastrar parâmetros no appsettings.json
             * e injetar dependência do IConfiguration no construtor para recuperá-los
             */

            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
            };
        }

        public void Publish (string queue, byte[] message)
        {
            //
            // Inicializar conexão
            using var connection = _connectionFactory.CreateConnection();

            //
            // Criar canal para realizar as operações
            using var channel = connection.CreateModel();

            //
            // Garantir que a fila esteja criada
            channel.QueueDeclare(
                queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            //
            // Publicar a mensagem
            channel.BasicPublish(
                exchange: string.Empty,     // Agente responsável por rotear as mensagens
                routingKey: queue,          // Chave de roteamento, geralmente leva o nome da fila
                basicProperties: null,      
                body: message);
        }
    }
}
