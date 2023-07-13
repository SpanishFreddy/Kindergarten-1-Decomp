using UnityEngine;

namespace DG.Tweening.Core.Surrogates
{
	public struct QuaternionWrapper
	{
		public Quaternion value;

		public QuaternionWrapper(Quaternion value)
		{
			this.value = value;
		}

		public static implicit operator Quaternion(QuaternionWrapper v)
		{
			return v.value;
		}

		public static implicit operator QuaternionWrapper(Quaternion v)
		{
			return new QuaternionWrapper(v);
		}
	}
}
