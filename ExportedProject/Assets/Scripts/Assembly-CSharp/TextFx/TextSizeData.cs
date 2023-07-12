using System;

namespace TextFx
{
	[Serializable]
	public class TextSizeData
	{
		public float m_text_line_width;

		public float m_text_line_height;

		public float m_total_text_width;

		public float m_total_text_height;

		public float m_line_height_offset;

		public float m_y_max;

		public TextSizeData(float text_line_width, float text_line_height, float line_height_offset, float y_max)
		{
			m_text_line_width = text_line_width;
			m_text_line_height = text_line_height;
			m_line_height_offset = line_height_offset;
			m_y_max = y_max;
		}
	}
}
