using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Huchell.Unity.Editor
{
	public sealed partial class DurationAttributePropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			return property.propertyType switch
			{
				SerializedPropertyType.Integer => this.CreateIntegerDurationField(property),
				SerializedPropertyType.Float => this.CreateDoubleDurationField(property),
				_ => this.CreateInvalidPropertyGUI(property),
			};
		}

		private IntegerDurationField CreateIntegerDurationField(SerializedProperty property)
		{
			var field = new IntegerDurationField(property.displayName);
			field.BaseUnit = this.Attribute.BaseUnit;
			field.isDelayed = true;
			field.AddToClassList("unity-base-field__aligned");
			field.BindProperty(property);
			return field;
		}

		private DoubleDurationField CreateDoubleDurationField(SerializedProperty property)
		{
			var field = new DoubleDurationField(property.displayName);
			field.BaseUnit = this.Attribute.BaseUnit;
			field.isDelayed = true;
			field.AddToClassList("unity-base-field__aligned");
			field.BindProperty(property);
			return field;
		}

		private VisualElement CreateInvalidPropertyGUI(SerializedProperty property)
		{
			var root = new TextField();
			root.AddToClassList("unity-base-field__aligned");
			root.label = property.displayName;
			root.isReadOnly = true;
			root.SetEnabled(false);
			root.value = Content.InvalidTypeError.text;
			root.focusable = false;
			return root;
		}
	}
}
