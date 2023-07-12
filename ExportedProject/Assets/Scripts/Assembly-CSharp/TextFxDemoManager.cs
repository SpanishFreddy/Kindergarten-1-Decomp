using System.Collections;
using System.Collections.Generic;
using TextFx;
using UnityEngine;
using UnityEngine.UI;

public class TextFxDemoManager : MonoBehaviour
{
	public TextFxUGUI m_titleEffect;

	public RectTransform m_animEditorContent;

	public TextFxUGUI m_effect;

	private readonly Color SECTION_INACTIVE_COLOUR = new Color(1f, 1f, 1f, 20f / 51f);

	private readonly Color SECTION_ACTIVE_COLOUR = new Color(0.75686276f, 0.96862745f, 1f, 1f);

	private readonly Color SECTION_SELECTED_COLOUR = new Color(22f / 85f, 0.8f, 0.8745098f, 1f);

	public Image[] m_sectionHeaderImages;

	public RectTransform m_menuSectionsContainer;

	public RectTransform[] m_animSectionTransforms;

	public Dropdown[] m_animSectionDropdowns;

	public Button m_playButton;

	public Text m_playButtonText;

	public Button m_resetButton;

	public Button m_continueButton;

	public FloatVariableSetting m_floatVariableSettingPrefab;

	public Vector3VariableSetting m_vector3VariableSettingPrefab;

	public ToggleVariableSetting m_toggleVariableSettingPrefab;

	public ColourVariableSetting m_colourVariableSettingPrefab;

	public InputField m_textInput;

	public int m_introStartEffect = -1;

	public int m_mainStartEffect = -1;

	public int m_outroStartEffect = -1;

	public bool m_skipTitleEffectIntro;

	private TextFxAnimationManager.PresetAnimationSection[] m_animationSections;

	private ObjectPool<FloatVariableSetting> m_floatVariableSettingsPool;

	private ObjectPool<Vector3VariableSetting> m_vector3VariableSettingsPool;

	private ObjectPool<ToggleVariableSetting> m_toggleVariableSettingsPool;

	private ObjectPool<ColourVariableSetting> m_colourVariableSettingsPool;

	private List<FloatVariableSetting>[] m_floatVariableSettingsInUse;

	private List<Vector3VariableSetting>[] m_vector3VariableSettingsInUse;

	private List<ToggleVariableSetting>[] m_toggleVariableSettingsInUse;

	private List<ColourVariableSetting>[] m_colourVariableSettingsInUse;

	private bool[] m_sectionsActive;

	private int m_activeSectionIndex = -1;

	private Color m_playbackButtonHighlightColour = new Color(20f / 51f, 1f, 0.5882353f);

