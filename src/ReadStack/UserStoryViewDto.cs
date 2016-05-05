using System;

namespace ReadStack
{
	public class UserStoryViewDto
	{
		public DateTime ModifyDate { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public int AssignedPersonsCount { get; set; }
		public int Version { get; set; }
	}
}