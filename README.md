# com.huchell.unity.custom-duration-attribute

A custom property drawer for displaying a duration in the Unity3d editor

## Usage

The [`TimeUnit`](Runtime/TimeUnit.cs) passed into the [`DurationAttribute`](Runtime/DurationAttribute.cs) tells the property drawer which time unti the field should represent. In the example below the value in the `durationInSeconds` field will be converted to seconds.

```cs
using Huchell.Unity;
using UnityEngine;

public class ExampleClass1 : ScriptableObject
{
    [SerializedField, Duration(TimeUnit.Second)]
    private int durationInSeconds = 500;
    [SerializedField, Duration(TimeUnit.Minute)]
    private int durationInMinutes = 500;
}
```
