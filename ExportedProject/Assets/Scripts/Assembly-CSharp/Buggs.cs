using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Buggs : NPCBehavior
{
	public TextureBlock bloody;

	[HideInInspector]
	public bool mDead;

	[HideInInspector]
	public bool mBeatUpPlayer;

	[HideInInspector]
	public bool mCallOverMorningTime;

	public bool deadInHallway;

	public TextureBlock stabbed;

	public float tookMoney;

	public Sprite hallwayBloody;

	private void BringPhone()
	{
		GameObject gameObject = GameObject.Find("Phone");
		player.GetItem(Item.Phone);
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().color = Color.black;
	}

	private void GetThreatened()
	{
		UnlockHint("Teacher", 1);
		UnlockHint("Jerome", 5);
		UnlockHint("Cindy", 1);
		UnityEngine.Object.FindObjectOfType<PauseMenu>().UnlockHint("General", 3);
		UnityEngine.Object.FindObjectOfType<Teacher>().ActivateMission(Mission.TeacherTellOnBuggs, Room.SchoolYard);
	}

	private void SavedByTeacher()
	{
		UnityEngine.Object.FindObjectOfType<Teacher>().ExpelBugs();
		UnityEngine.Object.FindObjectOfType<Cindy>().conversations[Room.Classroom1].Nodes[2].Options[0].DestinationID = 41;
	}

	public void StartPunching()
	{
		StartCoroutine(PunchingCoroutine());
		SetAnimation("punch");
		mySkeleton.timeScale = 1f;
	}

	private IEnumerator PunchingCoroutine()
	{
		player.GetSkeleton().timeScale = 0.66f;
		player.EnableOverride();
		yield return new WaitForSeconds(0.25f);
		player.PlayAnimation("hit");
		player.ApplyTextureBlock(player.bruisedBloody);
		SFXManager.Instance.PlaySound("BasicHit");
		while (mySkeleton.AnimationName == "punch")
		{
			yield return new WaitForSeconds(0.5f);
			SFXManager.Instance.PlaySound("BasicHit");
		}
	}

	private void KnockPlayerOver()
	{
		StartCoroutine(PunchPlayerToDeath("You won't win a fight with Buggs. Perhaps the teacher could put a stop to it.", "No ganarás a Buggs. Tal vez la maestra acabará con esto."));
	}

	public IEnumerator PunchPlayerToDeath(string m, string span)
	{
		player.inAnim = true;
		UI.showDeath = true;
		ParticleSystem p = GameObject.Find("PlayerBloodSplat").GetComponent<ParticleSystem>();
		pCollider.enabled = false;
		player.PlayAnimation("writhing");
		player.GetSkeleton().timeScale = 0.1f;
		SetAnimation("walk");
		if (base.transform.position.x > player.transform.position.x)
		{
			base.transform.DOMove(player.transform.position + new Vector3(-0.5f, 0.5f, -0.1f), 0.5f).SetEase(Ease.Linear);
		}
		else
		{
			p.transform.localPosition -= 2f * Vector3.right * p.transform.localPosition.x;
			base.transform.DOMove(player.transform.position + new Vector3(0.5f, 0.5f, -0.1f), 0.5f).SetEase(Ease.Linear);
		}
		yield return new WaitForSeconds(0.5f);
		SetAnimation("punchGround");
		player.GetSkeleton().timeScale = 1f;
		for (int i = 0; i < 4; i++)
		{
			yield return new WaitForSeconds(0.2667f);
			player.ActivateBloody();
			p.Play();
			SFXManager.Instance.PlaySound("GoreSplat" + UnityEngine.Random.Range(2, 4));
			yield return new WaitForSeconds(0.2333f);
			ApplyTextureBlock(bloody);
		}
		int x = 0;
		while (player.GetSkeleton().timeScale > 0.01f)
		{
			yield return new WaitForSeconds(0.23f);
			p.Play();
			player.GetSkeleton().timeScale *= 0.5f;
			SFXManager.Instance.PlaySound("GoreSplat" + UnityEngine.Random.Range(2, 4));
			yield return new WaitForSeconds(0.27f);
			x++;
			if (x == 2)
			{
				UI.ShowDeath(m, span);
			}
		}
		SetAnimation("idle");
	}

	public void StealMoney()
	{
		StartCoroutine(StealMoneyCoroutine());
	}

	private void KillPlayerSteal()
	{
		StartCoroutine(PunchPlayerToDeath("Either bring less money to school or give half to Buggs.", "Trae menos dinero a la escuela o dá la mitad a Buggs."));
	}

	private IEnumerator StealMoneyCoroutine()
	{
		player.inAnim = true;
		yield return new WaitForSeconds(1f);
		float x = (float)Math.Round(player.money / 2f, 2);
		conversations[Room.SchoolYard].Nodes[11].Options[0].MoneyLock = x;
		conversations[Room.SchoolYard].Nodes[13].Options[0].MoneyLock = x;
		conversations[Room.SchoolYard].Nodes[15].Options[0].MoneyLock = x;
		tookMoney = x;
		SetDirection(false);
		player.inAnim = false;
		StartCoroutine(RunDialogueSection("Hmmmm...~I smell money...~*sniff*...~\\hYeah, someone is definitely carrying more than $3.00 around here./h", "Hmmmm...~Huelo dinero...~*olfatea*...~\\hSí!, Definitvamente alguien por aquí trae mucho más de $3.00./h ", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Vector3 v = base.transform.position;
		WalkToPoint(new Vector3(-26.3f, -4.75f, -4.74f), 0.5f);
		yield return new WaitForSeconds(0.5f);
		SetCurrentConversation(10);
		Interact();
		yield return null;
		while (player.inDialogue)
		{
			yield return null;
		}
		if (mySkeleton.AnimationName == "idle")
		{
			SetDirection(true);
			WalkToPoint(v, 0.5f);
		}
	}

	private void UseCigarettes()
	{
		player.UseItem(Item.Cigarettes);
	}

	private void GetCactusOutlaw()
	{
		player.GetItem(Item.CactusOutlaw);
		UnlockHint("Monstermon", 1);
	}

	public void SetCallOverPlayer(bool x)
	{
		mCallOverMorningTime = x;
	}

	public void CheckCallOverPlayer()
	{
		if (mCallOverMorningTime)
		{
			mCallOverMorningTime = false;
			StartCoroutine(MovePlayerToBugs());
		}
	}

	private IEnumerator MovePlayerToBugs()
	{
		while (player.inDialogue)
		{
			yield return null;
		}
		player.inAnim = true;
		yield return new WaitForSeconds(1f);
		conversations[Room.Classroom1].currentConversation = 1;
		Interact();
		yield return new WaitForSeconds(1f);
		player.ForceAnim(true);
		Vector3[] v;
		if (player.transform.position.x < 0f)
		{
			v = new Vector3[1]
			{
				new Vector3(-2.84f, -4.16f, -2.84f)
			};
			player.ForceChangeDirection(true);
		}
		else
		{
			player.ForceChangeDirection(false);
			v = new Vector3[2]
			{
				new Vector3(5.78f, -4.16f, -2.84f),
				new Vector3(-0.84f, -4.16f, -2.84f)
			};
		}
		player.transform.DOPath(v, 3f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.inAnim = false;
			player.ForceAnim(false);
			player.SetDepth();
			UpdateOptions();
		});
	}

	private void GetKnife()
	{
		player.GetItem(Item.Knife);
	}

	private void EnableMontyDistraction()
	{
		ActivateMission(Mission.BuggsCheckForDistraction, Room.Classroom1);
	}

	private void EnableDistractionTriggers()
	{
		GameObject.Find("DistractionTrigger1").GetComponent<BoxCollider2D>().enabled = true;
		GameObject.Find("DistractionTrigger2").GetComponent<BoxCollider2D>().enabled = true;
		CompleteMission(Mission.BuggsTellGotDistraction);
		ActivateMission(Mission.BuggsPlaceDevice, Room.Classroom1);
	}

	private void PlaceDevice()
	{
		player.UseItem(Item.Bug);
		GameObject.Find("DistractionTrigger1").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find("DistractionTrigger2").GetComponent<BoxCollider2D>().enabled = false;
		SetCurrentConversation(24);
		CompleteMission(Mission.BuggsPlaceDevice);
		ActivateMission(Mission.BuggsGoToLunchWithTeacher, Room.Classroom1);
	}

	private void ActivateFindMission()
	{
		ActivateMission(Mission.BuggsFindOutWhenAlone, Room.Classroom1);
		Teacher teacher = UnityEngine.Object.FindObjectOfType<Teacher>();
		teacher.conversations[Room.Classroom1].Nodes[1].Options[1].IsAvailable = true;
		teacher.conversations[Room.Classroom1].Nodes[50].Options[0].IsAvailable = true;
	}

	private void CompletePoisonBuggs()
	{
		player.UseItem(Item.PoisonNugget);
		if (UI.IsMissionActive(Mission.NuggetPoisonBuggs))
		{
			CompleteMission(Mission.NuggetPoisonBuggs);
			UnityEngine.Object.FindObjectOfType<Nugget>().ActivateMission(Mission.NuggetGetAntidote);
		}
		else
		{
			CompleteMission(Mission.NuggetKillBuggs);
			ActivateMission(Mission.NuggetTellPoisonedBuggs);
		}
		mDead = true;
	}

	private void ActivateBuggsFindOutGoldStars()
	{
		CompleteMission(Mission.BuggsTalkAtLunch);
		ActivateMission(Mission.BuggsFindOutGoldStars);
	}

	private void GiveBuggsDonut()
	{
		player.UseItem(Item.Donut);
		player.UseItem(Item.PoisonNugget);
		CompletePoisonBuggs();
	}

	private void DisableSissy()
	{
		conversations[Room.Cafeteria].Nodes[11].Options[3].isComplete = true;
	}

	private void StabBuggs()
	{
		SFXManager.Instance.PlaySound("GoreSplat2");
		UnityEngine.Object.FindObjectOfType<Teacher>().conversations[Room.Recess].Nodes[0].Options[0].IsAvailable = true;
		player.UseItem(Item.Knife);
		ApplyTextureBlock(stabbed);
		GameObject.Find("BuggsKnife").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("BuggsBloodStream").GetComponent<ParticleSystem>().Play();
		if (UI.IsMissionActive(Mission.NuggetPoisonBuggs))
		{
			CompleteMission(Mission.NuggetPoisonBuggs);
			CompleteMission(Mission.TeacherGetBuggsInTrouble);
		}
		else
		{
			CompleteMission(Mission.NuggetKillBuggs);
		}
		UnityEngine.Object.FindObjectOfType<Nugget>().ActivateMission(Mission.NuggetTellStabbedBuggs);
		mDead = true;
	}

	private void RunOutOfCafeteria()
	{
		StartCoroutine(RunOutOfCafeteriaCoroutine());
	}

	private IEnumerator RunOutOfCafeteriaCoroutine()
	{
		WalkToPoint(new Vector3(19.5f, -0.6f, -0.59f), 1.5f);
		SetDirection(true);
		yield return new WaitForSeconds(1.5f);
		base.transform.position = new Vector3(0f, 50f, 0f);
		deadInHallway = true;
	}

	public void PukeAndDie()
	{
		pCollider.enabled = false;
		GameObject.Find("BuggsBody").GetComponent<BoxCollider2D>().enabled = true;
		SetDirection(false);
		StartCoroutine(PukeCoroutine());
		StartCoroutine(PukeParticles());
	}

	private IEnumerator PukeCoroutine()
	{
		player.inAnim = true;
		yield return new WaitForSeconds(1f);
		mySkeleton.loop = false;
		SetAnimation("puke");
		mySkeleton.timeScale = 1f;
		yield return new WaitForSeconds(2.5f);
		while (mySkeleton.timeScale > 0f)
		{
			yield return null;
			mySkeleton.timeScale -= 0.01f;
		}
		mySkeleton.timeScale = 0f;
		player.inAnim = false;
	}

	private IEnumerator PukeParticles()
	{
		yield return new WaitForSeconds(1f);
		SFXManager.Instance.PlaySound("Puking");
		ParticleSystem p = GameObject.Find("PukeParticles").GetComponent<ParticleSystem>();
		yield return new WaitForSeconds(0.75f);
		p.Play();
	}

	public void CheckDeadInCafeteria()
	{
		if (deadInHallway)
		{
			base.transform.position = new Vector3(0f, 50f, 0f);
		}
	}

	private void TakeOutKnife()
	{
		StartCoroutine(TakeOutKnifeCoroutine());
	}

	private IEnumerator TakeOutKnifeCoroutine()
	{
		yield return null;
	}

	public void CheckDeadInHallway()
	{
		if (deadInHallway)
		{
			GameObject.Find("Buggs/Shadow").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("Hallway").GetComponent<SpriteRenderer>().sprite = hallwayBloody;
			GameObject.Find("BuggsTile").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Find("BuggsBodyHallway").GetComponent<BoxCollider2D>().enabled = true;
			mySkeleton.timeScale = 0f;
			base.transform.position = new Vector3(13f, -15f, -15.85f);
			base.transform.eulerAngles = new Vector3(0f, 0f, -90f);
			pCollider.enabled = false;
			if (tookMoney < 1f)
			{
				GameObject.Find("BuggsTile").GetComponent<ObjectInteractable>().dialogue.currentConversation = 1;
			}
		}
	}

	private void GetMoneyBack()
	{
		player.GetMoney(tookMoney);
	}

	private void GetCardFromStash()
	{
		player.GetItem(Item.MartianOrbMan);
		UnlockHint("Monstermon", 3);
	}

	public void SetBeatUpAtRecess(bool x)
	{
		mBeatUpPlayer = x;
	}

	private void ActivateHoleMission()
	{
		CompleteMission(Mission.BuggsDisposeOfKnife);
		ActivateMission(Mission.BuggsConvinceHide);
	}

	public void InterceptAtRecess()
	{
		if (!removedFromGame && mBeatUpPlayer)
		{
			SetCurrentConversation(10);
			Interact();
			WalkToPoint(new Vector3(-0.7f, player.transform.position.y, player.transform.position.z), 0.5f);
		}
	}

	private void BeatUpAtRecess()
	{
		StartCoroutine(BeatUpRecessCoroutine());
	}

	private void UnlockShroomHint()
	{
		UnlockHint("Monstermon", 5);
	}

	private void UseBreathalyzer()
	{
		player.UseItem(Item.Breathalyzer);
	}

	private void UseFlask()
	{
		player.UseItem(Item.Flask);
	}

	private void GetShroomTurtle()
	{
		player.GetItem(Item.ShroomTurtle);
	}

	private IEnumerator BeatUpRecessCoroutine()
	{
		StartCoroutine(RunDialogueSection("Cause I need to give you this!", "¡Porque necesito darte ésto!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.showDeath = true;
		SetAnimation("punch");
		mySkeleton.timeScale = 1f;
		player.GetSkeleton().timeScale = 0.66f;
		player.EnableOverride();
		yield return new WaitForSeconds(0.25f);
		player.PlayAnimation("hit");
		player.ApplyTextureBlock(player.bruisedBloody);
		SFXManager.Instance.PlaySound("BasicHit");
		for (int i = 0; i < 3; i++)
		{
			yield return new WaitForSeconds(0.5f);
			SFXManager.Instance.PlaySound("BasicHit");
		}
		ParticleSystem p = GameObject.Find("PlayerBloodSplat").GetComponent<ParticleSystem>();
		pCollider.enabled = false;
		player.PlayAnimation("writhing");
		player.GetSkeleton().timeScale = 0.1f;
		yield return new WaitForSeconds(0.5f);
		SetAnimation("walk");
		if (base.transform.position.x > player.transform.position.x)
		{
			base.transform.DOMove(player.transform.position + new Vector3(-0.5f, 0.5f, -0.1f), 0.5f).SetEase(Ease.Linear);
		}
		else
		{
			p.transform.localPosition -= 2f * Vector3.right * p.transform.localPosition.x;
			base.transform.DOMove(player.transform.position + new Vector3(0.5f, 0.5f, -0.1f), 0.5f).SetEase(Ease.Linear);
		}
		yield return new WaitForSeconds(0.5f);
		SetAnimation("punchGround");
		player.GetSkeleton().timeScale = 1f;
		for (int j = 0; j < 4; j++)
		{
			yield return new WaitForSeconds(0.2667f);
			player.ActivateBloody();
			p.Play();
			SFXManager.Instance.PlaySound("GoreSplat" + UnityEngine.Random.Range(2, 4));
			yield return new WaitForSeconds(0.2333f);
			ApplyTextureBlock(bloody);
		}
		int x = 0;
		while (player.GetSkeleton().timeScale > 0.01f)
		{
			yield return new WaitForSeconds(0.23f);
			p.Play();
			player.GetSkeleton().timeScale *= 0.5f;
			SFXManager.Instance.PlaySound("GoreSplat" + UnityEngine.Random.Range(2, 4));
			yield return new WaitForSeconds(0.27f);
			x++;
			if (x == 2)
			{
				UI.ShowDeath("Buggs is kinda mad about this morning.", "Buggs está un poco enojado por lo que pasó esta mañana.");
			}
		}
		SetAnimation("idle");
	}

	public void CheckDeath()
	{
		if (mDead)
		{
			RemoveFromGame();
			GameObject.Find("BuggsKnife").SetActive(false);
			mDead = false;
		}
		else if (mBeatUpPlayer)
		{
			base.transform.position = new Vector3(-2.7f, -9.67f, -9.66f);
		}
	}

	public override IEnumerator EndDayCoroutine()
	{
		yield return null;
		Interact();
	}

	private void BuggsEndDay()
	{
		SetDirection(false);
		WalkToPoint(new Vector3(-36f, UnityEngine.Random.Range(-2f, -1.5f), -1.14f), 7f, false);
		UI.CompleteDay(Item.Phone);
		SteamScript.UnlockAchievement("BuggsAchievement");
		UnityEngine.Object.FindObjectOfType<PauseMenu>().UnlockAllHints("Buggs");
	}
}
