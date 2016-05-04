using System;
using WriteStack.Messages;

namespace WriteStack
{
	public class Person
	{
		public PersonId Id { get; } = new PersonId(Guid.NewGuid());
	}
}