using UnityEngine;

namespace TextFx
{
	public class CustomCharacterInfo
	{
		public bool flipped;

		public Rect uv = default(Rect);

		public Rect vert = default(Rect);

		public float width;

		public CustomCharacterInfo()
		{
		}

		public CustomCharacterInfo(CustomCharacterInfo char_info)
		{
			flipped = char_info.flipped;
			uv = char_info.uv;
			vert = char_info.vert;
			width = char_info.width;
		}

		public void ScaleClone(float scale, ref CustomCharacterInfo char_info)
		{
			char_info.flipped = flipped;
			char_info.uv = new Rect(uv);
			char_info.vert = new Rect(vert);
			char_info.width = width;
			char_info.vert.x /= scale;
			char_info.vert.y /= scale;
			char_info.vert.width /= scale;
			char_info.vert.height /= scale;
			char_info.width /= scale;
		}
	}
}
