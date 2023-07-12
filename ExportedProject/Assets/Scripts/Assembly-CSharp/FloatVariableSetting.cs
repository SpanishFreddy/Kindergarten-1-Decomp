using System;
using System.Collections.Generic;
using TextFx;
using UnityEngine;
using UnityEngine.UI;

public class FloatVariableSetting : BaseVariableSetting
{
	public InputField m_fromValue;

	public InputField m_toValue;

	public InputField m_thenValue;

	public GameObject m_toValueObject;

	public GameObject m_thenValueObject;

	public override void Setup(string labelName, List<PresetEffectSetting.VariableStateListener> varStateListeners, Action valueChangedCallback, bool isSubSetting)
	{
		base.Setup(labelName, varStateListeners, valueChangedCallback, isSubSetting);
		if (varStateListeners.Count == 1)
		{
			m_toValueObject.SetActive(false);
			m_thenValueObject.SetActive(false);
		}
		else if (varStateListeners.Count == 2)
		{
			m_toValueObject.SetActive(true);
			m_thenValueObject.SetActive(false);
		}
		else if (varStateListeners.Count == 3)
		{
			m_toValueObject.SetActive(true);
			m_thenValueObject.SetActive(true);
		}
		m_fromValue.text = varStateListeners[0].m_startFloatValue.ToString();
		m_fromValue.onValueChanged.AddListener(delegate(string newValue)
		{
			varStateListeners[0].m_onFloatStateChangeCallback(float.Parse(newValue));
			if (valueChangedCallback != null)
			{
				valueChangedCallback();
			}
		});
		if (varStateListeners.Count > 1)
		{
			m_toValue.text = varStateListeners[1].m_startFloatValue.ToString();
			m_toValue.onValueChanged.AddListener(delegate(string newValue)
			{
				varStateListeners[1].m_onFloatStateChangeCallback(float.Parse(newValue));
				if (valueChangedCallback != null)
				{
					valueChangedCallback();
				}
			});
		}
		if (varStateListeners.Count <= 2)
		{
			return;
		}
		m_thenValue.text = varStateListeners[2].m_startFloatValue.ToString();
		m_thenValue.onValueChanged.AddListener(delegate(string newValue)
		{
			varStateListeners[2].m_onFloatStateChangeCallback(float.Parse(newValue));
			if (valueChangedCallback != null)
			{
				valueChangedCallback();
			}
		});
	}
}
