using UnityEditor;
using UnityEngine;

namespace Huchell.Unity.Editor
{
	public sealed partial class DurationAttributePropertyDrawer
	{
		public static class Content
		{
			public static readonly GUIContent InvalidTypeError = new GUIContent("Invalid Type for DurationAttribute");
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!HasValidProperty(property))
			{
				EditorGUI.LabelField(position, label, Content.InvalidTypeError);
				return;
			}

			var timeStr = GetTimeString(property, this.Attribute.BaseUnit);
			timeStr = EditorGUI.DelayedTextField(position, label, timeStr);
			if (GUI.changed)
			{
				SetTimeString(property, timeStr, this.Attribute.BaseUnit);
			}
		}

	}
}
