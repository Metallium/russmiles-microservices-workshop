using System;
using WriteStack;
using WriteStack.Messages;
using NUnit.Framework;

namespace Tests
{
	public class UserStoryStateTests
	{
		[Test]
		public void BringIntoTheWorldTest()
		{
			Action<UserStory, IEvent> bublish = (us, @event) => { };
			var story = UserStory.BringIntoTheWorld("true story", bublish);
			Assert.That(story.Id, Is.Not.Null);
			Assert.That(story.Name, Is.EqualTo("true story"));
			Assert.That(story.AssignedPersonIds, Is.Empty);
		}

		[Test]
		public void AssignPersonTest()
		{
			Action<UserStory, IEvent> bublish = (us, @event) => { };
			var story = UserStory.BringIntoTheWorld("true story", bublish);
			var person = new Person();
			story.AssignPerson(person);
			Assert.That(story.AssignedPersonIds, Is.EquivalentTo(new [] {person.Id}));
		}
	}
}