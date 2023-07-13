using System;
using System.Collections.Generic;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class ActionFloatProgression : ActionVariableProgression
	{
		[SerializeField]
		private float[] m_values;

		[SerializeField]
		private float m_from;

		[SerializeField]
		private float m_to;

		[SerializeField]
		private float m_to_to;

		public float ValueFrom
		{
			get
			{
				return m_from;
			}
		}

		public float ValueTo
		{
			get
			{
				return m_to;
			}
		}

		public float ValueThen
		{
			get
			{
				return m_to_to;
			}
		}

		public float[] Values
		{
			get
			{
				return m_values;
			}
			set
			{
				m_values = value;
			}
		}

		public ActionFloatProgression(float start_val)
		{
			m_from = start_val;
			m_to = start_val;
			m_to_to = start_val;
		}

		public void SetConstant(float constant_value)
		{
			m_progression_idx = 0;
			m_from = constant_value;
		}

		public void SetRandom(float random_min, float random_max, bool unique_randoms = false)
		{
			m_progression_idx = 1;
			m_from = random_min;
			m_to = random_max;
			m_unique_randoms = unique_randoms;
		}

		public void SetEased(float eased_from, float eased_to)
		{
			SetEased(eased_from, eased_to, m_ease_type);
		}

		public void SetEased(float eased_from, float eased_to, float eased_then)
		{
			SetEased(eased_from, eased_to, eased_then, m_ease_type);
		}

		public void SetEased(float eased_from, float eased_to, EasingEquation easing_function)
		{
			m_progression_idx = 2;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to_bool = false;
			m_ease_type = easing_function;
		}

		public void SetEased(float eased_from, float eased_to, float eased_then, EasingEquation easing_function)
		{
			m_progression_idx = 2;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to = eased_then;
			m_to_to_bool = true;
			m_ease_type = easing_function;
		}

		public void SetEasedCustom(float eased_from, float eased_to)
		{
			SetEasedCustom(eased_from, eased_to, m_custom_ease_curve);
		}

		public void SetEasedCustom(float eased_from, float eased_to, AnimationCurve easing_curve)
		{
			m_progression_idx = 3;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to_bool = false;
			m_custom_ease_curve = easing_curve;
		}

		public float GetValue(AnimationProgressionVariables progression_variables, AnimatePerOptions animate_per_default, bool consider_white_space = false)
		{
			return GetValue(GetProgressionIndex(progression_variables, animate_per_default, consider_white_space));
		}

		public float GetValue(int progression_idx)
		{
			int num = m_values.Length;
			if (num > 1 && progression_idx < num)
			{
				return m_values[progression_idx];
			}
			if (num == 1)
			{
				return m_values[0];
			}
			return 0f;
		}

		public void CalculateUniqueRandom(AnimationProgressionVariables progression_variables, AnimatePerOptions animate_per)
		{
			m_values[GetProgressionIndex(progression_variables, animate_per)] = m_from + (m_to - m_from) * UnityEngine.Random.value;
		}

		public void CalculateProgressions(int num_progressions)
		{
			m_values = new float[(base.Progression != 2 && base.Progression != 3 && base.Progression != 1) ? 1 : num_progressions];
			if (base.Progression == 1)
			{
				for (int i = 0; i < num_progressions; i++)
				{
					m_values[i] = m_from + (m_to - m_from) * UnityEngine.Random.value;
				}
			}
			else if (base.Progression == 2)
			{
				for (int j = 0; j < num_progressions; j++)
				{
					float num = ((num_progressions != 1) ? ((float)j / ((float)num_progressions - 1f)) : 0f);
					if (m_to_to_bool)
					{
						if (num <= 0.5f)
						{
							m_values[j] = m_from + (m_to - m_from) * EasingManager.GetEaseProgress(m_ease_type, num / 0.5f);
							continue;
						}
						num -= 0.5f;
						m_values[j] = m_to + (m_to_to - m_to) * EasingManager.GetEaseProgress(EasingManager.GetEaseTypeOpposite(m_ease_type), num / 0.5f);
					}
					else
					{
						m_values[j] = m_from + (m_to - m_from) * EasingManager.GetEaseProgress(m_ease_type, num);
					}
				}
			}
			else if (base.Progression == 3)
			{
				for (int k = 0; k < num_progressions; k++)
				{
					float time = ((num_progressions != 1) ? ((float)k / ((float)num_progressions - 1f)) : 0f);
					m_values[k] += m_from + (m_to - m_from) * m_custom_ease_curve.Evaluate(time);
				}
			}
			else if (base.Progression == 0)
			{
				m_values[0] = m_from;
			}
		}

		public ActionFloatProgression Clone()
		{
			ActionFloatProgression actionFloatProgression = new ActionFloatProgression(0f);
			actionFloatProgression.m_progression_idx = base.Progression;
			actionFloatProgression.m_ease_type = m_ease_type;
			actionFloatProgression.m_from = m_from;
			actionFloatProgression.m_to = m_to;
			actionFloatProgression.m_to_to = m_to_to;
			actionFloatProgression.m_to_to_bool = m_to_to_bool;
			actionFloatProgression.m_unique_randoms = m_unique_randoms;
			actionFloatProgression.m_override_animate_per_option = m_override_animate_per_option;
			actionFloatProgression.m_animate_per = m_animate_per;
			return actionFloatProgression;
		}

		public override tfxJSONValue ExportData()
		{
			tfxJSONObject json_data = new tfxJSONObject();
			ExportBaseData(ref json_data);
			json_data["m_from"] = m_from;
			json_data["m_to"] = m_to;
			json_data["m_to_to"] = m_to_to;
			return new tfxJSONValue(json_data);
		}

		public override void ImportData(tfxJSONObject json_data)
		{
			m_from = (float)json_data["m_from"].Number;
			m_to = (float)json_data["m_to"].Number;
			m_to_to = (float)json_data["m_to_to"].Number;
			ImportBaseData(json_data);
		}

		public void ImportLegacyData(string data_string)
		{
			List<object> list = data_string.StringToList(';', ':');
			foreach (object item in list)
			{
				KeyValuePair<string, string> value_pair = (KeyValuePair<string, string>)item;
				switch (value_pair.Key)
				{
				case "m_from":
					m_from = float.Parse(value_pair.Value);
					break;
				case "m_to":
					m_to = float.Parse(value_pair.Value);
					break;
				case "m_to_to":
					m_to_to = float.Parse(value_pair.Value);
					break;
				default:
					ImportBaseLagacyData(value_pair);
					break;
				}
			}
		}

		public List<PresetEffectSetting.VariableStateListener> GetStateListeners()
		{
			List<PresetEffectSetting.VariableStateListener> list = new List<PresetEffectSetting.VariableStateListener>();
			if (base.Progression == 0)
			{
				list.Add(new PresetEffectSetting.VariableStateListener
				{
					m_type = PresetEffectSetting.VariableStateListener.TYPE.FLOAT,
					m_startFloatValue = m_from,
					m_onFloatStateChangeCallback = delegate(float value)
					{
						SetConstant(value);
					}
				});
			}
			else
			{
				list.Add(new PresetEffectSetting.VariableStateListener
				{
					m_type = PresetEffectSetting.VariableStateListener.TYPE.FLOAT,
					m_startFloatValue = m_from,
					m_onFloatStateChangeCallback = delegate(float value)
					{
						if (base.Progression == 2)
						{
							if (m_to_to_bool)
							{
								SetEased(value, m_to, m_to_to);
							}
							else
							{
								SetEased(value, m_to);
							}
						}
						else if (base.Progression == 3)
						{
							SetEasedCustom(value, m_to);
						}
						else if (base.Progression == 1)
						{
							SetRandom(value, m_to);
						}
					}
				});
				list.Add(new PresetEffectSetting.VariableStateListener
				{
					m_type = PresetEffectSetting.VariableStateListener.TYPE.FLOAT,
					m_startFloatValue = m_to,
					m_onFloatStateChangeCallback = delegate(float value)
					{
						if (base.Progression == 2)
						{
							if (m_to_to_bool)
							{
								SetEased(m_from, value, m_to_to);
							}
							else
							{
								SetEased(m_from, value);
							}
						}
						else if (base.Progression == 3)
						{
							SetEasedCustom(m_from, value);
						}
						else if (base.Progression == 1)
						{
							SetRandom(m_from, value);
						}
					}
				});
				if (base.Progression == 2 && m_to_to_bool)
				{
					list.Add(new PresetEffectSetting.VariableStateListener
					{
						m_type = PresetEffectSetting.VariableStateListener.TYPE.FLOAT,
						m_startFloatValue = m_to_to,
						m_onFloatStateChangeCallback = delegate(float value)
						{
							SetEased(m_from, m_to, value);
						}
					});
				}
			}
			return list;
		}
	}
}
