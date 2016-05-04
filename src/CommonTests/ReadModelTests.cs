using System;
using EventStore;
using ReadStack;
using WriteStack;
using WriteStack.Messages;
using NUnit.Framework;

namespace Tests
{
	public class ReadModelTests
	{
		[Test]
		public void BringIntoTheWorldProjectionTest()
		{
			var store = new Store();
			Func<UserStory, string> toStreamName = us => us.Id.Value.ToString("N");
			Action<UserStory, IEvent> publish = (us, @event) => store.Append(EventSederializer.Serialize(toStreamName(us), @event));

			var readModel = new ReadModel(store);

			var timestamp = DateTime.UtcNow;
			var userStory = UserStory.BringIntoTheWorld("cool name", publish, timestamp: timestamp);

			var dto = readModel.GetStory(toStreamName(userStory));
			Assert.That(dto.AssignedPersonsCount, Is.EqualTo(0));
			Assert.That(dto.Id, Is.Not.Empty);
			Assert.That(dto.Name, Is.EqualTo("cool name"));
			Assert.That(dto.ModifyDate, Is.EqualTo(timestamp));
		}

		[Test]
		public void AssignPersonProjectionTest()
		{
			var store = new Store();
			Func<UserStory, string> toStreamName = us => us.Id.Value.ToString("N");
			Action<UserStory, IEvent> publish = (us, @event) => store.Append(EventSederializer.Serialize(toStreamName(us), @event));

			var readModel = new ReadModel(store);

			var userStory = UserStory.BringIntoTheWorld("cool name", publish, timestamp: DateTime.UtcNow);
			var assignTimestamp = DateTime.UtcNow;
			userStory.AssignPerson(new Person(), assignTimestamp);

			var dto = readModel.GetStory(toStreamName(userStory));
			Assert.That(dto.AssignedPersonsCount, Is.EqualTo(1));
			Assert.That(dto.ModifyDate, Is.EqualTo(assignTimestamp));
		}
	}
}