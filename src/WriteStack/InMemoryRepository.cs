using System.Collections.Generic;
using EventStore;
using WriteStack.Messages;

namespace WriteStack
{
	public class InMemoryRepository
	{
		private readonly Store _store;

		public InMemoryRepository(Store store)
		{
			_store = store;
//			UserStories.Add(new UserStory(Store));
//			Persons.Add(new Person());
		}
		// FIXME hide these exposed collections
		public IList<UserStory> UserStories { get; } = new List<UserStory>(); 
		public IList<Person> Persons { get; } = new List<Person>(); 
		public void Store(string streamName, IEvent @event)
		{
			_store.Append(EventSederializer.Serialize(streamName, @event));
		}
	}
}