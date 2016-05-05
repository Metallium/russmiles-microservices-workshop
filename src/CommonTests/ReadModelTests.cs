using System;
using System.Threading;
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
			var readModel = new ReadModel(new ReadStack.Store());

			Func<UserStory, string> toStreamName = us => "aggregate-userStory-" + us.Id.Value.ToString("N");
			Action<UserStory, IEvent> publish = (us, @event) => new WriteStack.Store().Append(EventSederializer.Serialize(toStreamName(us), @event));


			var timestamp = DateTime.UtcNow;
			var userStory = UserStory.BringIntoTheWorld("cool name", publish, timestamp: timestamp);

			var dto = Waiter.WaitFor(() => readModel.GetStory(toStreamName(userStory)));
			
			Assert.That(dto.Version, Is.EqualTo(1));
			Assert.That(dto.AssignedPersonsCount, Is.EqualTo(0));
			Assert.That(dto.Id, Is.Not.Empty);
			Assert.That(dto.Name, Is.EqualTo("cool name"));
			Assert.That(dto.ModifyDate, Is.EqualTo(timestamp));
		}

		[Test]
		public void AssignPersonProjectionTest()
		{
			Func<UserStory, string> toStreamName = us => "aggregate-userStory-" + us.Id.Value.ToString("N");
			Action<UserStory, IEvent> publish = (us, @event) => new WriteStack.Store().Append(EventSederializer.Serialize(toStreamName(us), @event));

			var readModel = new ReadModel(new ReadStack.Store());

			var userStory = UserStory.BringIntoTheWorld("cool name", publish, timestamp: DateTime.UtcNow);
			var assignTimestamp = DateTime.UtcNow;
			userStory.AssignPerson(new Person(), assignTimestamp);

			var dto = Waiter.WaitFor(() => readModel.GetStory(toStreamName(userStory)), x => x?.Version > 1);

			Assert.That(dto.AssignedPersonsCount, Is.EqualTo(1));
			Assert.That(dto.ModifyDate, Is.EqualTo(assignTimestamp));
		}
	}
}