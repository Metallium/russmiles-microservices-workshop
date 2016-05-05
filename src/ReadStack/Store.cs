using System;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace ReadStack
{
	public class Store
	{
		private static IEventStoreConnection CreateEventStoreConnection()
		{
			return EventStoreConnection.Create(ConnectionSettings.Create(), new IPEndPoint(IPAddress.Loopback, 1113));
		}
		public void ListenToAll(Action<EventHolder> callback)
		{
			var esConn = CreateEventStoreConnection(); // TODO dispose
				esConn.ConnectAsync().Wait();
				var userCredentials = new UserCredentials("admin", "changeit");
				esConn.SubscribeToAllFrom(
					Position.Start,
					true,
					(esSubscription, resolvedEvent) =>
					{
						callback(new EventHolder
						{
							StreamName = resolvedEvent.OriginalEvent.EventStreamId,
							EventType = resolvedEvent.OriginalEvent.EventType,
							Body = System.Text.Encoding.Unicode.GetString(resolvedEvent.OriginalEvent.Data),
						});
					},
					OnSubscribed,
					SubscriptionDropped,
					userCredentials);
		}

		private void SubscriptionDropped(EventStoreCatchUpSubscription eventStoreCatchUpSubscription, SubscriptionDropReason subscriptionDropReason, Exception arg3)
		{
			Console.WriteLine("SubscriptionDropped");
		}

		private void OnSubscribed(EventStoreCatchUpSubscription obj)
		{
			Console.WriteLine("OnSubscribed");
		}
	}
}
