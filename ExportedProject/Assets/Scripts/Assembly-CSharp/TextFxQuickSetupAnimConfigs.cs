using Boomlagoon.TextFx.JSON;
using TextFx;
using UnityEngine;

public class TextFxQuickSetupAnimConfigs : MonoBehaviour
{
	private const string INTRO_ANIM_FOLDER_NAME = "Intros";

	private const string MAIN_ANIM_FOLDER_NAME = "Mains";

	private const string OUTRO_ANIM_FOLDER_NAME = "Outros";

	private static string[] m_introAnimNames;

	private static string[] m_mainAnimNames;

	private static string[] m_outroAnimNames;

	public static string[] IntroAnimNames
	{
		get
		{
			GetLatestEffectNameLists();
			return m_introAnimNames;
		}
	}

	public static string[] MainAnimNames
	{
		get
		{
			GetLatestEffectNameLists();
			return m_mainAnimNames;
		}
	}

	public static string[] OutroAnimNames
	{
		get
		{
			GetLatestEffectNameLists();
			return m_outroAnimNames;
		}
	}

	public static void GetLatestEffectNameLists(bool force = false)
	{
		if (m_introAnimNames != null && !force)
		{
			return;
		}
		tfxJSONObject tfxJSONObject = tfxJSONObject.Parse(Resources.Load<TextAsset>("textfxAnimNames").text);
		m_introAnimNames = new string[tfxJSONObject["Intros"].Array.Length + 1];
		m_introAnimNames[0] = "None";
		int num = 1;
		foreach (tfxJSONValue item in tfxJSONObject["Intros"].Array)
		{
			m_introAnimNames[num] = item.Str;
			num++;
		}
		m_mainAnimNames = new string[tfxJSONObject["Mains"].Array.Length + 1];
		m_mainAnimNames[0] = "None";
		num = 1;
		foreach (tfxJSONValue item2 in tfxJSONObject["Mains"].Array)
		{
			m_mainAnimNames[num] = item2.Str;
			num++;
		}
		m_outroAnimNames = new string[tfxJSONObject["Outros"].Array.Length + 1];
		m_outroAnimNames[0] = "None";
		num = 1;
		foreach (tfxJSONValue item3 in tfxJSONObject["Outros"].Array)
		{
			m_outroAnimNames[num] = item3.Str;
			num++;
		}
	}

	public static string GetConfig(TextFxAnimationManager.PRESET_ANIMATION_SECTION section, string animName)
	{
		string[] array = new string[3] { "Intros", "Mains", "Outros" };
		animName = animName.Trim();
		TextAsset textAsset = Resources.Load<TextAsset>(array[(int)section] + "/" + animName);
		return (!(textAsset != null)) ? string.Empty : textAsset.text;
	}
}
