using System.Collections.Generic;

namespace Boomlagoon.TextFx.JSON
{
	public static class BoomlagoonExtensions
	{
		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}
	}
}
