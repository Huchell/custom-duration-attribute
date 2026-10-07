using System;
using Huchell.Unity;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements.TestFramework;

namespace Huchell.Duration
{
	public sealed class DurationElementLabelDraggingTests : UITestFixture
	{
		private DurationElement field;

		[SetUp]
		public void SetUp()
		{
			this.panelSize = new(500, 100);

			this.field = new("Label");
			this.rootVisualElement.Add(field);
			this.simulate.FrameUpdate();
		}

		[Test]
		public void PositiveXDrag_IncreasesValue()
		{
			var startDragPosition = this.field.labelElement.worldBound.center;
			var endDragPosition = startDragPosition + new Vector2(50, 0);

			this.simulate.DragAndDrop(startDragPosition, endDragPosition);
			this.simulate.FrameUpdate();

			Assert.That(this.field.value, Is.GreaterThan(TimeSpan.Zero));
		}

		[Test]
		public void NegativeXDrag_DecreasesValue()
		{
			var startDragPosition = this.field.labelElement.worldBound.center;
			var endDragPosition = startDragPosition - new Vector2(50, 0);

			this.simulate.DragAndDrop(startDragPosition, endDragPosition);
			this.simulate.FrameUpdate();

			Assert.That(this.field.value, Is.LessThan(TimeSpan.Zero));
		}

		[Test]
		public void PositiveYDrag_DecreasesValue()
		{
			var startDragPosition = this.field.labelElement.worldBound.center;
			var endDragPosition = startDragPosition + new Vector2(0, 50);

			this.simulate.DragAndDrop(startDragPosition, endDragPosition);
			this.simulate.FrameUpdate();

			Assert.That(this.field.value, Is.LessThan(TimeSpan.Zero));
		}

		[Test]
		public void NegativeYDrag_IncreasesValue()
		{
			var startDragPosition = this.field.labelElement.worldBound.center;
			var endDragPosition = startDragPosition - new Vector2(0, 50);

			this.simulate.DragAndDrop(startDragPosition, endDragPosition);
			this.simulate.FrameUpdate();

			Assert.That(this.field.value, Is.GreaterThan(TimeSpan.Zero));
		}

		[Test]
		public void HoldingShiftWhileDragging_GivesBiggerChangesInValue()
		{
			var startDragPosition = this.field.labelElement.worldBound.center;
			var endDragPosition = startDragPosition - new Vector2(0, 50);

			this.simulate.DragAndDrop(startDragPosition, endDragPosition);
			this.simulate.FrameUpdate();

			var nonShiftValue = this.field.value;

			this.field.ClearValue();
			this.simulate.DragAndDrop(startDragPosition, endDragPosition, modifiers: EventModifiers.Shift);

			Assert.That(this.field.value, Is.GreaterThan(nonShiftValue));
		}

		[Test]
		public void HoldingAltWhileDragging_GivesSmallerChangesInValue()
		{
			var startDragPosition = this.field.labelElement.worldBound.center;
			var endDragPosition = startDragPosition - new Vector2(0, 50);

			this.simulate.DragAndDrop(startDragPosition, endDragPosition);
			this.simulate.FrameUpdate();

			var nonAltValue = this.field.value;

			this.field.ClearValue();
			this.simulate.DragAndDrop(startDragPosition, endDragPosition, modifiers: EventModifiers.Alt);

			Assert.That(this.field.value, Is.LessThan(nonAltValue));
		}
	}
}
