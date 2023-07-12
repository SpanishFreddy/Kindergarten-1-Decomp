using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Core.Surrogates;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	public class QuaternionWrapperPlugin : ABSTweenPlugin<QuaternionWrapper, Vector3Wrapper, QuaternionOptions>
	{
		public override void Reset(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t)
		{
		}

		public override void SetFrom(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t, bool isRelative)
		{
			Vector3 value = t.endValue.value;
			t.endValue.value = t.getter().value.eulerAngles;
			if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative)
			{
				t.startValue.value = value;
			}
			else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360)
			{
				t.startValue.value = t.endValue.value + value;
			}
			else
			{
				Quaternion value2 = t.getter().value;
				if (t.plugOptions.rotateMode == RotateMode.WorldAxisAdd)
				{
					t.startValue.value = (value2 * Quaternion.Inverse(value2) * Quaternion.Euler(value) * value2).eulerAngles;
				}
				else
				{
					t.startValue.value = (value2 * Quaternion.Euler(value)).eulerAngles;
				}
				t.endValue.value = -value;
			}
			t.setter(Quaternion.Euler(t.startValue.value));
		}

		public override Vector3Wrapper ConvertToStartValue(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t, QuaternionWrapper value)
		{
			return value.value.eulerAngles;
		}

		public override void SetRelativeEndValue(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t)
		{
			t.endValue.value += t.startValue.value;
		}

		public override void SetChangeValue(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t)
		{
			if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative)
			{
				Vector3 value = t.endValue.value;
				if (value.x > 360f)
				{
					value.x %= 360f;
				}
				if (value.y > 360f)
				{
					value.y %= 360f;
				}
				if (value.z > 360f)
				{
					value.z %= 360f;
				}
				Vector3 value2 = value - t.startValue.value;
				float num = ((value2.x > 0f) ? value2.x : (0f - value2.x));
				if (num > 180f)
				{
					value2.x = ((value2.x > 0f) ? (0f - (360f - num)) : (360f - num));
				}
				num = ((value2.y > 0f) ? value2.y : (0f - value2.y));
				if (num > 180f)
				{
					value2.y = ((value2.y > 0f) ? (0f - (360f - num)) : (360f - num));
				}
				num = ((value2.z > 0f) ? value2.z : (0f - value2.z));
				if (num > 180f)
				{
					value2.z = ((value2.z > 0f) ? (0f - (360f - num)) : (360f - num));
				}
				t.changeValue.value = value2;
			}
			else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360 || t.isRelative)
			{
				t.changeValue.value = t.endValue.value - t.startValue.value;
			}
			else
			{
				t.changeValue.value = t.endValue.value;
			}
		}

		public override float GetSpeedBasedDuration(QuaternionOptions options, float unitsXSecond, Vector3Wrapper changeValue)
		{
			return changeValue.value.magnitude / unitsXSecond;
		}

		public override void EvaluateAndApply(QuaternionOptions options, Tween t, bool isRelative, DOGetter<QuaternionWrapper> getter, DOSetter<QuaternionWrapper> setter, float elapsed, Vector3Wrapper startValue, Vector3Wrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			Vector3 value = startValue.value;
			if (t.loopType == LoopType.Incremental)
			{
				value += changeValue.value * (t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				value += changeValue.value * ((t.loopType != LoopType.Incremental) ? 1 : t.loops) * (t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			RotateMode rotateMode = options.rotateMode;
			if (rotateMode == RotateMode.WorldAxisAdd || rotateMode == RotateMode.LocalAxisAdd)
			{
				Quaternion quaternion = Quaternion.Euler(startValue.value);
				value.x = changeValue.value.x * num;
				value.y = changeValue.value.y * num;
				value.z = changeValue.value.z * num;
				if (options.rotateMode == RotateMode.WorldAxisAdd)
				{
					setter(quaternion * Quaternion.Inverse(quaternion) * Quaternion.Euler(value) * quaternion);
				}
				else
				{
					setter(quaternion * Quaternion.Euler(value));
				}
			}
			else
			{
				value.x += changeValue.value.x * num;
				value.y += changeValue.value.y * num;
				value.z += changeValue.value.z * num;
				setter(Quaternion.Euler(value));
			}
		}
	}
}
