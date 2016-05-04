using System;

namespace WriteStack.Messages
{
	public class PersonAssignedEvent : IEvent
	{
		public PersonAssignedEvent(PersonId person, UserStoryId userStory, DateTime timestamp)
		{
			PersonId = person;
			UserStoryId = userStory;
			Timestamp = timestamp;
		}

		public UserStoryId UserStoryId { get; }
		public PersonId PersonId { get; }
		public DateTime Timestamp { get; set; }
	}
}