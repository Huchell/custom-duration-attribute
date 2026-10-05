using System;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Huchell.Unity.Editor.Tests
{
	// [TestFixture]
	// public sealed class DurationAttributePropertyDrawerUIElementTests
	// {
	// 	private EditorWindow window;
	// 	private VisualElement root => window.rootVisualElement;
	//
	// 	private SerializedObject serializedObject;
	// 	private DurationAttributePropertyDrawer propertyDrawer;
	//
	// 	[OneTimeSetUp]
	// 	public void OneTimeSetUp()
	// 	{
	// 		this.window = EditorWindow.CreateWindow<EditorWindow>("UI Test window - please close");
	// 		this.window.position = Rect.zero;
	// 	}
	//
	// 	[SetUp]
	// 	public void SetUp()
	// 	{
	// 		var instance = ScriptableObject.CreateInstance<TestClass>();
	// 		this.serializedObject = new SerializedObject(instance);
	//
	// 		this.propertyDrawer = new DurationAttributePropertyDrawer();
	// 	}
	//
	// 	[TearDown]
	// 	public void TearDown()
	// 	{
	// 		this.root.Clear();
	//
	// 		ScriptableObject.DestroyImmediate(this.serializedObject.targetObject);
	// 		this.serializedObject = null;
	// 		this.propertyDrawer = null;
	// 	}
	//
	// 	[OneTimeTearDown]
	// 	public void OneTimeTearDown()
	// 	{
	// 		this.window.Close();
	// 		this.window = null;
	// 	}
	//
	// 	private void SetupPropertyDrawerAttributeForField(Type type, string fieldName)
	// 	{
	// 		var drawerType = typeof(DurationAttributePropertyDrawer);
	//
	// 		var fieldInfo = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
	// 		var drawerFieldInfo = drawerType.GetField("m_FieldInfo", BindingFlags.NonPublic | BindingFlags.Instance);
	// 		drawerFieldInfo.SetValue(propertyDrawer, fieldInfo);
	//
	// 		var attribute = fieldInfo.GetCustomAttribute<DurationAttribute>(inherit: true);
	// 		drawerFieldInfo = drawerType.GetField("m_Attribute", BindingFlags.NonPublic | BindingFlags.Instance);
	// 		drawerFieldInfo.SetValue(propertyDrawer, attribute);
	// 	}
	//
	// 	private VisualElement CreatePropertyGUI(string propertyName)
	// 	{
	// 		this.SetupPropertyDrawerAttributeForField(typeof(TestClass), propertyName);
	// 		var fieldProperty = this.serializedObject.FindProperty(propertyName);
	//
	// 		VisualElement element = this.propertyDrawer.CreatePropertyGUI(fieldProperty);
	// 		this.window.rootVisualElement.Add(element);
	// 		return element;
	// 	}
	//
	// 	[Test]
	// 	public void Creates_DisabledUI_WhenPropertyIsNotNumber()
	// 	{
	// 		var ui = this.CreatePropertyGUI(nameof(TestClass.invalidField));
	// 		Assert.That(ui.enabledSelf, Is.False);
	// 	}
	//
	// 	[Test]
	// 	public void Creates_EnabledUI_WhenPropertyIsValid()
	// 	{
	// 		var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.secondsField));
	// 		Assert.That(ui.enabledSelf, Is.True);
	// 	}
	//
	// 	[Test]
	// 	public void Creates_UI_WithDisplayName()
	// 	{
	// 		var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.secondsField));
	//
	// 		var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsField));
	// 		Assert.That(ui.label, Is.EqualTo(fieldProperty.displayName));
	// 	}
	//
	// 	[Test]
	// 	public void IntField_IsSetCorrectly()
	// 	{
	// 		var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.secondsField));
	// 		ui.value = "2m";
	//
	// 		var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsField));
	// 		Assert.That(fieldProperty.intValue, Is.EqualTo(120));
	// 	}
	//
	// 	[Test]
	// 	public void LongField_IsSetCorrectly()
	// 	{
	// 		var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.secondsFieldLong));
	// 		ui.value = "2m";
	//
	// 		var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsFieldLong));
	// 		Assert.That(fieldProperty.longValue, Is.EqualTo(120L));
	// 	}
	//
	// 	[Test]
	// 	public void FloatField_IsSetCorrectly()
	// 	{
	// 		var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.secondsFieldSingle));
	// 		ui.value = "2m";
	//
	// 		var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsFieldSingle));
	// 		Assert.That(fieldProperty.floatValue, Is.EqualTo(120f));
	// 	}
	//
	// 	[Test]
	// 	public void DoubleField_IsSetCorrectly()
	// 	{
	// 		var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.secondsFieldDouble));
	// 		ui.value = "2m";
	//
	// 		var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsFieldDouble));
	// 		Assert.That(fieldProperty.floatValue, Is.EqualTo(120f));
	// 	}
	//
	// 	[Test]
	// 	public void DoesNotUpdateProperty_WhenValueIsInvalid()
	// 	{
	// 		var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.secondsField));
	// 		ui.value = "2f";
	//
	// 		var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsField));
	// 		Assert.That(fieldProperty.intValue, Is.EqualTo(0));
	// 	}
	//
	// 	[Test]
	// 	public void UpdatesProperty_UsingCorrectTimeUnit()
	// 	{
	// 		var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.minuteField));
	// 		ui.value = "2m";
	//
	// 		var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.minuteField));
	// 		Assert.That(fieldProperty.intValue, Is.EqualTo(2));
	// 	}
	//
	// 	[Test]
	// 	public void UpdatesProperty_ToZero_WhenFieldSetToEmptyString()
	// 	{
	// 		var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.secondsField));
	// 		ui.value = "";
	//
	// 		var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsField));
	// 		Assert.That(fieldProperty.intValue, Is.EqualTo(0));
	// 	}
	//
	//
	// 	private class TestClass : ScriptableObject
	// 	{
	// 		[Duration(TimeUnit.Seconds)]
	// 		public int secondsField;
	// 		[Duration(TimeUnit.Seconds)]
	// 		public long secondsFieldLong;
	// 		[Duration(TimeUnit.Seconds)]
	// 		public float secondsFieldSingle;
	// 		[Duration(TimeUnit.Seconds)]
	// 		public double secondsFieldDouble;
	//
	// 		[Duration(TimeUnit.Minutes)]
	// 		public int minuteField;
	//
	// 		[Duration(TimeUnit.Seconds)]
	// 		public Rect invalidField;
	// 	}
	// }
}
