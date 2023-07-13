using UnityEngine;

namespace DG.Tweening.Core.Surrogates
{
	public struct Vector2Wrapper
	{
		public Vector2 value;

		public Vector2Wrapper(Vector2 value)
		{
			this.value = value;
		}

		public Vector2Wrapper(float x, float y)
		{
			value = new Vector2(x, y);
		}

		public static implicit operator Vector2(Vector2Wrapper v)
		{
			return v.value;
		}

		public static implicit operator Vector2Wrapper(Vector2 v)
		{
			return new Vector2Wrapper(v);
		}
	}
}
