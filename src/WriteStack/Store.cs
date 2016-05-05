using System;
using System.Net;
using EventStore.ClientAPI;

namespace WriteStack
{
	public class Store
	{
		private static IEventStoreConnection CreateEventStoreConnection()
		{
			return EventStoreConnection.Create(ConnectionSettings.Create(), new IPEndPoint(IPAddress.Loopback, 1113));
		}

		public void Append(EventHolder eventHolder)
		{
			using (var esConn = CreateEventStoreConnection())
			{
				esConn.ConnectAsync().Wait();
				var writeResult = esConn.AppendToStreamAsync(
					eventHolder.StreamName,
					ExpectedVersion.Any,
					new EventData(
						Guid.NewGuid(),
						eventHolder.EventType,
						true,
						System.Text.Encoding.UTF8.GetBytes(eventHolder.Body),
						null
						)).Result;
				;
			}
		}
	}
}
