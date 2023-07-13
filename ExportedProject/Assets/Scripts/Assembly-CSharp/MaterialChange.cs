using Spine.Unity;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialChange : MonoBehaviour
{
	public Material myMat;

	public Material defaultMat;

	private void OnValidate()
	{
		GetComponent<SkeletonAnimation>().CustomMaterialOverride.Clear();
		GetComponent<SkeletonAnimation>().CustomMaterialOverride.Add(defaultMat, myMat);
	}

	private void Start()
	{
		GetComponent<SkeletonAnimation>().CustomMaterialOverride.Clear();
		GetComponent<SkeletonAnimation>().CustomMaterialOverride.Add(defaultMat, myMat);
	}

	private void OnEnable()
	{
		GetComponent<SkeletonAnimation>().CustomMaterialOverride.Clear();
		GetComponent<SkeletonAnimation>().CustomMaterialOverride.Add(defaultMat, myMat);
	}
}
