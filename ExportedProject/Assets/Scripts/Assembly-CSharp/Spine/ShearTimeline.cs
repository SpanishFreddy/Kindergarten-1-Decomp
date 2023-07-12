namespace Spine
{
	public class ShearTimeline : TranslateTimeline
	{
		public override int PropertyId
		{
			get
			{
				return 50331648 + boneIndex;
			}
		}

		public ShearTimeline(int frameCount)
			: base(frameCount)
		{
		}

		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, bool setupPose, bool mixingOut)
		{
			Bone bone = skeleton.bones.Items[boneIndex];
			float[] array = frames;
			if (time < array[0])
			{
				if (setupPose)
				{
					bone.shearX = bone.data.shearX;
					bone.shearY = bone.data.shearY;
				}
				return;
			}
			float num;
			float num2;
			if (time >= array[array.Length - 3])
			{
				num = array[array.Length + -2];
				num2 = array[array.Length + -1];
			}
			else
			{
				int num3 = Animation.BinarySearch(array, time, 3);
				num = array[num3 + -2];
				num2 = array[num3 + -1];
				float num4 = array[num3];
				float curvePercent = GetCurvePercent(num3 / 3 - 1, 1f - (time - num4) / (array[num3 + -3] - num4));
				num += (array[num3 + 1] - num) * curvePercent;
				num2 += (array[num3 + 2] - num2) * curvePercent;
			}
			if (setupPose)
			{
				bone.shearX = bone.data.shearX + num * alpha;
				bone.shearY = bone.data.shearY + num2 * alpha;
			}
			else
			{
				bone.shearX += (bone.data.shearX + num - bone.shearX) * alpha;
				bone.shearY += (bone.data.shearY + num2 - bone.shearY) * alpha;
			}
		}
	}
}
