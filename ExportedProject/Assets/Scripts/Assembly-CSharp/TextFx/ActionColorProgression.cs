using System;
using System.Collections.Generic;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class ActionColorProgression : ActionVariableProgression
	{
		[SerializeField]
		private VertexColour[] m_values;

		[SerializeField]
		private VertexColour m_from = new VertexColour();

		[SerializeField]
		private VertexColour m_to = new VertexColour();

		[SerializeField]
		private VertexColour m_to_to = new VertexColour();

		[SerializeField]
		private bool m_override_alpha;

		[SerializeField]
		private bool m_use_colour_gradients;

		[SerializeField]
		protected PROGRESSION_VALUE_STATE m_value_state;

		[SerializeField]
		protected ActionVariableProgressionReferenceData m_offset_progression;

		private ActionColorProgression cachedColourProgression;

		public VertexColour ValueFrom
		{
			get
			{
				return (!m_use_colour_gradients) ? m_from.FlatColour : m_from;
			}
		}

		public VertexColour ValueTo
		{
			get
			{
				return (!m_use_colour_gradients) ? m_to.FlatColour : m_to;
			}
		}

		public VertexColour ValueThen
		{
			get
			{
				return (!m_use_colour_gradients) ? m_to_to.FlatColour : m_to_to;
			}
		}

		public VertexColour[] Values
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

		public bool UseColourGradients
		{
			get
			{
				return m_use_colour_gradients;
			}
			set
			{
				m_use_colour_gradients = value;
			}
		}

		public override bool UniqueRandom
		{
			get
			{
				return base.Progression == 1 && m_unique_randoms && m_value_state != PROGRESSION_VALUE_STATE.REFERENCE;
			}
		}

		public ActionColorProgression(VertexColour start_colour)
		{
			m_from = start_colour.Clone();
			m_to = start_colour.Clone();
			m_to_to = start_colour.Clone();
		}

		public ActionColorProgression(VertexColour start_colour, bool offsetFromLast)
		{
			m_from = start_colour.Clone();
			m_to = start_colour.Clone();
			m_to_to = start_colour.Clone();
			m_is_offset_from_last = offsetFromLast;
		}

		public ActionVariableProgressionReferenceData GetOffsetReference()
		{
			if (m_value_state == PROGRESSION_VALUE_STATE.UNIQUE)
			{
				return m_reference_data;
			}
			return m_offset_progression;
		}

		public void SetValueReference(ActionColorProgression progression)
		{
			m_value_state = PROGRESSION_VALUE_STATE.REFERENCE;
			m_offset_progression = progression.ReferenceData;
		}

		public void SetConstant(Color constant_value)
		{
			m_progression_idx = 0;
			m_from = new VertexColour(constant_value);
			m_use_colour_gradients = false;
		}

		public void SetConstant(VertexColour constant_value)
		{
			m_progression_idx = 0;
			m_from = constant_value;
			m_use_colour_gradients = true;
		}

		public void SetRandom(Color random_min, Color random_max, bool unique_randoms = false)
		{
			m_progression_idx = 1;
			m_from = new VertexColour(random_min);
			m_to = new VertexColour(random_max);
			m_unique_randoms = unique_randoms;
			m_use_colour_gradients = false;
		}

		public void SetRandom(VertexColour random_min, VertexColour random_max, bool unique_randoms = false)
		{
			m_progression_idx = 1;
			m_from = random_min;
			m_to = random_max;
			m_unique_randoms = unique_randoms;
			m_use_colour_gradients = true;
		}

		public void SetEased(VertexColour eased_from, VertexColour eased_to)
		{
			SetEased(eased_from, eased_to, m_ease_type);
		}

		public void SetEased(VertexColour eased_from, VertexColour eased_to, VertexColour eased_then)
		{
			SetEased(eased_from, eased_to, eased_then, m_ease_type);
		}

		public void SetEased(VertexColour eased_from, VertexColour eased_to, EasingEquation easing_function)
		{
			m_progression_idx = 2;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to_bool = false;
			m_ease_type = easing_function;
			m_use_colour_gradients = true;
		}

		public void SetEased(VertexColour eased_from, VertexColour eased_to, VertexColour eased_then, EasingEquation easing_function)
		{
			m_progression_idx = 2;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to = eased_then;
			m_to_to_bool = true;
			m_ease_type = easing_function;
			m_use_colour_gradients = true;
		}

		public void SetEasedCustom(VertexColour eased_from, VertexColour eased_to)
		{
			SetEasedCustom(eased_from, eased_to, m_custom_ease_curve);
		}

		public void SetEasedCustom(VertexColour eased_from, VertexColour eased_to, AnimationCurve easing_curve)
		{
			m_progression_idx = 3;
			m_from = eased_from;
			m_to = eased_to;
			m_to_to_bool = false;
			m_custom_ease_curve = easing_curve;
			m_use_colour_gradients = true;
		}

		public void SetValues(VertexColour[] colValues)
		{
			m_values = colValues;
			m_progression_idx = 2;
			m_use_colour_gradients = true;
		}

		public void GetValue(ref VertexColour colValue, List<LetterAction> all_letter_actions, AnimationProgressionVariables progression_variables, AnimatePerOptions animate_per_default, ActionColorProgression defaultAnimColourProg)
		{
			GetValue(ref colValue, all_letter_actions, GetProgressionIndex(progression_variables, animate_per_default), defaultAnimColourProg);
		}

		public void GetValue(ref VertexColour colValue, List<LetterAction> all_letter_actions, int progression_idx, ActionColorProgression defaultAnimColourProg)
		{
			colValue.Clear();
			if (m_value_state == PROGRESSION_VALUE_STATE.OFFSET_FROM_REFERENCE || m_value_state == PROGRESSION_VALUE_STATE.REFERENCE)
			{
				cachedColourProgression = m_offset_progression.GetColourProg(all_letter_actions, defaultAnimColourProg);
				if (cachedColourProgression != null && (cachedColourProgression.m_reference_data.m_action_index != m_reference_data.m_action_index || cachedColourProgression.m_reference_data.m_start_state != m_reference_data.m_start_state))
				{
					cachedColourProgression.GetValue(ref colValue, all_letter_actions, progression_idx, defaultAnimColourProg);
					if (m_value_state == PROGRESSION_VALUE_STATE.OFFSET_FROM_REFERENCE && m_override_alpha)
					{
						colValue.ClearAlpha();
					}
				}
			}
			if (m_value_state == PROGRESSION_VALUE_STATE.OFFSET_FROM_REFERENCE || m_value_state == PROGRESSION_VALUE_STATE.UNIQUE)
			{
				if (m_values.Length > 1 && progression_idx < m_values.Length)
				{
					colValue.AddInLine(m_values[progression_idx]);
				}
				else if (m_values.Length == 1)
				{
					colValue.AddInLine(m_values[0]);
				}
			}
		}

		public void ConvertFromFlatColourProg(ActionColorProgression flat_colour_progression)
		{
			m_progression_idx = flat_colour_progression.Progression;
			m_ease_type = flat_colour_progression.EaseType;
			m_from = new VertexColour(flat_colour_progression.ValueFrom);
			m_to = new VertexColour(flat_colour_progression.ValueTo);
			m_to_to = new VertexColour(flat_colour_progression.ValueThen);
			m_to_to_bool = flat_colour_progression.UsingThirdValue;
			m_is_offset_from_last = flat_colour_progression.IsOffsetFromLast;
			m_unique_randoms = flat_colour_progression.UniqueRandom;
		}

		public void CalculateUniqueRandom(AnimationProgressionVariables progression_variables, AnimatePerOptions animate_per, VertexColour[] offset_colours)
		{
			int progressionIndex = GetProgressionIndex(progression_variables, animate_per);
			bool flag = offset_colours != null && offset_colours.Length == 1;
			m_values[progressionIndex] = ((!m_is_offset_from_last) ? new VertexColour(new Color(0f, 0f, 0f, 0f)) : offset_colours[(!flag) ? progressionIndex : 0].Clone());
			m_values[progressionIndex] = m_values[progressionIndex].Add(m_from.Add(m_to.Sub(m_from).Multiply(UnityEngine.Random.value)));
		}

		public void CalculateProgressions(int num_progressions, ActionColorProgression offset_prog, bool variableActive = true)
		{
			if (!variableActive)
			{
				SetValueReference(offset_prog);
				return;
			}
			if (m_is_offset_from_last)
			{
				m_value_state = PROGRESSION_VALUE_STATE.OFFSET_FROM_REFERENCE;
				m_offset_progression = offset_prog.GetOffsetReference();
			}
			else
			{
				m_value_state = PROGRESSION_VALUE_STATE.UNIQUE;
			}
			m_values = new VertexColour[(base.Progression != 2 && base.Progression != 3 && base.Progression != 1) ? 1 : num_progressions];
			if (base.Progression == 1)
			{
				for (int i = 0; i < num_progressions; i++)
				{
					m_values[i] = ValueFrom.Add(ValueTo.Sub(ValueFrom).Multiply(UnityEngine.Random.value));
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
							m_values[j] = ValueFrom.Add(ValueTo.Sub(ValueFrom).Multiply(EasingManager.GetEaseProgress(m_ease_type, num / 0.5f)));
							continue;
						}
						num -= 0.5f;
						m_values[j] = ValueTo.Add(ValueThen.Sub(ValueTo).Multiply(EasingManager.GetEaseProgress(m_ease_type, num / 0.5f)));
					}
					else
					{
						m_values[j] = ValueFrom.Add(ValueTo.Sub(ValueFrom).Multiply(EasingManager.GetEaseProgress(m_ease_type, num)));
					}
				}
			}
			else if (base.Progression == 3)
			{
				for (int k = 0; k < num_progressions; k++)
				{
					float time = ((num_progressions != 1) ? ((float)k / ((float)num_progressions - 1f)) : 0f);
					m_values[k] = ValueFrom.Add(ValueTo.Sub(ValueFrom).Multiply(m_custom_ease_curve.Evaluate(time)));
				}
			}
			else if (base.Progression == 0)
			{
				for (int l = 0; l < m_values.Length; l++)
				{
					m_values[l] = ValueFrom;
				}
			}
		}

		public ActionColorProgression Clone()
		{
			ActionColorProgression actionColorProgression = new ActionColorProgression(new VertexColour());
			actionColorProgression.m_progression_idx = base.Progression;
			actionColorProgression.m_ease_type = m_ease_type;
			actionColorProgression.m_from = m_from.Clone();
			actionColorProgression.m_to = m_to.Clone();
			actionColorProgression.m_to_to = m_to_to.Clone();
			actionColorProgression.m_to_to_bool = m_to_to_bool;
			actionColorProgression.m_is_offset_from_last = m_is_offset_from_last;
			actionColorProgression.m_unique_randoms = m_unique_randoms;
			actionColorProgression.m_override_animate_per_option = m_override_animate_per_option;
			actionColorProgression.m_animate_per = m_animate_per;
			actionColorProgression.m_use_colour_gradients = m_use_colour_gradients;
			actionColorProgression.m_override_alpha = m_override_alpha;
			return actionColorProgression;
		}

		public override tfxJSONValue ExportData()
		{
			tfxJSONObject json_data = new tfxJSONObject();
			ExportBaseData(ref json_data);
			json_data["m_from"] = m_from.ExportData();
			json_data["m_to"] = m_to.ExportData();
			json_data["m_to_to"] = m_to_to.ExportData();
			json_data["m_use_colour_gradients"] = m_use_colour_gradients;
			json_data["m_override_alpha"] = m_override_alpha;
			return new tfxJSONValue(json_data);
		}

		public override void ImportData(tfxJSONObject json_data)
		{
			m_from = json_data["m_from"].Obj.JSONtoVertexColour();
			m_to = json_data["m_to"].Obj.JSONtoVertexColour();
			m_to_to = json_data["m_to_to"].Obj.JSONtoVertexColour();
			if (json_data.ContainsKey("m_use_colour_gradients"))
			{
				m_use_colour_gradients = json_data["m_use_colour_gradients"].Boolean;
			}
			else
			{
				m_use_colour_gradients = false;
			}
			if (json_data.ContainsKey("m_override_alpha"))
			{
				m_override_alpha = json_data["m_override_alpha"].Boolean;
			}
			else
			{
				m_override_alpha = false;
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
					m_from = new VertexColour(value_pair.Value.StringToColor('|', '<'));
					break;
				case "m_to":
					m_to = new VertexColour(value_pair.Value.StringToColor('|', '<'));
					break;
				case "m_to_to":
					m_to_to = new VertexColour(value_pair.Value.StringToColor('|', '<'));
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
					m_startColorValue = m_from.top_left,
					m_onColorStateChangeCallback = delegate(Color value)
					{
						SetConstant(value);
					}
				});
			}
			else
			{
				list.Add(new PresetEffectSetting.VariableStateListener
				{
					m_startColorValue = m_from.top_left,
					m_onColorStateChangeCallback = delegate(Color value)
					{
						if (base.Progression == 2)
						{
							if (m_to_to_bool)
							{
								SetEased(new VertexColour(value), m_to, m_to_to);
							}
							else
							{
								SetEased(new VertexColour(value), m_to);
							}
						}
						else if (base.Progression == 3)
						{
							SetEasedCustom(new VertexColour(value), m_to);
						}
						else if (base.Progression == 1)
						{
							SetRandom(value, m_to.top_left);
						}
					}
				});
				list.Add(new PresetEffectSetting.VariableStateListener
				{
					m_startColorValue = m_to.top_left,
					m_onColorStateChangeCallback = delegate(Color value)
					{
						if (base.Progression == 2)
						{
							if (m_to_to_bool)
							{
								SetEased(m_from, new VertexColour(value), m_to_to);
							}
							else
							{
								SetEased(m_from, new VertexColour(value));
							}
						}
						else if (base.Progression == 3)
						{
							SetEasedCustom(m_from, new VertexColour(value));
						}
						else if (base.Progression == 1)
						{
							SetRandom(m_from.top_left, value);
						}
					}
				});
				if (base.Progression == 2 && m_to_to_bool)
				{
					list.Add(new PresetEffectSetting.VariableStateListener
					{
						m_startColorValue = m_to_to.top_left,
						m_onColorStateChangeCallback = delegate(Color value)
						{
							SetEased(m_from, m_to, new VertexColour(value));
						}
					});
				}
			}
			return list;
		}
	}
}
