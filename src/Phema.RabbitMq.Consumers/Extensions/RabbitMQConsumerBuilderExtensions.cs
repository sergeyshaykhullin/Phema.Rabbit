using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumerBuilderExtensions
	{
		/// <summary>
		///   Declare consumer tag
		/// </summary>
		public static IRabbitMQConsumerBuilder Tag(
			this IRabbitMQConsumerBuilder builder,
			string consumerTag)
		{
			if (consumerTag is null)
				throw new ArgumentNullException(nameof(consumerTag));

			builder.Declaration.Tag = consumerTag;

			return builder;
		}

		/// <summary>
		///   Declare message prefetch count
		/// </summary>
		public static IRabbitMQConsumerBuilder Prefetched(
			this IRabbitMQConsumerBuilder builder,
			ushort prefetch,
			bool global = true)
		{
			builder.Declaration.PrefetchCount = prefetch;
			builder.Declaration.Global = global;

			return builder;
		}


		/// <summary>
		///   Declare parallel consumers count
		/// </summary>
		public static IRabbitMQConsumerBuilder Count(
			this IRabbitMQConsumerBuilder builder,
			uint count)
		{
			builder.Declaration.Count = count;

			return builder;
		}

		/// <summary>
		///   Declare consumer as exclusive
		/// </summary>
		public static IRabbitMQConsumerBuilder Exclusive(this IRabbitMQConsumerBuilder builder)
		{
			builder.Declaration.Exclusive = true;
			return builder;
		}

		/// <summary>
		///   Declare no-local flag. Broker will not send messages to the connection that published them
		/// </summary>
		public static IRabbitMQConsumerBuilder NoLocal(this IRabbitMQConsumerBuilder builder)
		{
			builder.Declaration.NoLocal = true;

			return builder;
		}

		/// <summary>
		///   Declare auto-ack flag. Consumer will ack messages when received, not processed
		/// </summary>
		public static IRabbitMQConsumerBuilder AutoAck(this IRabbitMQConsumerBuilder builder)
		{
			builder.Declaration.AutoAck = true;

			return builder;
		}

		/// <summary>
		///   Requeue message when fail to consume
		/// </summary>
		public static IRabbitMQConsumerBuilder Requeue(
			this IRabbitMQConsumerBuilder builder,
			bool multiple = false)
		{
			builder.Declaration.Requeue = true;
			builder.Declaration.Multiple = multiple;

			return builder;
		}

		public static IRabbitMQConsumerBuilder Priority(
			this IRabbitMQConsumerBuilder configuration,
			byte priority)
		{
			// TODO: Hack, because RabbitMQ.Client has no conversion to byte
			return configuration.WithArgument("x-priority", (int) priority);
		}

		/// <summary>
		///   Declare RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQConsumerBuilder WithArgument<TValue>(
			this IRabbitMQConsumerBuilder builder,
			string argument,
			TValue value)
		{
			if (argument is null)
				throw new ArgumentNullException(nameof(argument));

			if (builder.Declaration.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));

			builder.Declaration.Arguments.Add(argument, value);

			return builder;
		}
	}
}