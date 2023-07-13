using System;
using System.Collections.Generic;
using TextFx;
using UnityEngine;
using UnityEngine.UI;

public class ColourVariableSetting : BaseVariableSetting
{
	public Image m_fromValue;

	public Image m_toValue;

	public Image m_thenValue;

	public GameObject m_fromHighlight;

	public GameObject m_toHighlight;

	public GameObject m_thenHighlight;

	public GameObject m_toValueObject;

	public GameObject m_thenValueObject;

	public GameObject m_colourPickerSection;

	public HSVPicker m_colourPicker;

	public LayoutElement m_sectionLayoutElement;

	private bool m_colourSelected;

	private int m_currentColourIndex;

	private List<Action<Color>> m_stateListenerCallbacks;

	private Action m_valueChangedCallback;

	private Image[] m_colours;

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
		m_valueChangedCallback = valueChangedCallback;
		m_stateListenerCallbacks = new List<Action<Color>>();
		m_fromValue.color = varStateListeners[0].m_startColorValue;
		m_stateListenerCallbacks.Add(varStateListeners[0].m_onColorStateChangeCallback);
		if (varStateListeners.Count > 1)
		{
			m_toValue.color = varStateListeners[1].m_startColorValue;
			m_stateListenerCallbacks.Add(varStateListeners[1].m_onColorStateChangeCallback);
		}
		if (varStateListeners.Count > 2)
		{
			m_thenValue.color = varStateListeners[2].m_startColorValue;
			m_stateListenerCallbacks.Add(varStateListeners[2].m_onColorStateChangeCallback);
		}
		m_colourPicker.onValueChanged.AddListener(OnColourChanged);
		m_colours = new Image[3] { m_fromValue, m_toValue, m_thenValue };
	}

	public void ColourSelected(int index)
	{
		GameObject[] array = new GameObject[3] { m_fromHighlight, m_toHighlight, m_thenHighlight };
		m_colourSelected = !array[index].activeSelf;
		for (int i = 0; i < 3; i++)
		{
			if (i == index)
			{
				array[i].SetActive(m_colourSelected);
			}
			else
			{
				array[i].SetActive(false);
			}
		}
		if (m_colourSelected)
		{
			m_sectionLayoutElement.minHeight = 130f;
			Color color = m_colours[index].color;
			m_colourPickerSection.SetActive(true);
			m_colourPicker.AssignColor(color);
			m_currentColourIndex = index;
		}
		else
		{
			m_sectionLayoutElement.minHeight = 30f;
			m_colourPickerSection.SetActive(false);
		}
	}

	private void OnColourChanged(Color newColour)
	{
		m_colours[m_currentColourIndex].color = newColour;
		m_stateListenerCallbacks[m_currentColourIndex](newColour);
		if (m_valueChangedCallback != null)
		{
			m_valueChangedCallback();
		}
	}
}
