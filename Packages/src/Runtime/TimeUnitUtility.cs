using System;

namespace Huchell.Unity
{
	internal static class TimeUnitUtility
	{
		public static TimeSpan ConvertFromDouble(double num, TimeUnit timeUnit)
		{
			return timeUnit switch
			{
				TimeUnit.Days => TimeSpan.FromDays(num),
				TimeUnit.Hours => TimeSpan.FromHours(num),
				TimeUnit.Minutes => TimeSpan.FromMinutes(num),
				TimeUnit.Seconds => TimeSpan.FromSeconds(num),
				TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(num),
				_ => TimeSpan.FromTicks((long)Math.Round(num)),
			};
		}

		public static TimeSpan ConvertFromInt(long num, TimeUnit unit)
		{
			return unit switch
			{
				TimeUnit.Days => TimeSpan.FromDays(num),
				TimeUnit.Hours => TimeSpan.FromHours(num),
				TimeUnit.Minutes => TimeSpan.FromMinutes(num),
				TimeUnit.Seconds => TimeSpan.FromSeconds(num),
				TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(num),
				_ => TimeSpan.FromTicks(num),
			};
		}

		public static double ConvertToDouble(TimeSpan value, TimeUnit timeUnit)
		{
			return timeUnit switch
			{
				TimeUnit.Days => value.TotalDays,
				TimeUnit.Hours => value.TotalHours,
				TimeUnit.Minutes => value.TotalMinutes,
				TimeUnit.Seconds => value.TotalSeconds,
				TimeUnit.Milliseconds => value.TotalMilliseconds,
				_ => value.Ticks,
			};
		}

		public static long ConvertToInt(TimeSpan value, TimeUnit timeUnit)
		{
			return timeUnit switch
			{
				TimeUnit.Days => (long)Math.Round(value.TotalDays),
				TimeUnit.Hours => (long)Math.Round(value.TotalHours),
				TimeUnit.Minutes => (long)Math.Round(value.TotalMinutes),
				TimeUnit.Seconds => (long)Math.Round(value.TotalSeconds),
				TimeUnit.Milliseconds => (long)Math.Round(value.TotalMilliseconds),
				_ => value.Ticks,
			};
		}
	}
}
