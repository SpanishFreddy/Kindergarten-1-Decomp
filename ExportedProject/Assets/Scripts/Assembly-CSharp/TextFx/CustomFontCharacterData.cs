using System.Collections.Generic;

namespace TextFx
{
	public class CustomFontCharacterData
	{
		public Dictionary<int, CustomCharacterInfo> m_character_infos;

		public CustomFontCharacterData()
		{
			m_character_infos = new Dictionary<int, CustomCharacterInfo>();
		}
	}
}
