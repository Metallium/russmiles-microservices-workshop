using System;

namespace WriteStack.Messages
{
	public class UserStoryId
	{
		public Guid Value { get; }

		public UserStoryId(Guid value)
		{
			Value = value;
		}
	}
}