namespace Spine
{
	public class PathConstraintSpacingTimeline : PathConstraintPositionTimeline
	{
		public override int PropertyId
		{
			get
			{
				return 201326592 + pathConstraintIndex;
			}
		}

		public PathConstraintSpacingTimeline(int frameCount)
			: base(frameCount)
		{
		}

		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, bool setupPose, bool mixingOut)
		{
			PathConstraint pathConstraint = skeleton.pathConstraints.Items[pathConstraintIndex];
			float[] array = frames;
			if (time < array[0])
			{
				if (setupPose)
				{
					pathConstraint.spacing = pathConstraint.data.spacing;
				}
				return;
			}
			float num;
			if (time >= array[array.Length - 2])
			{
				num = array[array.Length + -1];
			}
			else
			{
				int num2 = Animation.BinarySearch(array, time, 2);
				num = array[num2 + -1];
				float num3 = array[num2];
				float curvePercent = GetCurvePercent(num2 / 2 - 1, 1f - (time - num3) / (array[num2 + -2] - num3));
				num += (array[num2 + 1] - num) * curvePercent;
			}
			if (setupPose)
			{
				pathConstraint.spacing = pathConstraint.data.spacing + (num - pathConstraint.data.spacing) * alpha;
			}
			else
			{
				pathConstraint.spacing += (num - pathConstraint.spacing) * alpha;
			}
		}
	}
}
