using System;
using UnityEngine;

public class EasingManager
{
	public static float GetEaseProgress(EasingEquation ease_type, float linear_progress)
	{
		switch (ease_type)
		{
		case EasingEquation.Linear:
			return linear_progress;
		case EasingEquation.BackEaseIn:
			return BackEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.BackEaseInOut:
			return BackEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.BackEaseOut:
			return BackEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.BackEaseOutIn:
			return BackEaseOutIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.BounceEaseIn:
			return BounceEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.BounceEaseInOut:
			return BounceEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.BounceEaseOut:
			return BounceEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.BounceEaseOutIn:
			return BounceEaseOutIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.CircEaseIn:
			return CircEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.CircEaseInOut:
			return CircEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.CircEaseOut:
			return CircEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.CircEaseOutIn:
			return CircEaseOutIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.CubicEaseIn:
			return CubicEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.CubicEaseInOut:
			return CubicEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.CubicEaseOut:
			return CubicEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.CubicEaseOutIn:
			return CubicEaseOutIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.ElasticEaseIn:
			return ElasticEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.ElasticEaseInOut:
			return ElasticEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.ElasticEaseOut:
			return ElasticEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.ElasticEaseOutIn:
			return ElasticEaseOutIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.ExpoEaseIn:
			return ExpoEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.ExpoEaseInOut:
			return ExpoEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.ExpoEaseOut:
			return ExpoEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.ExpoEaseOutIn:
			return ExpoEaseOutIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuadEaseIn:
			return QuadEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuadEaseInOut:
			return QuadEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuadEaseOut:
			return QuadEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuadEaseOutIn:
			return QuadEaseOutIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuartEaseIn:
			return QuartEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuartEaseInOut:
			return QuartEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuartEaseOut:
			return QuartEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuartEaseOutIn:
			return QuartEaseOutIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuintEaseIn:
			return QuintEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuintEaseInOut:
			return QuintEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuintEaseOut:
			return QuintEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.QuintEaseOutIn:
			return QuintEaseOutIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.SineEaseIn:
			return SineEaseIn(linear_progress, 0f, 1f, 1f);
		case EasingEquation.SineEaseInOut:
			return SineEaseInOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.SineEaseOut:
			return SineEaseOut(linear_progress, 0f, 1f, 1f);
		case EasingEquation.SineEaseOutIn:
			return SineEaseOutIn(linear_progress, 0f, 1f, 1f);
		default:
			return linear_progress;
		}
	}

