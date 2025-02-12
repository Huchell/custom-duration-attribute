using System;
using NUnit.Framework;

namespace Huchell.Unity.Editor.Tests
{
	[TestFixture]
	public sealed class TimeStringConverterTests
	{
		[TestCase("1s", 1)]
		[TestCase("2s", 2)]
		public void ToTimeSpan_CanConvertSecondString(string timeStr, int expectedSeconds)
		{
			var result = TimeStringConverter.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromSeconds(expectedSeconds);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1m", 1)]
		[TestCase("2m", 2)]
		public void ToTimeSpan_CanConvertMinuteString(string timeStr, int expectedMinutes)
		{
			var result = TimeStringConverter.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromMinutes(expectedMinutes);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1ms", 1)]
		[TestCase("2ms", 2)]
		public void ToTimeSpan_CanConvertMillisecondString(string timeStr, int expectedMilliseconds)
		{
			var result = TimeStringConverter.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromMilliseconds(expectedMilliseconds);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1h", 1)]
		[TestCase("2h", 2)]
		public void ToTimeSpan_CanConvertHourString(string timeStr, int expectedHours)
		{
			var result = TimeStringConverter.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromHours(expectedHours);
			Assert.That(result, Is.EqualTo(expected));
		}

		[TestCase("1d", 1)]
		[TestCase("2d", 2)]
		public void ToTimeSpan_CanConvertDayString(string timeStr, int expectedDays)
		{
			var result = TimeStringConverter.ToTimeSpan(timeStr.AsSpan());

			var expected = TimeSpan.FromDays(expectedDays);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void ToTimeSpan_CanConvertMultiUnitString()
		{
			var timeStr = "1d 1h 1m 1s 1ms";
			var result = TimeStringConverter.ToTimeSpan(timeStr.AsSpan());

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
			var result = TimeStringConverter.ToTimeSpan(timeStr.AsSpan());

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
			var result = TimeStringConverter.ToTimeSpan(timeStr);

			var expected = TimeSpan.FromHours(12);
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_Milliseconds()
		{
			var time = TimeSpan.FromMilliseconds(1);
			var result = TimeStringConverter.FromTimeSpan(time);

			var expected = "1ms";
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_Seconds()
		{
			var time = TimeSpan.FromSeconds(1);
			var result = TimeStringConverter.FromTimeSpan(time);

			var expected = "1s";
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_Minutes()
		{
			var time = TimeSpan.FromMinutes(1);
			var result = TimeStringConverter.FromTimeSpan(time);

			var expected = "1m";
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_Hours()
		{
			var time = TimeSpan.FromHours(1);
			var result = TimeStringConverter.FromTimeSpan(time);

			var expected = "1h";
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_Days()
		{
			var time = TimeSpan.FromDays(1);
			var result = TimeStringConverter.FromTimeSpan(time);

			var expected = "1d";
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_ComplexTimeSpan()
		{
			var time = TimeSpan.FromDays(2) + TimeSpan.FromMinutes(5) + TimeSpan.FromSeconds(50);
			var result = TimeStringConverter.FromTimeSpan(time);

			var expected = "2d 5m 50s";
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void Normalize_Removes_OverflowValues()
		{
			var timeStr = "72s";
			var result = TimeStringConverter.Normalize(timeStr);

			var expected = "1m 12s";
			Assert.That(result, Is.EqualTo(expected));
		}
	}
}
