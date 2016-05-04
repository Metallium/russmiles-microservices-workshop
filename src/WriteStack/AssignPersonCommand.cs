using WriteStack.Messages;

namespace WriteStack
{
	public class AssignPersonCommand
	{
		public UserStoryId UserStoryId { get; set; }
		public PersonId PersonId { get; set; }
	}
}
