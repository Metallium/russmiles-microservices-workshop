using System;
using System.Web.Http;
using WriteStack.Messages;

namespace WriteStack.Controllers
{
	public class HomeController : ApiController
	{
		public void AssignPerson(AssignPersonCommand assignPersonCommand)
		{
			Func<UserStory, string> toStreamName = us => "aggregate-userStory-" + us.Id.Value.ToString("N");
			Action<UserStory, IEvent> publish = (us, @event) => new Store().Append(EventSederializer.Serialize(toStreamName(us), @event));

			var timestamp = DateTime.UtcNow;
			var userStory = UserStory.BringIntoTheWorld("cool name", publish, timestamp: timestamp);
			userStory.AssignPerson(new Person {Id = assignPersonCommand.PersonId}, DateTime.UtcNow);
		}
	}
}