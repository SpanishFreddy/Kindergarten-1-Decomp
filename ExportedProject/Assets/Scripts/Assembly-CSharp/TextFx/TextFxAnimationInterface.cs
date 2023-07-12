using System;
using UnityEngine;

namespace TextFx
{
	public interface TextFxAnimationInterface
	{
		TextFxAnimationManager AnimationManager { get; }

		int LayerOverride { get; }

		float MovementScale { get; }

		string AssetNameSuffix { get; }

		TEXTFX_IMPLEMENTATION TextFxImplementation { get; }

		TextAlignment TextAlignment { get; }

		bool FlippedMeshVerts { get; }

		Action OnMeshUpdateCall { get; set; }

		GameObject GameObject { get; }

		UnityEngine.Object ObjectInstance { get; }

		bool CurvePositioningEnabled { get; }

		bool RenderToCurve { get; set; }

		TextFxBezierCurve BezierCurve { get; set; }

		int NumMeshVerts { get; }

		void UpdateTextFxMesh();

		void SetText(string text);

		void SetColour(Color colour);

		Vector3 GetMeshVert(int index);

		Color GetMeshColour(int index);
	}
}
