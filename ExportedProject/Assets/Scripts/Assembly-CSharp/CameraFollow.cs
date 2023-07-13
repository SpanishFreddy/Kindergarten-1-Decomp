using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public bool overrideCamera;

	public Transform player;

	public float minPosX;

	public float maxPosX;

	public float minPosY;

	public float maxPosY;

	private void Start()
	{
		if (PlayerPrefs.GetInt("SavedWidth", 0) == 0 || PlayerPrefs.GetInt("SavedHeight", 0) != 0)
		{
		}
		RefreshScreen(Camera.main.aspect);
	}

	public void RefreshScreen(float x)
	{
		if (Mathf.Approximately(x, 1.7777778f))
		{
			minPosY = -4.5f;
			maxPosY = 1f;
			minPosX = -5.7f;
			maxPosX = 5.7f;
		}
		else if (Mathf.Approximately(x, 1.3333334f))
		{
			minPosY = -3.7f;
			maxPosY = 1f;
			minPosX = -12.75f;
			maxPosX = 12.75f;
		}
		else if (Mathf.Approximately(x, 1.6f))
		{
			minPosY = -4.2f;
			maxPosY = 1f;
			minPosX = -8.5f;
			maxPosX = 8.5f;
		}
	}

	private void LateUpdate()
	{
		if (!overrideCamera)
		{
			base.transform.position = new Vector3(Mathf.Clamp(player.position.x, minPosX, maxPosX), Mathf.Clamp(player.position.y, minPosY, maxPosY), -30f);
		}
	}

	public void CameraShake()
	{
		overrideCamera = true;
		base.transform.DOShakePosition(2f, 0.5f, 50).OnComplete(delegate
		{
			overrideCamera = false;
		});
	}

	public void CameraShake(float d)
	{
		overrideCamera = true;
		base.transform.DOShakePosition(d, 0.5f, 50).OnComplete(delegate
		{
			overrideCamera = false;
		});
	}

	public void CameraShake(float d, bool x)
	{
		base.transform.DOShakePosition(d, 0.5f, 50);
	}
}
