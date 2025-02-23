# com.huchell.unity.custom-duration-attribute

A custom property drawer for displaying a duration in the Unity3d editor

## Usage

The `TimeUnit`[time_unit] passed into the `DurationAttribute`[duration_attribute] tells the property drawer which time unti the field should represent. In the example below the value in the `duration` field will be converted to seconds.

```cs
using Huchell.Unity;
using UnityEngine;

public class ExampleClass1 : ScriptableObject
{
    [SerializedField, Duration(TimeUnit.Seconds)]
    private int duration = 500;
}
```

[time_unit relative link]: ./Runtime/TimeUnit.cs
[duration_attribute relative link]: ./Runtime/DurationAttribute.cs
