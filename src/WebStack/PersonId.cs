using System;

namespace WebStack
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