using Huchell.Unity;
using NUnit.Framework;
using UnityEngine.UIElements.TestFramework;

namespace Huchell.Duration
{
	public sealed class IntegerDurationFieldTests_Value : UITestFixture
	{
		private IntegerDurationField field;

		[SetUp]
		public void SetUp()
		{
			this.panelSize = new(100, 100);

			this.field = new();
			this.rootVisualElement.Add(this.field);
		}

		[TestCase(1, "1m", TimeUnit.Minutes)]
		[TestCase(1, "1h", TimeUnit.Hours)]
		[TestCase(60, "1h", TimeUnit.Minutes)]
		public void Value_SetsText_BasedOnTimeUnit(long value, string timeSpanStr, TimeUnit timeUnit)
		{
			this.field.BaseUnit = timeUnit;

			this.field.value = value;

			Assert.That(this.field.text, Is.EqualTo(timeSpanStr));
		}

		[TestCase("1m", 1, TimeUnit.Minutes)]
		[TestCase("1h", 1, TimeUnit.Hours)]
		[TestCase("1h", 60, TimeUnit.Minutes)]
		public void Text_SetsValue_WhenTextIsValidTimeSpanString(string timeSpanStr, long expectedValue, TimeUnit timeUnit)
		{
			this.field.BaseUnit = timeUnit;

			this.simulate.FrameUpdate();

			this.field.Focus();
			this.simulate.TypingText(timeSpanStr);
			this.simulate.FrameUpdate();

			Assert.That(this.field.value, Is.EqualTo(expectedValue));
		}

		[Test]
		public void Value_UpdatesWhenNewTimeUnitIsSet()
		{
			this.field.BaseUnit = TimeUnit.Seconds;
			this.field.value = 60;

			this.field.BaseUnit = TimeUnit.Minutes;

			Assert.That(this.field.value, Is.EqualTo(1));
		}
	}
}
