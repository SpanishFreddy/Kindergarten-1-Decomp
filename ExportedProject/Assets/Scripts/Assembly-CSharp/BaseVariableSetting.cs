using System;
using System.Collections.Generic;
using TextFx;
using UnityEngine;
using UnityEngine.UI;

public class BaseVariableSetting : MonoBehaviour
{
	private readonly Color SETTING_COLOUR = new Color(0.6392157f, 76f / 85f, 1f, 20f / 51f);

	private readonly Color SUB_SETTING_COLOUR = new Color(0f, 32f / 51f, 1f, 13f / 51f);

	public Text m_labelText;

	public Image m_bgImage;

	private bool m_subSetting;

	private bool m_subSettingActive = true;

	public bool IsSubSetting
	{
		get
		{
			return m_subSetting;
		}
	}

	public bool SubSettingActive
	{
		get
		{
			return m_subSettingActive;
		}
	}

	public void SubSettingSetActive(bool state)
	{
		m_subSettingActive = state;
		base.gameObject.SetActive(state);
	}

	public virtual void Setup(PresetEffectSetting settingData, List<PresetEffectSetting.VariableStateListener> varStateListener, Action valueChangedCallback, bool isSubSetting)
	{
		Setup(settingData.m_setting_name, varStateListener, valueChangedCallback, isSubSetting);
	}

	public virtual void Setup(string labelName, List<PresetEffectSetting.VariableStateListener> varStateListeners, Action valueChangedCallback, bool isSubSetting)
	{
		m_subSetting = isSubSetting;
		m_bgImage.color = ((!m_subSetting) ? SETTING_COLOUR : SUB_SETTING_COLOUR);
		m_labelText.text = labelName;
	}
}
