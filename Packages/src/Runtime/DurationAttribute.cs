using UnityEngine;

namespace Huchell.Unity
{
	public sealed class DurationAttribute : PropertyAttribute
	{
		public TimeUnit BaseUnit { get; }

		public DurationAttribute(TimeUnit baseUnit)
		{
			this.BaseUnit = baseUnit;
		}
	}
}
