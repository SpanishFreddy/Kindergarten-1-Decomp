using System;
using System.Collections;
using DG.Tweening;
using DialogueTree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nugget : NPCBehavior
{
	public TextureBlock crying;

	public TextureBlock slop;

	public TextureBlock bloodySlop;

	public Transform knife;

	public Vector3 offset;

	public Sprite recessRed;

	public bool mPoisonedPlayer;

	private bool mEndWorld;

	private void BringBillyNote()
	{
		GameObject gameObject = GameObject.Find("BillyNote");
		player.GetItem(Item.BillyNote);
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().color = Color.black;
	}

	private void GetCards()
	{
		GameObject gameObject = GameObject.Find("MonstermonComplete");
		player.GetItem(Item.MonstermonCardComplete);
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().color = Color.black;
	}

	public void GetNugget()
	{
		player.GetItem(Item.Nugget);
	}

	private void GetBug()
	{
		player.GetItem(Item.Bug);
	}

	public void GetPill()
	{
		player.GetItem(Item.Pill);
	}

	private void GetGlass()
	{
		player.GetItem(Item.MagnifyingGlass);
	}

	private void SendToOffice()
	{
		Teacher t = UnityEngine.Object.FindObjectOfType<Teacher>();
		t.SetAnimation("walk");
		player.inAnim = true;
		UI.CollapseDialogue();
		if (t.transform.position.x < 0f)
		{
			SetDirection(true);
		}
		t.transform.DOMove(new Vector3(-1.53f, -1.26f, -1.23f), 1.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetDirection(false);
			player.ForceChangeDirection(true);
			t.SetCurrentConversation(18);
			t.mySkeleton.AnimationName = "idle";
			t.Interact();
			player.inAnim = false;
		});
	}

	private void ActivateTalkToNuggetInCS1()
	{
		ActivateMission(Mission.NuggetStartCollectNuggets, Room.Recess);
		conversations[Room.Classroom1].currentConversation = 7;
		conversations[Room.Cafeteria].currentConversation = 45;
		conversations[Room.Recess].currentConversation = 44;
	}

	private void ActivateNuggetCubbyMission()
	{
		conversations[Room.Cafeteria].currentConversation = 45;
		CompleteMission(Mission.NuggetStartCollectNuggets);
		ActivateMission(Mission.NuggetGetCubbyNugget, Room.Classroom1);
	}

	private void SkipNuggetCubbyMission()
	{
		CompleteMission(Mission.NuggetStartCollectNuggets);
	}

	private void ActivateLilyNuggetMission()
	{
		CompleteMission(Mission.NuggetStartThirdNugget);
		ActivateMission(Mission.NuggetGiveLoveLetter, Room.Classroom1);
		player.GetItem(Item.LoveLetter);
	}

	private void ActivateLunchNugget()
	{
		GetNugget();
		CompleteMission(Mission.NuggetDeliveredLetter);
		ActivateMission(Mission.NuggetTalkAtLunch, Room.Cafeteria);
		conversations[Room.Cafeteria].currentConversation = 44;
	}

	private void DetonatePlayer()
	{
		if (!UI.IsMissionComplete(Mission.BuggsCheckForDistraction))
		{
			player.ExplodePlayer("It is very important to Nugget that you successfully collect the nuggets of friendship.", "Es muy importante para Nugget que tú recojas todos los nuggets de amistad.");
		}
		else if (EnvironmentController.currentRoom == Room.Cafeteria)
		{
			lastSelectedOption.DestinationID = 47;
			conversations[Room.Recess].currentConversation = 48;
		}
		else if (EnvironmentController.currentRoom == Room.Recess)
		{
			lastSelectedOption.DestinationID = 46;
		}
	}

	private void FailGiveLetter()
	{
		FailMission(Mission.NuggetGiveLoveLetter);
	}

	private void ActivateGetMagnifyingGlass()
	{
		ActivateMission(Mission.NuggetGetMagnifyingGlass, Room.Classroom1);
	}

	private void GiveMagnifyingGlass()
	{
		CompleteMission(Mission.NuggetGetMagnifyingGlass);
		player.UseItem(Item.MagnifyingGlass);
	}

	private void GetTowerBeetle()
	{
		player.GetItem(Item.KingTowerbeetle);
		UnlockHint("Monstermon", 4);
	}

	public bool GetPoisonedPlayer()
	{
		return mPoisonedPlayer;
	}

	private void GetPoisonNugget()
	{
		player.GetItem(Item.PoisonNugget);
	}

	private void EatPoisonNugget()
	{
		player.UseItem(Item.PoisonNugget);
		mPoisonedPlayer = true;
		if (EnvironmentController.currentRoom == Room.Classroom1)
		{
			conversations[Room.Cafeteria].currentConversation = 43;
			ActivateMission(Mission.NuggetJoinAtLunch, Room.Cafeteria);
		}
	}

	private void LosePoisonNugget()
	{
		player.UseItem(Item.PoisonNugget);
		FailMission(Mission.TeacherNuggetGainTrust);
	}

	private void ThrowSlop()
	{
		StartCoroutine(ThrowSlopCoroutine());
	}

	private void ActivateKillBuggs()
	{
		CompleteMission(Mission.NuggetTalkAtLunch);
		ActivateMission(Mission.NuggetKillBuggs, Room.Cafeteria);
	}

	private void ActivatePoisonBuggs()
	{
		CompleteMission(Mission.NuggetJoinAtLunch);
		ActivateMission(Mission.NuggetPoisonBuggs, Room.Cafeteria);
	}

	private IEnumerator ThrowSlopCoroutine()
	{
		Buggs b = UnityEngine.Object.FindObjectOfType<Buggs>();
		if (b.removedFromGame)
		{
			lastSelectedOption.DestinationID = 39;
			yield break;
		}
		GameObject sg = GameObject.Find("SlopGlob");
		sg.GetComponent<SpriteRenderer>().enabled = true;
		b.SetDirection(false);
		b.mySkeleton.timeScale = 1f;
		b.SetAnimation("throw");
		player.inAnim = true;
		yield return new WaitForSeconds(0.25f);
		sg.transform.DOMove(new Vector3(-6f, -8f, -20f), 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			if (mySkeleton.skeleton.flipX)
			{
				SetAnimation("HitInHeadFront");
			}
			else
			{
				SetAnimation("HitInHeadBack");
			}
			SFXManager.Instance.PlaySound("SlopSplat");
			ApplyTextureBlock(slop);
			sg.GetComponent<SpriteRenderer>().enabled = false;
			player.inAnim = false;
		});
		yield return new WaitForSeconds(0.25f);
		b.SetAnimation("idle");
		b.mySkeleton.timeScale = 1.5f;
		yield return new WaitForSeconds(0.58f);
		SetAnimation("idle");
	}

	private void BuggsFallOver()
	{
		UI.CompleteMission(Mission.TeacherGetBuggsInTrouble);
		UnityEngine.Object.FindObjectOfType<Buggs>().PukeAndDie();
		UnityEngine.Object.FindObjectOfType<Teacher>().conversations[Room.Recess].Nodes[0].Options[0].IsAvailable = true;
	}

	private void StabNugget()
	{
		SFXManager.Instance.PlaySound("GoreSplat2");
		ApplyTextureBlock(bloodySlop);
		player.UseItem(Item.Knife);
		GameObject gameObject = GameObject.Find("NuggetKnife");
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
	}

	private void ActivateNuggetCave()
	{
		CompleteMission(Mission.TeacherNuggetGainTrust);
		UnityEngine.Object.FindObjectOfType<Teacher>().ActivateMission(Mission.TeacherTellGainedTrust, Room.Recess);
	}

	private void ActivateNuggetAtRecess()
	{
		ActivateMission(Mission.NuggetTalkAtRecess, Room.Recess);
		CompleteMission(Mission.NuggetTellPoisonedBuggs);
		CompleteMission(Mission.NuggetTellStabbedBuggs);
		conversations[Room.Recess].currentConversation = 17;
	}

	public void CheckPoison()
	{
		EnvironmentController.Instance.RingBell();
		if (mPoisonedPlayer)
		{
			StartCoroutine(PlayerPoisoned());
			return;
		}
		LunchLady lunchLady = UnityEngine.Object.FindObjectOfType<LunchLady>();
		lunchLady.SetCurrentConversation(12);
		lunchLady.Interact();
	}

	private IEnumerator PlayerPoisoned()
	{
		player.inAnim = true;
		UI.showDeath = true;
		Renderer r = player.GetComponent<Renderer>();
		while (r.material.color.r > 0.8f)
		{
			r.material.color -= new Color(0.025f, 0f, 0.025f, 0f);
			yield return new WaitForSeconds(0.1f);
		}
		player.PlayAnimation("fallover");
		yield return new WaitForSeconds(1f);
		player.GetSkeleton().timeScale = 0f;
		UI.ShowDeath("You've been poisoned. That's bad.", "Te envenenaron. Esto está mal");
	}

	private void TakeAntidote()
	{
		CompleteMission(Mission.NuggetJoinAtLunch);
		CompleteMission(Mission.NuggetGetAntidote);
		mPoisonedPlayer = false;
		ActivateNuggetCave();
	}

	private void JumpIntoHole()
	{
		GameObject.Find("HoleCover").GetComponent<SpriteRenderer>().enabled = true;
		player.DisableGravity(base.transform.position.x < player.transform.position.x);
		StartCoroutine(FallIntoHole());
	}

	private void CheckForDevice()
	{
		if (player.HasItem(Item.Bug))
		{
			lastSelectedOption.DestinationID = -1;
			player.ExplodePlayer("Why did you pick up a second device?", "¿Por qué cogiste un segundo dispositivo?");
		}
	}

	private void GetKnifeHint()
	{
		UnlockHint("Buggs", 5);
	}

	private IEnumerator FallIntoHole()
	{
		player.inAnim = true;
		player.inDialogue = true;
		UI.showDeath = true;
		yield return new WaitForSeconds(3f);
		SFXManager.Instance.PlaySound("Thud");
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		yield return new WaitForSeconds(1f);
		UI.ShowDeath("Jumping into holes you can't see the bottom of is generally considered a bad idea.", "Saltar a los agujeros sin ver el fondo se considera generalmente una mala idea.");
	}

	private void GotRidOfKnife()
	{
		GameObject.Find("HoleCover").GetComponent<SpriteRenderer>().enabled = false;
		Buggs buggs = UnityEngine.Object.FindObjectOfType<Buggs>();
		DialogueNode currentConversation = buggs.GetCurrentConversation();
		currentConversation.Options[0].IsAvailable = true;
		currentConversation.Options[1].IsAvailable = false;
		currentConversation.Options[2].IsAvailable = false;
	}

	private void ThrewKnifeInHole()
	{
		StartCoroutine(ThrewKnifeInHoleCoroutine());
	}

	private void ThrowKnife()
	{
		player.UseItem(Item.BloodyKnife);
		GameObject gameObject = GameObject.Find("BloodyKnife");
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		Rigidbody2D component = gameObject.GetComponent<Rigidbody2D>();
		gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -7.15f);
		GameObject.Find("HoleCover").GetComponent<SpriteRenderer>().enabled = true;
		component.isKinematic = false;
		component.AddForce(new Vector2(0f, 400f));
		component.AddTorque(150f);
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -7.15f);
		StartCoroutine(DisableKnife(gameObject));
	}

	private IEnumerator DisableKnife(GameObject k)
	{
		yield return new WaitForSeconds(3f);
		k.SetActive(false);
	}

	private IEnumerator ThrewKnifeInHoleCoroutine()
	{
		player.inAnim = true;
		player.inDialogue = true;
		ThrowKnife();
		yield return new WaitForSeconds(2f);
		player.inAnim = false;
		player.inDialogue = true;
		SetEmptyOptions();
		StartCoroutine(RunDialogueSection("Guh...naaaaaaaa...~but...", "Guh...naaaaaaaa...~pero...", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(1f);
		StartCoroutine(RunDialogueSection("\\bRAAAAAA!!/b", "\\b¡¡RAAAAAA!!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		pCollider.enabled = false;
		SetAnimation("walk");
		player.inAnim = true;
		base.transform.DOMoveY(-7f, 1.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetAnimation("idle");
		})
			.OnUpdate(base.SetDepth);
		yield return new WaitForSeconds(1.5f);
		player.inAnim = false;
		StartCoroutine(RunDialogueSection("NUGGET SAID NO!!", "¡¡NUGGET DIJO NO!!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.showDeath = true;
		player.inAnim = true;
		UI.CollapseDialogue();
		SetAnimation("walk");
		base.transform.DOMove(player.transform.position, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetAnimation("idle");
			player.DisableGravity(base.transform.position.x < player.transform.position.x);
		});
		yield return new WaitForSeconds(3f);
		SFXManager.Instance.PlaySound("Thud");
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		yield return new WaitForSeconds(1f);
		UI.ShowDeath("No means no.", "No significa no.");
	}

	private void DropNuggetsInHole()
	{
		StartCoroutine(DropNuggets());
	}

	private IEnumerator DropNuggets()
	{
		player.inAnim = true;
		GameObject.Find("NuggetParticleSystem").GetComponent<ParticleSystem>().Play();
		GameObject.Find("HoleCover").GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds(3f);
		player.inAnim = false;
		Interact();
	}

	private void EnterNuggetCave()
	{
		player.DisableGravity(base.transform.position.x < player.transform.position.x);
		StartCoroutine(EnteringCave());
	}

	private IEnumerator EnteringCave()
	{
		player.inAnim = true;
		yield return new WaitForSeconds(2f);
		EnvironmentController.Instance.ChangeEnvironment(Room.NuggetCave);
		yield return new WaitForSeconds(1f);
		player.inAnim = false;
		player.ResetGravity();
	}

	private void GiveBugToNugget()
	{
		player.UseItem(Item.Bug);
		StartCoroutine(GiveBugToNuggetCoroutine(true));
	}

	private void RetrieveBug()
	{
		StartCoroutine(GiveBugToNuggetCoroutine(false));
	}

	private IEnumerator GiveBugToNuggetCoroutine(bool g)
	{
		player.inAnim = true;
		SetDirection(false);
		SetAnimation("walk");
		pCollider.enabled = false;
		base.transform.DOMoveX(-23f, 0.75f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(0.75f);
		SetDirection(true);
		GameObject.Find("RecessBug").GetComponent<SpriteRenderer>().enabled = g;
		base.transform.DOMoveX(-11.41f, 0.75f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(0.75f);
		SetDirection(base.transform.position.x > player.transform.position.x);
		SetAnimation("idle");
		pCollider.enabled = true;
		player.inAnim = false;
		Interact();
		if (!g)
		{
			CompleteMission(Mission.NuggetTalkAtRecess);
			ActivateMission(Mission.NuggetPlaceDevice);
			player.GetItem(Item.Bug);
		}
	}

	private void PlaceBug()
	{
		player.UseItem(Item.Bug);
		CompleteMission(Mission.NuggetPlaceDevice);
		ActivateMission(Mission.NuggetTalkToLily, Room.Recess);
	}

	private void GetFinalNugget()
	{
		GetNugget();
		CompleteMission(Mission.NuggetGetLastNugget);
		CompleteMission(Mission.NuggetCollectNuggets);
	}

	private void TakeAllNuggets()
	{
		for (int i = 0; i < 5; i++)
		{
			player.UseItem(Item.Nugget);
		}
	}

	private void TakeNugget()
	{
		player.UseItem(Item.Nugget);
	}

	public void CheckRemoveNuggetPiles()
	{
		if (player.GetItemCount(Item.Nugget) >= 5)
		{
			GameObject.Find("NuggetPileCoverMemorial").SetActive(false);
			GameObject.Find("NuggetPileCoverSign").SetActive(false);
		}
		Interact();
	}

	private void GetCaveCard()
	{
		player.GetItem(Item.WizardWorm);
		UnlockHint("Monstermon", 12);
		GameObject gameObject = GameObject.Find("NuggetCaveCard");
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}

	private void PlaceFlower()
	{
		player.UseItem(Item.Flower);
		GameObject.Find("NuggetCaveFlower").GetComponent<SpriteRenderer>().enabled = true;
		player.inAnim = true;
		player.ForceChangeDirection(false);
		SetAnimation("walk");
		base.transform.DOMove(new Vector3(-3.75f, -3f, -7.9f), 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.inAnim = false;
			SetCrying();
			player.ForceChangeDirection(true);
			SetAnimation("idle");
		});
		SetEndDay(true);
	}

	private void SetCrying()
	{
		ApplyTextureBlock(crying);
	}

	private void BellRingMemorial()
	{
		SFXManager.Instance.PlaySound("Bell");
		StartCoroutine(ExitNuggetCaveToCS2());
	}

	private void StartMemorial()
	{
		UnlockHint("Nugget", 6);
		SetCurrentConversation(2);
		Interact();
	}

	private void StartSignPost()
	{
		SetCurrentConversation(3);
		Interact();
	}

	private void StartDog()
	{
		SetCurrentConversation(9);
		Interact();
	}

	private IEnumerator ExitNuggetCaveToCS2()
	{
		player.inAnim = true;
		yield return new WaitForSeconds(1f);
		player.inAnim = false;
		StartCoroutine(RunDialogueSection("That is the bell. We must depart from the Nugget Cave. I will see you again soon.", "Está sonando el timbre. Debemos irnos de la Cueva de Nugget. Te veré pronto.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom2);
		yield return new WaitForSeconds(1f);
		if (UI.ReceivedMission(Mission.NuggetKillBuggs))
		{
			ApplyTextureBlock(slop);
		}
	}

	private void GetDog()
	{
		player.GetItem(Item.CindyDog);
		GameObject.Find("CindyDog").SetActive(false);
		if (UI.IsMissionActive(Mission.TeacherCollectEvidence))
		{
			CompleteMission(Mission.TeacherCollectEvidence);
			ActivateMission(Mission.TeacherShowEvidence);
		}
	}

	private void ExitToRecess()
	{
		EnvironmentController.Instance.SetSpawnPoint(1);
		EnvironmentController.Instance.ChangeEnvironment(Room.Recess);
	}

	public void EndNuggetCave()
	{
		StartCoroutine(EndNuggetCaveDefault());
	}

	private IEnumerator EndNuggetCaveDefault()
	{
		player.inAnim = true;
		yield return new WaitForSeconds(0.25f);
		player.inAnim = false;
		EnvironmentController.Instance.RingBell();
		SetCurrentConversation(11);
		Interact();
	}

	private void LeaveCave()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom2);
	}

	private void PlaceCards()
	{
		SFXManager.Instance.StopMusic();
		player.UseItem(Item.MonstermonCardComplete);
		GameObject.Find("beam").GetComponent<SpriteRenderer>().DOColor(Color.red, 1f);
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(1f);
		mEndWorld = true;
	}

	public override IEnumerator EndDayCoroutine()
	{
		Interact();
		SetCrying();
		yield return null;
	}

	private void GetNote()
	{
		player.GetItem(Item.BillyNote);
	}

	private void WalkAwayEndDay()
	{
		SteamScript.UnlockAchievement("NuggetAchievement");
		SetDirection(false);
		WalkToPoint(new Vector3(-36f, UnityEngine.Random.Range(-2f, -1.5f), -1.14f), 7f, false);
		UI.CompleteDay(Item.BillyNote);
		UnityEngine.Object.FindObjectOfType<PauseMenu>().UnlockAllHints("Nugget");
	}

	public void CheckEndWorld()
	{
		if (mEndWorld)
		{
			StartCoroutine(RunEndWorld());
		}
	}

	private IEnumerator RunEndWorld()
	{
		UI.showDeath = true;
		player.inAnim = true;
		GameObject.Find("Recess").GetComponent<SpriteRenderer>().sprite = recessRed;
		GameObject ps = GameObject.Find("PrincipalShooting");
		ps.GetComponent<MeshRenderer>().enabled = true;
		SFXManager.Instance.PlayMusic("EndWorld");
		Principal p = UnityEngine.Object.FindObjectOfType<Principal>();
		Teacher t = UnityEngine.Object.FindObjectOfType<Teacher>();
		Cindy c = UnityEngine.Object.FindObjectOfType<Cindy>();
		Jerome je = UnityEngine.Object.FindObjectOfType<Jerome>();
		Janitor ja = UnityEngine.Object.FindObjectOfType<Janitor>();
		Monty j = UnityEngine.Object.FindObjectOfType<Monty>();
		Lily i = UnityEngine.Object.FindObjectOfType<Lily>();
		t.transform.position = new Vector3(21.8f, -4.3f, -4.37f);
		ps.transform.position = new Vector3(29.6f, -7.85f, -7.8f);
		ja.transform.position = new Vector3(24.8f, -13.1f, -13.26f);
		Transform bolt = GameObject.Find("LightningBolt").transform;
		CameraFollow cf = UnityEngine.Object.FindObjectOfType<CameraFollow>();
		cf.overrideCamera = true;
		cf.transform.DOMoveX(cf.maxPosX, 2f);
		yield return new WaitForSeconds(3f);
		player.inAnim = false;
		StartCoroutine(RunDialogueSection("What's going on out here?!~ The sky has turned red!", "¡¿Qué está pasando aquí?!~ ¡El cielo se ha puesto rojo!", p));
		while (pEmptyOptions)
		{
			yield return null;
		}
		t.SetDirection(true);
		StartCoroutine(RunDialogueSection("Oh, that's actually happening? I thought it was just the pills...~ Hey why do you have your gun out?", "¿En serio ésto está pasando? Creí que era el efecto de las pastillas…~¿Oye, por qué sacas la pistola?", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("The sky's turned red. It could be an emergency.", "El cielo se está poniendo rojo. Podría ser una emergencia.", p));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("What are you going to do? Shoot the sky?", "¿Qué vas a hacer? ¿Dispararle al cielo?", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		c.SetDirection(true);
		cf.transform.DOMoveX(cf.minPosX, 1f);
		yield return new WaitForSeconds(1f);
		t.SetDirection(false);
		StartCoroutine(RunDialogueSection("Ms. Applegate...~I don't feel good. Can I go to the nu--", "Señora Applegate...~No me siento bien. Puedo irme a la enfe—", c));
		while (pEmptyOptions)
		{
			yield return null;
		}
		ExplodeCharacter(GameObject.Find("CindyExplosion").transform, c);
		yield return null;
		yield return null;
		yield return null;
		bolt.transform.position = new Vector3(-50f, 0f, 0f);
		yield return new WaitForSeconds(1f);
		t.SetDirection(false);
		cf.transform.DOMoveX(cf.maxPosX, 2f);
		if (!j.removedFromGame)
		{
			StartCoroutine(RunDialogueSection("Oh goodness! I think the red sky is an answer to my prayers!~ Do Monty next!", "¡Ay, por Dios! ¡Creo que el cielo rojo es la respuesta a mis oraciones! ~ ¡El siguiente es Monty!", t));
			while (pEmptyOptions)
			{
				yield return null;
			}
			j.SetDirection(true);
			StartCoroutine(RunDialogueSection("Hey, what the hell?! I didn't do noth--", "¡¿Hey, que diablos?! Yo no hice na—", j));
			while (pEmptyOptions)
			{
				yield return null;
			}
			ExplodeCharacter(GameObject.Find("MontyExplosion").transform, j);
			yield return null;
			yield return null;
			yield return null;
			bolt.position = new Vector3(50f, 0f, 0f);
			StartCoroutine(RunDialogueSection("Hell yeah! Now do Jerome!", "¡Al diablo, eh! ¡Sigue Jerome!", t));
			while (pEmptyOptions)
			{
				yield return null;
			}
		}
		else
		{
			StartCoroutine(RunDialogueSection("Oh goodness! I think the red sky is an answer to my prayers!~ Do Jerome next!", "¡Ay, Dios mío! ¡Creo que el cielo rojo es la respuesta a mis oraciones! ~ ¡El siguiente es  Jerome!", t));
			while (pEmptyOptions)
			{
				yield return null;
			}
		}
		je.SetDirection(true);
		StartCoroutine(RunDialogueSection("What?! No!~ Dad, stop her!!", "¡¿Qué?! ¡No!~ ¡¡Papi, deténla!!", je));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I don't think she has any say in what--", "No creo que ella tenga voz en lo—", p));
		while (pEmptyOptions)
		{
			yield return null;
		}
		ExplodeCharacter(GameObject.Find("TeacherExplosion").transform, t);
		yield return null;
		yield return null;
		yield return null;
		bolt.position = new Vector3(50f, 0f, 0f);
		StartCoroutine(RunDialogueSection("\\bAAAAAHHH!!/b", "\\b¡¡AAAAAHHH!!/b", je));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Woah...~I thought that was gonna be me.~ So what now?! We're all gonna die?!", "Wow...~pensé que seguía yo. ~ ¡¿Y ahora qué?! ¡¿Todos moriremos", je));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("It appears so. All the child kidnapping and pill dealing and for what?~ All for the apocalypse to come and ruin it all.", "Parece que sí. ¿Todo el secuestro de niños y las pastillas para qué?~ Todo para que el apocalipsis viniera y arruinara todo.", p));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("What are you talking about?", "¿De qué estás hablando?", je));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I'll tell you when you're older--~ well actually I won't considering the situation.", "Te diré cuando seas grande--~ bueno, de hecho no voy a considerar la situación.", p));
		while (pEmptyOptions)
		{
			yield return null;
		}
		ExplodeCharacter(GameObject.Find("JeromeExplosion").transform, je);
		yield return null;
		yield return null;
		yield return null;
		bolt.position = new Vector3(50f, 0f, 0f);
		yield return new WaitForSeconds(0.5f);
		ExplodeCharacter(GameObject.Find("PrincipalExplosion").transform, p);
		yield return null;
		yield return null;
		yield return null;
		bolt.position = new Vector3(50f, 0f, 0f);
		StartCoroutine(RunDialogueSection("Well Mr. Sweepy, it looks like this is the end.", "Está bien, Señor Sweepy, parece que se acabó.", ja));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Lord knows I wouldn't want to clean this mess up anyway.", "Dios sabe que no quisiera limpiar todo este desastre.", ja));
		while (pEmptyOptions)
		{
			yield return null;
		}
		ExplodeCharacter(GameObject.Find("JanitorExplosionEndWorld").transform, ja);
		yield return null;
		yield return null;
		yield return null;
		bolt.position = new Vector3(50f, 0f, 0f);
		yield return new WaitForSeconds(0.5f);
		if (!i.removedFromGame)
		{
			StartCoroutine(RunDialogueSection("I'm coming Billy...", "Ya voy, Billy.", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I'm coming.", "Ya voy.", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			ExplodeCharacter(GameObject.Find("LilyExplosion").transform, i);
			yield return null;
			yield return null;
			yield return null;
			bolt.position = new Vector3(50f, 0f, 0f);
		}
		UI.CollapseDialogue();
		player.inAnim = true;
		yield return new WaitForSeconds(2f);
		SFXManager.Instance.PlayMusic("TitleMusic");
		cf.transform.DOMoveX(player.transform.position.x, 1f);
		yield return new WaitForSeconds(1f);
		cf.overrideCamera = false;
		SetCurrentConversation(36);
		Interact();
		player.ForceChangeDirection(false);
	}

	public void ExplodeCharacter(Transform t, NPCBehavior n)
	{
		if (t.name != "PrincipalExplosion")
		{
			t.transform.position = n.transform.position;
		}
		else
		{
			t.transform.position = GameObject.Find("PrincipalShooting").transform.position;
			GameObject.Find("PrincipalShooting").GetComponent<MeshRenderer>().enabled = false;
		}
		n.RemoveFromGame();
		t.GetComponent<ParticleSystem>().Play();
		GameObject.Find("LightningBolt").transform.position = n.transform.position + Vector3.up * 10f;
		SFXManager.Instance.PlaySound("Bang");
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.25f, true);
		int num = 0;
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				Rigidbody2D component = transform.GetComponent<Rigidbody2D>();
				component.isKinematic = false;
				component.gravityScale = 2f;
				switch (num)
				{
				case 0:
					component.AddForce(new Vector2(UnityEngine.Random.Range(-100, 100), 700f));
					component.AddTorque(UnityEngine.Random.Range(-100, 100));
					break;
				case 1:
					component.AddForce(new Vector2(UnityEngine.Random.Range(-300, 300), 600f));
					component.AddTorque(UnityEngine.Random.Range(-100, 100));
					break;
				default:
					component.AddForce(new Vector2(UnityEngine.Random.Range(-300, 300), 400f));
					component.AddTorque(UnityEngine.Random.Range(-100, 100));
					break;
				}
				num++;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	private void PlayerWalkAway()
	{
		player.inAnim = true;
		player.ForceAnim(true);
		StartCoroutine(PlayerWalkAwayCoroutine());
	}

	private IEnumerator PlayerWalkAwayCoroutine()
	{
		player.transform.DOMove(new Vector3(-11.41f, -7.65f, -7.65f), 1f);
		player.GetComponent<BoxCollider2D>().isTrigger = true;
		yield return new WaitForSeconds(1f);
		player.transform.DOMoveX(-36f, 3.5f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(3.5f);
		Interact();
	}

	private void FinishGame()
	{
		StartCoroutine(FinishGameCoroutine());
	}

	private IEnumerator FinishGameCoroutine()
	{
		PlayerPrefs.SetInt("WorldEnder" + EnvironmentController.Instance.GetFileModifier(), 1);
		EnvironmentController.ShowHoleCover(true);
		BoxCollider2D b = GetComponent<BoxCollider2D>();
		Rigidbody2D r = base.gameObject.AddComponent<Rigidbody2D>();
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -7.15f);
		base.gameObject.layer = 9;
		b.offset = new Vector2(0f, 1.25f);
		b.size = new Vector2(1.26f, 2.5f);
		r.AddTorque(-100f);
		r.AddForce(Vector2.up * 20f);
		yield return new WaitForSeconds(1.5f);
		RemoveFromGame();
		yield return new WaitForSeconds(1.5f);
		SFXManager.Instance.PlaySound("Thud");
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		GameObject.Find("FinishGamePanel").GetComponent<CanvasGroup>().DOFade(1f, 1f);
		yield return new WaitForSeconds(5f);
		GameObject.Find("FinishGamePanel").GetComponent<CanvasGroup>().DOFade(0f, 1f);
		PlayerPrefs.SetInt("SetTuesday", 1);
		SteamScript.UnlockAchievement("WorldEnder");
		SceneManager.LoadScene("Kindergarten");
	}
}
