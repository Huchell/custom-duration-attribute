using System;
using System.Text;

namespace Duration
{
	public static class TimeString
	{
		public static TimeSpan Parse(ReadOnlySpan<char> span)
		{
			var reader = new TimeStringReader(span);

			TimeSpan finalTime = TimeSpan.Zero;
			while (reader.Seek())
			{
				var currentTimeSpan = reader.CurrentToken.ToTimeSpan() ?? throw new Exception("Failed to parse time span");
				finalTime += currentTimeSpan;
			}

			return finalTime;
		}

		public static bool TryParse(ReadOnlySpan<char> span, out TimeSpan timeSpan)
		{
			var reader = new TimeStringReader(span);

			TimeSpan finalTime = TimeSpan.Zero;
			while (reader.Seek())
			{
				var currentTimeSpan = reader.CurrentToken.ToTimeSpan();
				if (currentTimeSpan is null)
				{
					timeSpan = TimeSpan.Zero;
					return false;
				}

				finalTime += currentTimeSpan.Value;
			}

			timeSpan = finalTime;
			return true;
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
			return FromTimeSpan(Parse(span));
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
