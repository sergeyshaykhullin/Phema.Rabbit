using System;

namespace Phema.RabbitMq
{
	public static class RabbitMqProducersExtensions
	{
		public static IRabbitMqConfiguration AddProducers(
			this IRabbitMqConfiguration configuration,
			Action<IRabbitMqProducersConfiguration> options)
		{
			options(new RabbitMqProducersConfiguration(configuration.Services));
			return configuration;
		}
	}
}