	public static EasingEquation GetEaseTypeOpposite(EasingEquation ease_type)
	{
		switch (ease_type)
		{
		case EasingEquation.Linear:
			return EasingEquation.Linear;
		case EasingEquation.BackEaseIn:
			return EasingEquation.BackEaseOut;
		case EasingEquation.BackEaseInOut:
			return EasingEquation.BackEaseOutIn;
		case EasingEquation.BackEaseOut:
			return EasingEquation.BackEaseIn;
		case EasingEquation.BackEaseOutIn:
			return EasingEquation.BackEaseInOut;
		case EasingEquation.BounceEaseIn:
			return EasingEquation.BounceEaseOut;
		case EasingEquation.BounceEaseInOut:
			return EasingEquation.BounceEaseOutIn;
		case EasingEquation.BounceEaseOut:
			return EasingEquation.BounceEaseIn;
		case EasingEquation.BounceEaseOutIn:
			return EasingEquation.BounceEaseInOut;
		case EasingEquation.CircEaseIn:
			return EasingEquation.CircEaseOut;
		case EasingEquation.CircEaseInOut:
			return EasingEquation.CircEaseOutIn;
		case EasingEquation.CircEaseOut:
			return EasingEquation.CircEaseIn;
		case EasingEquation.CircEaseOutIn:
			return EasingEquation.CircEaseInOut;
		case EasingEquation.CubicEaseIn:
			return EasingEquation.CubicEaseOut;
		case EasingEquation.CubicEaseInOut:
			return EasingEquation.CubicEaseOutIn;
		case EasingEquation.CubicEaseOut:
			return EasingEquation.CubicEaseIn;
		case EasingEquation.CubicEaseOutIn:
			return EasingEquation.CubicEaseInOut;
		case EasingEquation.ElasticEaseIn:
			return EasingEquation.ElasticEaseOut;
		case EasingEquation.ElasticEaseInOut:
			return EasingEquation.ElasticEaseOutIn;
		case EasingEquation.ElasticEaseOut:
			return EasingEquation.ElasticEaseIn;
		case EasingEquation.ElasticEaseOutIn:
			return EasingEquation.ElasticEaseInOut;
		case EasingEquation.ExpoEaseIn:
			return EasingEquation.ExpoEaseOut;
		case EasingEquation.ExpoEaseInOut:
			return EasingEquation.ExpoEaseOutIn;
		case EasingEquation.ExpoEaseOut:
			return EasingEquation.ExpoEaseIn;
		case EasingEquation.ExpoEaseOutIn:
			return EasingEquation.ExpoEaseInOut;
		case EasingEquation.QuadEaseIn:
			return EasingEquation.QuadEaseOut;
		case EasingEquation.QuadEaseInOut:
			return EasingEquation.QuadEaseOutIn;
		case EasingEquation.QuadEaseOut:
			return EasingEquation.QuadEaseIn;
		case EasingEquation.QuadEaseOutIn:
			return EasingEquation.QuadEaseInOut;
		case EasingEquation.QuartEaseIn:
			return EasingEquation.QuartEaseOut;
		case EasingEquation.QuartEaseInOut:
			return EasingEquation.QuartEaseOutIn;
		case EasingEquation.QuartEaseOut:
			return EasingEquation.QuartEaseIn;
		case EasingEquation.QuartEaseOutIn:
			return EasingEquation.QuartEaseInOut;
		case EasingEquation.QuintEaseIn:
			return EasingEquation.QuintEaseOut;
		case EasingEquation.QuintEaseInOut:
			return EasingEquation.QuintEaseOutIn;
		case EasingEquation.QuintEaseOut:
			return EasingEquation.QuintEaseIn;
		case EasingEquation.QuintEaseOutIn:
			return EasingEquation.QuintEaseInOut;
		case EasingEquation.SineEaseIn:
			return EasingEquation.SineEaseOut;
		case EasingEquation.SineEaseInOut:
			return EasingEquation.SineEaseOutIn;
		case EasingEquation.SineEaseOut:
			return EasingEquation.SineEaseIn;
		case EasingEquation.SineEaseOutIn:
			return EasingEquation.SineEaseInOut;
		default:
			return EasingEquation.Linear;
		}
	}

	public static float Linear(float t, float b, float c, float d)
	{
		return c * t / d + b;
	}

	public static float ExpoEaseOut(float t, float b, float c, float d)
	{
		return (t != d) ? (c * (0f - Mathf.Pow(2f, -10f * t / d) + 1f) + b) : (b + c);
	}

	public static float ExpoEaseIn(float t, float b, float c, float d)
	{
		return (t != 0f) ? (c * Mathf.Pow(2f, 10f * (t / d - 1f)) + b) : b;
	}

