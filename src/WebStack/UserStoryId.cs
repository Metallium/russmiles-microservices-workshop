using System;

namespace WebStack
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