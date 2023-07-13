using Spine.Unity.MeshGeneration;
using UnityEngine;
using UnityEngine.UI;

namespace Spine.Unity
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform))]
	[DisallowMultipleComponent]
	[AddComponentMenu("Spine/SkeletonGraphic (Unity UI Canvas)")]
	public class SkeletonGraphic : MaskableGraphic, ISkeletonComponent, IAnimationStateComponent, ISkeletonAnimation
	{
		public SkeletonDataAsset skeletonDataAsset;

		[SpineSkin("", "skeletonDataAsset")]
		public string initialSkinName = "default";

		[SpineAnimation("", "skeletonDataAsset")]
		public string startingAnimation;

		public bool startingLoop;

		public float timeScale = 1f;

		public bool freeze;

		public bool unscaledTime;

		protected Skeleton skeleton;

		protected AnimationState state;

		protected ISimpleMeshGenerator spineMeshGenerator;

		public SkeletonDataAsset SkeletonDataAsset
		{
			get
			{
				return skeletonDataAsset;
			}
		}

		public override Texture mainTexture
		{
			get
			{
				return (!(skeletonDataAsset == null)) ? skeletonDataAsset.atlasAssets[0].materials[0].mainTexture : null;
			}
		}

		public Skeleton Skeleton
		{
			get
			{
				return skeleton;
			}
		}

		public SkeletonData SkeletonData
		{
			get
			{
				return (skeleton != null) ? skeleton.data : null;
			}
		}

		public bool IsValid
		{
			get
			{
				return skeleton != null;
			}
		}

		public AnimationState AnimationState
		{
			get
			{
				return state;
			}
		}

		public ISimpleMeshGenerator SpineMeshGenerator
		{
			get
			{
				return spineMeshGenerator;
			}
		}

		public event UpdateBonesDelegate UpdateLocal;

		public event UpdateBonesDelegate UpdateWorld;

		public event UpdateBonesDelegate UpdateComplete;

		protected override void Awake()
		{
			base.Awake();
			if (!IsValid)
			{
				Initialize(false);
				Rebuild(CanvasUpdate.PreRender);
			}
		}

		public override void Rebuild(CanvasUpdate update)
		{
			base.Rebuild(update);
			if (!base.canvasRenderer.cull && update == CanvasUpdate.PreRender)
			{
				UpdateMesh();
			}
		}

		public virtual void Update()
		{
			if (!freeze)
			{
				Update((!unscaledTime) ? Time.deltaTime : Time.unscaledDeltaTime);
			}
		}

		public virtual void Update(float deltaTime)
		{
			if (IsValid)
			{
				deltaTime *= timeScale;
				skeleton.Update(deltaTime);
				state.Update(deltaTime);
				state.Apply(skeleton);
				if (this.UpdateLocal != null)
				{
					this.UpdateLocal(this);
				}
				skeleton.UpdateWorldTransform();
				if (this.UpdateWorld != null)
				{
					this.UpdateWorld(this);
					skeleton.UpdateWorldTransform();
				}
				if (this.UpdateComplete != null)
				{
					this.UpdateComplete(this);
				}
			}
		}

		public void LateUpdate()
		{
			if (!freeze)
			{
				UpdateMesh();
			}
		}

		public void Clear()
		{
			skeleton = null;
			base.canvasRenderer.Clear();
		}

		public void Initialize(bool overwrite)
		{
			if ((IsValid && !overwrite) || skeletonDataAsset == null)
			{
				return;
			}
			SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(false);
			if (skeletonData == null || skeletonDataAsset.atlasAssets.Length <= 0 || skeletonDataAsset.atlasAssets[0].materials.Length <= 0)
			{
				return;
			}
			state = new AnimationState(skeletonDataAsset.GetAnimationStateData());
			if (state == null)
			{
				Clear();
				return;
			}
			skeleton = new Skeleton(skeletonData);
			spineMeshGenerator = new ArraysSimpleMeshGenerator();
			spineMeshGenerator.PremultiplyVertexColors = true;
			if (!string.IsNullOrEmpty(initialSkinName))
			{
				skeleton.SetSkin(initialSkinName);
			}
			if (!string.IsNullOrEmpty(startingAnimation))
			{
				state.SetAnimation(0, startingAnimation, startingLoop);
			}
		}

		public void UpdateMesh()
		{
			if (IsValid)
			{
				skeleton.SetColor(color);
				if (base.canvas != null)
				{
					spineMeshGenerator.Scale = base.canvas.referencePixelsPerUnit;
				}
				base.canvasRenderer.SetMesh(spineMeshGenerator.GenerateMesh(skeleton));
			}
		}
	}
}
