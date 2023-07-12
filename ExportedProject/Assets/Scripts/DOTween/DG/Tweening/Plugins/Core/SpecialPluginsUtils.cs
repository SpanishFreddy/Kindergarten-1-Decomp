using DG.Tweening.Core;
using DG.Tweening.Core.Surrogates;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
	internal static class SpecialPluginsUtils
	{
		internal static bool SetLookAt(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t)
		{
			Transform transform = t.target as Transform;
			Vector3 forward = t.endValue;
			forward -= transform.position;
			switch (t.plugOptions.axisConstraint)
			{
			case AxisConstraint.X:
				forward.x = 0f;
				break;
			case AxisConstraint.Y:
				forward.y = 0f;
				break;
			case AxisConstraint.Z:
				forward.z = 0f;
				break;
			}
			Vector3 eulerAngles = Quaternion.LookRotation(forward, t.plugOptions.up).eulerAngles;
			t.endValue = eulerAngles;
			return true;
		}

		internal static bool SetPunch(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
		{
			Vector3 vector;
			try
			{
				vector = t.getter();
			}
			catch
			{
				return false;
			}
			t.isRelative = (t.isSpeedBased = false);
			t.easeType = Ease.OutQuad;
			t.customEase = null;
			int num = t.endValue.Length;
			for (int i = 0; i < num; i++)
			{
				t.endValue[i] = t.endValue[i] + vector;
			}
			return true;
		}

		internal static bool SetShake(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
		{
			if (!SetPunch(t))
			{
				return false;
			}
			t.easeType = Ease.Linear;
			return true;
		}

		internal static bool SetCameraShakePosition(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
		{
			if (!SetShake(t))
			{
				return false;
			}
			Camera camera = t.target as Camera;
			if (camera == null)
			{
				return false;
			}
			Vector3 vector = t.getter();
			Transform transform = camera.transform;
			int num = t.endValue.Length;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector2 = t.endValue[i];
				t.endValue[i] = transform.localRotation * (vector2 - vector) + vector;
			}
			return true;
		}
	}
}
