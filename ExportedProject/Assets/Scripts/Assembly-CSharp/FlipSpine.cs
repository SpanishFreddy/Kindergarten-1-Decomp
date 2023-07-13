using Spine.Unity;
using UnityEngine;

public class FlipSpine : MonoBehaviour
{
	private void Start()
	{
		GetComponent<SkeletonAnimation>().skeleton.flipX = true;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			ScreenCapture.CaptureScreenshot("KG.png");
		}
	}
}
