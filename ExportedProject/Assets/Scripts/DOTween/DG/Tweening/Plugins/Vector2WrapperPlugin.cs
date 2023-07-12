using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Core.Surrogates;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	public class Vector2WrapperPlugin : ABSTweenPlugin<Vector2Wrapper, Vector2Wrapper, VectorOptions>
	{
		public override void Reset(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t)
		{
		}

		public override void SetFrom(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t, bool isRelative)
		{
			Vector2 vector = t.endValue;
			t.endValue = t.getter().value;
			t.startValue.value = (isRelative ? (t.endValue.value + vector) : vector);
			Vector2 vector2 = t.endValue;
			switch (t.plugOptions.axisConstraint)
			{
			case AxisConstraint.X:
				vector2.x = t.startValue.value.x;
				break;
			case AxisConstraint.Y:
				vector2.y = t.startValue.value.y;
				break;
			default:
				vector2 = t.startValue;
				break;
			}
			if (t.plugOptions.snapping)
			{
				vector2.x = (float)Math.Round(vector2.x);
				vector2.y = (float)Math.Round(vector2.y);
			}
			t.setter(vector2);
		}

		public override Vector2Wrapper ConvertToStartValue(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t, Vector2Wrapper value)
		{
			return value.value;
		}

		public override void SetRelativeEndValue(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t)
		{
			t.endValue.value += t.startValue.value;
		}

		public override void SetChangeValue(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t)
		{
			switch (t.plugOptions.axisConstraint)
			{
			case AxisConstraint.X:
				t.changeValue.value = new Vector2(t.endValue.value.x - t.startValue.value.x, 0f);
				break;
			case AxisConstraint.Y:
				t.changeValue.value = new Vector2(0f, t.endValue.value.y - t.startValue.value.y);
				break;
			default:
				t.changeValue.value = t.endValue.value - t.startValue.value;
				break;
			}
		}

		public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector2Wrapper changeValue)
		{
			return changeValue.value.magnitude / unitsXSecond;
		}

		public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector2Wrapper> getter, DOSetter<Vector2Wrapper> setter, float elapsed, Vector2Wrapper startValue, Vector2Wrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue.value += changeValue.value * (t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue.value += changeValue.value * ((t.loopType != LoopType.Incremental) ? 1 : t.loops) * (t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			switch (options.axisConstraint)
			{
			case AxisConstraint.X:
			{
				Vector2 value = getter().value;
				value.x = startValue.value.x + changeValue.value.x * num;
				if (options.snapping)
				{
					value.x = (float)Math.Round(value.x);
				}
				setter(value);
				return;
			}
			case AxisConstraint.Y:
			{
				Vector2 value2 = getter().value;
				value2.y = startValue.value.y + changeValue.value.y * num;
				if (options.snapping)
				{
					value2.y = (float)Math.Round(value2.y);
				}
				setter(value2);
				return;
			}
			}
			startValue.value.x += changeValue.value.x * num;
			startValue.value.y += changeValue.value.y * num;
			if (options.snapping)
			{
				startValue.value.x = (float)Math.Round(startValue.value.x);
				startValue.value.y = (float)Math.Round(startValue.value.y);
			}
			setter(startValue.value);
		}
	}
}