	public static float ExpoEaseInOut(float t, float b, float c, float d)
	{
		if (t == 0f)
		{
			return b;
		}
		if (t == d)
		{
			return b + c;
		}
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b;
		}
		return c / 2f * (0f - Mathf.Pow(2f, -10f * (t -= 1f)) + 2f) + b;
	}

	public static float ExpoEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return ExpoEaseOut(t * 2f, b, c / 2f, d);
		}
		return ExpoEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float CircEaseOut(float t, float b, float c, float d)
	{
		return c * Mathf.Sqrt(1f - (t = t / d - 1f) * t) + b;
	}

	public static float CircEaseIn(float t, float b, float c, float d)
	{
		return (0f - c) * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
	}

	public static float CircEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return (0f - c) / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + b;
		}
		return c / 2f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
	}

	public static float CircEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return CircEaseOut(t * 2f, b, c / 2f, d);
		}
		return CircEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float QuadEaseOut(float t, float b, float c, float d)
	{
		return (0f - c) * (t /= d) * (t - 2f) + b;
	}

	public static float QuadEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t + b;
	}

	public static float QuadEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t + b;
		}
		return (0f - c) / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
	}

	public static float QuadEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return QuadEaseOut(t * 2f, b, c / 2f, d);
		}
		return QuadEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float SineEaseOut(float t, float b, float c, float d)
	{
		return c * Mathf.Sin(t / d * ((float)Math.PI / 2f)) + b;
	}

	public static float SineEaseIn(float t, float b, float c, float d)
	{
		return (0f - c) * Mathf.Cos(t / d * ((float)Math.PI / 2f)) + c + b;
	}

	public static float SineEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * Mathf.Sin((float)Math.PI * t / 2f) + b;
		}
		return (0f - c) / 2f * (Mathf.Cos((float)Math.PI * (t -= 1f) / 2f) - 2f) + b;
	}

	public static float SineEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return SineEaseOut(t * 2f, b, c / 2f, d);
		}
		return SineEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float CubicEaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1f) * t * t + 1f) + b;
	}

	public static float CubicEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t + b;
	}

	public static float CubicEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t * t + b;
		}
		return c / 2f * ((t -= 2f) * t * t + 2f) + b;
	}

	public static float CubicEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return CubicEaseOut(t * 2f, b, c / 2f, d);
		}
		return CubicEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float QuartEaseOut(float t, float b, float c, float d)
	{
		return (0f - c) * ((t = t / d - 1f) * t * t * t - 1f) + b;
	}

	public static float QuartEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t * t + b;
	}

	public static float QuartEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t * t * t + b;
		}
		return (0f - c) / 2f * ((t -= 2f) * t * t * t - 2f) + b;
	}

	public static float QuartEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return QuartEaseOut(t * 2f, b, c / 2f, d);
		}
		return QuartEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float QuintEaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
	}

	public static float QuintEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t * t * t + b;
	}

	public static float QuintEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t * t * t * t + b;
		}
		return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
	}

	public static float QuintEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return QuintEaseOut(t * 2f, b, c / 2f, d);
		}
		return QuintEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float ElasticEaseOut(float t, float b, float c, float d)
	{
		if ((t /= d) == 1f)
		{
			return b + c;
		}
		float num = d * 0.3f;
		float num2 = num / 4f;
		return c * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - num2) * ((float)Math.PI * 2f) / num) + c + b;
	}

	public static float ElasticEaseIn(float t, float b, float c, float d)
	{
		if ((t /= d) == 1f)
		{
			return b + c;
		}
		float num = d * 0.3f;
		float num2 = num / 4f;
		return 0f - c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * ((float)Math.PI * 2f) / num) + b;
	}

	public static float ElasticEaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) == 2f)
		{
			return b + c;
		}
		float num = d * 0.45000002f;
		float num2 = num / 4f;
		if (t < 1f)
		{
			return -0.5f * (c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * ((float)Math.PI * 2f) / num)) + b;
		}
		return c * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * ((float)Math.PI * 2f) / num) * 0.5f + c + b;
	}

	public static float ElasticEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return ElasticEaseOut(t * 2f, b, c / 2f, d);
		}
		return ElasticEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float BounceEaseOut(float t, float b, float c, float d)
	{
		if ((t /= d) < 0.36363637f)
		{
			return c * (7.5625f * t * t) + b;
		}
		if (t < 0.72727275f)
		{
			return c * (7.5625f * (t -= 0.54545456f) * t + 0.75f) + b;
		}
		if (t < 0.90909094f)
		{
			return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
		}
		return c * (7.5625f * (t -= 21f / 22f) * t + 63f / 64f) + b;
	}

	public static float BounceEaseIn(float t, float b, float c, float d)
	{
		return c - BounceEaseOut(d - t, 0f, c, d) + b;
	}

	public static float BounceEaseInOut(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return BounceEaseIn(t * 2f, 0f, c, d) * 0.5f + b;
		}
		return BounceEaseOut(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
	}

	public static float BounceEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return BounceEaseOut(t * 2f, b, c / 2f, d);
		}
		return BounceEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float BackEaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1f) * t * (2.70158f * t + 1.70158f) + 1f) + b;
	}

	public static float BackEaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * (2.70158f * t - 1.70158f) + b;
	}

	public static float BackEaseInOut(float t, float b, float c, float d)
	{
		float num = 1.70158f;
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
		}
		return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
	}

	public static float BackEaseOutIn(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return BackEaseOut(t * 2f, b, c / 2f, d);
		}
		return BackEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
	}
}
