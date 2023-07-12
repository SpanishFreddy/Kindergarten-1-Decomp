using UnityEngine;

public class ExplosionCollisionDetection : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter2D(Collision2D c)
	{
		if (c.transform == base.transform.parent)
		{
			if (Random.Range(0, 2) == 1)
			{
				SFXManager.Instance.PlaySound("BasicHit");
			}
			else
			{
				SFXManager.Instance.PlaySound("GoreSplat2");
			}
		}
	}
}