	private void Start()
	{
		List<string[]> list = new List<string[]>();
		list.Add(TextFxQuickSetupAnimConfigs.IntroAnimNames);
		list.Add(TextFxQuickSetupAnimConfigs.MainAnimNames);
		list.Add(TextFxQuickSetupAnimConfigs.OutroAnimNames);
		List<string[]> list2 = list;
		int num = Mathf.Clamp(m_introStartEffect, 1, list2[0].Length);
		int num2 = Mathf.Clamp(m_mainStartEffect, 1, list2[1].Length);
		int num3 = Mathf.Clamp(m_outroStartEffect, 1, list2[2].Length);
		if (m_introStartEffect < 1)
		{
			do
			{
				num = Random.Range(1, list2[0].Length);
			}
			while (list2[0][num] == "Epic" || list2[0][num] == "Mental House" || list2[0][num] == "Type Writer");
		}
		if (m_mainStartEffect < 1)
		{
			do
			{
				num2 = Random.Range(1, list2[1].Length);
			}
			while (list2[1][num2] == "Windy");
		}
		if (m_outroStartEffect < 1)
		{
			do
			{
				num3 = Random.Range(1, list2[2].Length);
			}
			while (list2[2][num3] == "Shot Drop");
		}
		int[] array = new int[3] { num, num2, num3 };
		for (int i = 0; i < 3; i++)
		{
			List<Dropdown.OptionData> list3 = new List<Dropdown.OptionData>();
			string[] array2 = list2[i];
			foreach (string text in array2)
			{
				list3.Add(new Dropdown.OptionData(text));
			}
			m_animSectionDropdowns[i].options = list3;
			TextFxAnimationManager.PRESET_ANIMATION_SECTION animSection = (TextFxAnimationManager.PRESET_ANIMATION_SECTION)i;
			m_animSectionDropdowns[i].value = array[i];
			m_animSectionDropdowns[i].onValueChanged.AddListener(delegate(int index)
			{
				AnimationSectionSet(animSection, index, true);
			});
			m_sectionHeaderImages[i].color = SECTION_INACTIVE_COLOUR;
		}
		m_animationSections = new TextFxAnimationManager.PresetAnimationSection[3]
		{
			m_effect.AnimationManager.m_preset_intro,
			m_effect.AnimationManager.m_preset_main,
			m_effect.AnimationManager.m_preset_outro
		};
		m_floatVariableSettingsPool = new ObjectPool<FloatVariableSetting>(m_floatVariableSettingPrefab.gameObject, 5, string.Empty);
		m_vector3VariableSettingsPool = new ObjectPool<Vector3VariableSetting>(m_vector3VariableSettingPrefab.gameObject, 5, string.Empty);
		m_toggleVariableSettingsPool = new ObjectPool<ToggleVariableSetting>(m_toggleVariableSettingPrefab.gameObject, 5, string.Empty);
		m_colourVariableSettingsPool = new ObjectPool<ColourVariableSetting>(m_colourVariableSettingPrefab.gameObject, 5, string.Empty);
		m_floatVariableSettingsInUse = new List<FloatVariableSetting>[3];
		m_vector3VariableSettingsInUse = new List<Vector3VariableSetting>[3];
		m_toggleVariableSettingsInUse = new List<ToggleVariableSetting>[3];
		m_colourVariableSettingsInUse = new List<ColourVariableSetting>[3];
		m_playButton.onClick.AddListener(delegate
		{
			if (!m_effect.AnimationManager.Playing)
			{
				PlayAnimation();
			}
			else if (m_effect.AnimationManager.Paused)
			{
				m_effect.AnimationManager.Paused = false;
				m_playButtonText.text = "Pause";
			}
			else
			{
				m_effect.AnimationManager.Paused = true;
				m_playButtonText.text = "Play";
			}
		});
		m_resetButton.onClick.AddListener(delegate
		{
			m_effect.AnimationManager.ResetAnimation();
			m_playButtonText.text = "Play";
			HideContinueButton();
		});
		m_continueButton.onClick.AddListener(delegate
		{
			if (!m_effect.AnimationManager.Paused)
			{
				LetterAnimation animation = m_effect.AnimationManager.GetAnimation(0);
				if (animation != null && (animation.CurrentAnimationState == LETTER_ANIMATION_STATE.WAITING || animation.CurrentAnimationState == LETTER_ANIMATION_STATE.WAITING_INFINITE))
				{
					m_effect.AnimationManager.ContinuePastBreak();
				}
				else
				{
					m_effect.AnimationManager.ContinuePastLoop();
				}
			}
		});
		m_continueButton.gameObject.SetActive(false);
		m_sectionsActive = new bool[3];
		m_textInput.text = "Hello World!";
		m_textInput.onValueChanged.AddListener(delegate(string new_string)
		{
			m_effect.SetText(new_string);
		});
		AnimationSectionSet(TextFxAnimationManager.PRESET_ANIMATION_SECTION.INTRO, num);
		AnimationSectionSet(TextFxAnimationManager.PRESET_ANIMATION_SECTION.MAIN, num2);
		AnimationSectionSet(TextFxAnimationManager.PRESET_ANIMATION_SECTION.OUTRO, num3);
		AnimationSectionSelected(0);
		StartCoroutine(DoIntro(6f, m_skipTitleEffectIntro));
	}

	private IEnumerator DoIntro(float waitTime, bool skipTitleEffect = false)
	{
		yield return false;
		m_animEditorContent.gameObject.SetActive(false);
		m_titleEffect.GameObject.SetActive(true);
		m_titleEffect.AnimationManager.PlayAnimation(1f);
		if (!skipTitleEffect)
		{
			yield return new WaitForSeconds(waitTime);
		}
		m_titleEffect.GameObject.SetActive(false);
		float timer = 0f;
		Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
		Vector3 rotationVec = new Vector3(0f, 90f, 0f);
		m_animEditorContent.rotation = Quaternion.Euler(0f, 90f, 0f);
		m_animEditorContent.gameObject.SetActive(true);
		while (timer <= 0.5f)
		{
			rotationVec.y = Mathf.Lerp(90f, 0f, timer / 0.5f);
			rotation.eulerAngles = rotationVec;
			m_animEditorContent.rotation = rotation;
			timer += Time.deltaTime;
			yield return false;
		}
		rotationVec.y = 0f;
		rotation.eulerAngles = rotationVec;
		m_animEditorContent.rotation = rotation;
		PlayAnimation(0.5f);
	}

