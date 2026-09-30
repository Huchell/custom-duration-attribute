using System;
using NUnit.Framework;

namespace Duration.Tests
{
	public sealed partial class TimeStringTests
	{
		[Test]
		public void FromTimeSpan_Converts_Milliseconds()
		{
			var time = TimeSpan.FromMilliseconds(1);
			var result = TimeString.FromTimeSpan(time);

			var expected = "1ms";
			Assert.That(result.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_Seconds()
		{
			var time = TimeSpan.FromSeconds(1);
			var result = TimeString.FromTimeSpan(time);

			var expected = "1s";
			Assert.That(result.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_Minutes()
		{
			var time = TimeSpan.FromMinutes(1);
			var result = TimeString.FromTimeSpan(time);

			var expected = "1m";
			Assert.That(result.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_Hours()
		{
			var time = TimeSpan.FromHours(1);
			var result = TimeString.FromTimeSpan(time);

			var expected = "1h";
			Assert.That(result.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_Days()
		{
			var time = TimeSpan.FromDays(1);
			var result = TimeString.FromTimeSpan(time);

			var expected = "1d";
			Assert.That(result.ToString(), Is.EqualTo(expected));
		}

		[Test]
		public void FromTimeSpan_Converts_ComplexTimeSpan()
		{
			var time = TimeSpan.FromDays(2) + TimeSpan.FromMinutes(5) + TimeSpan.FromSeconds(50);
			var result = TimeString.FromTimeSpan(time);

			var expected = "2d 5m 50s";
			Assert.That(result.ToString(), Is.EqualTo(expected));
		}
	}
}
