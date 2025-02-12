using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Huchell.Unity.Editor
{
	public sealed partial class DurationAttributePropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			if (!HasValidProperty(property))
			{
				return this.CreateInvalidPropertyGUI(property);
			}

			var textField = new TextField(property.displayName);
			textField.AddToClassList("unity-base-field__aligned");
			textField.isDelayed = true;
			textField.RegisterValueChangedCallback((@event) => this.TextField_ValueChanged(@event, property));

			var timeStr = GetTimeString(property, this.Attribute.BaseUnit);
			textField.SetValueWithoutNotify(timeStr);

			return textField;
		}

		private VisualElement CreateInvalidPropertyGUI(SerializedProperty property)
		{
			var root = new TextField();
			root.AddToClassList("unity-base-field__aligned");
			root.label = property.displayName;
			root.isReadOnly = true;
			root.value = Content.InvalidTypeError.text;
			root.focusable = false;
			return root;
		}

		private void TextField_ValueChanged(ChangeEvent<string> @event, SerializedProperty property)
		{
			TextField field = (TextField)@event.target;
			try
			{
				string normalizedTimeStr = TimeStringConverter.Normalize(@event.newValue);
				if (string.IsNullOrEmpty(normalizedTimeStr))
				{
					field.SetValueWithoutNotify(@event.previousValue);
					@event.StopImmediatePropagation();
					return;
				}

				SetTimeString(property, normalizedTimeStr, this.Attribute.BaseUnit);
				property.serializedObject.ApplyModifiedProperties();
				field.value = normalizedTimeStr;
			}
			catch (FormatException)
			{
				field.value = @event.previousValue;
				@event.StopImmediatePropagation();
			}
		}
	}
}
