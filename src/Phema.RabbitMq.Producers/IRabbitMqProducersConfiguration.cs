using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Phema.Serialization;

using RabbitMQ.Client;

namespace Phema.RabbitMq
{
	public interface IRabbitMqProducersConfiguration
	{
		IRabbitMqProducerConfiguration AddProducer<TPayload>(string exchangeName, string queueName);
	}

	internal sealed class RabbitMqProducersConfiguration : IRabbitMqProducersConfiguration
	{
		private readonly IServiceCollection services;

		public RabbitMqProducersConfiguration(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMqProducerConfiguration AddProducer<TPayload>(string exchangeName, string queueName)
		{
			var producer = new RabbitMqProducer(exchangeName, queueName);

			services.TryAddSingleton<IRabbitMqProducer<TPayload>>(provider =>
			{
				var channel = provider.GetRequiredService<IConnection>().CreateModel();

				var exchange = provider.GetRequiredService<IOptions<RabbitMqExchangesOptions>>()
					.Value
					.Exchanges
					.FirstOrDefault(ex => ex.Name == producer.ExchangeName);

				if (exchange != null)
					channel.ExchangeDeclareNoWait(
						exchange.Name,
						exchange.Type,
						exchange.Durable,
						exchange.AutoDelete,
						exchange.Arguments);

				var queue = provider.GetRequiredService<IOptions<RabbitMqQueuesOptions>>()
					.Value
					.Queues
					.FirstOrDefault(q => q.Name == producer.QueueName);

				if (queue != null)
					channel.QueueDeclareNoWait(
						queue.Name,
						queue.Durable,
						queue.Exclusive,
						queue.AutoDelete,
						queue.Arguments);

				channel.QueueBindNoWait(
					producer.QueueName,
					producer.ExchangeName,
					producer.QueueName,
					queue?.Arguments);

				var serializer = provider.GetRequiredService<ISerializer>();

				var properties = channel.CreateBasicProperties();

				foreach (var property in producer.Properties)
					property(properties);

				return new RabbitMqProducer<TPayload>(payload =>
				{
					channel.BasicPublish(
						producer.ExchangeName,
						producer.QueueName,
						producer.Mandatory,
						properties,
						serializer.Serialize(payload));
				});
			});

			return new RabbitMqProducerConfiguration(producer);
		}
	}
}