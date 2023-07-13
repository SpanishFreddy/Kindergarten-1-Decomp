using System;
using System.Collections.Generic;
using UnityEngine;

public static class HSVUtil
{
	public static HsvColor ConvertRgbToHsv(Color color)
	{
		return ConvertRgbToHsv((int)(color.r * 255f), (int)(color.g * 255f), (int)(color.b * 255f));
	}

	public static HsvColor ConvertRgbToHsv(double r, double b, double g)
	{
		double num = 0.0;
		double num2 = Math.Min(Math.Min(r, g), b);
		double num3 = Math.Max(Math.Max(r, g), b);
		double num4 = num3 - num2;
		double num5 = ((num3 != 0.0) ? (num4 / num3) : 0.0);
		if (num5 == 0.0)
		{
			num = 0.0;
		}
		else
		{
			if (r == num3)
			{
				num = (g - b) / num4;
			}
			else if (g == num3)
			{
				num = 2.0 + (b - r) / num4;
			}
			else if (b == num3)
			{
				num = 4.0 + (r - g) / num4;
			}
			num *= 60.0;
			if (num < 0.0)
			{
				num += 360.0;
			}
		}
		HsvColor result = default(HsvColor);
		result.H = num;
		result.S = num5;
		result.V = num3 / 255.0;
		return result;
	}

	public static Color ConvertHsvToRgb(HsvColor color)
	{
		return ConvertHsvToRgb(color.H, color.S, color.V);
	}

	public static Color ConvertHsvToRgb(double h, double s, double v)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		if (s == 0.0)
		{
			num = v;
			num2 = v;
			num3 = v;
		}
		else
		{
			h = ((h != 360.0) ? (h / 60.0) : 0.0);
			int num4 = (int)h;
			double num5 = h - (double)num4;
			double num6 = v * (1.0 - s);
			double num7 = v * (1.0 - s * num5);
			double num8 = v * (1.0 - s * (1.0 - num5));
			switch (num4)
			{
			case 0:
				num = v;
				num2 = num8;
				num3 = num6;
				break;
			case 1:
				num = num7;
				num2 = v;
				num3 = num6;
				break;
			case 2:
				num = num6;
				num2 = v;
				num3 = num8;
				break;
			case 3:
				num = num6;
				num2 = num7;
				num3 = v;
				break;
			case 4:
				num = num8;
				num2 = num6;
				num3 = v;
				break;
			default:
				num = v;
				num2 = num6;
				num3 = num7;
				break;
			}
		}
		return new Color((float)num, (float)num2, (float)num3, 1f);
	}

	public static List<Color> GenerateHsvSpectrum(int minHue = 0, int maxHue = 360)
	{
		List<Color> list = new List<Color>(8);
		for (int i = minHue; i < maxHue - 1; i++)
		{
			list.Add(ConvertHsvToRgb(i, 1.0, 1.0));
		}
		list.Add(ConvertHsvToRgb(maxHue, 1.0, 1.0));
		return list;
	}

	public static Texture2D GenerateHSVTexture(int width, int height, int minHue = 0, int maxHue = 360)
	{
		List<Color> list = GenerateHsvSpectrum(minHue, maxHue);
		float num = (float)list.Count / (float)height;
		Texture2D texture2D = new Texture2D(width, height);
		int num2 = Mathf.Max(1, (int)(1f / ((float)list.Count / num) * (float)height));
		int num3 = 0;
		Color white = Color.white;
		for (float num4 = 0f; num4 < (float)list.Count; num4 += num)
		{
			white = list[(int)num4];
			Color[] array = new Color[width * num2];
			for (int i = 0; i < width * num2; i++)
			{
				array[i] = white;
			}
			if (num3 < height)
			{
				texture2D.SetPixels(0, num3, width, num2, array);
			}
			num3 += num2;
		}
		texture2D.Apply();
		return texture2D;
	}

	public static Texture2D GenerateColorTexture(Color mainColor, Texture2D texture, int minHue = 0, int maxHue = 360, float minSat = 0f, float maxSat = 1f, float minV = 0f, float maxV = 1f)
	{
		int width = texture.width;
		int height = texture.height;
		HsvColor hsvColor = ConvertRgbToHsv(mainColor);
		double h = (double)(minHue + maxHue) - (360.0 - hsvColor.H);
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				HsvColor hsvColor2 = hsvColor;
				hsvColor2.V = Mathf.Clamp((float)i / (float)height, minV, maxV);
				hsvColor2.S = Mathf.Clamp((float)j / (float)width, minSat, maxSat);
				Color color = ConvertHsvToRgb(h, hsvColor2.S, hsvColor2.V);
				texture.SetPixel(j, i, color);
			}
		}
		texture.Apply();
		return texture;
	}

	public static Texture2D GenerateColorTexture(int width, int height, Color mainColor, int minHue = 0, int maxHue = 360, float minSat = 0f, float maxSat = 1f, float minV = 0f, float maxV = 1f)
	{
		return GenerateColorTexture(mainColor, new Texture2D(width, height), minHue, maxHue, minSat, maxSat, minV, maxV);
	}
}
