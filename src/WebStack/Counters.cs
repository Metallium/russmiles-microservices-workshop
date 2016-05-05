using Prometheus;

namespace WebStack
{
	public static class Counters
	{
		public static readonly Counter Requests = Metrics.CreateCounter("WebStack:requests", "Requests made from WebStack");
	}
}