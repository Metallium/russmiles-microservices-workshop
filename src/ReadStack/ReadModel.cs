using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using ReadStack.Messages;

namespace ReadStack
{
	public class ReadModel
	{
		private readonly IDictionary<string, UserStoryViewDto> _userStories = new ConcurrentDictionary<string, UserStoryViewDto>();

		public ReadModel(Store store)
		{
			Task.Factory.StartNew(() =>
			{
				store.ListenToAll(Handle);
			}, TaskCreationOptions.LongRunning);
		}

		public volatile int Position;

		private void Handle(EventHolder eventHolder)
		{
			if (!eventHolder.StreamName.StartsWith("aggregate-userStory-"))
			{
				return;
			}

			var @event = EventSederializer.Deserialize(eventHolder);
			var personAssignedEvent = @event as PersonAssignedEvent;
			if (personAssignedEvent != null)
			{
				var dto = _userStories[eventHolder.StreamName];
				dto.AssignedPersonsCount += 1;
				dto.ModifyDate = personAssignedEvent.Timestamp;
				dto.Version += 1;
				return;
			}
			var birthdayEvent = @event as StoryBirthdayEvent;
			if (birthdayEvent != null)
			{
				_userStories.Add(eventHolder.StreamName, new UserStoryViewDto
				{
					Id = birthdayEvent.Id.Value.ToString("N"),
					Name = birthdayEvent.Name,
					ModifyDate = birthdayEvent.Timestamp,
					Version = 1,
				});
				return;
			}
			throw new InvalidDataException($"Unknown event type {@event.GetType()}");
		}

		public UserStoryViewDto GetStory(string streamName)
		{
			UserStoryViewDto result;
			_userStories.TryGetValue(streamName, out result);
			return result;
		}
	}
}
