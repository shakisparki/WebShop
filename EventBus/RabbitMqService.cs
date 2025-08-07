using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WebShop.EventBus.Events;

namespace WebShop.EventBus
{
    public class RabbitMqService : IDisposable
    {
        private readonly IConnection _connection;

        //constructor with rabbitmq connection string
        public RabbitMqService(string connectionString)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };
            _connection = factory.CreateConnection();
        }

        public void SetupEventBus()
        {
            using var channel = _connection.CreateModel();
            // Declare the necessary queues, exchanges, and bindings here
            channel.ExchangeDeclare(exchange: "orderExchange", type: ExchangeType.Direct, durable: false, autoDelete: false, arguments: null);

            channel.QueueDeclare(queue: "basketQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.QueueDeclare(queue: "catalogQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.QueueBind(queue: "basketQueue",
                                 exchange: "orderExchange",
                                 routingKey: "basketQueue");

            channel.QueueBind(queue: "catalogQueue",
                                 exchange: "orderExchange",
                                 routingKey: "catalogQueue");
        }

        private void DeleteEventBus()
        {
            using var channel = _connection.CreateModel();

            // Delete the queues and exchange if needed
            channel.QueueDelete(queue: "basketQueue");
            channel.QueueDelete(queue: "catalogQueue");
            channel.ExchangeDelete(exchange: "orderExchange");
        }

        public void Dispose()
        {
            DeleteEventBus();

            _connection.Dispose();
            GC.SuppressFinalize(this);
        }

        // Method to publish a message to a specific queue
        public void PublishMessage(string queueName, IMessage message)
        {
            // Ensure the channel is open and ready to publish messages
            using var channel = _connection.CreateModel();
            // Convert the message to a byte array
            var msgString = System.Text.Json.JsonSerializer.Serialize(message);
            var body = System.Text.Encoding.UTF8.GetBytes(msgString);
            var basicProperties = channel.CreateBasicProperties();
            basicProperties.Type = message.Type; // Set the message type for routing
            // Publish the message to the specified queue
            channel.BasicPublish(exchange: "orderExchange",
                                 routingKey: queueName,
                                 basicProperties: basicProperties,
                                 body: body);
        }

        public string AddSubscription(string queueName, Action<IMessage> messageHandler)
        {
            // Ensure the channel is open and ready to consume messages
            using var channel = _connection.CreateModel();
            // Set up a consumer to listen for messages on the specified queue
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                var messageType = ea.BasicProperties.Type;

                // Deserialize the message to the IMessage type
                var messageObj = messageType switch
                {
                    EventTypes.PriceChanged => System.Text.Json.JsonSerializer.Deserialize<PriceChangedEvent>(message),
                    _ => throw new InvalidOperationException("Unknown message type")
                };

                messageHandler(messageObj);
            };
            
            // Start consuming messages from the specified queue
            return channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Unsubscribe(string consumerTag)
        {
            // Ensure the channel is open and ready to cancel the consumer
            using var channel = _connection.CreateModel();
            // Cancel the consumer with the specified tag
            channel.BasicCancel(consumerTag);
        }
    }
}
