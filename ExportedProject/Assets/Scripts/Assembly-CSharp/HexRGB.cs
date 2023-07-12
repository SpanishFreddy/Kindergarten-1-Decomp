using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class HexRGB : MonoBehaviour
{
	public InputField textColor;

	public HSVPicker hsvpicker;

	public void ManipulateViaRGB2Hex()
	{
		Color color = hsvpicker.currentColor;
		string text = ColorToHex(color);
		textColor.text = text;
	}

	public static string ColorToHex(Color color)
	{
		int num = Mathf.RoundToInt(color.r * 255f);
		int num2 = Mathf.RoundToInt(color.g * 255f);
		int num3 = Mathf.RoundToInt(color.b * 255f);
		return string.Format("#{0:X2}{1:X2}{2:X2}", num, num2, num3);
	}

	public void ManipulateViaHex2RGB()
	{
		string text = textColor.text;
		Color color = Hex2RGB(text);
		hsvpicker.AssignColor(color);
	}

	private static Color NormalizeVector4(Vector3 v, float r, float a)
	{
		float r2 = v.x / r;
		float g = v.y / r;
		float b = v.z / r;
		return new Color(r2, g, b, a);
	}

	private Color Hex2RGB(string hexColor)
	{
		if (hexColor.IndexOf('#') != -1)
		{
			hexColor = hexColor.Replace("#", string.Empty);
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (hexColor.Length == 6)
		{
			num = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
			num2 = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
			num3 = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);
		}
		else if (hexColor.Length == 3)
		{
			num = int.Parse(hexColor[0].ToString() + hexColor[0], NumberStyles.AllowHexSpecifier);
			num2 = int.Parse(hexColor[1].ToString() + hexColor[1], NumberStyles.AllowHexSpecifier);
			num3 = int.Parse(hexColor[2].ToString() + hexColor[2], NumberStyles.AllowHexSpecifier);
		}
		Color32 color = new Color32((byte)num, (byte)num2, (byte)num3, byte.MaxValue);
		return color;
	}
}
