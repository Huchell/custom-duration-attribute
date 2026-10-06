using System;
using NUnit.Framework;

namespace Duration.Tests
{
	public sealed partial class TimeStringTests
	{
		[TestCase("1s", 1)]
		[TestCase("2s", 2)]
		public void Parse_CanConvertSecondString(string timeStr, int expectedSeconds)
		{
			var result = TimeString.Parse(timeStr.AsSpan());

			var expected = TimeSpan.FromSeconds(expectedSeconds);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1m", 1)]
		[TestCase("2m", 2)]
		public void Parse_CanConvertMinuteString(string timeStr, int expectedMinutes)
		{
			var result = TimeString.Parse(timeStr.AsSpan());

			var expected = TimeSpan.FromMinutes(expectedMinutes);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1ms", 1)]
		[TestCase("2ms", 2)]
		public void Parse_CanConvertMillisecondString(string timeStr, int expectedMilliseconds)
		{
			var result = TimeString.Parse(timeStr.AsSpan());

			var expected = TimeSpan.FromMilliseconds(expectedMilliseconds);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1h", 1)]
		[TestCase("2h", 2)]
		public void Parse_CanConvertHourString(string timeStr, int expectedHours)
		{
			var result = TimeString.Parse(timeStr.AsSpan());

			var expected = TimeSpan.FromHours(expectedHours);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1d", 1)]
		[TestCase("2d", 2)]
		public void Parse_CanConvertDayString(string timeStr, int expectedDays)
		{
			var result = TimeString.Parse(timeStr.AsSpan());

			var expected = TimeSpan.FromDays(expectedDays);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void Parse_CanConvertMultiUnitString()
		{
			var timeStr = "1d 1h 1m 1s 1ms";
			var result = TimeString.Parse(timeStr.AsSpan());

			var expected = TimeSpan.FromDays(1)
				+ TimeSpan.FromHours(1)
				+ TimeSpan.FromMinutes(1)
				+ TimeSpan.FromSeconds(1)
				+ TimeSpan.FromMilliseconds(1);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void Parse_CanConvertMultiUnitStringWithoutSpaces()
		{
			var timeStr = "1d1h1m1s1ms";
			var result = TimeString.Parse(timeStr.AsSpan());

			var expected = TimeSpan.FromDays(1)
				+ TimeSpan.FromHours(1)
				+ TimeSpan.FromMinutes(1)
				+ TimeSpan.FromSeconds(1)
				+ TimeSpan.FromMilliseconds(1);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void Parse_Converts_StringsWithDecimalPoint()
		{
			var timeStr = "0.5d";
			var result = TimeString.Parse(timeStr);

			var expected = TimeSpan.FromHours(12);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void TryParse_ReturnsFalse_IfStringIsMalformed()
		{
			var malformedStr = "not a time span string";
			var result = TimeString.TryParse(malformedStr, out var _);
			Assert.That(result, Is.False);
		}

		[Test]
		public void TryParse_ReturnedTimeSpanIsZero_IfStringIsMalformed()
		{
			var malformedStr = "not a time span string";
			TimeString.TryParse(malformedStr, out var timeSpan);
			Assert.That(timeSpan, Is.EqualTo(TimeSpan.Zero));
		}

		[Test]
		public void TryParse_ReturnsTrue_WhenSuccessfullyParsed()
		{
			var correctStr = "1m";
			var result = TimeString.TryParse(correctStr, out var _);
			Assert.That(result, Is.True);
		}

		[TestCase("1s", 1)]
		[TestCase("2s", 2)]
		public void TryParse_CanConvertSecondString(string timeStr, int expectedSeconds)
		{
			TimeString.TryParse(timeStr.AsSpan(), out var result);

			var expected = TimeSpan.FromSeconds(expectedSeconds);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1m", 1)]
		[TestCase("2m", 2)]
		public void TryParse_CanConvertMinuteString(string timeStr, int expectedMinutes)
		{
			TimeString.TryParse(timeStr.AsSpan(), out var result);

			var expected = TimeSpan.FromMinutes(expectedMinutes);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1ms", 1)]
		[TestCase("2ms", 2)]
		public void TryParse_CanConvertMillisecondString(string timeStr, int expectedMilliseconds)
		{
			TimeString.TryParse(timeStr.AsSpan(), out var result);

			var expected = TimeSpan.FromMilliseconds(expectedMilliseconds);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1h", 1)]
		[TestCase("2h", 2)]
		public void TryParse_CanConvertHourString(string timeStr, int expectedHours)
		{
			TimeString.TryParse(timeStr.AsSpan(), out var result);

			var expected = TimeSpan.FromHours(expectedHours);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1d", 1)]
		[TestCase("2d", 2)]
		public void TryParse_CanConvertDayString(string timeStr, int expectedDays)
		{
			TimeString.TryParse(timeStr.AsSpan(), out var result);

			var expected = TimeSpan.FromDays(expectedDays);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void TryParse_CanConvertMultiUnitString()
		{
			var timeStr = "1d 1h 1m 1s 1ms";
			TimeString.TryParse(timeStr.AsSpan(), out var result);

			var expected = TimeSpan.FromDays(1)
				+ TimeSpan.FromHours(1)
				+ TimeSpan.FromMinutes(1)
				+ TimeSpan.FromSeconds(1)
				+ TimeSpan.FromMilliseconds(1);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void TryParse_CanConvertMultiUnitStringWithoutSpaces()
		{
			var timeStr = "1d1h1m1s1ms";
			TimeString.TryParse(timeStr.AsSpan(), out var result);

			var expected = TimeSpan.FromDays(1)
				+ TimeSpan.FromHours(1)
				+ TimeSpan.FromMinutes(1)
				+ TimeSpan.FromSeconds(1)
				+ TimeSpan.FromMilliseconds(1);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void TryParse_Converts_StringsWithDecimalPoint()
		{
			var timeStr = "0.5d";
			TimeString.TryParse(timeStr, out var result);

			var expected = TimeSpan.FromHours(12);
			Assert.That(result, Is.EqualTo(expected));
		}
	}
}
