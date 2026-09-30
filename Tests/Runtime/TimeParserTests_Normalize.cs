using NUnit.Framework;

namespace Duration.Tests
{
	public sealed partial class TimeParserTests
	{
		[Test]
		public void Normalize_Removes_OverflowValues()
		{
			var timeStr = "72s";
			var result = TimeParser.Normalize(timeStr);

			var expected = "1m 12s";
			Assert.That(result.ToString(), Is.EqualTo(expected));
		}
	}
}
