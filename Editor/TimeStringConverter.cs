using System;
using System.Text;

namespace Huchell.Unity.Editor
{
	internal static class TimeStringConverter
	{
		public static TimeSpan ToTimeSpan(ReadOnlySpan<char> span)
		{
			var reader = new TimeStringReader(span);

			var finalTime = TimeSpan.Zero;
			while (reader.Seek())
			{
				finalTime += reader.CurrentToken.ToTimeSpan();
			}

			return finalTime;
		}

		public static double ToDouble(ReadOnlySpan<char> span, TimeUnit baseUnit)
		{
			var timeSpan = ToTimeSpan(span);
			return baseUnit switch
			{
				TimeUnit.Milliseconds => timeSpan.TotalMilliseconds,
				TimeUnit.Seconds => timeSpan.TotalSeconds,
				TimeUnit.Minutes => timeSpan.TotalMinutes,
				TimeUnit.Hours => timeSpan.TotalHours,
				TimeUnit.Days => timeSpan.TotalDays,
				_ => timeSpan.Ticks,
			};
		}

		public static float ToSingle(ReadOnlySpan<char> span, TimeUnit baseUnit)
		{
			var timeSpan = ToTimeSpan(span);
			return baseUnit switch
			{
				TimeUnit.Milliseconds => (float)timeSpan.TotalMilliseconds,
				TimeUnit.Seconds => (float)timeSpan.TotalSeconds,
				TimeUnit.Minutes => (float)timeSpan.TotalMinutes,
				TimeUnit.Hours => (float)timeSpan.TotalHours,
				TimeUnit.Days => (float)timeSpan.TotalDays,
				_ => timeSpan.Ticks,
			};
		}

		public static long ToInt64(ReadOnlySpan<char> span, TimeUnit baseUnit)
		{
			var timeSpan = ToTimeSpan(span);
			return baseUnit switch
			{
				TimeUnit.Milliseconds => (long)timeSpan.TotalMilliseconds,
				TimeUnit.Seconds => (long)timeSpan.TotalSeconds,
				TimeUnit.Minutes => (long)timeSpan.TotalMinutes,
				TimeUnit.Hours => (long)timeSpan.TotalHours,
				TimeUnit.Days => (long)timeSpan.TotalDays,
				_ => timeSpan.Ticks,
			};
		}

		public static int ToInt32(ReadOnlySpan<char> span, TimeUnit baseUnit)
		{
			var timeSpan = ToTimeSpan(span);
			return baseUnit switch
			{
				TimeUnit.Milliseconds => (int)timeSpan.TotalMilliseconds,
				TimeUnit.Seconds => (int)timeSpan.TotalSeconds,
				TimeUnit.Minutes => (int)timeSpan.TotalMinutes,
				TimeUnit.Hours => (int)timeSpan.TotalHours,
				TimeUnit.Days => (int)timeSpan.TotalDays,
				_ => (int)timeSpan.Ticks,
			};
		}

		public static ulong ToUInt64(ReadOnlySpan<char> span, TimeUnit baseUnit)
		{
			var timeSpan = ToTimeSpan(span);
			return baseUnit switch
			{
				TimeUnit.Milliseconds => (ulong)timeSpan.TotalMilliseconds,
				TimeUnit.Seconds => (ulong)timeSpan.TotalSeconds,
				TimeUnit.Minutes => (ulong)timeSpan.TotalMinutes,
				TimeUnit.Hours => (ulong)timeSpan.TotalHours,
				TimeUnit.Days => (ulong)timeSpan.TotalDays,
				_ => (ulong)timeSpan.Ticks,
			};
		}

		public static uint ToUInt32(ReadOnlySpan<char> span, TimeUnit baseUnit)
		{
			var timeSpan = ToTimeSpan(span);
			return baseUnit switch
			{
				TimeUnit.Milliseconds => (uint)timeSpan.TotalMilliseconds,
				TimeUnit.Seconds => (uint)timeSpan.TotalSeconds,
				TimeUnit.Minutes => (uint)timeSpan.TotalMinutes,
				TimeUnit.Hours => (uint)timeSpan.TotalHours,
				TimeUnit.Days => (uint)timeSpan.TotalDays,
				_ => (uint)timeSpan.Ticks,
			};
		}

