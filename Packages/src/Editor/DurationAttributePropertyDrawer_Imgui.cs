using UnityEditor;
using UnityEngine;

namespace Huchell.Unity.Editor
{
	public sealed partial class DurationAttributePropertyDrawer
	{
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
				var normalizedTime = TimeStringConverter.Normalize(timeStr);
				if (string.IsNullOrEmpty(normalizedTime))
				{
					return;
				}

				SetTimeString(property, normalizedTime, this.Attribute.BaseUnit);
			}
		}

	}
}
