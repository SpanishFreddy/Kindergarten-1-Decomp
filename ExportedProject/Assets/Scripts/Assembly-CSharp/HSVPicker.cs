using UnityEngine;
using UnityEngine.UI;

public class HSVPicker : MonoBehaviour
{
	public HexRGB hexrgb;

	public Color32 currentColor;

	private byte r;

	private byte g;

	private byte b;

	public Image colorImage;

	public RawImage hsvSlider;

	public RawImage hsvImage;

	public HsvSliderPicker sliderPicker;

	public BoxSlider boxSlider;

	public Slider sliderR;

	public Slider sliderG;

	public Slider sliderB;

	public Text sliderRText;

	public Text sliderGText;

	public Text sliderBText;

	public float pointerPos;

	public int minHue;

	public int maxHue = 360;

	public float minSat;

	public float maxSat = 1f;

	public float minV;

	public float maxV = 1f;

	public float cursorX;

	public float cursorY;

	public HSVSliderEvent onValueChanged = new HSVSliderEvent();

	private bool isChanging;

	private void Awake()
	{
		if ((bool)hsvSlider)
		{
			hsvSlider.texture = HSVUtil.GenerateHSVTexture((int)hsvSlider.rectTransform.rect.width, (int)hsvSlider.rectTransform.rect.height, minHue, maxHue);
		}
		if ((bool)sliderR)
		{
			sliderR.onValueChanged.AddListener(delegate(float newValue)
			{
				if (!isChanging)
				{
					isChanging = true;
					r = (byte)newValue;
					currentColor.r = r;
					AssignColor(currentColor);
					sliderRText.text = "R:" + r;
					if (hexrgb != null)
					{
						hexrgb.ManipulateViaRGB2Hex();
					}
					isChanging = false;
				}
			});
		}
		if ((bool)sliderG)
		{
			sliderG.onValueChanged.AddListener(delegate(float newValue)
			{
				if (!isChanging)
				{
					isChanging = true;
					g = (byte)newValue;
					currentColor.g = g;
					AssignColor(currentColor);
					sliderGText.text = "G:" + g;
					if (hexrgb != null)
					{
						hexrgb.ManipulateViaRGB2Hex();
					}
					isChanging = false;
				}
			});
		}
		if ((bool)sliderB)
		{
			sliderB.onValueChanged.AddListener(delegate(float newValue)
			{
				if (!isChanging)
				{
					isChanging = true;
					b = (byte)newValue;
					currentColor.b = b;
					AssignColor(currentColor);
					sliderBText.text = "B:" + b;
					if (hexrgb != null)
					{
						hexrgb.ManipulateViaRGB2Hex();
					}
					isChanging = false;
				}
			});
		}
		if ((bool)hsvImage)
		{
			Color mainColor = Color.white;
			if ((bool)hsvSlider)
			{
				mainColor = ((Texture2D)hsvSlider.texture).GetPixelBilinear(0f, 0.035f);
			}
			hsvImage.texture = HSVUtil.GenerateColorTexture((int)hsvImage.rectTransform.rect.width, (int)hsvImage.rectTransform.rect.width, mainColor, minHue, maxHue, minSat, maxSat, minV, maxV);
		}
		MoveCursor(cursorX, cursorY);
		sliderPicker.SetSliderPosition(1f - pointerPos);
	}

	public void AssignColor(Color color)
	{
		HsvColor hsvColor = HSVUtil.ConvertRgbToHsv(color);
		float newPos = (float)(hsvColor.H / 360.0);
		MovePointer(newPos, false);
		MoveCursor((float)hsvColor.S, (float)hsvColor.V, false);
		currentColor = color;
		colorImage.color = currentColor;
		onValueChanged.Invoke(currentColor);
	}

	public void PlaceCursor(float posX, float posY)
	{
		MoveCursor(posX, posY);
	}

