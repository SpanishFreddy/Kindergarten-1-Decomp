using UnityEngine;
using UnityEngine.EventSystems;

public class HsvBoxSelector : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public HSVDragger dragger;

	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
	}

	private void PlaceCursor(PointerEventData eventData)
	{
		Vector2 position = eventData.position;
		Debug.Log(string.Concat(position, " ", rectTransform.position));
	}

	public void OnDrag(PointerEventData eventData)
	{
		PlaceCursor(eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		PlaceCursor(eventData);
	}
}
