using UnityEngine;
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

			var durationField = new DurationElement(property.displayName);
			durationField.AddToClassList("unity-base-field__aligned");
			durationField.BindProperty(property);
			durationField.isDelayed = true;
			return durationField;
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
