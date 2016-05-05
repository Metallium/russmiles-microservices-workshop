using System;
using System.Threading;

namespace Tests
{
	public static class Waiter
	{
		public static T WaitFor<T>(Func<T> valueFactory)
			where T : class
		{
			return WaitFor(valueFactory, null);
		}

		public static T WaitFor<T>(Func<T> valueFactory, Func<T, bool> predicate)
			where T : class
		{
			return WaitFor(valueFactory, predicate, 30, TimeSpan.FromMilliseconds(100));
		}

		public static T WaitFor<T>(Func<T> valueFactory, Func<T, bool> predicate, int retries, TimeSpan waitTime)
			where T : class
		{
			predicate = predicate ?? (value => value != null);

			for (var retry = 0; retry < retries; ++retry)
			{
				var value = valueFactory();

				if (predicate(value))
				{
					return value;
				}

				Thread.Sleep(waitTime);
			}

			throw new TimeoutException($"WaitFor was not completed after {retries} retires with {waitTime} wait time each.");
		}
	}
}