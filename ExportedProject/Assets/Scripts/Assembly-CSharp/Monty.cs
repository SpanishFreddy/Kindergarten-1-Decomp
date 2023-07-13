using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Monty : NPCBehavior
{
	public TextureBlock bloody;

	public TextureBlock noGlasses;

	public bool inRecessScene;

	[HideInInspector]
	public bool mDead;

	private void BringPrincipalKey()
	{
		GameObject gameObject = GameObject.Find("PrincipalKey");
		player.GetItem(Item.PrincipalKey);
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().color = Color.black;
	}

	private void BuyYoyo()
	{
		player.GetItem(Item.Yoyo);
		conversations[Room.SchoolYard].Nodes[2].Options[0].IsAvailable = false;
		conversations[Room.Classroom1].Nodes[2].Options[1].IsAvailable = false;
		UnlockHint("Jerome", 1);
		UnlockHint("Jerome", 2);
	}

	private void BuyCigs()
	{
		player.GetItem(Item.Cigarettes);
		conversations[Room.SchoolYard].Nodes[2].Options[1].IsAvailable = false;
		conversations[Room.Classroom1].Nodes[2].Options[3].IsAvailable = false;
		UnlockHint("Jerome", 1);
	}

	private void BuyVoiceRecorder()
	{
		player.GetItem(Item.VoiceRecorder);
		conversations[Room.SchoolYard].Nodes[2].Options[2].IsAvailable = false;
	}

	private void BuyScrewdriver()
	{
		player.GetItem(Item.Screwdriver);
		conversations[Room.SchoolYard].Nodes[2].Options[3].IsAvailable = false;
		conversations[Room.Classroom1].Nodes[2].Options[2].IsAvailable = false;
		UnlockHint("Jerome", 1);
	}

	private void SellPill()
	{
		player.UseItem(Item.Pill);
		player.money += 1f;
		UI.UpdateMoneyText(true);
	}

	private void SellPills()
	{
		UnlockHint("Monty", 5);
		player.UseItem(Item.Pills);
		player.GetMoney(4f);
	}

	private void SellFlask()
	{
		player.UseItem(Item.Flask);
		player.GetMoney(2f);
	}

	private void Givemold()
	{
		player.UseItem(Item.KeyMold);
		SetEndDay(true);
	}

	private void ActivatemoldMission()
	{
		ActivateMission(Mission.MontyCollectTwenty);
	}

	private void GiveNote()
	{
		player.UseItem(Item.BillyNote);
		CompleteMission(Mission.LilyReadNote);
	}

	private void ActivateTellLilyAboutNote()
	{
		ActivateMission(Mission.LilyGetNoteFromMonty, Room.Cafeteria);
		Object.FindObjectOfType<Lily>().ActivateMission(Mission.LilyTellNote, Room.SchoolYard);
	}

	private void SellLunchPass()
	{
		UnlockHint("Monty", 5);
		player.UseItem(Item.LunchPass);
		player.GetMoney(3f);
	}

	private void SellGoldStar()
	{
		player.UseItem(Item.GoldStar);
		player.GetMoney(1f);
	}

	private void UnlockBugHint()
	{
		UnlockHint("Buggs", 3);
	}

	private void SellPass()
	{
		Object.FindObjectOfType<Jerome>().conversations[Room.Classroom1].Nodes[17].Options[1].IsAvailable = true;
		player.UseItem(Item.HallPass);
		player.GetMoney(5f);
	}

	private void SellBreathalyzer()
	{
		player.UseItem(Item.Breathalyzer);
		player.money += 2.25f;
		UI.UpdateMoneyText(true);
	}

	private void GiveBug()
	{
		player.UseItem(Item.Bug);
	}

	private void GetRemote()
	{
		player.GetItem(Item.Bug);
		player.GetItem(Item.Remote);
	}

	private void ActivateTalkToBuggs()
	{
		CompleteMission(Mission.BuggsCheckForDistraction);
		Object.FindObjectOfType<Buggs>().ActivateMission(Mission.BuggsTellGotDistraction, Room.Classroom1);
	}

	private void SpeltWrong()
	{
		Object.FindObjectOfType<Janitor>().conversations[Room.Cafeteria].Nodes[0].Options[1].IsAvailable = true;
	}

	private void GetGlasses()
	{
		player.GetItem(Item.MontyGlasses);
		GameObject.Find("MontyGlasses").transform.position = new Vector3(50f, 0f, 0f);
	}

	private void GetCode()
	{
		player.GetItem(Item.BillyCode);
		CompleteMission(Mission.LilyGetNoteFromMonty);
		Object.FindObjectOfType<Lily>().ActivateMission(Mission.LilyShowCode, Room.Cafeteria);
	}

	public void ResizeCollider()
	{
		GetComponent<BoxCollider2D>().size = new Vector2(2.38f, 1.06f);
		GetComponent<BoxCollider2D>().offset = new Vector2(0.55f, 0.33f);
	}

	private void SellChocolateBar()
	{
		UnlockHint("Monty", 5);
		player.UseItem(Item.ChocolateBar);
		player.GetMoney(5f);
	}

	private void BuySalad()
	{
		player.GetItem(Item.Salad);
		conversations[Room.Cafeteria].Nodes[2].Options[0].IsAvailable = false;
	}

	private void SellDonut()
	{
		player.UseItem(Item.Donut);
		player.GetMoney(3f);
	}

	private void BuyMcGlobs()
	{
		player.GetItem(Item.McGlobs);
	}

	private void SellShoe()
	{
		player.UseItem(Item.Shoe);
		player.GetMoney(2.5f);
	}

	public void SetDead()
	{
		mDead = true;
	}

	public void CheckDeath()
	{
		if (mDead)
		{
			RemoveFromGame();
			GameObject.Find("GeneralBloodStream").transform.position = new Vector3(0f, -50f, 0f);
		}
	}

	private void BuyHolyKnight()
	{
		player.GetItem(Item.HolyKnight);
		UnlockHint("Monstermon", 21);
	}

	private void StopPlayerFromJerome()
	{
		SetCurrentConversation(10);
		StartCoroutine(StopPlayerCoroutine());
	}

	private void ReadRecipe()
	{
		CompleteMission(Mission.CindyReadPaper);
		Object.FindObjectOfType<Cindy>().ActivateMission(Mission.CindyTellPaper, Room.Recess);
	}

	private IEnumerator StopPlayerCoroutine()
	{
		FacePlayer();
		player.ForceAnim(false);
		StartCoroutine(RunDialogueSection("Hey kid!~~ Come over here!", "¡Hola, niño!~~ ¡Ven aquí!", this));
		GameObject.Find("MontyIntercept").SetActive(false);
		while (pEmptyOptions)
		{
			yield return null;
		}
		CompleteMission(Mission.JeromeBringLaserRecess);
		player.ForceAnim(true);
		Vector3 v;
		if (player.transform.position.x < base.transform.position.x)
		{
			v = base.transform.position - Vector3.right * 2f;
			player.ForceChangeDirection(true);
		}
		else
		{
			v = base.transform.position + Vector3.right * 2f;
			player.ForceChangeDirection(false);
		}
		player.inAnim = true;
		player.ForceAnim(true);
		player.transform.DOMove(v, 1f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(1f);
		player.inAnim = false;
		Interact();
	}

	private void ActivatePlaceDeviceRecess()
	{
		ActivateMission(Mission.JeromePlaceDevice, Room.Recess);
		GameObject.Find("DropBugTrigger").GetComponent<BoxCollider2D>().enabled = true;
	}

	private void TakeLaserRecess()
	{
		player.UseItem(Item.LaserPointer);
	}

	private void GetBugAtRecess()
	{
		player.GetItem(Item.Bug);
		ActivateMission(Mission.JeromeSaveFromJanitor);
	}

	private void PlaceBugByJanitor()
	{
		player.UseItem(Item.Bug);
		GameObject.Find("RecessBugJanitor").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("DropBugTrigger").GetComponent<BoxCollider2D>().enabled = false;
		CompleteMission(Mission.JeromePlaceDevice);
		ActivateMission(Mission.JeromeTellMontyDevicePlaced, Room.Recess);
	}

	private void DetonateBomb()
	{
		StartCoroutine(DetonateBombCoroutine());
	}

	private IEnumerator DetonateBombCoroutine()
	{
		inRecessScene = true;
		StartCoroutine(RunDialogueSection("Excellent. This is gonna be good.", "Excelente. Ésto va a estar bueno.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(false);
		StartCoroutine(RunDialogueSection("\\bNOW JEROME!!!/b", "\\b¡¡¡AHORA, JEROME!!!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Jerome jer = Object.FindObjectOfType<Jerome>();
		Janitor jan = Object.FindObjectOfType<Janitor>();
		jer.WalkToPoint(new Vector3(0f, -11.5f, -11.49f), 1f);
		StartCoroutine(RunDialogueSection("Get back here! I ain't done questioning you yet!", "¡Regresa! ¡Todavía no he terminado de interrogarte!", jan));
		yield return new WaitForSeconds(1f);
		jer.SetDirection(true);
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		if (UI.IsMissionComplete(Mission.JeromePlaceDevice))
		{
			jan.ExplodeJanitor();
			CompleteMission(Mission.JeromeTellMontyDevicePlaced);
			CompleteMission(Mission.JeromeSaveFromJanitor);
			player.inAnim = true;
			yield return new WaitForSeconds(4f);
			jer.WalkToPoint(new Vector3(11.43f, -5.25f, -5.24f), 2f);
			StartCoroutine(RunDialogueSection("Jeez dude!~ I told you distract him, not blow him up!", "¡Dios Santo, amigo!~ Te pedí distraerlo no explotarlo!", jer));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Eh...~he had it coming.", "Eh...~él se lo merecía.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UI.CollapseDialogue();
			Teacher t = Object.FindObjectOfType<Teacher>();
			t.SetDirection(false);
			t.WalkToPoint(new Vector3(20.93f, -4.4f, -4.37f), 1f);
			yield return new WaitForSeconds(1f);
			player.inAnim = true;
			yield return new WaitForSeconds(1f);
			SetDirection(true);
			player.ForceChangeDirection(true);
			yield return new WaitForSeconds(1f);
			player.inAnim = false;
			StartCoroutine(RunDialogueSection("The janitor just exploded.", "El conserje acaba de explotar.", t));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Yes...~he did.", "Sí...~así fue.", jer));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("The irony here is that there's no one to clean up the...~parts.", "Lo irónico es que ahora no hay nadie aquí para limpiar...~los pedazos", t));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UI.CollapseDialogue();
			player.inAnim = true;
			yield return new WaitForSeconds(1f);
			player.inAnim = false;
			StartCoroutine(RunDialogueSection("Can we just go inside?", "¿Podemos entrar?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Yes, I think that would be for the best.", "Sí, creo que sería lo mejor.", t));
			while (pEmptyOptions)
			{
				yield return null;
			}
			jer.SetEndDay(true);
			EnvironmentController.Instance.ChangeEnvironment(Room.Classroom2);
		}
		else
		{
			player.ExplodePlayer("You should have placed the device like Monty said.", "La próxima vez solo haz lo que dice Monty.");
		}
	}

	private void RecordedTheSale()
	{
		StartCoroutine(RecordedTheSaleCoroutine());
	}

	private IEnumerator RecordedTheSaleCoroutine()
	{
		Teacher t = Object.FindObjectOfType<Teacher>();
		StartCoroutine(RunDialogueSection("Damn that stupid voice recorder! I never shoulda sold it to a rat like you.", "¡Diablos, estupida grabadora! Jamás tendría que venderla a un traidor como tú.", t));
		yield return new WaitForSeconds(0.5f);
		SetEmptyOptions();
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Well Monty, it looks like you've been ratted out by the best rat in the business. Now get your slimy ass out of my classroom!", "Está bien, Monty, parece que has sido delatado por el mejor traidor en el negocio. ¡Ahora saca tu asqueroso trasero de mi salón!", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		t.SetCurrentConversation(2);
		t.Interact();
	}

	public override IEnumerator EndDayCoroutine()
	{
		yield return null;
		Interact();
	}

	private void GetPrincipalKey()
	{
		player.GetItem(Item.PrincipalKey);
	}

	private void EndDayMonty()
	{
		SteamScript.UnlockAchievement("MontyAchievement");
		UI.CompleteDay(Item.PrincipalKey);
		SetDirection(false);
		WalkToPoint(new Vector3(-36f, Random.Range(-2f, -1.5f), -1.14f), 7f, false);
		Object.FindObjectOfType<PauseMenu>().UnlockAllHints("Monty");
	}

	private void FailToBuy()
	{
		UI.CompleteDay(Item.None);
		SetDirection(false);
		WalkToPoint(new Vector3(-36f, Random.Range(-2f, -1.5f), -1.14f), 7f, false);
	}
}
