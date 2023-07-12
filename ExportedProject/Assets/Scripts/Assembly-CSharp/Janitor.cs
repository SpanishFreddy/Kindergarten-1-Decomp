using System;
using System.Collections;
using DG.Tweening;
using DialogueTree;
using UnityEngine;

public class Janitor : NPCBehavior
{
	public TextureBlock bloody;

	public TextureBlock ear;

	public Sprite cleanBathroomSprite;

	private bool walkingBackFromToilet;

	private bool stall1Check;

	private bool stall2Check;

	private int mStoredConvo;

	private bool mUnscrewedShelf;

	public void KillPlayer()
	{
		StartCoroutine(KillPlayerCoroutine("Don't take the janitor's warnings lightly.", "No tome las advertencias del portero ligeramente."));
		pCollider.enabled = false;
	}

	public void KillPlayer(string s, string span)
	{
		StartCoroutine(KillPlayerCoroutine(s, span));
		pCollider.enabled = false;
	}

	private IEnumerator KillPlayerCoroutine(string s, string span)
	{
		player.inAnim = true;
		UI.showDeath = true;
		ParticleSystem p = GameObject.Find("PlayerBloodSplat").GetComponent<ParticleSystem>();
		SetAnimation("attack");
		player.GetSkeleton().timeScale = 0.66f;
		yield return new WaitForSeconds(0.2667f);
		player.PlayAnimation("hit");
		for (int i = 0; i < 3; i++)
		{
			SFXManager.Instance.PlaySound("BasicHit");
			yield return new WaitForSeconds(0.5f);
			player.ApplyTextureBlock(player.bruisedBloody);
		}
		player.PlayAnimation("writhing");
		player.GetSkeleton().timeScale = 0.1f;
		yield return new WaitForSeconds(0.5f);
		SetAnimation("walk");
		if (base.transform.position.x > player.transform.position.x)
		{
			base.transform.DOMove(player.transform.position + Vector3.right + Vector3.up * 0.1f, 0.5f).SetEase(Ease.Linear);
		}
		else
		{
			p.transform.localPosition -= 2f * Vector3.right * p.transform.localPosition.x;
			base.transform.DOMove(player.transform.position - Vector3.right + Vector3.up * 0.1f, 0.5f).SetEase(Ease.Linear);
		}
		yield return new WaitForSeconds(0.5f);
		SetDepth();
		SetAnimation("attack");
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
				UI.ShowDeath(s, span);
			}
		}
		SetAnimation("idle");
	}

	public void EnableToiletConversation()
	{
		conversations[Room.Bathroom].Nodes[0].Options[1].SetAvailable();
	}

	private void GetBuck()
	{
		player.GetMoney(1f);
	}

	private void CheckEndBathroom()
	{
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			lastSelectedOption.DestinationID = 71;
		}
	}

	public void CheckStall1()
	{
		stall1Check = true;
		if (stall2Check)
		{
			conversations[Room.Bathroom].Nodes[23].Options[0].SetAvailable();
			conversations[Room.Bathroom].Nodes[56].Options[0].SetAvailable();
		}
	}

	public void CheckStall2()
	{
		stall2Check = true;
		if (stall1Check)
		{
			conversations[Room.Bathroom].Nodes[23].Options[0].SetAvailable();
			conversations[Room.Bathroom].Nodes[56].Options[0].SetAvailable();
		}
	}

	public void WalkToStall()
	{
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			lastSelectedOption.DestinationID = 69;
			return;
		}
		GameObject.Find("BathroomBoxes").GetComponent<Collider2D>().enabled = false;
		GameObject.Find("BathroomBox1").GetComponent<Collider2D>().enabled = true;
		GameObject.Find("BathroomBox2").GetComponent<Collider2D>().enabled = true;
		GameObject.Find("BathroomBox3").GetComponent<Collider2D>().enabled = true;
		SetDirection(true);
		WalkToPoint(new Vector3(25f, -3.21f), 3f);
		StartCoroutine(CloseJanDialogue());
	}

	private IEnumerator CloseJanDialogue()
	{
		StartCoroutine(RunDialogueSection("Stupid kid...~making me clean the stupid toilet...~with my stupid mop...~cause it's my stupid job.", "Estúpido niño...~haciendome limpiar el estúpido baño...~con mi estúpido trapeador...~porque es mi estúpido trabajo.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		conversations[Room.Bathroom].currentConversation = 26;
		SFXManager.Instance.PlaySound("Confirm");
		UI.CollapseDialogue();
		StartCoroutine(WaitWalkBack());
	}

	private IEnumerator WaitWalkBack()
	{
		EnvironmentController.Instance.waitForNextAction = true;
		while (EnvironmentController.Instance.waitForNextAction && EnvironmentController.currentRoom == Room.Bathroom)
		{
			yield return null;
		}
		GameObject.Find("BathroomBoxes").GetComponent<Collider2D>().enabled = true;
		GameObject.Find("BathroomBox1").GetComponent<Collider2D>().enabled = false;
		GameObject.Find("BathroomBox2").GetComponent<Collider2D>().enabled = false;
		GameObject.Find("BathroomBox3").GetComponent<Collider2D>().enabled = false;
		SetDirection(false);
		GameObject.Find("Bathroom").GetComponent<SpriteRenderer>().sprite = cleanBathroomSprite;
		GameObject.Find("Toilet").GetComponent<ObjectInteractable>().GetDialogue()
			.currentConversation = 1;
		if (EnvironmentController.currentRoom == Room.Bathroom)
		{
			walkingBackFromToilet = true;
			WalkToPoint(new Vector3(-12.4f, -8.3f, -8.4f), 3f);
			yield return new WaitForSeconds(3f);
			if (EnvironmentController.Instance.ActionsLeft() == 1)
			{
				SetCurrentConversation(68);
			}
			else
			{
				SetCurrentConversation(20);
				walkingBackFromToilet = false;
			}
			Interact();
		}
		else
		{
			conversations[Room.Bathroom].currentConversation = 20;
		}
	}

	private void GetCardBathroom()
	{
		UnlockHint("Monstermon", 16);
		player.GetItem(Item.EyeOfTheButtholder);
	}

	public void GetNickel()
	{
		player.GetMoney(0.05f);
	}

	public void GetFive()
	{
		player.GetMoney(5f);
	}

	private void GetOglebopGolem()
	{
		player.GetItem(Item.OglebopGolem);
		UnlockHint("Monstermon", 15);
		GameObject.Find("OglebopGolemTrigger").GetComponent<BoxCollider2D>().enabled = false;
	}

	private void EnableCheapChocolate()
	{
		LunchLady lunchLady = UnityEngine.Object.FindObjectOfType<LunchLady>();
		UnlockHint("Monty", 3);
		lunchLady.conversations[Room.Cafeteria].Nodes[18].Options[2].IsAvailable = true;
		lunchLady.conversations[Room.Cafeteria].Nodes[21].Options[1].IsAvailable = true;
	}

	public void CheckBoxes()
	{
		mStoredConvo = conversations[Room.Bathroom].currentConversation;
		SetCurrentConversation(62);
		Interact();
	}

	private void ResetConvo()
	{
		conversations[Room.Bathroom].currentConversation = mStoredConvo;
	}

	private void KillPlayerAt5()
	{
		KillPlayer("I would catch his drift next time.", "La próxima vez lo entenderé.");
	}

	private void GetFinger()
	{
		player.GetItem(Item.SeveredFinger);
		if (UI.IsMissionActive(Mission.LilyFindEvidence))
		{
			CompleteMission(Mission.LilyFindEvidence);
			UnityEngine.Object.FindObjectOfType<Lily>().ActivateMission(Mission.LilyShowEvidence);
		}
	}

	private void GetCardJeromePass()
	{
		player.GetItem(Item.ManWithLongArm);
		UnlockHint("Monstermon", 17);
	}

	private void GetShoe()
	{
		player.GetItem(Item.Shoe);
		if (UI.IsMissionActive(Mission.LilyFindEvidence))
		{
			CompleteMission(Mission.LilyFindEvidence);
			UnityEngine.Object.FindObjectOfType<Lily>().ActivateMission(Mission.LilyShowEvidence);
		}
	}

	public void EndBathroom()
	{
		if (!walkingBackFromToilet)
		{
			SetCurrentConversation(63);
			Interact();
		}
	}

	private void GoBackToClass()
	{
		EnvironmentController.Instance.SetSpawnPoint(1);
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom1);
	}

	public void EndBathroomCafeteria()
	{
		base.transform.position = new Vector3(-23.34f, -0.6f, 0f);
		SetDepth();
		SetDirection(true);
		player.ForceChangeDirection(false);
		SetCurrentConversation(64);
		Interact();
	}

	private void BathroomCafeteria()
	{
		StartCoroutine(RunBathroomCafeteria());
	}

	private IEnumerator RunBathroomCafeteria()
	{
		WalkToPoint(player.transform.position - Vector3.right * 3f, 2f);
		StartCoroutine(RunDialogueSection("Fat chance! You and the principal's boy have been acting strangely since my closet got broken into.~ Empty your pockets! Now!", "¡Gran oportunidad! Tú y el hijo del director han estado actuando raro desde que mi aramario fue asaltado. ~ ¡Vacien sus bolsillos! ¡Ahora!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		if (player.HasItem(Item.LaserPointer))
		{
			SetCurrentConversation(66);
		}
		else
		{
			SetCurrentConversation(65);
		}
		Interact();
	}

	private void KillPlayerLaserBathroom()
	{
		KillPlayer("The janitor comes to get the trash after the bell rings.", "El conserje viene a recoger la basura después de que suena la campana.");
	}

	private void GoToRecess()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Recess);
	}

	private void ExitBathroom()
	{
		EnvironmentController.Instance.SetSpawnPoint(4);
		EnvironmentController.Instance.ChangeEnvironment(Room.Hallway);
	}

	private void GetBiscuitBall()
	{
		player.GetItem(Item.BiscuitBall);
	}

	private void KillMonty()
	{
		conversations[Room.Cafeteria].Nodes[0].Options[1].IsAvailable = false;
		StartCoroutine(GoKillMonty());
	}

	private IEnumerator GoKillMonty()
	{
		pCollider.enabled = false;
		Vector3 origin = base.transform.position;
		Monty i = UnityEngine.Object.FindObjectOfType<Monty>();
		StartCoroutine(RunDialogueSection("Ugh. Is that the dweeb with the glasses?~ Who am I kidding? Of course it is.", "Iugh. ¿Aquel es el bobo con lentes?~ ¿A quién engaño? Por supuesto que es él.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(true);
		WalkToPoint(new Vector3(11.7f, -10f, -10.5f), 3f);
		player.ForceChangeDirection(true);
		i.SetDirection(false);
		StartCoroutine(RunDialogueSection("Hey!~ Nerd!", "¡Hey! ~ ¡nerdo!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Ummm...~what do you want? Got some janitor stuff you want to sell me?", "Ummm...~ ¿Qué quieres? ¿Tienes alguna cosa del conserje que quieras venderme?", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("No, but I got a coffin for you to buy!", "¡No, pero tengo un ataúd para que compres!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetAnimation("attack");
		yield return new WaitForSeconds(0.25f);
		i.SetAnimation("fallover");
		i.ApplyTextureBlock(i.noGlasses);
		SFXManager.Instance.PlaySound("BasicHit");
		GameObject g = GameObject.Find("MontyGlasses");
		g.GetComponent<SpriteRenderer>().enabled = true;
		g.GetComponent<Rigidbody2D>().simulated = true;
		g.GetComponent<Rigidbody2D>().AddForce(new Vector2(-300f, 500f));
		g.GetComponent<Rigidbody2D>().AddTorque(50f);
		player.inAnim = true;
		yield return new WaitForSeconds(0.25f);
		SetAnimation("walk");
		base.transform.DOMove(new Vector3(13.5f, -10f, -9.5f), 1f).SetEase(Ease.Linear).OnComplete(delegate
		{
			i.SetAnimation("writhing");
			SetAnimation("attack");
			player.inAnim = false;
		});
		StartCoroutine(RunDialogueSection("Let this be a lesson to you about correcting people's spelling!", "¡Dejemos ésto como una lección para ti sobre corregir el deltreo de las personas!", this));
		ParticleSystem p = GameObject.Find("GeneralBloodSplat").GetComponent<ParticleSystem>();
		ParticleSystem ps = GameObject.Find("CafeteriaBloodStream").GetComponent<ParticleSystem>();
		p.transform.position = new Vector3(16f, -10f, -14f);
		yield return new WaitForSeconds(1.2667f);
		ps.transform.localPosition = new Vector3(16.25f, -9.19f, -15f);
		p.Play();
		SFXManager.Instance.PlaySound("GoreSplat" + UnityEngine.Random.Range(2, 4));
		yield return new WaitForSeconds(0.2333f);
		ApplyTextureBlock(bloody);
		i.ApplyTextureBlock(i.bloody);
		while (pEmptyOptions)
		{
			yield return new WaitForSeconds(0.2667f);
			p.Play();
			SFXManager.Instance.PlaySound("GoreSplat" + UnityEngine.Random.Range(2, 4));
			yield return new WaitForSeconds(0.2333f);
		}
		ps.Play();
		SetAnimation("idle");
		i.mySkeleton.timeScale = 0.5f;
		StartCoroutine(RunDialogueSection("There...~lessons were learned. Lines were drawn.", "Ahí...~lecciones aprendidas. La guerra está declarada.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		SetDirection(false);
		i.SetDead();
		UI.CompleteMission(Mission.TeacherGetMontyInTrouble);
		SetCurrentConversation(12);
		i.SetCurrentConversation(6);
		i.ResizeCollider();
		g.layer = 0;
		g.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
		player.inAnim = true;
		WalkToPoint(g.transform.position + Vector3.right * 0.5f, 1f, true);
		yield return new WaitForSeconds(1.5f);
		g.SetActive(false);
		WalkToPoint(origin, 2f, true);
		yield return new WaitForSeconds(2f);
		Interact();
		player.inAnim = false;
	}

	private void GetGlasses()
	{
		player.GetItem(Item.MontyGlasses);
	}

	private void StopWait()
	{
		EnvironmentController.Instance.waitForNextAction = false;
		player.SetInteractable(null);
	}

	public IEnumerator AppearAndSearch()
	{
		EnvironmentController.Instance.waitForNextAction = true;
		LunchLady j = UnityEngine.Object.FindObjectOfType<LunchLady>();
		Jerome i = UnityEngine.Object.FindObjectOfType<Jerome>();
		SetDirection(false);
		UnlockHint("Jerome", 4);
		while (EnvironmentController.Instance.waitForNextAction)
		{
			yield return null;
		}
		base.transform.position = new Vector3(19.81f, -0.87f, -0.97f);
		player.inDialogue = true;
		player.ForceChangeDirection(player.transform.position.x < base.transform.position.x);
		StartCoroutine(RunDialogueSection("Well?!~ Anyone want to step forward?~~ No?~ Well what a surprise!", "¡¿Entonces?!~ ¿Alguien quiere dar un paso al frente? ~~ ¿No?~ ¡Qué sorpresa!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		WalkToPoint(new Vector3(16.94f, -3.4f, -3.5f), 1f);
		StartCoroutine(RunDialogueSection("What about you, little junior principal? I took your lasery thingy this morning. I'm sure you want it back!", "¿Y qué tal tú, directorcito?  Yo tomé tu cosita de lasercito esta mañana. ¡Seguro la quieres de vuelta!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Search me man.~ I don't have it, so screw off or I'll tell my dad you peed in the water fountain again.", "A mí que me registren. ~ Yo no lo tengo, así que jódete o le diré a mi padre que otra vez orinaste la fuente de agua.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("...~You win this round kid.", "...~Ganaste esta ronda, niño.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		if (player.transform.position.x > base.transform.position.x)
		{
			player.ForceChangeDirection(false);
			SetDirection(true);
			WalkToPoint(player.transform.position - new Vector3(2.5f, -0.1f, 0f), 1f);
		}
		else
		{
			player.ForceChangeDirection(true);
			SetDirection(false);
			WalkToPoint(player.transform.position + new Vector3(2.5f, 0.1f, 0f), 1f);
		}
		StartCoroutine(RunDialogueSection("What about you?~ You been in my closet?~ Turn out your pockets.", "¿Y tú?~ ¿Has estado en mi armario?~ Vacia tus bolcillos.", this));
		yield return new WaitForSeconds(1f);
		pCollider.enabled = false;
		while (pEmptyOptions)
		{
			yield return null;
		}
		if (player.HasItem(Item.LaserPointer))
		{
			StartCoroutine(RunDialogueSection("The lasery thingy! I knew you had it!", "¡La cosita de lasercito! ¡Sabía que la tenías!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			KillPlayer("Hide the laser pointer before the janitor comes to the cafeteria.", "Oculta el puntero láser antes de que el conserje llegue a la cafetería.");
			yield break;
		}
		StartCoroutine(RunDialogueSection("Hmmmm...nothin.' I still got my eye on you.", "Hmmmm...nada.' Te sigo vigilando.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Oh just leave the poor things alone! You're always lookin' for a reason to impale a child on that mop of yours!", "¡Oh, deja en paz a esta pobre criaturita! ¡Tú siempre estás buscando una razón para empalar a un niño con ese trapeador!", j));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("So what if I am?~ Ain't nothin' wrong with impaling a child!", "¿Y qué sí lo hago?~ ¡No hay nada malo con empalar a un niño!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Just go sell your mystery meat!", "¡Vete a vender tu carne misteriosa!", j));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Fine! But mark my words...someone will pay for going into my closet.", "¡Perfecto! Pero recuerda mis palabras…..Alguien pagará por meterse en mi armario.", this));
		WalkToPoint(new Vector3(-6f, -5.4f, -5.5f), 1.5f);
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Actually before you get started, that garbage can needs emptying. Go take care of it.", "De hecho, antes de que empieces ese bote de basura necesita ser vaciado. Así que encárgate de eso.", j));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(true);
		WalkToPoint(new Vector3(3.18f, -1.33f, -1.44f), 1f);
		StartCoroutine(RunDialogueSection("*Grumble grumble*~ Stupid lunch lady...~making me do my stupid job.", "*Refunfuña*~ Cocinera estúpida...~me hizo hacer mi estúpido trabajo.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		base.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
		WalkToPoint(new Vector3(18.35f, -1.33f, -1.44f), 1.5f);
		yield return new WaitForSeconds(1.5f);
		WalkToPoint(new Vector3(20.11f, -0.3f, -0.4f), 0.5f);
		yield return new WaitForSeconds(0.5f);
		base.transform.position = new Vector3(0f, 50f, 0f);
		yield return new WaitForSeconds(2f);
		base.transform.position = new Vector3(20.11f, -1.5f, -0.4f);
		SetDirection(false);
		base.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
		WalkToPoint(new Vector3(1.67f, -3.5f, -3.6f), 2f);
		StartCoroutine(RunDialogueSection("Well that was fast! There's no way you took it all the way to the dumpster!", "¡Eso fue rápido! ¡Es imposible que lo lleves todo el camino hasta el contenedor de basura!", j));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Would you relax, woman!~ \\hI dropped it off in the bathroom!/h I'll get it after the bell rings!", "¡Relájate, mujer!~ \\h  ¡Lo dejé en el baño!/h ¡Lo recogeré después de que suene la campana!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		WalkToPoint(new Vector3(-19.67f, -10.41f, -10.52f), 2f);
		StartCoroutine(RunDialogueSection("Sheesh! Old broad is just mad the kids like my food better than hers!", "¡Uuuuy! ¡La vieja se volvió loca porque a los niños les gusta más mi comida que la de ella!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		if (UI.IsMissionComplete(Mission.JeromeHideLaser))
		{
			player.ForceChangeDirection(true);
			i.SetDirection(false);
			player.inAnim = true;
			player.ForceAnim(true);
			player.transform.DOMove(i.transform.position + new Vector3(-2.5f, 0.1f, 0f), 1f).SetEase(Ease.Linear);
			yield return new WaitForSeconds(1.5f);
			player.inAnim = false;
			player.ForceAnim(false);
			i.SetCurrentConversation(5);
			i.Interact();
		}
		else
		{
			UI.CollapseDialogue();
		}
		GameObject.Find("Blocker1").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find("Blocker2").GetComponent<BoxCollider2D>().enabled = false;
	}

	private void GetCardBiscuit()
	{
		player.GetItem(Item.EvilThwarter);
		UnlockHint("Monstermon", 18);
	}

	public void GetLaser()
	{
		player.GetItem(Item.LaserPointer);
		DialogueNode dialogueNode = UnityEngine.Object.FindObjectOfType<Jerome>().conversations[Room.Cafeteria].Nodes[2];
		dialogueNode.Options[0].IsAvailable = true;
		dialogueNode.Options[1].IsAvailable = false;
		dialogueNode.Options[2].IsAvailable = false;
		if (UI.IsMissionActive(Mission.JeromeGetLaserFromBox))
		{
			CompleteMission(Mission.JeromeGetLaserFromBox);
			UnityEngine.Object.FindObjectOfType<Jerome>().ActivateMission(Mission.JeromeBringLaser);
		}
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			SFXManager.Instance.PlaySound("Bell");
		}
	}

	private void TakeBucket()
	{
		UnlockHint("Cindy", 5);
		player.GetItem(Item.BucketBlood);
		GameObject gameObject = GameObject.Find("BucketBlood");
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		if (UI.IsMissionActive(Mission.CindyFindSomethingGross))
		{
			CompleteMission(Mission.CindyFindSomethingGross);
			UnityEngine.Object.FindObjectOfType<Cindy>().ActivateMission(Mission.CindyGoBackToCafeteria);
		}
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			SFXManager.Instance.PlaySound("Bell");
		}
	}

	private void TakeNote()
	{
		UnlockHint("Monty", 4);
		player.GetItem(Item.Recipe);
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			SFXManager.Instance.PlaySound("Bell");
		}
	}

	private void TakeMoney()
	{
		UnlockHint("Monty", 4);
		player.GetMoney(5f);
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			SFXManager.Instance.PlaySound("Bell");
		}
	}

	private void TakeCardFromBox()
	{
		UnlockHint("Monty", 4);
		player.GetItem(Item.SneakySnake);
		UnlockHint("Monstermon", 14);
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			SFXManager.Instance.PlaySound("Bell");
		}
	}

	private void UnscrewShelf()
	{
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			EnvironmentController.Instance.RingBell();
		}
		UnlockHint("Jerome", 3);
		mUnscrewedShelf = true;
		Transform target = GameObject.Find("ShelfAxel").transform;
		player.inAnim = true;
		GameObject.Find("StolenStuffOutOfReach").SetActive(false);
		UnityEngine.Object.FindObjectOfType<Jerome>().conversations[Room.Cafeteria].Nodes[2].Options[2].IsAvailable = false;
		UnityEngine.Object.FindObjectOfType<Jerome>().conversations[Room.Cafeteria].Nodes[2].Options[1].IsAvailable = true;
		target.DORotate(new Vector3(0f, 0f, 20f), 0.2f).SetEase(Ease.OutSine).OnComplete(delegate
		{
			SFXManager.Instance.PlaySound("ShelfClank");
			StartCoroutine(WaitForBoxToFall());
		});
	}

	public bool GetUnscrewedShelf()
	{
		return mUnscrewedShelf;
	}

	private IEnumerator WaitForBoxToFall()
	{
		GameObject g = GameObject.Find("StolenStuff");
		yield return new WaitForSeconds(4f);
		GameObject.Find("ScrewTrigger").SetActive(false);
		g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.GetComponent<BoxCollider2D>().bounds.min.y);
		g.layer = 0;
		UnityEngine.Object.Destroy(g.GetComponent<Rigidbody2D>());
		player.inAnim = false;
	}

	public void AppearJanitor()
	{
		UnlockHint("General", 6);
		base.transform.position = new Vector3(13.6f, -0.9f, 0f);
		SetDepth();
		StartCoroutine(JanitorCloset());
	}

	private IEnumerator JanitorCloset()
	{
		yield return null;
		Interact();
		player.inAnim = true;
		float duration = Vector3.Distance(player.transform.position, base.transform.position) / 10f;
		yield return new WaitForSeconds(0.5f);
		SetAnimation("walk");
		if (player.transform.position.x > base.transform.position.x)
		{
			player.ForceChangeDirection(false);
			SetDirection(true);
			base.transform.DOMove(player.transform.position - Vector3.right * 3f + Vector3.up * 0.1f, duration).SetEase(Ease.Linear).OnComplete(delegate
			{
				SetAnimation("idle");
				player.inAnim = false;
				UpdateOptions();
			});
		}
		else
		{
			player.ForceChangeDirection(true);
			SetDirection(false);
			base.transform.DOMove(player.transform.position + Vector3.right * 3f + Vector3.up * 0.1f, duration).SetEase(Ease.Linear).OnComplete(delegate
			{
				SetAnimation("idle");
				player.inAnim = false;
				UpdateOptions();
			});
		}
	}

	private void LeaveCloset()
	{
		EnvironmentController.Instance.SetSpawnPoint(3);
		EnvironmentController.Instance.ChangeEnvironment(Room.Hallway);
	}

	private void KilledInCloset()
	{
		KillPlayer("You should have left when the bell rang.", "Deberias haberte ido cuando la campana sonó.");
	}

	private void GoToJanitorCloset()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Closet);
		if (UI.IsMissionActive(Mission.CindyGoToCloset))
		{
			CompleteMission(Mission.CindyGoToCloset);
			UnityEngine.Object.FindObjectOfType<Cindy>().ActivateMission(Mission.CindyFindSomethingGross, Room.Cafeteria);
		}
	}

	private void EnterBathroom()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Bathroom);
	}

	public void CheckAppearAtRecess()
	{
		if (UI.IsMissionComplete(Mission.BuggsKillTeacher))
		{
			base.transform.position = UnityEngine.Object.FindObjectOfType<Teacher>().transform.position;
			SetDepth();
		}
		if (UI.IsMissionComplete(Mission.JeromeHideLaser))
		{
			base.transform.position = new Vector3(17.3f, -10.94f, -11.05f);
			UnityEngine.Object.FindObjectOfType<Jerome>().SetCurrentConversation(1);
			SetCurrentConversation(4);
			if (!UnityEngine.Object.FindObjectOfType<Monty>().removedFromGame)
			{
				GameObject.Find("MontyIntercept").GetComponent<BoxCollider2D>().enabled = true;
			}
		}
	}

	public void ExplodeJanitor()
	{
		StartCoroutine(ExplodeJanitorCoroutine());
	}

	private IEnumerator ExplodeJanitorCoroutine()
	{
		player.inAnim = true;
		SFXManager.Instance.PlaySound("Beep");
		yield return new WaitForSeconds(0.5f);
		SFXManager.Instance.PlaySound("Beep");
		yield return new WaitForSeconds(0.5f);
		SFXManager.Instance.PlaySound("Beep");
		yield return new WaitForSeconds(0.5f);
		GameObject.Find("RecessBugJanitor").GetComponent<SpriteRenderer>().enabled = false;
		SFXManager.Instance.PlaySound("Bang");
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		Transform t = GameObject.Find("JanitorExplosion").transform;
		t.transform.position = base.transform.position + new Vector3(0f, 1.75f, 0f);
		GetComponent<Renderer>().enabled = false;
		GameObject.Find("Janitor/Shadow").SetActive(false);
		t.GetComponent<ParticleSystem>().Play();
		int i = 0;
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				Rigidbody2D r = transform.GetComponent<Rigidbody2D>();
				r.isKinematic = false;
				r.gravityScale = 2f;
				switch (i)
				{
				case 0:
					transform.GetComponent<SpriteRenderer>().flipX = mySkeleton.skeleton.flipX;
					r.AddForceAtPosition(new Vector2(UnityEngine.Random.Range(-400, -200), 1000f), t.position + Vector3.up * 1.5f);
					r.AddTorque(UnityEngine.Random.Range(-300, -100));
					break;
				case 1:
					r.AddForceAtPosition(new Vector2(UnityEngine.Random.Range(-400, -200), 200f), t.position + Vector3.up * 1.5f);
					break;
				default:
					r.AddForceAtPosition(new Vector2(UnityEngine.Random.Range(-400, -200), 400f), t.position + Vector3.up * 1.5f);
					break;
				}
				i++;
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable2 = (disposable = enumerator as IDisposable);
			if (disposable != null)
			{
				disposable2.Dispose();
			}
		}
		yield return new WaitForSeconds(2f);
		IEnumerator enumerator2 = t.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Transform transform2 = (Transform)enumerator2.Current;
				transform2.transform.position = new Vector3(transform2.transform.position.x, transform2.transform.position.y, transform2.GetComponent<BoxCollider2D>().bounds.min.y);
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable3 = (disposable = enumerator2 as IDisposable);
			if (disposable != null)
			{
				disposable3.Dispose();
			}
		}
		player.inAnim = false;
	}

	public void EndRecess()
	{
		SFXManager.Instance.PlaySound("Bell");
		SetCurrentConversation(3);
		Interact();
	}

	private void GoToCS2()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom2);
	}

	private void TurnBack()
	{
		SetDirection(false);
	}

	private void KillPlayerRecess()
	{
		KillPlayer("Do as Monty says. Don't talk to Jerome until you resolve the issue with the janitor.", "Haz lo que dice Monty. No hables con Jerome hasta que resuelvas el asunto con el conserje.");
	}
}
