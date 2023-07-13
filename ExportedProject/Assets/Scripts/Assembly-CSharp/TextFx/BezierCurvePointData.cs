using System;
using UnityEngine;

namespace TextFx
{
	[Serializable]
	public struct BezierCurvePointData
	{
		public BezierCurvePoint m_anchor_point_1;

		public BezierCurvePoint m_anchor_point_2;

		public BezierCurvePoint m_anchor_point_3;

		public BezierCurvePoint m_anchor_point_4;

		public BezierCurvePoint m_anchor_point_5;

		public int m_numActiveCurvePoints;

		public Vector3 GetAnchorPoint(int index)
		{
			switch (index)
			{
			case 0:
				return m_anchor_point_1.m_anchor_point;
			case 1:
				return m_anchor_point_2.m_anchor_point;
			case 2:
				return m_anchor_point_3.m_anchor_point;
			case 3:
				return m_anchor_point_4.m_anchor_point;
			case 4:
				return m_anchor_point_5.m_anchor_point;
			default:
				return Vector3.zero;
			}
		}

		public void SetAnchorPoint(int index, Vector3 value)
		{
			switch (index)
			{
			case 0:
				m_anchor_point_1.m_anchor_point = value;
				break;
			case 1:
				m_anchor_point_2.m_anchor_point = value;
				break;
			case 2:
				m_anchor_point_3.m_anchor_point = value;
				break;
			case 3:
				m_anchor_point_4.m_anchor_point = value;
				break;
			case 4:
				m_anchor_point_5.m_anchor_point = value;
				break;
			}
		}

		public Vector3 GetHandlePoint(int index, bool inverse = false)
		{
			switch (index)
			{
			case 0:
				return (!inverse) ? m_anchor_point_1.m_handle_point : (m_anchor_point_1.m_anchor_point + (m_anchor_point_1.m_anchor_point - m_anchor_point_1.m_handle_point));
			case 1:
				return (!inverse) ? m_anchor_point_2.m_handle_point : (m_anchor_point_2.m_anchor_point + (m_anchor_point_2.m_anchor_point - m_anchor_point_2.m_handle_point));
			case 2:
				return (!inverse) ? m_anchor_point_3.m_handle_point : (m_anchor_point_3.m_anchor_point + (m_anchor_point_3.m_anchor_point - m_anchor_point_3.m_handle_point));
			case 3:
				return (!inverse) ? m_anchor_point_4.m_handle_point : (m_anchor_point_4.m_anchor_point + (m_anchor_point_4.m_anchor_point - m_anchor_point_4.m_handle_point));
			case 4:
				return (!inverse) ? m_anchor_point_5.m_handle_point : (m_anchor_point_5.m_anchor_point + (m_anchor_point_5.m_anchor_point - m_anchor_point_5.m_handle_point));
			default:
				return Vector3.zero;
			}
		}

		public void SetHandlePoint(int index, Vector3 value)
		{
			switch (index)
			{
			case 0:
				m_anchor_point_1.m_handle_point = value;
				break;
			case 1:
				m_anchor_point_2.m_handle_point = value;
				break;
			case 2:
				m_anchor_point_3.m_handle_point = value;
				break;
			case 3:
				m_anchor_point_4.m_handle_point = value;
				break;
			case 4:
				m_anchor_point_5.m_handle_point = value;
				break;
			}
		}
	}
}
