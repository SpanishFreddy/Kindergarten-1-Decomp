using Steamworks;
using UnityEngine;

public class SteamScript : MonoBehaviour
{
	public static void UnlockAchievement(string x)
	{
		if (SteamManager.Initialized)
		{
			bool pbAchieved;
			SteamUserStats.GetAchievement(x, out pbAchieved);
			if (!pbAchieved)
			{
				SteamUserStats.SetAchievement(x);
				SteamUserStats.StoreStats();
			}
		}
	}

	public static void SetAchievementProgress(string a, int currentAmount, int max)
	{
		bool pbAchieved;
		if (SteamManager.Initialized && !SteamUserStats.GetAchievement(a, out pbAchieved))
		{
			if (SteamUserStats.IndicateAchievementProgress(a, (uint)currentAmount, (uint)max))
			{
				SteamUserStats.StoreStats();
			}
			if (currentAmount == max)
			{
				UnlockAchievement(a);
			}
		}
	}
}
