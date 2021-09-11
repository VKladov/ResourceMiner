using System;

namespace UniRx
{
	public abstract class Message
	{
	}

	public abstract class Request : Message
	{
	}

	public abstract class Signal : Message
	{
	}

	public static class MessageBus
	{
		public static IObservable<T> Receive<T>()
		{
			return MessageBroker.Default.Receive<T>();
		}
	}
}