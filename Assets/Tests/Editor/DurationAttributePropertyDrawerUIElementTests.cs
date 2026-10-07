using System;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.TestFramework;

namespace Huchell.Unity.Editor.Tests
{
	[TestFixture]
	public sealed class DurationAttributePropertyDrawerUIElementTests : UITestFixture
	{
		private SerializedObject serializedObject;
		private DurationAttributePropertyDrawer propertyDrawer;

		[SetUp]
		public void SetUp()
		{
			var instance = ScriptableObject.CreateInstance<TestClass>();
			this.serializedObject = new SerializedObject(instance);

			this.propertyDrawer = new DurationAttributePropertyDrawer();
		}

		[TearDown]
		public void TearDown()
		{
			ScriptableObject.DestroyImmediate(this.serializedObject.targetObject);
			this.serializedObject = null;
			this.propertyDrawer = null;
		}

		private void SetupPropertyDrawerAttributeForField(Type type, string fieldName)
		{
			var drawerType = typeof(DurationAttributePropertyDrawer);

			var fieldInfo = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			var drawerFieldInfo = drawerType.GetField("m_FieldInfo", BindingFlags.NonPublic | BindingFlags.Instance);
			drawerFieldInfo.SetValue(propertyDrawer, fieldInfo);

			var attribute = fieldInfo.GetCustomAttribute<DurationAttribute>(inherit: true);
			drawerFieldInfo = drawerType.GetField("m_Attribute", BindingFlags.NonPublic | BindingFlags.Instance);
			drawerFieldInfo.SetValue(propertyDrawer, attribute);
		}

		private VisualElement CreatePropertyGUI(string propertyName)
		{
			this.SetupPropertyDrawerAttributeForField(typeof(TestClass), propertyName);
			var fieldProperty = this.serializedObject.FindProperty(propertyName);

			VisualElement element = this.propertyDrawer.CreatePropertyGUI(fieldProperty);
			this.rootVisualElement.Add(element);
			return element;
		}

		[Test]
		public void Creates_DisabledUI_WhenPropertyIsNotNumber()
		{
			var ui = this.CreatePropertyGUI(nameof(TestClass.invalidField));
			this.simulate.FrameUpdate();

			Assert.That(ui.enabledSelf, Is.False);
		}

		[Test]
		public void Creates_EnabledUI_WhenPropertyIsValid()
		{
			var ui = (BaseField<string>)this.CreatePropertyGUI(nameof(TestClass.secondsField));
			this.simulate.FrameUpdate();

			Assert.That(ui.enabledSelf, Is.True);
		}

		[Test]
		public void Creates_UI_WithDisplayName()
		{
			var ui = this.CreatePropertyGUI(nameof(TestClass.secondsField));
			this.simulate.FrameUpdate();

			var label = ui.Q<Label>();
			Assert.That(label, Is.Not.Null);

			var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsField));
			Assert.That(label.text, Is.EqualTo(fieldProperty.displayName));
		}

		[Test]
		public void IntField_IsSetCorrectly()
		{
			var ui = this.CreatePropertyGUI(nameof(TestClass.secondsField));
			this.simulate.FrameUpdate();

			ui.Focus();
			this.simulate.TypingText("2m");
			this.simulate.KeyPress(KeyCode.Return);
			this.simulate.FrameUpdate();

			var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsField));
			Assert.That(fieldProperty.intValue, Is.EqualTo(120));
		}

		[Test]
		public void LongField_IsSetCorrectly()
		{
			var ui = this.CreatePropertyGUI(nameof(TestClass.secondsFieldLong));
			this.simulate.FrameUpdate();

			ui.Focus();
			this.simulate.TypingText("2m");
			this.simulate.KeyPress(KeyCode.Return);
			this.simulate.FrameUpdate();

			var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsFieldLong));
			Assert.That(fieldProperty.longValue, Is.EqualTo(120L));
		}

		[Test]
		public void FloatField_IsSetCorrectly()
		{
			var ui = this.CreatePropertyGUI(nameof(TestClass.secondsFieldSingle));
			this.simulate.FrameUpdate();

			ui.Focus();
			this.simulate.TypingText("2m");
			this.simulate.KeyPress(KeyCode.Return);
			this.simulate.FrameUpdate();

			var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsFieldSingle));
			Assert.That(fieldProperty.floatValue, Is.EqualTo(120f));
		}

		[Test]
		public void DoubleField_IsSetCorrectly()
		{
			var ui = this.CreatePropertyGUI(nameof(TestClass.secondsFieldDouble));
			this.simulate.FrameUpdate();

			ui.Focus();
			this.simulate.TypingText("2m");
			this.simulate.KeyPress(KeyCode.Return);
			this.simulate.FrameUpdate();

			var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsFieldDouble));
			Assert.That(fieldProperty.floatValue, Is.EqualTo(120f));
		}

		[Test]
		public void DoesNotUpdateProperty_WhenValueIsInvalid()
		{
			var ui = this.CreatePropertyGUI(nameof(TestClass.secondsField));
			this.simulate.FrameUpdate();

			ui.Focus();
			this.simulate.TypingText("2f");
			this.simulate.KeyPress(KeyCode.Return);
			this.simulate.FrameUpdate();

			var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsField));
			Assert.That(fieldProperty.intValue, Is.EqualTo(0));
		}

		[Test]
		public void UpdatesProperty_UsingCorrectTimeUnit()
		{
			var ui = this.CreatePropertyGUI(nameof(TestClass.minuteField));
			this.simulate.FrameUpdate();

			ui.Focus();
			this.simulate.TypingText("2m");
			this.simulate.KeyPress(KeyCode.Return);
			this.simulate.FrameUpdate();

			var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.minuteField));
			Assert.That(fieldProperty.intValue, Is.EqualTo(2));
		}

		// [Test]
		// public void UpdatesProperty_ToZero_WhenFieldSetToEmptyString()
		// {
		// 	var ui = this.CreatePropertyGUI(nameof(TestClass.secondsField));
		// 	ui.Focus();
		// 	this.simulate.TypingText("2m");
		// 	this.simulate.FrameUpdate();
		//
		// 	var fieldProperty = this.serializedObject.FindProperty(nameof(TestClass.secondsField));
		// 	Assert.That(fieldProperty.intValue, Is.EqualTo(0));
		// }

		private class TestClass : ScriptableObject
		{
			[Duration(TimeUnit.Seconds)]
			public int secondsField;
			[Duration(TimeUnit.Seconds)]
			public long secondsFieldLong;
			[Duration(TimeUnit.Seconds)]
			public float secondsFieldSingle;
			[Duration(TimeUnit.Seconds)]
			public double secondsFieldDouble;

			[Duration(TimeUnit.Minutes)]
			public int minuteField;

			[Duration(TimeUnit.Seconds)]
			public Rect invalidField;
		}
	}
}
