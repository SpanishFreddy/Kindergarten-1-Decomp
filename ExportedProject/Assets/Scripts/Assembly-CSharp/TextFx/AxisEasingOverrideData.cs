using System;
using Boomlagoon.TextFx.JSON;

namespace TextFx
{
	[Serializable]
	public class AxisEasingOverrideData
	{
		public bool m_override_default;

		public EasingEquation m_x_ease;

		public EasingEquation m_y_ease;

		public EasingEquation m_z_ease;

		public AxisEasingOverrideData Clone()
		{
			AxisEasingOverrideData axisEasingOverrideData = new AxisEasingOverrideData();
			axisEasingOverrideData.m_override_default = m_override_default;
			axisEasingOverrideData.m_x_ease = m_x_ease;
			axisEasingOverrideData.m_y_ease = m_y_ease;
			axisEasingOverrideData.m_z_ease = m_z_ease;
			return axisEasingOverrideData;
		}

		public tfxJSONValue ExportData()
		{
			tfxJSONObject tfxJSONObject = new tfxJSONObject();
			tfxJSONObject["m_override_default"] = m_override_default;
			tfxJSONObject["m_x_ease"] = (double)m_x_ease;
			tfxJSONObject["m_y_ease"] = (double)m_y_ease;
			tfxJSONObject["m_z_ease"] = (double)m_z_ease;
			return new tfxJSONValue(tfxJSONObject);
		}

		public void ImportData(tfxJSONObject json_data)
		{
			m_override_default = json_data["m_override_default"].Boolean;
			m_x_ease = (EasingEquation)json_data["m_x_ease"].Number;
			m_y_ease = (EasingEquation)json_data["m_y_ease"].Number;
			m_z_ease = (EasingEquation)json_data["m_z_ease"].Number;
		}
	}
}
