using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WriteStack.Messages
{
	public static class EventSederializer
	{
		public static EventHolder Serialize(string streamName, IEvent @event)
		{
			var body = JsonConvert.SerializeObject(
				@event,
				new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				});
			var eventType = @event.GetType().Name;
			return new EventHolder
			{
				StreamName = streamName,
				EventType = eventType,
				Body = body
			};
		}

		public static IEvent Deserialize(EventHolder eventHolder)
		{
			if (eventHolder.EventType == "PersonAssignedEvent")
			{
				return JsonConvert.DeserializeObject<PersonAssignedEvent>(eventHolder.Body);
			}
			if (eventHolder.EventType == "StoryBirthdayEvent")
			{
				return JsonConvert.DeserializeObject<StoryBirthdayEvent>(eventHolder.Body);
			}
			throw new InvalidDataException("Unknown eventType: " + eventHolder.EventType);
		}
	}
}