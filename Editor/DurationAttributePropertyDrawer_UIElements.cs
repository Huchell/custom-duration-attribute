using UnityEditor;
using UnityEditor.UIElements;
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
			textField.RegisterValueChangedCallback(this.TextField_ValueChanged);
			textField.TrackPropertyValue(property, (property) => this.TextField_PropertyValueUpdated(textField, property));
			textField.userData = property;

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

		private void TextField_PropertyValueUpdated(TextField textField, SerializedProperty property)
		{
			var time = GetTimeString(property, this.Attribute.BaseUnit);
			textField.value = time;
		}

		private void TextField_ValueChanged(ChangeEvent<string> @event)
		{
			var field = (TextField)@event.target;
			var property = (SerializedProperty)field.userData;

			var normalizedTime = TimeStringConverter.Normalize(@event.newValue);
			if (string.IsNullOrEmpty(normalizedTime))
			{
				field.SetValueWithoutNotify(@event.previousValue);
				@event.StopImmediatePropagation();
				return;
			}

			if (normalizedTime != @event.newValue)
			{
				field.value = normalizedTime;
				return;
			}

			Undo.RecordObjects(property.serializedObject.targetObjects, "DurationValueUpdate");
			SetTimeString(property, normalizedTime, this.Attribute.BaseUnit);
			property.serializedObject.ApplyModifiedProperties();
		}
	}
}
