using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HsvSliderPicker : MonoBehaviour, IDragHandler, IPointerDownHandler, IEventSystemHandler
{
	public HSVPicker picker;

	public Slider slider;

	private void PlacePointer(PointerEventData eventData)
	{
		Vector2 vector = new Vector2(eventData.position.x - picker.hsvSlider.rectTransform.position.x, picker.hsvSlider.rectTransform.position.y - eventData.position.y);
		vector.y /= picker.hsvSlider.rectTransform.rect.height * picker.hsvSlider.canvas.transform.lossyScale.y;
		vector.y = Mathf.Clamp(vector.y, 0f, 1f);
		picker.MovePointer(vector.y);
	}

	public void OnDrag(PointerEventData eventData)
	{
		PlacePointer(eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		PlacePointer(eventData);
	}

	public void SliderPositionChanged(float sliderValue)
	{
		picker.MovePointer(sliderValue);
	}

	internal void SetSliderPosition(float pointerPos)
	{
		slider.normalizedValue = pointerPos;
	}
}
