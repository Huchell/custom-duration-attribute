using Duration;
using Huchell.Unity;
using NUnit.Framework;
using UnityEngine.UIElements;
using UnityEngine.UIElements.TestFramework;

namespace Huchell.Duration
{
	public sealed class DurationElementValueTests : UITestFixture
	{
		private DurationElement field;

		[SetUp]
		public void SetUp()
		{
			new IntegerField();
			panelSize = new(100, 100);

			field = new();
			rootVisualElement.Add(field);
		}

		[TestCase("1m")]
		[TestCase("1h")]
		public void Value_SetsText(string timeSpanStr)
		{
			var timeSpan = TimeString.Parse(timeSpanStr);

			field.value = timeSpan;

			Assert.That(field.text, Is.EqualTo(timeSpanStr));
		}

		[TestCase("1m")]
		[TestCase("1h")]
		public void Text_SetsValue_WhenTextIsValidTimeSpanString(string timeSpanStr)
		{
			simulate.FrameUpdate();

			field.Focus();
			simulate.TypingText(timeSpanStr);
			simulate.FrameUpdate();

			var timeSpan = TimeString.Parse(timeSpanStr);
			Assert.That(field.value, Is.EqualTo(timeSpan));
		}
	}
}
