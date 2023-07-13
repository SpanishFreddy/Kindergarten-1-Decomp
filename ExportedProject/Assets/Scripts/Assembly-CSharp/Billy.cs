using UnityEngine;

public class Billy : NPCBehavior
{
	public TextureBlock eyesClosed;

	public void ResetLair()
	{
		base.transform.position = new Vector3(0f, 50f, 0f);
		pRend.enabled = false;
		ApplyTextureBlock(eyesClosed);
		mySkeleton.timeScale = 0f;
	}
}
