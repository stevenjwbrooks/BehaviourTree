using System;

namespace BehaviourTree
{
	public sealed class Clock : IClock
	{
		public ulong GetTimeStampInMilliseconds()
		{
			return (ulong)TimeSpan.FromTicks(DateTime.UtcNow.Ticks).Milliseconds;
		}
	}
}
