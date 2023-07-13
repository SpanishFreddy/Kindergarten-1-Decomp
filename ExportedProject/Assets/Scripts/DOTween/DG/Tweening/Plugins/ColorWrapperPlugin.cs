using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Core.Surrogates;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	public class ColorWrapperPlugin : ABSTweenPlugin<ColorWrapper, ColorWrapper, ColorOptions>
	{
		public override void Reset(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t)
		{
		}

		public override void SetFrom(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t, bool isRelative)
		{
			Color color = t.endValue;
			t.endValue.value = t.getter().value;
			t.startValue.value = (isRelative ? (t.endValue.value + color) : color);
			Color value = t.endValue.value;
			if (!t.plugOptions.alphaOnly)
			{
				value = t.startValue.value;
			}
			else
			{
				value.a = t.startValue.value.a;
			}
			t.setter(value);
		}

		public override ColorWrapper ConvertToStartValue(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t, ColorWrapper value)
		{
			return value.value;
		}

		public override void SetRelativeEndValue(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t)
		{
			t.endValue.value += t.startValue.value;
		}

		public override void SetChangeValue(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t)
		{
			t.changeValue.value = t.endValue.value - t.startValue.value;
		}

		public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, ColorWrapper changeValue)
		{
			return 1f / unitsXSecond;
		}

		public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<ColorWrapper> getter, DOSetter<ColorWrapper> setter, float elapsed, ColorWrapper startValue, ColorWrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
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
			if (!options.alphaOnly)
			{
				startValue.value.r += changeValue.value.r * num;
				startValue.value.g += changeValue.value.g * num;
				startValue.value.b += changeValue.value.b * num;
				startValue.value.a += changeValue.value.a * num;
				setter(startValue);
			}
			else
			{
				Color value = getter().value;
				value.a = startValue.value.a + changeValue.value.a * num;
				setter(value);
			}
		}
	}
}
