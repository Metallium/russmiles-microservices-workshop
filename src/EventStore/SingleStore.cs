using System;
using System.Threading;

namespace EventStore
{
	public class SingleStore
	{
		private static readonly Lazy<Store> Shits = new Lazy<Store>(
			() => new Store(),
			LazyThreadSafetyMode.ExecutionAndPublication
			);

		public static Store Instance => Shits.Value;
	}
}