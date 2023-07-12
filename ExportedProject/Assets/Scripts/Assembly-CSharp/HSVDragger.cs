using UnityEngine;
using UnityEngine.UI;

public class HSVDragger : MonoBehaviour
{
	public RectTransform parentPanel;

	[HideInInspector]
	public RectTransform rectTransform;

	public ScrollRect scrollRect;

	public HSVPicker picker;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		Vector3 localPosition = rectTransform.localPosition;
		localPosition.x = Mathf.Clamp(localPosition.x, (0f - parentPanel.sizeDelta.x) / 2f, parentPanel.sizeDelta.x / 2f);
		localPosition.y = Mathf.Clamp(localPosition.y, (0f - parentPanel.sizeDelta.y) / 2f, parentPanel.sizeDelta.y / 2f);
		rectTransform.localPosition = localPosition;
		localPosition.x += parentPanel.sizeDelta.x / 2f;
		localPosition.y += parentPanel.sizeDelta.y / 2f;
		localPosition.x /= parentPanel.sizeDelta.x;
		localPosition.y /= parentPanel.sizeDelta.y;
	}

	public void ScrollValueChanged(Vector2 value)
	{
		Vector3 localPosition = rectTransform.localPosition;
		localPosition.x = Mathf.Clamp(localPosition.x, (0f - parentPanel.sizeDelta.x) / 2f, parentPanel.sizeDelta.x / 2f);
		localPosition.y = Mathf.Clamp(localPosition.y, (0f - parentPanel.sizeDelta.y) / 2f, parentPanel.sizeDelta.y / 2f);
		rectTransform.localPosition = localPosition;
		localPosition.x += parentPanel.sizeDelta.x / 2f;
		localPosition.y += parentPanel.sizeDelta.y / 2f;
		localPosition.x /= parentPanel.sizeDelta.x;
		localPosition.y /= parentPanel.sizeDelta.y;
		picker.MoveCursor(localPosition.x, localPosition.y);
	}

	public void SetSelectorPosition(float posX, float posY)
	{
		Vector3 localPosition = new Vector3(posX, posY, rectTransform.localPosition.z);
		localPosition.x *= parentPanel.sizeDelta.x;
		localPosition.y *= parentPanel.sizeDelta.y;
		localPosition.x -= parentPanel.sizeDelta.x / 2f;
		localPosition.y -= parentPanel.sizeDelta.y / 2f;
		rectTransform.localPosition = localPosition;
	}
}
