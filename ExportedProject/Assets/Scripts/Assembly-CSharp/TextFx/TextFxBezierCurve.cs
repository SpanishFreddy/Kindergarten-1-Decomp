using System;
using Boomlagoon.TextFx.JSON;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public struct TextFxBezierCurve
	{
		private const int m_gizmo_line_subdivides = 25;

		private const int NUM_CURVE_SAMPLE_SUBSECTIONS = 50;

		private const int MAX_NUM_ANCHOR_POINTS = 5;

		public BezierCurvePointData m_pointData;

		public float m_baselineOffset;

		private Vector3[] m_temp_anchor_points;

		private Vector3 rot;

		public void AddNewAnchor()
		{
			if (m_pointData.m_numActiveCurvePoints < 2)
			{
				m_pointData.m_numActiveCurvePoints = 2;
				m_pointData.SetAnchorPoint(0, new Vector3(-5f, 0f, 0f));
				m_pointData.SetHandlePoint(0, new Vector3(-2.5f, 4f, 0f));
				m_pointData.SetAnchorPoint(1, new Vector3(5f, 0f, 0f));
				m_pointData.SetHandlePoint(1, new Vector3(2.5f, 4f, 0f));
			}
			else if (m_pointData.m_numActiveCurvePoints < 5)
			{
				m_pointData.SetAnchorPoint(m_pointData.m_numActiveCurvePoints, m_pointData.GetAnchorPoint(m_pointData.m_numActiveCurvePoints - 1) + new Vector3(5f, 0f, 0f));
				m_pointData.SetHandlePoint(m_pointData.m_numActiveCurvePoints, m_pointData.GetHandlePoint(m_pointData.m_numActiveCurvePoints - 1) + new Vector3(5f, 0f, 0f));
				m_pointData.m_numActiveCurvePoints++;
			}
		}

		public void AddNewAnchor(Vector3 anchorPointPos, Vector3 handlePointPos)
		{
			m_pointData.SetAnchorPoint(m_pointData.m_numActiveCurvePoints, anchorPointPos);
			m_pointData.SetHandlePoint(m_pointData.m_numActiveCurvePoints, handlePointPos);
			m_pointData.m_numActiveCurvePoints++;
		}

		public Vector3 GetCurvePoint(float progress, int num_anchors = 4, int curve_idx = -1, float yOffset = 0f)
		{
			if (m_pointData.m_numActiveCurvePoints < 2)
			{
				return Vector3.zero;
			}
			if (m_temp_anchor_points == null || m_temp_anchor_points.Length < num_anchors)
			{
				m_temp_anchor_points = new Vector3[num_anchors];
			}
			if (progress < 0f)
			{
				progress = 0f;
			}
			if (curve_idx < 0)
			{
				curve_idx = Mathf.FloorToInt(progress);
				progress %= 1f;
			}
			if (curve_idx >= m_pointData.m_numActiveCurvePoints - 1)
			{
				curve_idx = m_pointData.m_numActiveCurvePoints - 2;
				progress = 1f;
			}
			for (int i = 1; i < num_anchors; i++)
			{
				if (num_anchors == 4)
				{
					switch (i)
					{
					case 1:
						m_temp_anchor_points[i - 1] = m_pointData.GetAnchorPoint(curve_idx) + (m_pointData.GetHandlePoint(curve_idx, curve_idx > 0) - m_pointData.GetAnchorPoint(curve_idx)) * progress;
						break;
					case 2:
						m_temp_anchor_points[i - 1] = m_pointData.GetHandlePoint(curve_idx, curve_idx > 0) + (m_pointData.GetHandlePoint(curve_idx + 1) - m_pointData.GetHandlePoint(curve_idx, curve_idx > 0)) * progress;
						break;
					case 3:
						m_temp_anchor_points[i - 1] = m_pointData.GetHandlePoint(curve_idx + 1) + (m_pointData.GetAnchorPoint(curve_idx + 1) - m_pointData.GetHandlePoint(curve_idx + 1)) * progress;
						break;
					}
				}
				else
				{
					m_temp_anchor_points[i - 1] = m_temp_anchor_points[i - 1] + (m_temp_anchor_points[i] - m_temp_anchor_points[i - 1]) * progress;
				}
			}
			if (num_anchors == 2)
			{
				if (yOffset != 0f)
				{
					Vector3 vector = Vector3.Cross(m_temp_anchor_points[1] - m_temp_anchor_points[0], Vector3.forward);
					m_temp_anchor_points[0] -= vector.normalized * yOffset;
				}
				return m_temp_anchor_points[0];
			}
			return GetCurvePoint(progress, num_anchors - 1, curve_idx, yOffset);
		}

		public Vector3 GetCurvePointRotation(float progress, int curve_idx = -1)
		{
			if (m_pointData.m_numActiveCurvePoints < 2)
			{
				return Vector3.zero;
			}
			if (curve_idx < 0)
			{
				curve_idx = Mathf.FloorToInt(progress);
				progress %= 1f;
			}
			if (curve_idx >= m_pointData.m_numActiveCurvePoints - 1)
			{
				curve_idx = m_pointData.m_numActiveCurvePoints - 2;
				progress = 1f;
			}
			if (progress < 0f)
			{
				progress = 0f;
			}
			float progress2 = Mathf.Clamp(progress + 0.01f, 0f, 1f);
			int curve_idx2 = curve_idx;
			Vector3 curvePoint = GetCurvePoint(progress2, 4, curve_idx2);
			progress2 = Mathf.Clamp(progress - 0.01f, 0f, 1f);
			curve_idx2 = curve_idx;
			Vector3 vector = curvePoint - GetCurvePoint(progress2, 4, curve_idx2);
			if (vector.Equals(Vector3.zero))
			{
				return Vector3.zero;
			}
			rot = (Quaternion.AngleAxis(-90f, vector) * Quaternion.LookRotation(Vector3.Cross(vector, Vector3.forward), Vector3.forward)).eulerAngles;
			rot.x -= ((!(rot.x < 180f)) ? 360 : 0);
			rot.y -= ((!(rot.y < 180f)) ? 360 : 0);
			rot.z -= ((!(rot.z < 180f)) ? 360 : 0);
			return rot;
		}

		public float[] GetLetterProgressions(TextFxAnimationManager anim_manager, ref LetterSetup[] letters, TextAlignment alignment = TextAlignment.Left)
		{
			float[] array = new float[letters.Length];
			if (letters.Length == 0)
			{
				return array;
			}
			if (m_pointData.m_numActiveCurvePoints < 2)
			{
				for (int i = 0; i < letters.Length; i++)
				{
					array[i] = 0f;
				}
				return array;
			}
			float num = 0.02f;
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			int num2 = 0;
			int num3 = 0;
			float num4 = 0f;
			float letters_offset = 0f;
			LetterSetup letter = null;
			float num5 = 0f;
			float num6 = 0f;
			float curve_length = 0f;
			float renderedTextWidth;
			Action action = delegate
			{
				if (alignment == TextAlignment.Center || alignment == TextAlignment.Right)
				{
					renderedTextWidth = ((!(anim_manager.TextWidthScaled(letter.m_progression_variables.LineValue) < curve_length)) ? curve_length : anim_manager.TextWidthScaled(letter.m_progression_variables.LineValue));
					if (alignment == TextAlignment.Center)
					{
						letters_offset += curve_length / 2f - renderedTextWidth / 2f;
					}
					else if (alignment == TextAlignment.Right)
					{
						letters_offset += curve_length - renderedTextWidth;
					}
				}
			};
			letter = letters[0];
			if (alignment == TextAlignment.Center || alignment == TextAlignment.Right)
			{
				for (int j = 0; j < m_pointData.m_numActiveCurvePoints - 1; j++)
				{
					curve_length += GetCurveLength(j);
				}
			}
			num4 = letter.BaseVerticesCenter.x / anim_manager.AnimationInterface.MovementScale - letter.Width / anim_manager.AnimationInterface.MovementScale / 2f;
			letters_offset = letter.Width / anim_manager.AnimationInterface.MovementScale / 2f;
			action();
			bool flag = false;
			while (!flag)
			{
				for (int k = 0; k < m_pointData.m_numActiveCurvePoints - 1; k++)
				{
					for (int l = 0; l <= 50; l++)
					{
						float num7 = (float)l * num;
						float progress = num7;
						int curve_idx = k;
						vector = GetCurvePoint(progress, 4, curve_idx);
						if (l > 0)
						{
							num6 += (vector - vector2).magnitude;
							while (num2 < letters.Length && num6 > letters_offset)
							{
								num7 = (float)k + (float)(l - 1) * num + (letters_offset - num5) / (num6 - num5) * num;
								array[num2] = num7;
								num2++;
								if (num2 < letters.Length && !letters[num2].StubInstance && letters[num2].VisibleCharacter)
								{
									letter = letters[num2];
									if (letter.m_progression_variables.LineValue > num3)
									{
										num3 = letter.m_progression_variables.LineValue;
										num4 = letter.BaseVerticesCenter.x / anim_manager.AnimationInterface.MovementScale - letter.Width / anim_manager.AnimationInterface.MovementScale / 2f;
										k = 0;
										l = -1;
										num6 = 0f;
									}
									letters_offset = letter.BaseVerticesCenter.x / anim_manager.AnimationInterface.MovementScale - num4;
									action();
								}
							}
							if (num2 == letters.Length)
							{
								flag = true;
								break;
							}
						}
						vector2 = vector;
						num5 = num6;
					}
				}
				for (int m = num2; m < letters.Length; m++)
				{
					letter = letters[num2];
					if (letter.m_progression_variables.LineValue == num3)
					{
						array[m] = (float)m_pointData.m_numActiveCurvePoints - 1.001f;
					}
					else if (letter.VisibleCharacter && !letter.StubInstance)
					{
						num3 = letter.m_progression_variables.LineValue;
						num6 = 0f;
						num4 = letter.BaseVerticesCenter.x / anim_manager.AnimationInterface.MovementScale - letter.Width / anim_manager.AnimationInterface.MovementScale / 2f;
						letters_offset = letter.BaseVerticesCenter.x / anim_manager.AnimationInterface.MovementScale - num4;
						action();
						break;
					}
					num2++;
				}
				if (num2 == letters.Length)
				{
					flag = true;
					break;
				}
			}
			return array;
		}

		private float GetCurveLength(int curve_idx)
		{
			int num = 50;
			Vector3? vector = null;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				float progress = (float)i / (float)(num - 1);
				Vector3 curvePoint = GetCurvePoint(progress, 4, curve_idx);
				if (vector.HasValue)
				{
					num2 += (curvePoint - vector).Value.magnitude;
				}
				vector = curvePoint;
			}
			return num2;
		}

		public tfxJSONValue ExportData()
		{
			tfxJSONObject obj = new tfxJSONObject();
			return new tfxJSONValue(obj);
		}

		public void ImportData(tfxJSONObject json_data)
		{
		}
	}
}
