using System;
using System.Text;

namespace Duration
{
	public static class TimeParser
	{
		public static TimeSpan ToTimeSpan(ReadOnlySpan<char> span)
		{
			var reader = new TimeStringReader(span);

			TimeSpan finalTime = default;
			while (reader.Seek())
			{
				var currentTimeSpan = reader.CurrentToken.ToTimeSpan();
				finalTime += currentTimeSpan;
			}

			return finalTime;
		}

		public static ReadOnlySpan<char> FromTimeSpan(TimeSpan timeSpan)
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

		public static ReadOnlySpan<char> Normalize(ReadOnlySpan<char> span)
		{
			var timeSpan = ToTimeSpan(span);
			return FromTimeSpan(timeSpan);
		}

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