	private void PlayAnimation(float delay = 0f)
	{
		m_effect.AnimationManager.PlayAnimation(delay, AnimationFinished);
		m_playButtonText.text = "Pause";
		ShowContinueButton();
		StartCoroutine("HighlightContinueButton");
	}

	private void ShowContinueButton()
	{
		m_continueButton.image.color = Color.white;
		m_continueButton.gameObject.SetActive(true);
		m_playButton.image.color = Color.white;
	}

	private void HideContinueButton()
	{
		m_continueButton.gameObject.SetActive(false);
		StopCoroutine("HighlightContinueButton");
		m_playButton.image.color = m_playbackButtonHighlightColour;
	}

	private IEnumerator HighlightContinueButton()
	{
		yield return new WaitForSeconds(4f);
		m_continueButton.image.color = m_playbackButtonHighlightColour;
	}

	private void AnimationFinished()
	{
		m_playButtonText.text = "Play";
		HideContinueButton();
	}

	private void RecycleSectionSettingComponents(int sectionIndex)
	{
		if (m_floatVariableSettingsInUse[sectionIndex] != null)
		{
			foreach (FloatVariableSetting item in m_floatVariableSettingsInUse[sectionIndex])
			{
				m_floatVariableSettingsPool.Recycle(item);
			}
		}
		if (m_vector3VariableSettingsInUse[sectionIndex] != null)
		{
			foreach (Vector3VariableSetting item2 in m_vector3VariableSettingsInUse[sectionIndex])
			{
				m_vector3VariableSettingsPool.Recycle(item2);
			}
		}
		if (m_toggleVariableSettingsInUse[sectionIndex] != null)
		{
			foreach (ToggleVariableSetting item3 in m_toggleVariableSettingsInUse[sectionIndex])
			{
				m_toggleVariableSettingsPool.Recycle(item3);
			}
		}
		if (m_colourVariableSettingsInUse[sectionIndex] == null)
		{
			return;
		}
		foreach (ColourVariableSetting item4 in m_colourVariableSettingsInUse[sectionIndex])
		{
			m_colourVariableSettingsPool.Recycle(item4);
		}
	}

	public void AnimationSectionSelected(int index)
	{
		if (m_sectionsActive[index] && m_activeSectionIndex != index)
		{
			if (m_activeSectionIndex >= 0)
			{
				m_sectionHeaderImages[m_activeSectionIndex].color = SECTION_ACTIVE_COLOUR;
				CloseAnimationSection(m_activeSectionIndex);
			}
			m_sectionHeaderImages[index].color = SECTION_SELECTED_COLOUR;
			m_activeSectionIndex = index;
			OpenAnimationSection(m_activeSectionIndex);
		}
	}

	private void CloseAnimationSection(int sectionIndex)
	{
		foreach (FloatVariableSetting item in m_floatVariableSettingsInUse[sectionIndex])
		{
			item.gameObject.SetActive(false);
		}
		foreach (Vector3VariableSetting item2 in m_vector3VariableSettingsInUse[sectionIndex])
		{
			item2.gameObject.SetActive(false);
		}
		foreach (ToggleVariableSetting item3 in m_toggleVariableSettingsInUse[sectionIndex])
		{
			item3.gameObject.SetActive(false);
		}
		foreach (ColourVariableSetting item4 in m_colourVariableSettingsInUse[sectionIndex])
		{
			item4.gameObject.SetActive(false);
		}
	}

	private void OpenAnimationSection(int sectionIndex)
	{
		foreach (FloatVariableSetting item in m_floatVariableSettingsInUse[sectionIndex])
		{
			item.gameObject.SetActive(!item.IsSubSetting || item.SubSettingActive);
		}
		foreach (Vector3VariableSetting item2 in m_vector3VariableSettingsInUse[sectionIndex])
		{
			item2.gameObject.SetActive(!item2.IsSubSetting || item2.SubSettingActive);
		}
		foreach (ToggleVariableSetting item3 in m_toggleVariableSettingsInUse[sectionIndex])
		{
			item3.gameObject.SetActive(!item3.IsSubSetting || item3.SubSettingActive);
		}
		foreach (ColourVariableSetting item4 in m_colourVariableSettingsInUse[sectionIndex])
		{
			item4.gameObject.SetActive(!item4.IsSubSetting || item4.SubSettingActive);
		}
	}

