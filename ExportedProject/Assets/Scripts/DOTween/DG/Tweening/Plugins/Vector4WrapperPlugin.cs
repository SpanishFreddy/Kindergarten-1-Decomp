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
	public class Vector4WrapperPlugin : ABSTweenPlugin<Vector4Wrapper, Vector4Wrapper, VectorOptions>
	{
		public override void Reset(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t)
		{
		}

		public override void SetFrom(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t, bool isRelative)
		{
			Vector4 vector = t.endValue;
			t.endValue = t.getter().value;
			t.startValue = (isRelative ? (t.endValue + vector) : vector);
			Vector4 vector2 = t.endValue;
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
			case AxisConstraint.W:
				vector2.w = t.startValue.value.w;
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
				vector2.w = (float)Math.Round(vector2.w);
			}
			t.setter(vector2);
		}

		public override Vector4Wrapper ConvertToStartValue(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t, Vector4Wrapper value)
		{
			return value.value;
		}

		public override void SetRelativeEndValue(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t)
		{
			t.endValue.value += t.startValue.value;
		}

		public override void SetChangeValue(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t)
		{
			switch (t.plugOptions.axisConstraint)
			{
			case AxisConstraint.X:
				t.changeValue.value = new Vector4(t.endValue.value.x - t.startValue.value.x, 0f, 0f, 0f);
				break;
			case AxisConstraint.Y:
				t.changeValue.value = new Vector4(0f, t.endValue.value.y - t.startValue.value.y, 0f, 0f);
				break;
			case AxisConstraint.Z:
				t.changeValue.value = new Vector4(0f, 0f, t.endValue.value.z - t.startValue.value.z, 0f);
				break;
			case AxisConstraint.W:
				t.changeValue.value = new Vector4(0f, 0f, 0f, t.endValue.value.w - t.startValue.value.w);
				break;
			default:
				t.changeValue.value = t.endValue.value - t.startValue.value;
				break;
			}
		}

		public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector4Wrapper changeValue)
		{
			return changeValue.value.magnitude / unitsXSecond;
		}

		public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector4Wrapper> getter, DOSetter<Vector4Wrapper> setter, float elapsed, Vector4Wrapper startValue, Vector4Wrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
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
				Vector4 value2 = getter().value;
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
				Vector4 value4 = getter().value;
				value4.y = startValue.value.y + changeValue.value.y * num;
				if (options.snapping)
				{
					value4.y = (float)Math.Round(value4.y);
				}
				setter(value4);
				return;
			}
			case AxisConstraint.Z:
			{
				Vector4 value = getter().value;
				value.z = startValue.value.z + changeValue.value.z * num;
				if (options.snapping)
				{
					value.z = (float)Math.Round(value.z);
				}
				setter(value);
				return;
			}
			case AxisConstraint.W:
			{
				Vector4 value3 = getter().value;
				value3.w = startValue.value.w + changeValue.value.w * num;
				if (options.snapping)
				{
					value3.w = (float)Math.Round(value3.w);
				}
				setter(value3);
				return;
			}
			}
			startValue.value.x += changeValue.value.x * num;
			startValue.value.y += changeValue.value.y * num;
			startValue.value.z += changeValue.value.z * num;
			startValue.value.w += changeValue.value.w * num;
			if (options.snapping)
			{
				startValue.value.x = (float)Math.Round(startValue.value.x);
				startValue.value.y = (float)Math.Round(startValue.value.y);
				startValue.value.z = (float)Math.Round(startValue.value.z);
				startValue.value.w = (float)Math.Round(startValue.value.w);
			}
			setter(startValue.value);
		}
	}
}
