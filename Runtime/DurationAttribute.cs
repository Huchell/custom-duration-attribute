using UnityEngine;

namespace Huchell.Unity
{
	public sealed class DurationAttribute : PropertyAttribute
	{
		public TimeUnit BaseUnit { get; }

		public DurationAttribute()
		{
			this.BaseUnit = TimeUnit.Seconds;
		}

		public DurationAttribute(TimeUnit baseUnit)
		{
			this.BaseUnit = baseUnit;
		}
	}

	public enum TimeUnit
	{
		Milliseconds,
		Seconds,
		Minutes,
		Hours,
		Days,
	}
}
