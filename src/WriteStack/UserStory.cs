using System;
using System.Collections.Generic;
using System.IO;
using WriteStack.Messages;

namespace WriteStack
{
	public class UserStory
	{
		private readonly Action<UserStory, IEvent> _publish;

		private readonly IList<PersonId> _assignedPersonIds = new List<PersonId>();

		public IReadOnlyList<PersonId> AssignedPersonIds => (IReadOnlyList<PersonId>)_assignedPersonIds;
		public UserStoryId Id { get; private set; }
		public string Name { get; private set; }
		
		//FIXME consider converting publish to event?
		private UserStory(Action<UserStory, IEvent> publish)
		{
			_publish = publish;
		}

		public void AssignPerson(Person person, DateTime? timestamp = null)
		{
			Apply(new PersonAssignedEvent(person.Id, Id, timestamp ?? DateTime.UtcNow));
		}

		private void Apply(IEvent @event)
		{
			Handle(@event);
			_publish(this, @event);
		}

		private void Handle(IEvent @event)
		{
			var personAssignedEvent = @event as PersonAssignedEvent;
			if (personAssignedEvent != null)
			{
				_assignedPersonIds.Add(personAssignedEvent.PersonId);
				return;
			}
			var birthdayEvent = @event as StoryBirthdayEvent;
			if (birthdayEvent != null)
			{
				Id = birthdayEvent.Id;
				Name = birthdayEvent.Name;
				return;
			}
			throw new InvalidDataException($"Unknown event type {@event.GetType()}");
		}

		public static UserStory BringIntoTheWorld(string name, Action<UserStory, IEvent> publish, UserStoryId id = null, DateTime? timestamp = null)
		{
			var userStory = new UserStory(publish);
			userStory.Apply(new StoryBirthdayEvent { Id = id ?? new UserStoryId(Guid.NewGuid()), Name = name, Timestamp = timestamp ?? DateTime.UtcNow });

			return userStory;
		}

		public static UserStory RestoreFromEvents(IList<IEvent> events, Action<UserStory, IEvent> publish)
		{
			var userStory = new UserStory(publish);

			foreach (var @event in events)
			{
				userStory.Apply(@event);
			}

			return userStory;
		}
	}
}
