using System.Threading.Tasks;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducerExtensions
	{
		public static Task<bool> BatchProduce<TPayload>(
			this IRabbitMQProducer<TPayload> producer,
			params TPayload[] payloads)
		{
			return producer.BatchProduce(payloads);
		}
	}
}