using System;
using Duration;
using UnityEngine;
using UnityEngine.UIElements;

namespace Huchell.Unity
{
	public sealed class IntegerDurationField : TextValueField<long>
	{
		private TimeUnit baseUnit;

		public IntegerDurationField()
			: base(int.MaxValue, new IntegerDurationFieldInput())
		{
		}

		public IntegerDurationField(string label)
			: base(label, int.MaxValue, new IntegerDurationFieldInput())
		{
			this.AddLabelDragger<long>();
		}

		private IntegerDurationFieldInput DurationInput => (IntegerDurationFieldInput)this.textInputBase;

		public TimeUnit BaseUnit
		{
			get => this.baseUnit;
			set
			{
				var newValue = TimeUnitUtility.ConvertFromInt(this.value, this.baseUnit);
				this.baseUnit = value;
				this.value = TimeUnitUtility.ConvertToInt(newValue, this.baseUnit);
			}
		}

		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, long startValue)
		{
			this.DurationInput.ApplyInputDeviceDelta(delta, speed, startValue);
		}

		protected override long StringToValue(string str)
		{
			var timeSpan = TimeString.Parse(str);
			return TimeUnitUtility.ConvertToInt(timeSpan, this.BaseUnit);
		}

		protected override string ValueToString(long value)
		{
			var timeSpan = TimeUnitUtility.ConvertFromInt(value, this.BaseUnit);
			return TimeString.FromTimeSpan(timeSpan).ToString();
		}

		private sealed class IntegerDurationFieldInput : TextValueInput
		{
			private IntegerDurationField ParentDurationField => (IntegerDurationField)this.parent;

			protected override string allowedCharacters => "0123456789 dhms";

			public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, long startValue)
			{
				double dragSensitivity = NumericFieldDraggerUtility.CalculateIntDragSensitivity(startValue);
				float acceleration = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);

				long deltaNum = (long)Math.Round((double)NumericFieldDraggerUtility.NiceDelta(delta, acceleration) * dragSensitivity);

				long currentValue = this.StringToValue(this.text);
				long newValue = currentValue + deltaNum;

				if (this.textEdition.isDelayed)
				{
					this.text = this.ValueToString(newValue);
					return;
				}

				this.ParentDurationField.value = newValue;
			}

			protected override string ValueToString(long value)
			{
				return this.ParentDurationField.ValueToString(value);
			}

			protected override long StringToValue(string str)
			{
				return this.ParentDurationField.StringToValue(str);
			}
		}
	}
}
