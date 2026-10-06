using System;
using UnityEditor;
using UnityEngine;

namespace Huchell.Unity.Editor
{
	[CustomPropertyDrawer(typeof(DurationAttribute))]
	public sealed partial class DurationAttributePropertyDrawer : PropertyDrawer
	{
		public static class Content
		{
			public static readonly GUIContent InvalidTypeError = new GUIContent("Invalid Type for DurationAttribute");
		}

		public DurationAttribute Attribute => (DurationAttribute)this.attribute;

		private static bool HasValidProperty(SerializedProperty property)
		{
			return property.propertyType switch
			{
				SerializedPropertyType.Integer => true,
				SerializedPropertyType.Float => true,
				_ => false,
			};
		}

		private static string GetTimeString(SerializedProperty property, TimeUnit baseUnit)
		{
			return property.numericType switch
			{
				SerializedPropertyNumericType.Float => TimeStringConverter.FromSingle(property.floatValue, baseUnit),
				SerializedPropertyNumericType.Double => TimeStringConverter.FromDouble(property.doubleValue, baseUnit),
				SerializedPropertyNumericType.Int8 => TimeStringConverter.FromInt32(property.intValue, baseUnit),
				SerializedPropertyNumericType.Int16 => TimeStringConverter.FromInt32(property.intValue, baseUnit),
				SerializedPropertyNumericType.Int32 => TimeStringConverter.FromInt32(property.intValue, baseUnit),
				SerializedPropertyNumericType.Int64 => TimeStringConverter.FromInt64(property.longValue, baseUnit),
				SerializedPropertyNumericType.UInt8 => TimeStringConverter.FromUInt32(property.uintValue, baseUnit),
				SerializedPropertyNumericType.UInt16 => TimeStringConverter.FromUInt32(property.uintValue, baseUnit),
				SerializedPropertyNumericType.UInt32 => TimeStringConverter.FromUInt32(property.uintValue, baseUnit),
				SerializedPropertyNumericType.UInt64 => TimeStringConverter.FromUInt64(property.ulongValue, baseUnit),
				_ => throw new Exception(),
			};
		}

		private static void SetTimeString(SerializedProperty property, string timeStr, TimeUnit baseUnit)
		{
			switch (property.numericType)
			{
				case SerializedPropertyNumericType.Double:
					property.doubleValue = TimeStringConverter.ToDouble(timeStr, baseUnit);
					break;
				case SerializedPropertyNumericType.Float:
					property.floatValue = TimeStringConverter.ToSingle(timeStr, baseUnit);
					break;
				case SerializedPropertyNumericType.Int8:
				case SerializedPropertyNumericType.Int16:
				case SerializedPropertyNumericType.Int32:
					property.intValue = TimeStringConverter.ToInt32(timeStr, baseUnit);
					break;
				case SerializedPropertyNumericType.Int64:
					property.longValue = TimeStringConverter.ToInt64(timeStr, baseUnit);
					break;
				case SerializedPropertyNumericType.UInt8:
				case SerializedPropertyNumericType.UInt16:
				case SerializedPropertyNumericType.UInt32:
					property.uintValue = TimeStringConverter.ToUInt32(timeStr, baseUnit);
					break;
				case SerializedPropertyNumericType.UInt64:
					property.ulongValue = TimeStringConverter.ToUInt64(timeStr, baseUnit);
					break;
			}
		}
	}
}
