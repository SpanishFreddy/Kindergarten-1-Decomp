using System;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public class ActionPositionVector3Progression : ActionVector3Progression
	{
		[SerializeField]
		private bool m_force_position_override;

		public bool ForcePositionOverride
		{
			get
			{
				return m_force_position_override;
			}
			set
			{
				m_force_position_override = value;
			}
		}

		public ActionPositionVector3Progression(Vector3 start_vec)
		{
			m_from = start_vec;
			m_to = start_vec;
			m_to_to = start_vec;
		}

		public void SetConstant(Vector3 constant_value, bool force_this_position = false)
		{
			m_progression_idx = 0;
			m_from = constant_value;
			m_force_position_override = force_this_position;
		}

		public ActionPositionVector3Progression CloneThis()
		{
			ActionPositionVector3Progression actionPositionVector3Progression = new ActionPositionVector3Progression(Vector3.zero);
			actionPositionVector3Progression.m_progression_idx = base.Progression;
			actionPositionVector3Progression.m_ease_type = m_ease_type;
			actionPositionVector3Progression.m_from = m_from;
			actionPositionVector3Progression.m_to = m_to;
			actionPositionVector3Progression.m_to_to = m_to_to;
			actionPositionVector3Progression.m_to_to_bool = m_to_to_bool;
			actionPositionVector3Progression.m_is_offset_from_last = m_is_offset_from_last;
			actionPositionVector3Progression.m_unique_randoms = m_unique_randoms;
			actionPositionVector3Progression.m_force_position_override = m_force_position_override;
			actionPositionVector3Progression.m_override_animate_per_option = m_override_animate_per_option;
			actionPositionVector3Progression.m_animate_per = m_animate_per;
			actionPositionVector3Progression.m_ease_curve_per_axis = m_ease_curve_per_axis;
			actionPositionVector3Progression.m_custom_ease_curve = new AnimationCurve(m_custom_ease_curve.keys);
			actionPositionVector3Progression.m_custom_ease_curve_y = new AnimationCurve(m_custom_ease_curve_y.keys);
			actionPositionVector3Progression.m_custom_ease_curve_z = new AnimationCurve(m_custom_ease_curve_z.keys);
			return actionPositionVector3Progression;
		}

		public void CalculatePositionProgressions(TextFxAnimationManager anim_manager, LetterAnimation letter_animation, LetterSetup[] letters, int num_progressions, ActionPositionVector3Progression offset_prog, bool variableActive = true)
		{
			CalculateProgressions(num_progressions, offset_prog, variableActive);
			if ((m_value_state != PROGRESSION_VALUE_STATE.REFERENCE) ? m_force_position_override : GetOffsetReference().GetPositionVector3Prog(letter_animation.LetterActions).m_force_position_override)
			{
				Vector3 vector = Vector3.zero;
				if (m_values.Length == 1)
				{
					vector = m_values[0];
					m_values = new Vector3[letters.Length];
				}
				for (int i = 0; i < letters.Length; i++)
				{
					m_values[i] += vector;
					m_values[i] -= letters[i].BaseVerticesCenter / anim_manager.MovementScale;
				}
			}
		}

		public override tfxJSONValue ExportData()
		{
			tfxJSONObject obj = base.ExportData().Obj;
			obj["m_force_position_override"] = m_force_position_override;
			return new tfxJSONValue(obj);
		}

		public override void ImportData(tfxJSONObject json_data)
		{
			base.ImportData(json_data);
			m_force_position_override = json_data["m_force_position_override"].Boolean;
		}
	}
}
