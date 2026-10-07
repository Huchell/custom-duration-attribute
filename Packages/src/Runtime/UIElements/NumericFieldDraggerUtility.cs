using System;
using UnityEngine;

namespace Huchell.Unity
{
	/// <remarks>
	/// Class and methods taken from https://github.com/Unity-Technologies/UnityCsReference/blob/6000.3/Runtime/Export/NumericFieldDraggerUtility.cs
	/// </remarks>
	internal static class NumericFieldDraggerUtility
	{
		private static bool s_UseYSign = false;
		const float kDragSensitivity = .03f;

		public static float Acceleration(bool shiftPressed, bool altPressed)
		{
			return (shiftPressed ? 4 : 1) * (altPressed ? .25f : 1);
		}

		public static float NiceDelta(Vector2 deviceDelta, float acceleration)
		{
			deviceDelta.y = -deviceDelta.y;

			if (Mathf.Abs(Mathf.Abs(deviceDelta.x) - Mathf.Abs(deviceDelta.y)) / Mathf.Max(Mathf.Abs(deviceDelta.x), Mathf.Abs(deviceDelta.y)) > .1f)
			{
				if (Mathf.Abs(deviceDelta.x) > Mathf.Abs(deviceDelta.y))
					s_UseYSign = false;
				else
					s_UseYSign = true;
			}

			if (s_UseYSign)
				return Mathf.Sign(deviceDelta.y) * deviceDelta.magnitude * acceleration;
			else
				return Mathf.Sign(deviceDelta.x) * deviceDelta.magnitude * acceleration;
		}

		public static double CalculateIntDragSensitivity(double value)
		{
			return Math.Max(1, Math.Pow(Math.Abs(value), 0.5) * kDragSensitivity);
		}
	}
}
