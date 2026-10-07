#if UNITY_UIELEMENTS
using System;
using Duration;
using UnityEngine;
using UnityEngine.UIElements;

namespace Huchell.Unity
{
	public sealed class DurationElement : TextValueField<TimeSpan>
	{
		public DurationElement() : this(null, TimeSpan.Zero)
		{
		}

		public DurationElement(string label) : this(label, TimeSpan.Zero)
		{
		}

		public DurationElement(TimeSpan value) : this(null, value)
		{
		}

		public DurationElement(string label, TimeSpan value) : base(label, int.MaxValue, new DurationFieldInput())
		{
			this.value = value;
			this.AddLabelDragger<TimeSpan>();
		}

		private DurationFieldInput DurationInput => (DurationFieldInput)base.textInputBase;

		public TimeUnit Unit { get; set; } = TimeUnit.Seconds;

		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, TimeSpan startValue)
		{
			this.DurationInput.ApplyInputDeviceDelta(delta, speed, startValue);
		}

		protected override TimeSpan StringToValue(string str)
		{
			return TimeString.Parse(str);
		}

		protected override string ValueToString(TimeSpan value)
		{
			return TimeString.FromTimeSpan(value).ToString();
		}

		private sealed class DurationFieldInput : TextValueInput
		{
			private DurationElement ParentDurationField => (DurationElement)base.parent;

			protected override string allowedCharacters => "0123456789 dhms";

			public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, TimeSpan startValue)
			{
				double startNum = TimeUnitUtility.ConvertToDouble(startValue, this.ParentDurationField.Unit);
				double dragSensitivity = NumericFieldDraggerUtility.CalculateIntDragSensitivity(startNum);
				float acceleration = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);

				long deltaNum = (long)Math.Round((double)NumericFieldDraggerUtility.NiceDelta(delta, acceleration) * dragSensitivity);
				TimeSpan deltaTimeSpan = TimeUnitUtility.ConvertFromInt(deltaNum, this.ParentDurationField.Unit);

				TimeSpan currentValue = this.StringToValue(base.text);
				TimeSpan newValue = currentValue + deltaTimeSpan;

				if (this.textEdition.isDelayed)
				{
					base.text = this.ValueToString(newValue);
					return;
				}

				this.ParentDurationField.value = newValue;
			}

			protected override string ValueToString(TimeSpan value)
			{
				return this.ParentDurationField.ValueToString(value);
			}

			protected override TimeSpan StringToValue(string str)
			{
				return this.ParentDurationField.StringToValue(str);
			}
		}
	}
}
#endif
