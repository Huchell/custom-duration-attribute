# com.huchell.unity.custom-duration-attribute

A custom property drawer for displaying a duration in the Unity3d editor

## Usage

The `TimeUnit`[1] passed into the `DurationAttribute`[2] tells the property drawer which time unti the field should represent. In the example below the value in the `durationInSeconds` field will be converted to seconds.

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

[1 relative link]: ./Runtime/TimeUnit.cs
[2 relative link]: ./Runtime/DurationAttribute.cs
