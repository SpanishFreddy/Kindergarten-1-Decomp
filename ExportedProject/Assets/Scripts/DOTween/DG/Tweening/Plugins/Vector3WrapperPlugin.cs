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
	public class Vector3WrapperPlugin : ABSTweenPlugin<Vector3Wrapper, Vector3Wrapper, VectorOptions>
	{
		public override void Reset(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t)
		{
		}

		public override void SetFrom(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t, bool isRelative)
		{
			Vector3 vector = t.endValue;
			t.endValue = t.getter().value;
			t.startValue.value = (isRelative ? (t.endValue.value + vector) : vector);
			Vector3 vector2 = t.endValue;
			switch (t.plugOptions.axisConstraint)
			{
			case AxisConstraint.X:
				vector2.x = t.startValue.value.x;
				break;
			case AxisConstraint.Y:
				vector2.y = t.startValue.value.y;
				break;
			case AxisConstraint.Z:
				vector2.z = t.startValue.value.z;
				break;
			default:
				vector2 = t.startValue;
				break;
			}
			if (t.plugOptions.snapping)
			{
				vector2.x = (float)Math.Round(vector2.x);
				vector2.y = (float)Math.Round(vector2.y);
				vector2.z = (float)Math.Round(vector2.z);
			}
			t.setter(vector2);
		}

		public override Vector3Wrapper ConvertToStartValue(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t, Vector3Wrapper value)
		{
			return value.value;
		}

		public override void SetRelativeEndValue(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t)
		{
			t.endValue.value += t.startValue.value;
		}

		public override void SetChangeValue(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t)
		{
			switch (t.plugOptions.axisConstraint)
			{
			case AxisConstraint.X:
				t.changeValue.value = new Vector3(t.endValue.value.x - t.startValue.value.x, 0f, 0f);
				break;
			case AxisConstraint.Y:
				t.changeValue.value = new Vector3(0f, t.endValue.value.y - t.startValue.value.y, 0f);
				break;
			case AxisConstraint.Z:
				t.changeValue.value = new Vector3(0f, 0f, t.endValue.value.z - t.startValue.value.z);
				break;
			default:
				t.changeValue.value = t.endValue.value - t.startValue.value;
				break;
			}
		}

		public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector3Wrapper changeValue)
		{
			return changeValue.value.magnitude / unitsXSecond;
		}

		public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector3Wrapper> getter, DOSetter<Vector3Wrapper> setter, float elapsed, Vector3Wrapper startValue, Vector3Wrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
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
				Vector3 value2 = getter().value;
				value2.x = startValue.value.x + changeValue.value.x * num;
				if (options.snapping)
				{
					value2.x = (float)Math.Round(value2.x);
				}
				setter(value2);
				return;
			}
			case AxisConstraint.Y:
			{
				Vector3 value = getter().value;
				value.y = startValue.value.y + changeValue.value.y * num;
				if (options.snapping)
				{
					value.y = (float)Math.Round(value.y);
				}
				setter(value);
				return;
			}
			case AxisConstraint.Z:
			{
				Vector3 value3 = getter().value;
				value3.z = startValue.value.z + changeValue.value.z * num;
				if (options.snapping)
				{
					value3.z = (float)Math.Round(value3.z);
				}
				setter(value3);
				return;
			}
			}
			startValue.value.x += changeValue.value.x * num;
			startValue.value.y += changeValue.value.y * num;
			startValue.value.z += changeValue.value.z * num;
			if (options.snapping)
			{
				startValue.value.x = (float)Math.Round(startValue.value.x);
				startValue.value.y = (float)Math.Round(startValue.value.y);
				startValue.value.z = (float)Math.Round(startValue.value.z);
			}
			setter(startValue.value);
		}
	}
}
