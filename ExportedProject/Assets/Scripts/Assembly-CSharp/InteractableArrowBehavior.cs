using DG.Tweening;
using UnityEngine;

public class InteractableArrowBehavior : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		base.transform.DOKill();
		base.transform.DOMoveY(1.75f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear)
			.SetRelative(true);
	}
}
