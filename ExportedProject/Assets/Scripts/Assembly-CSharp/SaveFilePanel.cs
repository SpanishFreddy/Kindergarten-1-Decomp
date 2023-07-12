using UnityEngine;
using UnityEngine.UI;

public class SaveFilePanel : MonoBehaviour
{
	public Text dayCount;

	public Text moneyCount;

	public Text monstermonCount;

	public Image keyMold;

	public Image flower;

	public Image lunchPass;

	public Image phone;

	public Image principalKey;

	public Image billyNote;

	public Image nuggetFace;

	public bool file1;

	private void Start()
	{
		if (file1)
		{
			int @int = PlayerPrefs.GetInt("DaysComplete", 1);
			if (@int > 1)
			{
				dayCount.text = "Day #" + @int;
				moneyCount.text = PlayerPrefs.GetFloat("PiggyBank").ToString("c2");
				if (PlayerPrefs.GetInt("UnlockKeyMold", 0) != 0)
				{
					keyMold.color = Color.white;
				}
				if (PlayerPrefs.GetInt("UnlockFlower", 0) != 0)
				{
					flower.color = Color.white;
				}
				if (PlayerPrefs.GetInt("UnlockLunchPass", 0) != 0)
				{
					lunchPass.color = Color.white;
				}
				if (PlayerPrefs.GetInt("UnlockPhone", 0) != 0)
				{
					phone.color = Color.white;
				}
				if (PlayerPrefs.GetInt("UnlockPrincipalKey", 0) != 0)
				{
					principalKey.color = Color.white;
				}
				if (PlayerPrefs.GetInt("UnlockBillyNote", 0) != 0)
				{
					billyNote.color = Color.white;
				}
				int int2 = PlayerPrefs.GetInt("UnlockedCards", 0);
				if (int2 > 0)
				{
					monstermonCount.text = int2 + "/25";
				}
				else
				{
					monstermonCount.text = "?/25";
				}
				if (PlayerPrefs.GetInt("WorldEnder", 0) == 1)
				{
					nuggetFace.enabled = true;
				}
			}
			return;
		}
		int int3 = PlayerPrefs.GetInt("DaysComplete2", 1);
		if (int3 > 1)
		{
			dayCount.text = "Day #" + PlayerPrefs.GetInt("DaysComplete2", 1);
			moneyCount.text = PlayerPrefs.GetFloat("PiggyBank2").ToString("c2");
			if (PlayerPrefs.GetInt("UnlockKeyMold2", 0) != 0)
			{
				keyMold.color = Color.white;
			}
			if (PlayerPrefs.GetInt("UnlockFlower2", 0) != 0)
			{
				flower.color = Color.white;
			}
			if (PlayerPrefs.GetInt("UnlockLunchPass2", 0) != 0)
			{
				lunchPass.color = Color.white;
			}
			if (PlayerPrefs.GetInt("UnlockPhone2", 0) != 0)
			{
				phone.color = Color.white;
			}
			if (PlayerPrefs.GetInt("UnlockPrincipalKey2", 0) != 0)
			{
				principalKey.color = Color.white;
			}
			if (PlayerPrefs.GetInt("UnlockBillyNote2", 0) != 0)
			{
				billyNote.color = Color.white;
			}
			int int4 = PlayerPrefs.GetInt("UnlockedCards2", 0);
			if (int4 > 0)
			{
				monstermonCount.text = int4 + "/25";
			}
			else
			{
				monstermonCount.text = "?/25";
			}
			if (PlayerPrefs.GetInt("WorldEnder2", 0) == 1)
			{
				nuggetFace.enabled = true;
			}
		}
	}
}
