using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQExchangesOptions
	{
		public RabbitMQExchangesOptions()
		{
			Exchanges = new List<RabbitMQExchangeDeclaration>();
		}

		public IList<RabbitMQExchangeDeclaration> Exchanges { get; }
	}
}