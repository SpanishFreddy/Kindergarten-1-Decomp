using UnityEngine;

namespace DG.Tweening.Core.Surrogates
{
	public struct Vector3Wrapper
	{
		public Vector3 value;

		public Vector3Wrapper(Vector3 value)
		{
			this.value = value;
		}

		public static implicit operator Vector3(Vector3Wrapper v)
		{
			return v.value;
		}

		public static implicit operator Vector3Wrapper(Vector3 v)
		{
			return new Vector3Wrapper(v);
		}
	}
}
