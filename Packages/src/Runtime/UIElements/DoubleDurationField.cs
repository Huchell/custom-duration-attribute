using System;
using Duration;
using UnityEngine;
using UnityEngine.UIElements;

namespace Huchell.Unity
{
	public sealed class DoubleDurationField : TextValueField<double>
	{
		private TimeUnit baseUnit;

		public DoubleDurationField()
			: base(int.MaxValue, new DoubleDurationFieldInput())
		{
		}

		public DoubleDurationField(string label)
			: base(label, int.MaxValue, new DoubleDurationFieldInput())
		{
			this.AddLabelDragger<double>();
		}

		private DoubleDurationFieldInput DurationInput => (DoubleDurationFieldInput)this.textInputBase;

		public TimeUnit BaseUnit
		{
			get => this.baseUnit;
			set
			{
				var newValue = TimeUnitUtility.ConvertFromDouble(this.value, this.baseUnit);
				this.baseUnit = value;
				this.value = TimeUnitUtility.ConvertToInt(newValue, this.baseUnit);
			}
		}

		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, double startValue)
		{
			this.DurationInput.ApplyInputDeviceDelta(delta, speed, startValue);
		}

		protected override double StringToValue(string str)
		{
			var timeSpan = TimeString.Parse(str);
			return TimeUnitUtility.ConvertToDouble(timeSpan, this.BaseUnit);
		}

		protected override string ValueToString(double value)
		{
			var timeSpan = TimeUnitUtility.ConvertFromDouble(value, this.BaseUnit);
			return TimeString.FromTimeSpan(timeSpan).ToString();
		}

		private sealed class DoubleDurationFieldInput : TextValueInput
		{
			private DoubleDurationField ParentDurationField => (DoubleDurationField)this.parent;

			protected override string allowedCharacters => "0123456789 dhms";

			public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, double startValue)
			{
				var dragSensitivity = NumericFieldDraggerUtility.CalculateIntDragSensitivity(startValue);
				var acceleration = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);

				var deltaNum = (double)NumericFieldDraggerUtility.NiceDelta(delta, acceleration) * dragSensitivity;

				var currentValue = this.StringToValue(this.text);
				var newValue = currentValue + deltaNum;

				if (this.textEdition.isDelayed)
				{
					this.text = this.ValueToString(newValue);
					return;
				}

				this.ParentDurationField.value = newValue;
			}

			protected override string ValueToString(double value)
			{
				return this.ParentDurationField.ValueToString(value);
			}

			protected override double StringToValue(string str)
			{
				return this.ParentDurationField.StringToValue(str);
			}
		}
	}
}
