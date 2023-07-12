using DG.Tweening;
using TextFx;
using UnityEngine;
using UnityEngine.UI;

public class TrailerChoreography : MonoBehaviour
{
	public RectTransform logoRect;

	public CanvasGroup Logo;

	public Image fader;

	public TextFxUGUI t;

	public Animation a;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
			fader.DOFade(0f, 2f).SetDelay(1.5f);
			a.Play();
		}
	}
}
