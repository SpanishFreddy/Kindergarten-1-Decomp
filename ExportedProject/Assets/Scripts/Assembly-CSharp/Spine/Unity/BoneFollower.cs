using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spine.Unity
{
	[ExecuteInEditMode]
	[AddComponentMenu("Spine/BoneFollower")]
	public class BoneFollower : MonoBehaviour
	{
		public SkeletonRenderer skeletonRenderer;

		[SpineBone("", "skeletonRenderer")]
		public string boneName;

		public Vector3 offset;

		public float offsetRotation;

		public bool followZPosition = true;

		public bool followBoneRotation = true;

		[Tooltip("Follows the skeleton's flip state by controlling this Transform's local scale.")]
		public bool followSkeletonFlip;

		[FormerlySerializedAs("resetOnAwake")]
		public bool initializeOnAwake = true;

		[NonSerialized]
		public bool valid;

		[NonSerialized]
		public Bone bone;

		private Transform skeletonTransform;

		public SkeletonRenderer SkeletonRenderer
		{
			get
			{
				return skeletonRenderer;
			}
			set
			{
				skeletonRenderer = value;
				Initialize();
			}
		}

		public void Awake()
		{
			if (initializeOnAwake)
			{
				Initialize();
			}
		}

		public void HandleRebuildRenderer(SkeletonRenderer skeletonRenderer)
		{
			Initialize();
		}

		public void Initialize()
		{
			bone = null;
			valid = skeletonRenderer != null && skeletonRenderer.valid;
			if (valid)
			{
				skeletonTransform = skeletonRenderer.transform;
				SkeletonRenderer obj = skeletonRenderer;
				obj.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(HandleRebuildRenderer));
				SkeletonRenderer obj2 = skeletonRenderer;
				obj2.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(obj2.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(HandleRebuildRenderer));
			}
		}

		private void OnDestroy()
		{
			if (skeletonRenderer != null)
			{
				SkeletonRenderer obj = skeletonRenderer;
				obj.OnRebuild = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnRebuild, new SkeletonRenderer.SkeletonRendererDelegate(HandleRebuildRenderer));
			}
		}

		public void LateUpdate()
		{
			if (!valid)
			{
				Initialize();
				return;
			}
			if (bone == null)
			{
				if (string.IsNullOrEmpty(boneName))
				{
					return;
				}
				bone = skeletonRenderer.skeleton.FindBone(boneName);
				if (bone == null)
				{
					Debug.LogError("Bone not found: " + boneName, this);
					return;
				}
			}
			Transform transform = base.transform;
			if (transform.parent == skeletonTransform)
			{
				if (followSkeletonFlip)
				{
					if (!bone.skeleton.flipX)
					{
						transform.localPosition = new Vector3(bone.worldX, bone.worldY, (!followZPosition) ? transform.localPosition.z : 0f) + offset;
					}
					else
					{
						transform.localPosition = new Vector3(bone.worldX, bone.worldY, (!followZPosition) ? transform.localPosition.z : 0f) + new Vector3(0f - offset.x, offset.y, offset.z);
					}
				}
				else
				{
					transform.localPosition = new Vector3(bone.worldX, bone.worldY, (!followZPosition) ? transform.localPosition.z : 0f) + offset;
				}
				if (followBoneRotation)
				{
					if (!bone.skeleton.flipX)
					{
						transform.localRotation = Quaternion.Euler(0f, 0f, bone.WorldRotationX + offsetRotation);
					}
					else
					{
						transform.localRotation = Quaternion.Euler(0f, 0f, bone.WorldRotationX + offsetRotation - 45f);
					}
				}
			}
			else
			{
				Vector3 vector = skeletonTransform.TransformPoint(new Vector3(bone.worldX, bone.worldY, 0f) + offset);
				if (!followZPosition)
				{
					vector.z = transform.position.z;
				}
				if (followSkeletonFlip)
				{
					if (bone.skeleton.flipX)
					{
						transform.position = vector - new Vector3(2f * offset.x, 0f, 0f);
					}
					else
					{
						transform.position = vector;
					}
				}
				else
				{
					transform.position = vector;
				}
				if (followBoneRotation)
				{
					Vector3 eulerAngles = skeletonTransform.rotation.eulerAngles;
					if (followSkeletonFlip)
					{
						if (bone.skeleton.flipX)
						{
							transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, skeletonTransform.rotation.eulerAngles.z + bone.WorldRotationX + offsetRotation - 45f);
						}
						else
						{
							transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, skeletonTransform.rotation.eulerAngles.z + bone.WorldRotationX + offsetRotation);
						}
					}
					else
					{
						transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, skeletonTransform.rotation.eulerAngles.z + bone.WorldRotationX + offsetRotation);
					}
				}
			}
			if (followSkeletonFlip && offset == Vector3.zero)
			{
				float y = ((!(bone.skeleton.flipX ^ bone.skeleton.flipY)) ? 1f : (-1f));
				transform.localScale = new Vector3(1f, y, 1f);
			}
		}
	}
}
