using System;

namespace ReadStack.Messages
{
	public class PersonAssignedEvent : IEvent
	{
		public UserStoryId UserStoryId { get; set;  }
		public PersonId PersonId { get; set; }
		public DateTime Timestamp { get; set; }
	}
}