	public Color MoveCursor(float posX, float posY, bool updateInputs = true)
	{
		if (posX > 1f)
		{
			posX %= 1f;
		}
		if (posY > 1f)
		{
			posY %= 1f;
		}
		posY = Mathf.Clamp(posY, 0f, 1f);
		posX = Mathf.Clamp(posX, 0f, 1f);
		cursorX = posX;
		cursorY = posY;
		boxSlider.normalizedValue = posX;
		boxSlider.normalizedValueY = posY;
		if (isChanging)
		{
			return currentColor;
		}
		isChanging = true;
		currentColor = GetColor(cursorX, cursorY);
		colorImage.color = currentColor;
		r = currentColor.r;
		g = currentColor.g;
		b = currentColor.b;
		if (updateInputs)
		{
			UpdateInputs();
			onValueChanged.Invoke(currentColor);
			if (hexrgb != null)
			{
				hexrgb.ManipulateViaRGB2Hex();
			}
		}
		isChanging = false;
		return currentColor;
	}

	public Color GetColor(float posX, float posY)
	{
		posY = Mathf.Clamp(posY, minV, maxV);
		posX = Mathf.Clamp(posX, minSat, maxSat);
		return HSVUtil.ConvertHsvToRgb(pointerPos * (float)(-(minHue + maxHue)) + (float)(minHue + maxHue), posX, posY);
	}

	public Color MovePointer(float newPos, bool updateInputs = true)
	{
		newPos = Mathf.Clamp(1f - newPos, 0.05f, 0.99f);
		pointerPos = newPos;
		Color pixelBilinear = ((Texture2D)hsvSlider.texture).GetPixelBilinear(0f, pointerPos);
		if ((bool)hsvImage && hsvImage.texture != null)
		{
			if ((int)hsvImage.rectTransform.rect.width != hsvImage.texture.width || (int)hsvImage.rectTransform.rect.height != hsvImage.texture.height)
			{
				Object.Destroy(hsvImage.texture);
				hsvImage.texture = null;
				hsvImage.texture = HSVUtil.GenerateColorTexture((int)hsvImage.rectTransform.rect.width, (int)hsvImage.rectTransform.rect.height, pixelBilinear, minHue, maxHue, minSat, maxSat, minV, maxV);
			}
			else
			{
				HSVUtil.GenerateColorTexture(pixelBilinear, (Texture2D)hsvImage.texture, minHue, maxHue, minSat, maxSat, minV, maxV);
			}
		}
		else
		{
			hsvImage.texture = HSVUtil.GenerateColorTexture((int)hsvImage.rectTransform.rect.width, (int)hsvImage.rectTransform.rect.height, pixelBilinear, minHue, maxHue, minSat, maxSat, minV, maxV);
		}
		if (isChanging)
		{
			return currentColor;
		}
		isChanging = true;
		currentColor = GetColor(cursorX, cursorY);
		colorImage.color = currentColor;
		r = currentColor.r;
		g = currentColor.g;
		b = currentColor.b;
		if (updateInputs)
		{
			UpdateInputs();
			onValueChanged.Invoke(currentColor);
			if (hexrgb != null)
			{
				hexrgb.ManipulateViaRGB2Hex();
			}
		}
		isChanging = false;
		return currentColor;
	}

	public void UpdateInputs()
	{
		if ((bool)sliderR)
		{
			sliderR.value = (int)r;
		}
		if ((bool)sliderG)
		{
			sliderG.value = (int)g;
		}
		if ((bool)sliderB)
		{
			sliderB.value = (int)b;
		}
		if ((bool)sliderRText)
		{
			sliderRText.text = "R:" + r;
		}
		if ((bool)sliderGText)
		{
			sliderGText.text = "G:" + g;
		}
		if ((bool)sliderBText)
		{
			sliderBText.text = "B:" + b;
		}
	}

	private void OnDestroy()
	{
		if ((bool)hsvSlider && hsvSlider.texture != null)
		{
			Object.Destroy(hsvSlider.texture);
		}
		if ((bool)hsvImage && hsvImage.texture != null)
		{
			Object.Destroy(hsvImage.texture);
		}
	}
}
