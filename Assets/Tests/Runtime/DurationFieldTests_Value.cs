using Duration;
using Huchell.Unity;
using NUnit.Framework;
using UnityEngine.UIElements.TestFramework;

namespace Huchell.Duration
{
	public sealed class DurationFieldTests_Value : UITestFixture
	{
		private DurationField field;

		[SetUp]
		public void SetUp()
		{
			this.panelSize = new(100, 100);

			this.field = new();
			this.rootVisualElement.Add(field);
		}

		[TestCase("1m")]
		[TestCase("1h")]
		public void Value_SetsText(string timeSpanStr)
		{
			var timeSpan = TimeString.Parse(timeSpanStr);

			this.field.value = timeSpan;

			Assert.That(this.field.text, Is.EqualTo(timeSpanStr));
		}

		[TestCase("1m")]
		[TestCase("1h")]
		public void Text_SetsValue_WhenTextIsValidTimeSpanString(string timeSpanStr)
		{
			this.simulate.FrameUpdate();

			this.field.Focus();
			this.simulate.TypingText(timeSpanStr);
			this.simulate.FrameUpdate();

			var timeSpan = TimeString.Parse(timeSpanStr);
			Assert.That(this.field.value, Is.EqualTo(timeSpan));
		}
	}
}
