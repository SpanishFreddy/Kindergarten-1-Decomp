using UnityEngine;
using UnityEngine.UI;

public class MissionUIBehavior : MonoBehaviour
{
	public enum MissionStatus
	{
		Active = 0,
		Complete = 1,
		Failed = 2
	}

	public Sprite completeSprite;

	public Sprite failedSprite;

	public Image bg;

	public Image grade;

	public Mission mission;

	public Text text;

	public Image face;

	public Room environmentLock;

	public MissionStatus status;

	public void SetInProgress()
	{
		grade.enabled = false;
		status = MissionStatus.Active;
	}

	public void SetComplete()
	{
		grade.enabled = true;
		grade.sprite = completeSprite;
		status = MissionStatus.Complete;
		grade.rectTransform.anchoredPosition = new Vector2(-64f, 10f);
	}

	public void SetFailed()
	{
		grade.enabled = true;
		grade.sprite = failedSprite;
		status = MissionStatus.Failed;
	}

	public void SetColor(Color c)
	{
		bg.color = c;
	}

	public void SetFace(Sprite t)
	{
		face.sprite = t;
	}
}
