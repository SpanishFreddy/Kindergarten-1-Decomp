using System;
using System.Collections.Generic;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public abstract class ActionVariableProgression
	{
		[SerializeField]
		protected ActionVariableProgressionReferenceData m_reference_data;

		[SerializeField]
		protected ValueProgression m_progression;

		[SerializeField]
		protected int m_progression_idx = -1;

		[SerializeField]
		protected EasingEquation m_ease_type;

		[SerializeField]
		protected bool m_is_offset_from_last;

		[SerializeField]
		protected bool m_to_to_bool;

		[SerializeField]
		protected bool m_unique_randoms;

		[SerializeField]
		protected AnimatePerOptions m_animate_per;

		[SerializeField]
		protected bool m_override_animate_per_option;

		[SerializeField]
		protected AnimationCurve m_custom_ease_curve = new AnimationCurve();

		public ActionVariableProgressionReferenceData ReferenceData
		{
			get
			{
				return m_reference_data;
			}
		}

		public virtual string[] ProgressionExtraOptions
		{
			get
			{
				return null;
			}
		}

		public virtual int[] ProgressionExtraOptionIndexes
		{
			get
			{
				return null;
			}
		}

		public EasingEquation EaseType
		{
			get
			{
				return m_ease_type;
			}
		}

		public bool IsOffsetFromLast
		{
			get
			{
				return m_is_offset_from_last;
			}
			set
			{
				m_is_offset_from_last = value;
			}
		}

		public bool UsingThirdValue
		{
			get
			{
				return m_to_to_bool;
			}
		}

		public AnimatePerOptions AnimatePer
		{
			get
			{
				return m_animate_per;
			}
			set
			{
				m_animate_per = value;
			}
		}

		public bool OverrideAnimatePerOption
		{
			get
			{
				return m_override_animate_per_option;
			}
			set
			{
				m_override_animate_per_option = value;
			}
		}

		public AnimationCurve CustomEaseCurve
		{
			get
			{
				return m_custom_ease_curve;
			}
		}

		public virtual bool UniqueRandom
		{
			get
			{
				return Progression == 1 && m_unique_randoms;
			}
		}

		public bool UniqueRandomRaw
		{
			get
			{
				return m_unique_randoms;
			}
		}

		public int Progression
		{
			get
			{
				if (m_progression_idx == -1)
				{
					m_progression_idx = (int)m_progression;
				}
				return m_progression_idx;
			}
		}

		public void SetReferenceData(int actionIdx, ANIMATION_DATA_TYPE data_type, bool startState)
		{
			m_reference_data.m_action_index = actionIdx;
			m_reference_data.m_data_type = data_type;
			m_reference_data.m_start_state = startState;
		}

		public int GetProgressionIndex(AnimationProgressionVariables progression_variables, AnimatePerOptions animate_per_default, bool consider_white_space = false)
		{
			return progression_variables.GetValue((!m_override_animate_per_option) ? animate_per_default : m_animate_per, consider_white_space);
		}

		protected void ExportBaseData(ref tfxJSONObject json_data)
		{
			json_data["m_progression"] = Progression;
			json_data["m_ease_type"] = (double)m_ease_type;
			json_data["m_is_offset_from_last"] = m_is_offset_from_last;
			json_data["m_to_to_bool"] = m_to_to_bool;
			json_data["m_unique_randoms"] = m_unique_randoms;
			json_data["m_animate_per"] = (double)m_animate_per;
			json_data["m_override_animate_per_option"] = m_override_animate_per_option;
			if (Progression == 3)
			{
				json_data["m_custom_ease_curve"] = m_custom_ease_curve.ExportData();
			}
		}

		protected void ImportBaseData(tfxJSONObject json_data)
		{
			m_progression_idx = (int)json_data["m_progression"].Number;
			m_ease_type = (EasingEquation)json_data["m_ease_type"].Number;
			m_is_offset_from_last = json_data["m_is_offset_from_last"].Boolean;
			m_to_to_bool = json_data["m_to_to_bool"].Boolean;
			m_unique_randoms = json_data["m_unique_randoms"].Boolean;
			m_animate_per = (AnimatePerOptions)json_data["m_animate_per"].Number;
			m_override_animate_per_option = json_data["m_override_animate_per_option"].Boolean;
			if (json_data.ContainsKey("m_custom_ease_curve"))
			{
				m_custom_ease_curve = json_data["m_custom_ease_curve"].Array.JSONtoAnimationCurve();
			}
		}

		public abstract tfxJSONValue ExportData();

		public abstract void ImportData(tfxJSONObject json_data);

		public void ImportBaseLagacyData(KeyValuePair<string, string> value_pair)
		{
			switch (value_pair.Key)
			{
			case "m_progression":
				m_progression_idx = int.Parse(value_pair.Value);
				break;
			case "m_ease_type":
				m_ease_type = (EasingEquation)int.Parse(value_pair.Value);
				break;
			case "m_is_offset_from_last":
				m_is_offset_from_last = bool.Parse(value_pair.Value);
				break;
			case "m_to_to_bool":
				m_to_to_bool = bool.Parse(value_pair.Value);
				break;
			case "m_unique_randoms":
				m_unique_randoms = bool.Parse(value_pair.Value);
				break;
			case "m_animate_per":
				m_animate_per = (AnimatePerOptions)int.Parse(value_pair.Value);
				break;
			case "m_override_animate_per_option":
				m_override_animate_per_option = bool.Parse(value_pair.Value);
				break;
			case "m_custom_ease_curve":
				m_custom_ease_curve = value_pair.Value.ToAnimationCurve();
				break;
			}
		}
	}
}
