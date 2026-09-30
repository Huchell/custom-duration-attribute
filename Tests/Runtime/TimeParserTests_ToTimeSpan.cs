using System;
using NUnit.Framework;

namespace Duration.Tests
{
	public sealed partial class TimeParserTests
	{
		[TestCase("1s", 1)]
		[TestCase("2s", 2)]
		public void ToTimeSpan_CanConvertSecondString(string timeStr, int expectedSeconds)
		{
			var result = TimeParser.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromSeconds(expectedSeconds);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1m", 1)]
		[TestCase("2m", 2)]
		public void ToTimeSpan_CanConvertMinuteString(string timeStr, int expectedMinutes)
		{
			var result = TimeParser.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromMinutes(expectedMinutes);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1ms", 1)]
		[TestCase("2ms", 2)]
		public void ToTimeSpan_CanConvertMillisecondString(string timeStr, int expectedMilliseconds)
		{
			var result = TimeParser.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromMilliseconds(expectedMilliseconds);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1h", 1)]
		[TestCase("2h", 2)]
		public void ToTimeSpan_CanConvertHourString(string timeStr, int expectedHours)
		{
			var result = TimeParser.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromHours(expectedHours);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1d", 1)]
		[TestCase("2d", 2)]
		public void ToTimeSpan_CanConvertDayString(string timeStr, int expectedDays)
		{
			var result = TimeParser.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromDays(expectedDays);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void ToTimeSpan_CanConvertMultiUnitString()
		{
			var timeStr = "1d 1h 1m 1s 1ms";
			var result = TimeParser.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromDays(1)
				+ TimeSpan.FromHours(1)
				+ TimeSpan.FromMinutes(1)
				+ TimeSpan.FromSeconds(1)
				+ TimeSpan.FromMilliseconds(1);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void ToTimeSpan_CanConvertMultiUnitStringWithoutSpaces()
		{
			var timeStr = "1d1h1m1s1ms";
			var result = TimeParser.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromDays(1)
				+ TimeSpan.FromHours(1)
				+ TimeSpan.FromMinutes(1)
				+ TimeSpan.FromSeconds(1)
				+ TimeSpan.FromMilliseconds(1);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void ToTimeSpan_Converts_StringsWithDecimalPoint()
		{
			var timeStr = "0.5d";
			var result = TimeParser.ToTimeSpan(timeStr);

			var expected = TimeSpan.FromHours(12);
			Assert.That(result, Is.EqualTo(expected));
		}
	}
}
