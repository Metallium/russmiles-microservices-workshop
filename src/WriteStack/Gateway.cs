using System.Linq;
using WriteStack.Messages;

namespace WriteStack
{
	public class Gateway
	{
		private readonly InMemoryRepository _repo;

		public Gateway(InMemoryRepository repo)
		{
			_repo = repo;
		}

		public void Process(AssignPersonCommand command)
		{
			var userStory = _repo.UserStories.Single(x => x.Id.Value == command.UserStoryId.Value);
			var person = _repo.Persons.Single(x => x.Id.Value == command.PersonId.Value);

			userStory.AssignPerson(person);
		}

		public UserStoryViewDto PoorMansGetUserStory(UserStoryId id)
		{
			var userStory = _repo.UserStories.Single(x => x.Id.Value == id.Value);
			return new UserStoryViewDto
			{
				AssignedPersonIds = userStory.AssignedPersonIds.Select(x => x.Value).ToList()
			};
		}
	}
}