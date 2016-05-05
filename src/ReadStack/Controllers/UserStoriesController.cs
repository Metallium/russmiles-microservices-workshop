using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ReadStack.Controllers
{
	public class UserStoriesController : ApiController
	{
		public static ReadModel Model = new ReadModel(new Store());

		public List<UserStoryViewDto> List()
		{
			return Model.GetStories();
		}
	}
}