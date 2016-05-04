using System;

namespace ReadStack.Messages
{
	public class PersonId
	{
		public Guid Value { get; }

		public PersonId(Guid value)
		{
			Value = value;
		}
	}
}