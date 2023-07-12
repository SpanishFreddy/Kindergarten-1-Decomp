using System;
using System.Collections.Generic;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class ActionVector3Progression : ActionVariableProgression
	{
		[SerializeField]
		protected Vector3[] m_values;

		[SerializeField]
		protected Vector3 m_from = Vector3.zero;

		[SerializeField]
		protected Vector3 m_to = Vector3.zero;

		[SerializeField]
		protected Vector3 m_to_to = Vector3.zero;

		[SerializeField]
		protected bool m_ease_curve_per_axis;

		[SerializeField]
		protected AnimationCurve m_custom_ease_curve_y = new AnimationCurve();

		[SerializeField]
		protected AnimationCurve m_custom_ease_curve_z = new AnimationCurve();

		[SerializeField]
		protected PROGRESSION_VALUE_STATE m_value_state;

		[SerializeField]
		protected ActionVariableProgressionReferenceData m_offset_progression;

		public Vector3 ValueFrom
		{
			get
			{
				return m_from;
			}
		}

		public Vector3 ValueTo
		{
			get
			{
				return m_to;
			}
		}

		public Vector3 ValueThen
		{
			get
			{
				return m_to_to;
			}
		}

		public Vector3[] Values
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

		public bool EaseCurvePerAxis
		{
			get
			{
				return m_ease_curve_per_axis;
			}
		}

		public AnimationCurve CustomEaseCurveY
		{
			get
			{
				return m_custom_ease_curve_y;
			}
		}

		public AnimationCurve CustomEaseCurveZ
		{
			get
			{
				return m_custom_ease_curve_z;
			}
		}

		public override bool UniqueRandom
		{
			get
			{
				return base.Progression == 1 && m_unique_randoms && m_value_state != PROGRESSION_VALUE_STATE.REFERENCE;
			}
		}

		public ActionVector3Progression()
		{
		}

		public ActionVector3Progression(Vector3 start_vec)
		{
			m_from = start_vec;
			m_to = start_vec;
			m_to_to = start_vec;
		}

		public ActionVariableProgressionReferenceData GetOffsetReference()
		{
			if (m_value_state == PROGRESSION_VALUE_STATE.UNIQUE)
			{
				return m_reference_data;
			}
			return m_offset_progression;
		}

		public void SetValueReference(ActionVector3Progression progression)
		{
			m_value_state = PROGRESSION_VALUE_STATE.REFERENCE;
			m_offset_progression = progression.ReferenceData;
		}

		public void SetConstant(Vector3 constant_value)
		{
			m_progression_idx = 0;
			m_from = constant_value;
		}

		public void SetRandom(Vector3 random_min, Vector3 random_max, bool unique_randoms = false)
		{
			m_progression_idx = 1;
			m_from = random_min;
			m_to = random_max;
			m_unique_randoms = unique_randoms;
		}

		public void SetEased(Vector3 eased_from, Vector3 eased_to)
		{
			SetEased(eased_from, eased_to, m_ease_type);
		}

		public void SetEased(Vector3 eased_from, Vector3 eased_to, Vector3 eased_then)
		{
			SetEased(eased_from, eased_to, eased_then, m_ease_type);
		}

		public void SetEased(Vector3 eased_from, Vector3 eased_to, EasingEquation easing_function)
		{
			m_progression_idx = 2;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to_bool = false;
			m_ease_type = easing_function;
		}

		public void SetEased(Vector3 eased_from, Vector3 eased_to, Vector3 eased_then, EasingEquation easing_function)
		{
			m_progression_idx = 2;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to = eased_then;
			m_to_to_bool = true;
			m_ease_type = easing_function;
		}

		public void SetEasedCustom(Vector3 eased_from, Vector3 eased_to)
		{
			m_progression_idx = 3;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to_bool = false;
		}

		public void SetEasedCustom(Vector3 eased_from, Vector3 eased_to, AnimationCurve easing_curve)
		{
			m_progression_idx = 3;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to_bool = false;
			m_ease_curve_per_axis = false;
			m_custom_ease_curve = easing_curve;
		}

		public void SetEasedCustom(Vector3 eased_from, Vector3 eased_to, AnimationCurve easing_curve_x, AnimationCurve easing_curve_y, AnimationCurve easing_curve_z)
		{
			m_progression_idx = 3;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to_bool = false;
			m_ease_curve_per_axis = true;
			m_custom_ease_curve = easing_curve_x;
			m_custom_ease_curve_y = easing_curve_y;
			m_custom_ease_curve_z = easing_curve_z;
		}

		public Vector3 GetValue(List<LetterAction> all_letter_actions, AnimationProgressionVariables progression_variables, AnimatePerOptions animate_per_default, bool consider_white_space = false)
		{
			int progressionIndex = GetProgressionIndex(progression_variables, animate_per_default, consider_white_space);
			return GetValue(all_letter_actions, progressionIndex);
		}

		private Vector3 GetValue(List<LetterAction> all_letter_actions, int progression_idx)
		{
			Vector3 result = Vector3.zero;
			if ((m_value_state == PROGRESSION_VALUE_STATE.OFFSET_FROM_REFERENCE || m_value_state == PROGRESSION_VALUE_STATE.REFERENCE) && all_letter_actions != null)
			{
				ActionVector3Progression vector3Prog = m_offset_progression.GetVector3Prog(all_letter_actions);
				if (vector3Prog.m_reference_data.m_action_index != m_reference_data.m_action_index || vector3Prog.m_reference_data.m_start_state != m_reference_data.m_start_state)
				{
					result = vector3Prog.GetValue(all_letter_actions, progression_idx);
				}
			}
			if (m_value_state == PROGRESSION_VALUE_STATE.OFFSET_FROM_REFERENCE || m_value_state == PROGRESSION_VALUE_STATE.UNIQUE)
			{
				int num = m_values.Length;
				if (num > 1 && progression_idx < num && progression_idx >= 0)
				{
					result += m_values[progression_idx];
				}
				else if (num == 1)
				{
					result += m_values[0];
				}
				else
				{
					result += Vector3.zero;
				}
			}
			return result;
		}

		public void CalculateUniqueRandom(AnimationProgressionVariables progression_variables, AnimatePerOptions animate_per, Vector3[] offset_vec)
		{
			int progressionIndex = GetProgressionIndex(progression_variables, animate_per);
			bool flag = offset_vec != null && offset_vec.Length == 1;
			m_values[progressionIndex] = ((!m_is_offset_from_last) ? Vector3.zero : offset_vec[(!flag) ? progressionIndex : 0]);
			m_values[progressionIndex] += new Vector3(m_from.x + (m_to.x - m_from.x) * UnityEngine.Random.value, m_from.y + (m_to.y - m_from.y) * UnityEngine.Random.value, m_from.z + (m_to.z - m_from.z) * UnityEngine.Random.value);
		}

		public void CalculateRotationProgressions(int num_progressions, ActionVector3Progression offset_prog, bool variableActive = true)
		{
			CalculateProgressions(num_progressions, offset_prog, variableActive);
		}

		public virtual void CalculateProgressions(int num_progressions, ActionVector3Progression offset_prog, bool variableActive = true)
		{
			if (!variableActive)
			{
				SetValueReference(offset_prog);
				return;
			}
			if (m_is_offset_from_last && offset_prog != null)
			{
				m_value_state = PROGRESSION_VALUE_STATE.OFFSET_FROM_REFERENCE;
				m_offset_progression = offset_prog.GetOffsetReference();
			}
			else
			{
				m_value_state = PROGRESSION_VALUE_STATE.UNIQUE;
			}
			m_values = new Vector3[(base.Progression != 2 && base.Progression != 3 && base.Progression != 1) ? 1 : num_progressions];
			if (base.Progression == 1)
			{
				for (int i = 0; i < num_progressions; i++)
				{
					m_values[i] = new Vector3(m_from.x + (m_to.x - m_from.x) * UnityEngine.Random.value, m_from.y + (m_to.y - m_from.y) * UnityEngine.Random.value, m_from.z + (m_to.z - m_from.z) * UnityEngine.Random.value);
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
					if (m_ease_curve_per_axis)
					{
						m_values[k].x = m_from.x + (m_to.x - m_from.x) * m_custom_ease_curve.Evaluate(time);
						m_values[k].y = m_from.y + (m_to.y - m_from.y) * m_custom_ease_curve_y.Evaluate(time);
						m_values[k].z = m_from.z + (m_to.z - m_from.z) * m_custom_ease_curve_z.Evaluate(time);
					}
					else
					{
						m_values[k] = m_from + (m_to - m_from) * m_custom_ease_curve.Evaluate(time);
					}
				}
			}
			else if (base.Progression == 0)
			{
				for (int l = 0; l < m_values.Length; l++)
				{
					m_values[l] = m_from;
				}
			}
		}

		public ActionVector3Progression Clone()
		{
			ActionVector3Progression actionVector3Progression = new ActionVector3Progression(Vector3.zero);
			actionVector3Progression.m_progression_idx = base.Progression;
			actionVector3Progression.m_ease_type = m_ease_type;
			actionVector3Progression.m_from = m_from;
			actionVector3Progression.m_to = m_to;
			actionVector3Progression.m_to_to = m_to_to;
			actionVector3Progression.m_to_to_bool = m_to_to_bool;
			actionVector3Progression.m_is_offset_from_last = m_is_offset_from_last;
			actionVector3Progression.m_unique_randoms = m_unique_randoms;
			actionVector3Progression.m_override_animate_per_option = m_override_animate_per_option;
			actionVector3Progression.m_animate_per = m_animate_per;
			actionVector3Progression.m_ease_curve_per_axis = m_ease_curve_per_axis;
			actionVector3Progression.m_custom_ease_curve = new AnimationCurve(m_custom_ease_curve.keys);
			actionVector3Progression.m_custom_ease_curve_y = new AnimationCurve(m_custom_ease_curve_y.keys);
			actionVector3Progression.m_custom_ease_curve_z = new AnimationCurve(m_custom_ease_curve_z.keys);
			return actionVector3Progression;
		}

		public override tfxJSONValue ExportData()
		{
			tfxJSONObject json_data = new tfxJSONObject();
			ExportBaseData(ref json_data);
			json_data["m_from"] = m_from.ExportData();
			json_data["m_to"] = m_to.ExportData();
			json_data["m_to_to"] = m_to_to.ExportData();
			json_data["m_ease_curve_per_axis"] = m_ease_curve_per_axis;
			if (base.Progression == 3 && m_ease_curve_per_axis)
			{
				json_data["m_custom_ease_curve_y"] = m_custom_ease_curve_y.ExportData();
				json_data["m_custom_ease_curve_z"] = m_custom_ease_curve_z.ExportData();
			}
			return new tfxJSONValue(json_data);
		}

		public override void ImportData(tfxJSONObject json_data)
		{
			m_from = json_data["m_from"].Obj.JSONtoVector3();
			m_to = json_data["m_to"].Obj.JSONtoVector3();
			m_to_to = json_data["m_to_to"].Obj.JSONtoVector3();
			m_ease_curve_per_axis = json_data["m_ease_curve_per_axis"].Boolean;
			if (json_data.ContainsKey("m_custom_ease_curve_y"))
			{
				m_custom_ease_curve_y = json_data["m_custom_ease_curve_y"].Array.JSONtoAnimationCurve();
				m_custom_ease_curve_z = json_data["m_custom_ease_curve_z"].Array.JSONtoAnimationCurve();
			}
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
					m_from = value_pair.Value.StringToVector3('|', '<');
					break;
				case "m_to":
					m_to = value_pair.Value.StringToVector3('|', '<');
					break;
				case "m_to_to":
					m_to_to = value_pair.Value.StringToVector3('|', '<');
					break;
				case "m_ease_curve_per_axis":
					m_ease_curve_per_axis = bool.Parse(value_pair.Value);
					break;
				case "m_custom_ease_curve_y":
					m_custom_ease_curve_y = value_pair.Value.ToAnimationCurve();
					break;
				case "m_custom_ease_curve_z":
					m_custom_ease_curve_z = value_pair.Value.ToAnimationCurve();
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
					m_type = PresetEffectSetting.VariableStateListener.TYPE.VECTOR3,
					m_startVector3Value = m_from,
					m_onVector3StateChangeCallback = delegate(Vector3 value)
					{
						SetConstant(value);
					}
				});
			}
			else
			{
				list.Add(new PresetEffectSetting.VariableStateListener
				{
					m_type = PresetEffectSetting.VariableStateListener.TYPE.VECTOR3,
					m_startVector3Value = m_from,
					m_onVector3StateChangeCallback = delegate(Vector3 value)
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
					m_type = PresetEffectSetting.VariableStateListener.TYPE.VECTOR3,
					m_startVector3Value = m_to,
					m_onVector3StateChangeCallback = delegate(Vector3 value)
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
						m_type = PresetEffectSetting.VariableStateListener.TYPE.VECTOR3,
						m_startVector3Value = m_to_to,
						m_onVector3StateChangeCallback = delegate(Vector3 value)
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
