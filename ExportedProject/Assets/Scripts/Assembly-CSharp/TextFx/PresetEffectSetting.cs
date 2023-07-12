using System;
using System.Collections.Generic;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class PresetEffectSetting
	{
		public class VariableStateListener
		{
			public enum TYPE
			{
				VECTOR3 = 0,
				COLOUR = 1,
				FLOAT = 2,
				BOOL = 3
			}

			public TYPE m_type;

			public Action<float> m_onFloatStateChangeCallback;

			public Action<Vector3> m_onVector3StateChangeCallback;

			public Action<Color> m_onColorStateChangeCallback;

			public Action<bool> m_onToggleStateChangeCallback;

			public float m_startFloatValue;

			public Vector3 m_startVector3Value = Vector3.zero;

			public Color m_startColorValue = Color.white;

			public bool m_startToggleValue;
		}

		public ANIMATION_DATA_TYPE m_data_type;

		public bool m_startState = true;

		public string m_setting_name;

		public int m_animation_idx;

		public int m_action_idx;

		public float m_action_progress_state_override = -1f;

		private const float LINE_HEIGHT = 20f;

		private ActionFloatProgression GetFloatVariable(ANIMATION_DATA_TYPE type, LetterAction letterAction)
		{
			switch (type)
			{
			case ANIMATION_DATA_TYPE.DURATION:
				return letterAction.m_duration_progression;
			case ANIMATION_DATA_TYPE.DELAY:
				return letterAction.m_delay_progression;
			default:
				return null;
			}
		}

		private ActionVector3Progression GetVector3Variable(ANIMATION_DATA_TYPE type, LetterAction letterAction)
		{
			switch (type)
			{
			case ANIMATION_DATA_TYPE.GLOBAL_ROTATION:
				return (!m_startState) ? letterAction.m_global_end_euler_rotation : letterAction.m_global_start_euler_rotation;
			case ANIMATION_DATA_TYPE.LOCAL_ROTATION:
				return (!m_startState) ? letterAction.m_end_euler_rotation : letterAction.m_start_euler_rotation;
			case ANIMATION_DATA_TYPE.POSITION:
				return (!m_startState) ? letterAction.m_end_pos : letterAction.m_start_pos;
			case ANIMATION_DATA_TYPE.GLOBAL_SCALE:
				return (!m_startState) ? letterAction.m_global_end_scale : letterAction.m_global_start_scale;
			case ANIMATION_DATA_TYPE.LOCAL_SCALE:
				return (!m_startState) ? letterAction.m_end_scale : letterAction.m_start_scale;
			default:
				return null;
			}
		}

		public List<VariableStateListener> GetSettingData(TextFxAnimationManager animation_manager, int action_start_offset = 0, int loop_start_offset = 0)
		{
			LetterAnimation animation = animation_manager.GetAnimation(m_animation_idx);
			LetterAction letterAction = ((animation == null) ? null : animation.GetAction(m_action_idx + action_start_offset));
			if (letterAction == null)
			{
				return null;
			}
			if (m_data_type == ANIMATION_DATA_TYPE.DELAY || m_data_type == ANIMATION_DATA_TYPE.DURATION)
			{
				return GetFloatVariable(m_data_type, letterAction).GetStateListeners();
			}
			if (m_data_type == ANIMATION_DATA_TYPE.POSITION || m_data_type == ANIMATION_DATA_TYPE.GLOBAL_SCALE || m_data_type == ANIMATION_DATA_TYPE.LOCAL_SCALE || m_data_type == ANIMATION_DATA_TYPE.GLOBAL_ROTATION || m_data_type == ANIMATION_DATA_TYPE.LOCAL_ROTATION)
			{
				return GetVector3Variable(m_data_type, letterAction).GetStateListeners();
			}
			if (m_data_type == ANIMATION_DATA_TYPE.COLOUR)
			{
				return (!m_startState) ? letterAction.m_end_colour.GetStateListeners() : letterAction.m_start_colour.GetStateListeners();
			}
			if (m_data_type == ANIMATION_DATA_TYPE.DELAY_EASED_RANDOM_SWITCH)
			{
				List<VariableStateListener> list = new List<VariableStateListener>();
				list.Add(new VariableStateListener
				{
					m_startToggleValue = (letterAction.m_delay_progression.Progression == 1),
					m_onToggleStateChangeCallback = delegate(bool state)
					{
						if (state)
						{
							letterAction.m_delay_progression.SetRandom(letterAction.m_delay_progression.ValueFrom, letterAction.m_delay_progression.ValueTo, letterAction.m_delay_progression.UniqueRandomRaw);
						}
						else
						{
							letterAction.m_delay_progression.SetEased(letterAction.m_delay_progression.ValueFrom, letterAction.m_delay_progression.ValueTo);
						}
						animation_manager.PrepareAnimationData(ANIMATION_DATA_TYPE.DELAY);
					}
				});
				return list;
			}
			return null;
		}

		public tfxJSONValue ExportData()
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["name"] = m_setting_name;
			tfxJSONObject["data_type"] = (double)m_data_type;
			tfxJSONObject["anim_idx"] = m_animation_idx;
			tfxJSONObject["action_idx"] = m_action_idx;
			tfxJSONObject["startState"] = m_startState;
			return new tfxJSONValue(tfxJSONObject);
		}

		public void ImportData(tfxJSONObject jsonData)
		{
			m_setting_name = jsonData["name"].Str;
			m_data_type = (ANIMATION_DATA_TYPE)jsonData["data_type"].Number;
			m_animation_idx = (int)jsonData["anim_idx"].Number;
			m_action_idx = (int)jsonData["action_idx"].Number;
			if (jsonData.ContainsKey("startState"))
			{
				m_startState = jsonData["startState"].Boolean;
			}
		}
	}
}
