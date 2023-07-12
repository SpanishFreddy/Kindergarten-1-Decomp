using UnityEngine;

namespace DG.Tweening.Core.Surrogates
{
	public struct ColorWrapper
	{
		public Color value;

		public ColorWrapper(Color value)
		{
			this.value = value;
		}

		public static implicit operator Color(ColorWrapper v)
		{
			return v.value;
		}

		public static implicit operator Color32(ColorWrapper v)
		{
			return v.value;
		}

		public static implicit operator ColorWrapper(Color v)
		{
			return new ColorWrapper(v);
		}

		public static implicit operator ColorWrapper(Color32 v)
		{
			return new ColorWrapper(v);
		}
	}
}