	private void AnimationSectionSet(TextFxAnimationManager.PRESET_ANIMATION_SECTION section, int animIndex, bool restartAnimation = false)
	{
		if (m_activeSectionIndex >= 0 && m_activeSectionIndex != (int)section)
		{
			CloseAnimationSection(m_activeSectionIndex);
		}
		RecycleSectionSettingComponents((int)section);
		m_floatVariableSettingsInUse[(int)section] = new List<FloatVariableSetting>();
		m_vector3VariableSettingsInUse[(int)section] = new List<Vector3VariableSetting>();
		m_toggleVariableSettingsInUse[(int)section] = new List<ToggleVariableSetting>();
		m_colourVariableSettingsInUse[(int)section] = new List<ColourVariableSetting>();
		m_effect.AnimationManager.SetQuickSetupSection(section, m_animSectionDropdowns[(int)section].options[animIndex].text, false, false);
		if (m_activeSectionIndex >= 0)
		{
			m_sectionHeaderImages[m_activeSectionIndex].color = SECTION_ACTIVE_COLOUR;
		}
		if (animIndex > 0)
		{
			m_sectionHeaderImages[(int)section].color = SECTION_SELECTED_COLOUR;
			m_activeSectionIndex = (int)section;
			m_sectionsActive[m_activeSectionIndex] = true;
		}
		else
		{
			m_sectionHeaderImages[(int)section].color = SECTION_INACTIVE_COLOUR;
			m_sectionsActive[(int)section] = false;
			if (m_activeSectionIndex >= 0 && m_activeSectionIndex == (int)section)
			{
				m_activeSectionIndex = -1;
			}
		}
		TextFxAnimationManager.PresetAnimationSection animation_section = m_animationSections[(int)section];
		int siblingIndex = m_animSectionTransforms[(int)section].GetSiblingIndex();
		int num = 0;
		foreach (PresetEffectSetting preset_effect_setting in animation_section.m_preset_effect_settings)
		{
			RectTransform rectTransform = null;
			ANIMATION_DATA_TYPE data_type = preset_effect_setting.m_data_type;
			switch (data_type)
			{
			case ANIMATION_DATA_TYPE.DELAY:
			case ANIMATION_DATA_TYPE.DURATION:
			{
				FloatVariableSetting object4 = m_floatVariableSettingsPool.GetObject();
				m_floatVariableSettingsInUse[(int)section].Add(object4);
				object4.transform.SetParent(m_menuSectionsContainer);
				ANIMATION_DATA_TYPE dataType = preset_effect_setting.m_data_type;
				object4.Setup(preset_effect_setting, preset_effect_setting.GetSettingData(m_effect.AnimationManager, animation_section.m_start_action, animation_section.m_start_loop), delegate
				{
					m_effect.AnimationManager.PrepareAnimationData(dataType);
					if (dataType == ANIMATION_DATA_TYPE.DURATION)
					{
						m_effect.AnimationManager.PrepareAnimationData(ANIMATION_DATA_TYPE.DELAY);
					}
				}, false);
				rectTransform = object4.transform as RectTransform;
				break;
			}
			case ANIMATION_DATA_TYPE.POSITION:
			case ANIMATION_DATA_TYPE.LOCAL_ROTATION:
			case ANIMATION_DATA_TYPE.GLOBAL_ROTATION:
			case ANIMATION_DATA_TYPE.LOCAL_SCALE:
			case ANIMATION_DATA_TYPE.GLOBAL_SCALE:
			{
				Vector3VariableSetting object3 = m_vector3VariableSettingsPool.GetObject();
				m_vector3VariableSettingsInUse[(int)section].Add(object3);
				object3.transform.SetParent(m_menuSectionsContainer);
				ANIMATION_DATA_TYPE vecDataType = preset_effect_setting.m_data_type;
				object3.Setup(preset_effect_setting, preset_effect_setting.GetSettingData(m_effect.AnimationManager, animation_section.m_start_action, animation_section.m_start_loop), delegate
				{
					m_effect.AnimationManager.PrepareAnimationData(vecDataType);
				}, false);
				rectTransform = object3.transform as RectTransform;
				break;
			}
			case ANIMATION_DATA_TYPE.COLOUR:
			{
				ColourVariableSetting object2 = m_colourVariableSettingsPool.GetObject();
				m_colourVariableSettingsInUse[(int)section].Add(object2);
				object2.transform.SetParent(m_menuSectionsContainer);
				rectTransform = object2.transform as RectTransform;
				ANIMATION_DATA_TYPE colDataType = ANIMATION_DATA_TYPE.COLOUR;
				object2.Setup(preset_effect_setting, preset_effect_setting.GetSettingData(m_effect.AnimationManager, animation_section.m_start_action, animation_section.m_start_loop), delegate
				{
					m_effect.AnimationManager.PrepareAnimationData(colDataType);
				}, false);
				break;
			}
			case ANIMATION_DATA_TYPE.DELAY_EASED_RANDOM_SWITCH:
			{
				ToggleVariableSetting @object = m_toggleVariableSettingsPool.GetObject();
				m_toggleVariableSettingsInUse[(int)section].Add(@object);
				@object.transform.SetParent(m_menuSectionsContainer);
				preset_effect_setting.m_setting_name = "Randomised?";
				@object.Setup(preset_effect_setting, preset_effect_setting.GetSettingData(m_effect.AnimationManager, animation_section.m_start_action, animation_section.m_start_loop), null, false);
				rectTransform = @object.transform as RectTransform;
				break;
			}
			default:
				num--;
				break;
			}
			if (rectTransform != null)
			{
				rectTransform.SetSiblingIndex(siblingIndex + 1 + num);
				rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0f);
				rectTransform.localScale = Vector3.one;
			}
			num++;
		}
		if (animIndex > 0)
		{
			ToggleVariableSetting object5 = m_toggleVariableSettingsPool.GetObject();
			m_toggleVariableSettingsInUse[(int)section].Add(object5);
			FloatVariableSetting floatVarSetting = m_floatVariableSettingsPool.GetObject();
			m_floatVariableSettingsInUse[(int)section].Add(floatVarSetting);
			object5.transform.SetParent(m_menuSectionsContainer);
			RectTransform rectTransform2 = object5.transform as RectTransform;
			rectTransform2.SetSiblingIndex(siblingIndex + 1 + num);
			rectTransform2.localPosition = new Vector3(rectTransform2.localPosition.x, rectTransform2.localPosition.y, 0f);
			rectTransform2.localScale = Vector3.one;
			List<PresetEffectSetting.VariableStateListener> list = new List<PresetEffectSetting.VariableStateListener>();
			list.Add(new PresetEffectSetting.VariableStateListener
			{
				m_startToggleValue = animation_section.m_exit_pause,
				m_onToggleStateChangeCallback = delegate(bool state)
				{
					animation_section.SetExitPauseState(m_effect.AnimationManager, state);
					floatVarSetting.SubSettingSetActive(state);
				}
			});
			List<PresetEffectSetting.VariableStateListener> varStateListeners = list;
			object5.Setup("Exit Delay", varStateListeners, null, false);
			num++;
			floatVarSetting.transform.SetParent(m_menuSectionsContainer);
			rectTransform2 = floatVarSetting.transform as RectTransform;
			rectTransform2.SetSiblingIndex(siblingIndex + 1 + num);
			rectTransform2.localPosition = new Vector3(rectTransform2.localPosition.x, rectTransform2.localPosition.y, 0f);
			rectTransform2.localScale = Vector3.one;
			floatVarSetting.Setup("Duration", new List<PresetEffectSetting.VariableStateListener>
			{
				new PresetEffectSetting.VariableStateListener
				{
					m_startFloatValue = animation_section.m_exit_pause_duration,
					m_onFloatStateChangeCallback = delegate(float fVal)
					{
						animation_section.SetExitPauseDuration(m_effect.AnimationManager, fVal);
					}
				}
			}, null, true);
			floatVarSetting.SubSettingSetActive(animation_section.m_exit_pause);
		}
		m_playButtonText.text = "Play";
		HideContinueButton();
		if (restartAnimation)
		{
			PlayAnimation();
		}
	}
}
