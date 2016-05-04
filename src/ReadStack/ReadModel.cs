using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using EventStore;
using ReadStack.Messages;

namespace ReadStack
{
	public class ReadModel
	{
		private readonly IDictionary<string, UserStoryViewDto> _userStories = new ConcurrentDictionary<string, UserStoryViewDto>();

		public ReadModel(Store store)
		{
			store.OnAppended += Handle;
			//source.on<xxx>(xxx => state.xxx++)
		}

		private void Handle(object sender, EventHolder eventHolder)
		{
			var @event = EventSederializer.Deserialize(eventHolder);
			var personAssignedEvent = @event as PersonAssignedEvent;
			if (personAssignedEvent != null)
			{
				var dto = _userStories[eventHolder.StreamName];
				dto.AssignedPersonsCount += 1;
				dto.ModifyDate = personAssignedEvent.Timestamp;
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
				});
				return;
			}
			throw new InvalidDataException($"Unknown event type {@event.GetType()}");
		}


		public UserStoryViewDto GetStory(string streamName)
		{
			return _userStories[streamName];
		}
	}
}
