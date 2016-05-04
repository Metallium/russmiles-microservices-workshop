using System;

namespace WriteStack.Messages
{
	public class StoryBirthdayEvent : IEvent
	{
		public string Name { get; set; }
		public UserStoryId Id { get; set; }
		public DateTime Timestamp { get; set; }
	}
}