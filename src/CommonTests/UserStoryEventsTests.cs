using System;
using System.Collections.Generic;
using WriteStack;
using WriteStack.Messages;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
	public class UserStoryEventsTests
	{
		[Test]
		public void BringIntoTheWorldTest()
		{
			var userStoryId = new UserStoryId(Guid.NewGuid());
			var timestamp = DateTime.UtcNow;
			AssertEmitsEvents(publish =>
			{
				var story = UserStory.BringIntoTheWorld("true story", publish, userStoryId, timestamp);
			}, new IEvent []
			{
				new StoryBirthdayEvent {Id = userStoryId, Name = "true story", Timestamp = timestamp},
			});
		}

		[Test]
		public void AssignPersonTest()
		{
			var person = new Person();
			var userStoryId = new UserStoryId(Guid.NewGuid());
			var birthTimestamp = DateTime.UtcNow;
			var assignTimestamp = birthTimestamp.AddHours(1);
			AssertEmitsEvents(publish =>
			{
				var story = UserStory.BringIntoTheWorld("true story", publish, userStoryId, birthTimestamp);
				story.AssignPerson(person, assignTimestamp);
			}, new IEvent[]
			{
				new StoryBirthdayEvent {Id = userStoryId, Name = "true story", Timestamp = birthTimestamp},
				new PersonAssignedEvent(person.Id, userStoryId, assignTimestamp),
			});
		}

		private void AssertEmitsEvents(Action<Action<UserStory, IEvent>> test, IList<IEvent> expectedEvents)
		{
			var emittedEvents = new List<IEvent>();
			Action<UserStory, IEvent> bublish = (us, @event) => { emittedEvents.Add(@event); };

			test(bublish);

			Assert.That(JsonConvert.SerializeObject(emittedEvents), Is.EqualTo(JsonConvert.SerializeObject(expectedEvents)));
		}
	}
}