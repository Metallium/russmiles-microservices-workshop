using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EventStore
{
	public class Store
	{
		private readonly IDictionary<string, IList<object>> _storage = new ConcurrentDictionary<string, IList<object>>();

		public event EventHandler<EventHolder> OnAppended;

		public void Append(EventHolder eventHolder)
		{
			var streamName = eventHolder.StreamName;
		
			if (!_storage.ContainsKey(streamName))
			{
				_storage.Add(streamName, new List<object>());
			}
			
			_storage[streamName].Add(eventHolder);
			OnAppended?.Invoke(this, eventHolder);
		}
	}
}
