using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.EventBus
{

    public class RabbitMQEventBus : IEventBus, IAsyncDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly string _hostName;
        private readonly string _exchangeName;
        private readonly string _queueName;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMQEventBus(IServiceProvider serviceProvider, ILogger<RabbitMQEventBus> logger, string hostName, string exchangeName, string queueName)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _hostName = hostName;
            _exchangeName = exchangeName;
            _queueName = queueName;
        }

        private async Task EnsureConnectionAsync()
        {
            _logger.LogInformation("Ensuring RabbitMQ connection to {HostName}", _hostName);

            if (_connection != null && _channel != null)
                return;

            var factory = new ConnectionFactory() { HostName = _hostName };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(
                exchange: _exchangeName,
                type: ExchangeType.Direct,
                durable: true);

            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public async Task PublishAsync<T>(T @event) where T : class
        {
            await EnsureConnectionAsync();

            var eventName = typeof(T).Name;
            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            var props = new BasicProperties
            {
               DeliveryMode = DeliveryModes.Persistent // Make the message persistent
            };
          
            if (_channel == null)
            {
                throw new InvalidOperationException("Channel is not initialized.");
            }

            await _channel.BasicPublishAsync(
                exchange: _exchangeName,
                routingKey: eventName,
                mandatory: false,
                basicProperties: props,
                body: body
            );
        }

        public async Task SubscribeAsync<T, TH>()
            where T : class
            where TH : IEventHandler<T>
        {
            await EnsureConnectionAsync();

            var eventName = typeof(T).Name;

            await _channel!.QueueBindAsync(
                queue: _queueName,
                exchange: _exchangeName,
                routingKey: eventName
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var eventName = typeof(T).Name;

                try
                {
                    var @event = JsonSerializer.Deserialize<T>(message);
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<TH>();
                    await handler.Handle(@event!);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing event {EventName}", eventName);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: false,
                consumer: consumer
            );
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }
     }
}