		public static string FromTimeSpan(TimeSpan timeSpan)
		{
			var builder = new StringBuilder();
			AppendUnit(builder, timeSpan.Days, "d");
			AppendUnit(builder, timeSpan.Hours, "h");
			AppendUnit(builder, timeSpan.Minutes, "m");
			AppendUnit(builder, timeSpan.Seconds, "s");
			AppendUnit(builder, timeSpan.Milliseconds, "ms");

			if (builder.Length == 0)
			{
				builder.Append("0s");
			}
			return builder.ToString();
		}

		public static string FromDouble(double time, TimeUnit baseUnit)
		{
			var timeSpan = baseUnit switch
			{
				TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(time),
				TimeUnit.Seconds => TimeSpan.FromSeconds(time),
				TimeUnit.Minutes => TimeSpan.FromMinutes(time),
				TimeUnit.Hours => TimeSpan.FromHours(time),
				TimeUnit.Days => TimeSpan.FromDays(time),
				_ => new TimeSpan((long)time),
			};
			return FromTimeSpan(timeSpan);
		}

		public static string FromSingle(float time, TimeUnit baseUnit)
		{
			var timeSpan = baseUnit switch
			{
				TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(time),
				TimeUnit.Seconds => TimeSpan.FromSeconds(time),
				TimeUnit.Minutes => TimeSpan.FromMinutes(time),
				TimeUnit.Hours => TimeSpan.FromHours(time),
				TimeUnit.Days => TimeSpan.FromDays(time),
				_ => new TimeSpan((long)time),
			};
			return FromTimeSpan(timeSpan);
		}

		public static string FromInt64(long time, TimeUnit baseUnit)
		{
			var timeSpan = baseUnit switch
			{
				TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(time),
				TimeUnit.Seconds => TimeSpan.FromSeconds(time),
				TimeUnit.Minutes => TimeSpan.FromMinutes(time),
				TimeUnit.Hours => TimeSpan.FromHours(time),
				TimeUnit.Days => TimeSpan.FromDays(time),
				_ => new TimeSpan(time),
			};
			return FromTimeSpan(timeSpan);
		}

		public static string FromInt32(int time, TimeUnit baseUnit)
		{
			var timeSpan = baseUnit switch
			{
				TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(time),
				TimeUnit.Seconds => TimeSpan.FromSeconds(time),
				TimeUnit.Minutes => TimeSpan.FromMinutes(time),
				TimeUnit.Hours => TimeSpan.FromHours(time),
				TimeUnit.Days => TimeSpan.FromDays(time),
				_ => new TimeSpan(time),
			};
			return FromTimeSpan(timeSpan);
		}

		public static string FromUInt64(ulong time, TimeUnit baseUnit)
		{
			var timeSpan = baseUnit switch
			{
				TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(time),
				TimeUnit.Seconds => TimeSpan.FromSeconds(time),
				TimeUnit.Minutes => TimeSpan.FromMinutes(time),
				TimeUnit.Hours => TimeSpan.FromHours(time),
				TimeUnit.Days => TimeSpan.FromDays(time),
				_ => new TimeSpan((long)time),
			};
			return FromTimeSpan(timeSpan);
		}

		public static string FromUInt32(uint time, TimeUnit baseUnit)
		{
			var timeSpan = baseUnit switch
			{
				TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(time),
				TimeUnit.Seconds => TimeSpan.FromSeconds(time),
				TimeUnit.Minutes => TimeSpan.FromMinutes(time),
				TimeUnit.Hours => TimeSpan.FromHours(time),
				TimeUnit.Days => TimeSpan.FromDays(time),
				_ => new TimeSpan(time),
			};
			return FromTimeSpan(timeSpan);
		}

		public static string Normalize(ReadOnlySpan<char> span)
		{
			return FromTimeSpan(ToTimeSpan(span));
		}

		public static string Normalize(string timeStr) => Normalize(timeStr.AsSpan());

		private static void AppendUnit(StringBuilder builder, int time, ReadOnlySpan<char> unit)
		{
			if (time == 0)
			{
				return;
			}

			if (builder.Length > 0)
			{
				builder.Append(' ');
			}

			builder.Append(time);
			builder.Append(unit);
		}
	}
}
