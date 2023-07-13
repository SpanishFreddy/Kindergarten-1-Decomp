using System;
using System.Collections.Generic;
using TextFx;
using UnityEngine;
using UnityEngine.UI;

public class Vector3VariableSetting : BaseVariableSetting
{
	public InputField m_fromXValue;

	public InputField m_fromYValue;

	public InputField m_fromZValue;

	public GameObject m_toValueObject;

	public GameObject m_thenValueObject;

	private Vector3 m_fromVec;

	public override void Setup(string labelName, List<PresetEffectSetting.VariableStateListener> varStateListeners, Action valueChangedCallback, bool isSubSetting)
	{
		base.Setup(labelName, varStateListeners, valueChangedCallback, isSubSetting);
		m_fromVec = varStateListeners[0].m_startVector3Value;
		m_fromXValue.text = m_fromVec.x.ToString();
		m_fromXValue.onValueChanged.AddListener(delegate(string newValue)
		{
			m_fromVec.x = float.Parse(newValue);
			varStateListeners[0].m_onVector3StateChangeCallback(m_fromVec);
			valueChangedCallback();
		});
		m_fromYValue.text = m_fromVec.y.ToString();
		m_fromYValue.onValueChanged.AddListener(delegate(string newValue)
		{
			m_fromVec.y = float.Parse(newValue);
			varStateListeners[0].m_onVector3StateChangeCallback(m_fromVec);
			valueChangedCallback();
		});
		m_fromZValue.text = m_fromVec.z.ToString();
		m_fromZValue.onValueChanged.AddListener(delegate(string newValue)
		{
			m_fromVec.z = float.Parse(newValue);
			varStateListeners[0].m_onVector3StateChangeCallback(m_fromVec);
			valueChangedCallback();
		});
	}
}
