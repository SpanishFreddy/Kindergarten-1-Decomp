using System;
using System.Collections;
using DG.Tweening;
using DialogueTree;
using Spine.Unity;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class Principal : NPCBehavior
{
	public enum OfficeReason
	{
		OpenDoor = 0,
		OutOfOffice = 1,
		CindyRape = 2,
		SentByNugget = 3,
		CaughtByMontior = 4,
		CaughtByMonitorBathroom = 5,
		CaughtByMonitorBathroomLily = 6,
		BlewUpStatue = 7,
		NoItems = 8,
		ShowedKnife = 9,
		ShowedCigs = 10,
		ShowedPass = 11,
		ShowedPhone = 12,
		UsedKey = 13,
		None = 14
	}

	public TextureBlock bloody;

	public Sprite brokenGlass;

	[HideInInspector]
	public OfficeReason mReason = OfficeReason.None;

	[HideInInspector]
	public bool mPunishingBuggs;

	[HideInInspector]
	private int mCurrentTank = 3;

	private bool mSavedBilly;

	private bool mKilledCreature1;

	private bool mKilledCreature3;

	public void SetPunishingBuggs(bool x)
	{
		mPunishingBuggs = x;
	}

	public void SetCindy()
	{
		mReason = OfficeReason.CindyRape;
	}

	public void SetNugget()
	{
		mReason = OfficeReason.SentByNugget;
	}

	public void SetMonitor()
	{
		mReason = OfficeReason.CaughtByMontior;
	}

	public void SetOutOfOffice()
	{
		mReason = OfficeReason.OutOfOffice;
	}

	public void SetOpenedDoor()
	{
		mReason = OfficeReason.OpenDoor;
	}

	public void SetStatue()
	{
		mReason = OfficeReason.BlewUpStatue;
	}

	public void SetBathroom()
	{
		mReason = OfficeReason.CaughtByMonitorBathroom;
	}

	public void SetBathroomLily()
	{
		mReason = OfficeReason.CaughtByMonitorBathroomLily;
	}

	public void SetNoItems()
	{
		mReason = OfficeReason.NoItems;
	}

	public void SetPass()
	{
		mReason = OfficeReason.ShowedPass;
	}

	public void SetCigs()
	{
		mReason = OfficeReason.ShowedCigs;
	}

	public void SetKnife()
	{
		mReason = OfficeReason.ShowedKnife;
	}

	public void SetPhone()
	{
		mReason = OfficeReason.ShowedPhone;
	}

	public void StartPrincipal()
	{
		if (mPunishingBuggs)
		{
			if (mReason == OfficeReason.CindyRape)
			{
				StartCoroutine(InterruptBuggsCindy());
			}
			else if (mReason == OfficeReason.SentByNugget)
			{
				StartCoroutine(InterruptBuggsNugget());
			}
			return;
		}
		switch (mReason)
		{
		case OfficeReason.CindyRape:
			StartCoroutine(CindyPrincipal());
			break;
		case OfficeReason.SentByNugget:
			SetCurrentConversation(16);
			Interact();
			UnityEngine.Object.FindObjectOfType<Teacher>().SetComingFromOffice(true, false, "Nugget");
			if (UI.IsMissionComplete(Mission.LilyGetSentToOffice))
			{
				UnityEngine.Object.FindObjectOfType<Lily>().conversations[Room.Classroom1].currentConversation = 8;
				UnityEngine.Object.FindObjectOfType<Lily>().ActivateMission(Mission.LilyLookForSomethingSuspicious);
			}
			break;
		case OfficeReason.CaughtByMontior:
			SetCurrentConversation(32);
			Interact();
			break;
		case OfficeReason.OutOfOffice:
			base.transform.position = new Vector3(0f, 50f, 0f);
			GameObject.Find("OfficeDoor").GetComponent<ObjectInteractable>().startFunction = string.Empty;
			GameObject.Find("DeskBlocker").GetComponent<BoxCollider2D>().enabled = false;
			GameObject.Find("RugSprite").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("RugFlippedSprite").GetComponent<SpriteRenderer>().enabled = false;
			StartCoroutine(UnityEngine.Object.FindObjectOfType<Lily>().StartOffice());
			break;
		case OfficeReason.OpenDoor:
			MonoBehaviour.print("kill player for open door here");
			break;
		case OfficeReason.BlewUpStatue:
			StartCoroutine(BlewUpStatue());
			break;
		case OfficeReason.CaughtByMonitorBathroom:
			SetCurrentConversation(52);
			Interact();
			break;
		case OfficeReason.CaughtByMonitorBathroomLily:
		{
			Lily lily = UnityEngine.Object.FindObjectOfType<Lily>();
			lily.transform.position = new Vector3(-2.88f, -4.78f, -5f);
			lily.SetDirection(true);
			SetCurrentConversation(54);
			Interact();
			break;
		}
		case OfficeReason.NoItems:
			SetCurrentConversation(57);
			Interact();
			break;
		case OfficeReason.ShowedCigs:
			SetCurrentConversation(61);
			Interact();
			break;
		case OfficeReason.ShowedPass:
			SetCurrentConversation(66);
			Interact();
			break;
		case OfficeReason.ShowedKnife:
			SetCurrentConversation(64);
			Interact();
			break;
		case OfficeReason.ShowedPhone:
			SetCurrentConversation(69);
			Interact();
			break;
		case OfficeReason.UsedKey:
			SetCurrentConversation(72);
			Interact();
			break;
		default:
			Debug.Log("Sent to office without a reason");
			break;
		}
	}

	private IEnumerator InterruptBuggsCindy()
	{
		player.inAnim = true;
		Cindy c = UnityEngine.Object.FindObjectOfType<Cindy>();
		Buggs b = UnityEngine.Object.FindObjectOfType<Buggs>();
		b.RemoveFromGame(false);
		player.transform.position = new Vector3(-20.6f, -2f, -0.69f);
		c.transform.position = new Vector3(-25.7f, -2f, -0.8f);
		b.transform.position = new Vector3(0.7f, -6.3f, -6.29f);
		c.SetDirection(true);
		player.ForceChangeDirection(true);
		c.SetDepth();
		SetDirection(true);
		UnityEngine.Object.FindObjectOfType<Nugget>().conversations[Room.Cafeteria].Nodes[12].Options[0].IsAvailable = true;
		UnityEngine.Object.FindObjectOfType<Nugget>().conversations[Room.Cafeteria].Nodes[12].Options[1].IsAvailable = false;
		yield return new WaitForSeconds(0.5f);
		player.SetDepth();
		player.inAnim = false;
		StartCoroutine(RunDialogueSection("I'm sorry Buggs, but you leave me no choice but to--", "Lo siento, Buggs, pero no me dejas otra opción que—", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(false);
		StartCoroutine(RunDialogueSection("Oh what now?!~ What are you two doing here?", "¡¿Y Ahora qué?!~ ¿Ustedes dos qué están haciendo aquí?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("He tried to rape me!", "¡Él trató de violarme!", c));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Great...~well then, Buggs. It looks like you get off easy this time. Get out of here you little dumpster fire.", "Perfecto...~entonces, Buggs. Parece que se salió con la suya fácilmente esta vez. ¡Fuera de aquí, pequeña basura!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		b.SetAnimation("walk");
		b.SetDirection(false);
		player.inAnim = true;
		b.transform.DOMove(new Vector3(-23f, -3.3f, -3.3f), 2.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			b.SetDirection(true);
			b.SetAnimation("idle");
			player.inAnim = false;
			player.ForceChangeDirection(false);
			b.SetDepth();
		})
			.OnUpdate(base.SetDepth);
		yield return new WaitForSeconds(2.5f);
		StartCoroutine(RunDialogueSection("I'll be seeing you at recess...~pal.", "Lo estaré vigilando en el recreo...~compañero.", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		b.SetBeatUpAtRecess(true);
		b.SetAnimation("walk");
		player.inAnim = true;
		b.transform.DOMoveY(-0.5f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			b.SetAnimation("idle");
			b.transform.position = new Vector3(0f, 50f, 0f);
			player.inAnim = false;
		});
		player.ForceChangeDirection(true);
		StartCoroutine(RunDialogueSection("Okay you two. Let's get this over with.", "Ok, Ustedes dos. Acabemos con esto.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(CindyPrincipal());
	}

	private IEnumerator InterruptBuggsNugget()
	{
		yield return null;
		SetCurrentConversation(16);
		Interact();
		UnityEngine.Object.FindObjectOfType<Teacher>().SetComingFromOffice(true, false, "Nugget");
		if (UI.IsMissionComplete(Mission.LilyGetSentToOffice))
		{
			UnityEngine.Object.FindObjectOfType<Lily>().conversations[Room.Classroom1].currentConversation = 8;
			UnityEngine.Object.FindObjectOfType<Lily>().ActivateMission(Mission.LilyLookForSomethingSuspicious);
		}
	}

	private IEnumerator CindyPrincipal()
	{
		Cindy c = UnityEngine.Object.FindObjectOfType<Cindy>();
		player.transform.position = new Vector3(-20.6f, -2f, -0.69f);
		c.transform.position = new Vector3(-25.7f, -2f, -0.8f);
		player.ForceChangeDirection(true);
		yield return new WaitForSeconds(0.5f);
		if (mPunishingBuggs)
		{
			c.WalkToPoint(new Vector3(-2.88f, -4.78f, -5f), 3f);
			player.inAnim = true;
			player.ForceAnim(true);
			player.transform.DOMove(new Vector3(4f, -4.78f, -5f), 3f).SetEase(Ease.Linear).OnComplete(delegate
			{
				player.ForceAnim(false);
				player.inAnim = false;
				player.ForceChangeDirection(false);
			})
				.OnUpdate(player.SetDepth);
		}
		else
		{
			StartCoroutine(RunDialogueSection("Come on in children. Let's get this all sorted out.", "Éntren, niños. Vamos a resolver todo esto.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			c.WalkToPoint(new Vector3(-2.88f, -4.78f, -5f), 3f);
			player.inAnim = true;
			player.ForceAnim(true);
			player.ForceChangeDirection(true);
			player.transform.DOMove(new Vector3(4f, -4.78f, -5f), 3f).SetEase(Ease.Linear).OnComplete(delegate
			{
				player.ForceAnim(false);
				player.inAnim = false;
				player.ForceChangeDirection(false);
			})
				.OnUpdate(player.SetDepth);
			StartCoroutine(RunDialogueSection("So what seems to be the problem here?", "¿Cuál es el problema?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("He tried to rape me!", "¡Él trató de violarme!", c));
			while (pEmptyOptions)
			{
				yield return null;
			}
		}
		StartCoroutine(RunDialogueSection("So you're accusing your little friend here of trying to rape you.~ Really Cindy?", "Así que está acusando a su compañerito de intentar violarla. ~ ¿En serio, Cindy?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Yes sir! I could barely fight him off!", "¡Sí, señor! ¡Apenas pude defenderme!", c));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UpdateOptions();
		Interact();
	}

	private IEnumerator BlewUpStatue()
	{
		Lily i = UnityEngine.Object.FindObjectOfType<Lily>();
		i.SetDirection(true);
		i.transform.position = new Vector3(-2.88f, -4.78f, -5f);
		SetDirection(true);
		StartCoroutine(RunDialogueSection("How dare you trick me into blowing up that statue!?~ I spent 75% of our budget on that and now it's a pile of rubble!", "¡¿Cómo te atreves a engañarme para volar esa estatua?!~ ¡Gasté el 75% de nuestro presupuesto en eso y ahora es un montón de escombros!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(false);
		StartCoroutine(RunDialogueSection("Mr. Principal it wasn't me! It was--~wait. Did you just say you blew up the statue?", "¡Sreñor Director, no fui yo! Fue--~espera. ¿Acaba de decir que usted voló la estatua?", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Of course. I know you didn't do anything Lily.", "Claro. Yo sé que usted no hizo nada, Lily.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("So...~I'm free to go?", "Así que...~¿Puedo irme?", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Nonsense!~ I've been looking for a reason to get rid of you for awhile, Lily. You're getting too close to all of this.", "¡Tonterías!~ He estado buscando una razón para deshacerme de usted por un tiempo, Lily. Usted se está acercando demasiado a todo esto.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(true);
		SetCurrentConversation(51);
		Interact();
	}

	private void ShootPlayerNoItems()
	{
		ShootPlayer("You have to bring something to show and tell.", "Tienes que traer algo para mostrar y contar.");
	}

	private void ShootPlayerCigs()
	{
		ShootPlayer("Drugs are bad. Don't show them to anyone.", "Las drogas son malas.No las enseñes a nadie.");
	}

	private void ShootPlayerPhone()
	{
		ShootPlayer("It's not that surprising that the teacher is upset with you for having her phone.", "No es muy sorprendente que la maestra está enojada contigo porque tienes su teléfono.");
	}

	private void ShootPlayerKnife()
	{
		ShootPlayer("It's probably best not to advertise that you have a weapon at school.", "Sería mejor no decir a nadie que tienes un arma en el cole.");
	}

	private void ShootPlayerPass()
	{
		ShootPlayer("That pass is stolen. Don't show it at show and tell.", "Este pase es robado. No lo muestres en mostrar y contar.");
	}

	private void ShootPlayerStatue()
	{
		ShootPlayer("Be prepared to bribe the teacher with something after blowing up the statue.", "Prepárate para sobornar a la maestra con algo después de volar la estatua.");
	}

	private void ShootPlayerCaughtInBathroom()
	{
		ShootPlayer("The hall monitor will catch you if you go the bathroom without cigarettes.", "La cámara del vestíbulo te atrapará si vas al baño sin cigarrillos.");
	}

	private void ShootPlayerCaughtInBathroomLily()
	{
		ShootPlayer("Buy some cigarettes from Monty before heading to the bathroom.", "Compra unos cigarrillos de Monty antes de ir al baño.");
	}

	private void KillPlayer1()
	{
		ShootPlayer("Maybe you shouldn't admit to crimes you didn't commit.", "Tal vez no debes de aceptar los crímenes que no cometiste.");
	}

	private void KillPlayer2()
	{
		ShootPlayer("I would take the deal next time.", "Tomaré el acuerdo la próxima vez.");
	}

	private void ShootPlayerInsistNotUpset()
	{
		ShootPlayer("You're clearly a little upset by Billy's disappearence.", "Estás claramente un poco afectado por la desaparición de Billy. ");
	}

	private void ShootPlayerDontKnowAnything()
	{
		ShootPlayer("Perhaps the only way out of that situation is to admit defeat.", "Tal vez la única solución es admitir la derrota.");
	}

	private void ShootPlayerKey()
	{
		ShootPlayer("You shouldn't go in there while he's in there.", "No vayas por allá cuando él está allí.", true);
	}

	private void Recorded()
	{
		SetEmptyOptions();
		player.UseItem(Item.VoiceRecorder);
		StartCoroutine(RecordedCoroutine());
	}

	private IEnumerator RecordedCoroutine()
	{
		Cindy c = UnityEngine.Object.FindObjectOfType<Cindy>();
		StartCoroutine(RunDialogueSection("You did?~ Well that makes this a hell of a lot easier!", "¿Usted lo hizo?~ ¡Ok, esto lo hace mucho más fácil!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(false);
		StartCoroutine(RunDialogueSection("Cindy...~do I even have to listen to this or will you make it easy on yourself and just admit nothing happened?", "Cindy...~ ¿Tengo que escuchar todo ésto o lo hará más fácil  para usted y simplemente va a admitir que nada pasó?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("But...but...he...what?~ Okay fine! But he started it! He said I was mean.", "Pero...pero...él...¿qué?~ ¡Okay, perfecto! ¡Pero él empezó! Él dijo que yo soy malo.", c));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetCurrentConversation(10);
		Interact();
		UnityEngine.Object.FindObjectOfType<Teacher>().SetComingFromOffice(true, mPunishingBuggs, "Cindy");
	}

	private void ForceBackToClass()
	{
		player.inAnim = true;
		player.ForceAnim(true);
		EnvironmentController.Instance.SetSpawnPoint(1);
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom1);
		player.transform.DOMove(new Vector3(-4.1f, -4.7f, -4.67f), 0.95f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.inAnim = false;
			player.ForceAnim(false);
		});
	}

	private void ForceBackToClassExpelCindy()
	{
		player.inAnim = true;
		player.ForceAnim(true);
		EnvironmentController.Instance.SetSpawnPoint(1);
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom1);
		player.transform.DOMove(new Vector3(-4.1f, -4.7f, -4.67f), 0.95f).SetEase(Ease.Linear).OnComplete(delegate
		{
			UnityEngine.Object.FindObjectOfType<Cindy>().RemoveFromGame();
			player.inAnim = false;
			player.ForceAnim(false);
		});
	}

	private void NotExpelled()
	{
		UnlockHint("Teacher", 2);
		UnityEngine.Object.FindObjectOfType<Cindy>().ShutDownCindy(false);
		UnityEngine.Object.FindObjectOfType<Teacher>().SetComingFromOffice(false, mPunishingBuggs, "Cindy");
		ForceBackToClass();
	}

	private void LookUnderRug()
	{
		GameObject.Find("RugSprite").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("RugFlippedSprite").GetComponent<SpriteRenderer>().enabled = true;
		player.ForceChangeDirection(false);
		CompleteMission(Mission.LilyLookForSomethingSuspicious);
		UnityEngine.Object.FindObjectOfType<Lily>().ActivateMission(Mission.LilyTellAboutTheHatch, Room.Classroom1);
		SetCurrentConversation(13);
		Invoke("Interact", 0.1f);
	}

	private void GetMonstermonTrash()
	{
		player.GetItem(Item.WallOfCastle);
		UnlockHint("Monstermon", 23);
	}

	private void LeaveOffice()
	{
		EnvironmentController.Instance.SetSpawnPoint(1);
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom1);
	}

	private void GetPills()
	{
		player.GetItem(Item.Pills);
	}

	private void TakePill()
	{
		DialogueNode dialogueNode = UnityEngine.Object.FindObjectOfType<Teacher>().conversations[Room.Classroom1].Nodes[33];
		dialogueNode.Options[0].IsAvailable = true;
		dialogueNode.Options[1].IsAvailable = false;
		UnityEngine.Object.FindObjectOfType<Nugget>().conversations[Room.Classroom1].currentConversation = 2;
		StartCoroutine(SaturateCamera());
	}

	private void GetBug()
	{
		player.GetItem(Item.Bug);
	}

	private IEnumerator SaturateCamera()
	{
		ColorCorrectionCurves ccc = Camera.main.gameObject.GetComponent<ColorCorrectionCurves>();
		while (ccc.saturation < 1.3f)
		{
			yield return null;
			ccc.saturation += 0.01f;
		}
	}

	public IEnumerator DesaturateCamera()
	{
		ColorCorrectionCurves ccc = Camera.main.gameObject.GetComponent<ColorCorrectionCurves>();
		while (ccc.saturation > 1.15f)
		{
			yield return null;
			ccc.saturation -= 0.01f;
		}
	}

	private void KillPlayerPill()
	{
		ShootPlayer("Maybe take a pill next time.", "Toma una pastilla la próxima vez.");
	}

	private void GetCardFromPrincipal()
	{
		player.GetItem(Item.SpikeyFlimflam);
		UnlockHint("Monstermon", 19);
	}

	private void GetBuck()
	{
		player.GetMoney(1f);
	}

	public IEnumerator StartCS2()
	{
		player.inAnim = true;
		StartCoroutine(RunDialogueSection("Children...~I have some bad news. If you look to your right, you will see your formerly unstabbed teacher.", "Niños...~Tengo malas noticias. Si miran a su derecha, verán a su antigua profesora acuchillada.", this));
		yield return new WaitForSeconds(2f);
		player.inAnim = false;
		player.ForceChangeDirection(true);
		Cindy c = UnityEngine.Object.FindObjectOfType<Cindy>();
		Nugget l = UnityEngine.Object.FindObjectOfType<Nugget>();
		Buggs b = UnityEngine.Object.FindObjectOfType<Buggs>();
		Jerome i = UnityEngine.Object.FindObjectOfType<Jerome>();
		Lily j = UnityEngine.Object.FindObjectOfType<Lily>();
		Monty k = UnityEngine.Object.FindObjectOfType<Monty>();
		c.SetDirection(true);
		l.SetDirection(true);
		while (pEmptyOptions)
		{
			yield return null;
		}
		if (!c.removedFromGame)
		{
			StartCoroutine(RunDialogueSection("Oh my god! Is there a murderer on the loose? It's probably the creepy janitor!", "¡Oh, Dios mío! ¿Hay un asesino suelto? ¡Probablemente es el escalofriante conserje!", c));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("We've already cleared the janitor. He was in the cafeteria selling...~'Biscuit Balls' when her death occured.", "Ya hemos descartado al conserje. Él estaba en la cafetería vendiendo...~'Galletas' cuando ella murió.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
		}
		if (!b.removedFromGame)
		{
			StartCoroutine(RunDialogueSection("She had it coming to her!", "¡Ella se lo merecía!", b));
			while (pEmptyOptions)
			{
				yield return null;
			}
		}
		if (!i.removedFromGame)
		{
			StartCoroutine(RunDialogueSection("What are you gonna do about it, Dad?", "¡Qué harás con esto, papá?", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
		}
		if (!j.removedFromGame)
		{
			StartCoroutine(RunDialogueSection("First Billy, now this? We're not safe at this school!", "¿Primero fue Billy, ahora esto? ¡No estamos seguros en la escuela!", j));
			while (pEmptyOptions)
			{
				yield return null;
			}
		}
		if (!k.removedFromGame)
		{
			StartCoroutine(RunDialogueSection("This is totally gonna tank my sales for the rest of the week.", "Todo esto definitivamente bajará mis ventas para el resto de la semana.", k));
			while (pEmptyOptions)
			{
				yield return null;
			}
		}
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(1f);
		player.ForceChangeDirection(false);
		StartCoroutine(RunDialogueSection("\\bENOUGH!!/b", "\\b¡¡BASTA!!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		player.inDialogue = true;
		yield return new WaitForSeconds(1f);
		StartCoroutine(RunDialogueSection("Nugget likes the way she smells now.", "A Nugget le gusta como ella huele ahora.", l));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Shut up weirdo. Take one of your pills or something.~ Anyway I need to search all of you to make sure you aren't carrying anything that could have been used to kill your teacher.", "Cállese bicho raro. Tómese una de sus pastilla o algo. ~ De cualquier manera tengo que revisarlos a  todos para asegurarme de que no tienen nada que podría haber sido utilizado para matar a su profesora.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.ForceChangeDirection(false);
		StartCoroutine(RunDialogueSection("We'll start with you, the one covered in blood. Empty your pockets.", "Empezarémos contigo. El que está cubierto de sangre, ven acá y vacía los bolcillos", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		if (player.HasItem(Item.BloodyKnife))
		{
			StartCoroutine(RunDialogueSection("That's a knife!~ It's covered in blood! You stabbed her! Don't move children! I'll save you!", "¡Es un cuchillo!~ ¡Cubierto de sangre! ¡Usted la apuñaló! ¡No se muevan niños! ¡Los voy a salvar!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UI.CollapseDialogue();
			ShootPlayer("Get rid of the knife during recess next time.", "Deshazte del cuchillo durante el recreo la próxima vez.");
			yield break;
		}
		StartCoroutine(RunDialogueSection("Doesn't look like you have anything on you. Okay. Let's go ahead and search Buggs next.", "Parece que no tiene nada. Okay. Continuemos revisando a Buggs.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Vector3[] v = new Vector3[2]
		{
			new Vector3(-15.8f, -7.25f, -9.2f),
			new Vector3(-15.8f, -4f, -9.2f)
		};
		b.SetAnimation("walk");
		player.inAnim = true;
		b.transform.DOPath(v, 2.5f).OnWaypointChange(GivePhone).SetEase(Ease.Linear)
			.OnUpdate(base.SetDepth)
			.OnComplete(delegate
			{
				player.inAnim = false;
				b.SetAnimation("idle");
			});
		yield return new WaitForSeconds(3f);
		StartCoroutine(RunDialogueSection("Hmmmm...~doesn't look like you have anything incriminating on you either. Damn.~ Now I owe the lunch lady twenty bucks.", "Hmmmm...~parece que usted tampoco tiene algo que pueda incriminarlo. Diablos.~ Ahora le debo a la cocinera veinte dólares. ", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Okay, well it looks like no one here is responsible for killing the teacher. That being said, I'm sending you all home cause...~well...~your teacher is dead.", "Bueno, parece que aquí nadie es responsable de haber matado a la maestra. Después de decir esto, los estoy mandando a todos a casa porque...~pues...~su maestra está muerta.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		b.SetEndDay(true);
		EnvironmentController.Instance.ChangeEnvironment(Room.SchoolYardEndDay);
	}

	private void GivePhone(int x)
	{
		if (x == 1)
		{
			player.GetItem(Item.Phone);
		}
	}

	private void BringInLily()
	{
		StartCoroutine(BringInLilyCoroutine());
	}

	private void ShootBoth()
	{
		StartCoroutine(ShootBothCoroutine());
	}

	private IEnumerator ShootBothCoroutine()
	{
		StartCoroutine(RunDialogueSection("Oh...~well in that case.", "Oh...~pues en este caso.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		ShootPlayer("Sometimes honesty is the best policy.", "A veces honestidad es la mejor política.");
		UI.CollapseDialogue();
	}

	private IEnumerator BringInLilyCoroutine()
	{
		Lily i = UnityEngine.Object.FindObjectOfType<Lily>();
		StartCoroutine(RunDialogueSection("Lily...~we've been over this. I had nothing to do with your brother's disappearance. Why don't you step inside my office and we'll talk this over again.", "Lily...~ Ya hemos hablado de ésto. Yo no tuve nada que ver con la desaparición de su hermano. ¿Por qué no entras a mi oficina y hablaremos de ésto nuevamente?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I know it was you! One day I'll prove it!", "Yo sé que fue usted! ¡Un día lo probaré!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I'm getting real sick of this young lady. Office. NOW!", "Me estoy hartando de esto jovencita. A la oficina. ¡AHORA!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		i.SetAnimation("walk");
		i.transform.DOMoveX(-27.75f, 1.75f).SetEase(Ease.Linear).OnComplete(i.RemoveFromGame);
		yield return new WaitForSeconds(1.75f);
		StartCoroutine(RunDialogueSection("Please excuse me for a moment.", "Discúlpeme un momentom por favor.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.inAnim = true;
		Vector3 v = base.transform.position;
		base.transform.position = new Vector3(-50f, 0f, 0f);
		UI.CollapseDialogue();
		yield return new WaitForSeconds(2f);
		SFXManager.Instance.PlaySound("Gun");
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.25f);
		UI.CompleteMission(Mission.TeacherGetLilyInTrouble);
		yield return new WaitForSeconds(2f);
		player.inAnim = false;
		ApplyTextureBlock(bloody);
		base.transform.position = v;
		SetCurrentConversation(3);
		if (EnvironmentController.Instance.ActionsLeft() > 0)
		{
			conversations[Room.Hallway].Nodes[3].DialogueText = "Okay. Thank you for letting me know about her. Why don't you go finish your lunch?";
			conversations[Room.Hallway].Nodes[7].DialogueText = "Good. Now get back to lunch.";
			conversations[Room.Hallway].Nodes[7].Options[0].FunctionCall = "GoBackToOffice";
			conversations[Room.Hallway].Nodes[3].Options[2].FunctionCall = "GoBackToOffice";
		}
		else
		{
			UnityEngine.Object.FindObjectOfType<HallMonitor>().SetEndCafeteria();
		}
		Interact();
	}

	private void GoToRecess()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Recess);
	}

	private void GoBackToOffice()
	{
		base.transform.position = new Vector3(0f, 50f, 0f);
	}

	private void ShootPlayerHallMonitor()
	{
		StartCoroutine(ShootPlayerCoroutine("Don't get caught in the hallway without a pass.", "No se deje atrapar en el pasillo sin un pase."));
	}

	private void ShootPlayer()
	{
	}

	private void ShootPlayer(string s, string spa)
	{
		StartCoroutine(ShootPlayerCoroutine(s, spa));
	}

	private void ShootPlayer(string s, string span, bool x)
	{
		StartCoroutine(ShootPlayerCoroutine(s, span, x));
	}

	private IEnumerator ShootPlayerCoroutine(string s, string spa)
	{
		UI.showDeath = true;
		player.inAnim = true;
		GameObject g = GameObject.Find("PrincipalShooting");
		pRend.enabled = false;
		g.GetComponent<Renderer>().enabled = true;
		g.transform.position = base.transform.position;
		SkeletonAnimation sa = g.GetComponent<SkeletonAnimation>();
		sa.skeleton.flipX = true;
		yield return new WaitForSeconds(1f);
		sa.AnimationName = "shooting";
		yield return new WaitForSeconds(0.25f);
		player.ActivateHoleInHead();
		player.PlayAnimation("fallover");
		player.transform.DOMoveX(11f, 2f).SetRelative(true);
		player.ActivateBloodStream(new Vector3(1.25f, 0.5f, -3.6f));
		GameObject.Find("PlayerBloodSplat").transform.localPosition = new Vector3(0f, 2f, -3f);
		GameObject.Find("PlayerBloodSplat").GetComponent<ParticleSystem>().Play();
		SFXManager.Instance.PlaySound("Gun");
		yield return new WaitForSeconds(0.25f);
		sa.AnimationName = "idle";
		yield return new WaitForSeconds(1f);
		player.GetSkeleton().timeScale = 0f;
		UI.ShowDeath(s, spa);
	}

	private IEnumerator ShootPlayerCoroutine(string s, string span, bool x)
	{
		player.inAnim = true;
		UI.showDeath = true;
		GameObject g = GameObject.Find("PrincipalShooting");
		if (EnvironmentController.currentRoom == Room.Office)
		{
			g.transform.position = base.transform.position;
		}
		pRend.enabled = false;
		g.GetComponent<Renderer>().enabled = true;
		SkeletonAnimation sa = g.GetComponent<SkeletonAnimation>();
		sa.skeleton.flipX = false;
		yield return new WaitForSeconds(1f);
		sa.AnimationName = "shooting";
		yield return new WaitForSeconds(0.25f);
		player.ActivateHoleInHead();
		player.PlayAnimation("fallover");
		if (EnvironmentController.currentRoom == Room.Lair)
		{
			player.transform.DOMoveX(-5f, 2f).SetRelative(true);
		}
		else
		{
			player.transform.DOMoveX(-2f, 2f).SetRelative(true);
		}
		player.ActivateBloodStream(new Vector3(-1.25f, 0.5f, -3.6f));
		GameObject.Find("PlayerBloodSplat").transform.localPosition = new Vector3(0f, 2f, -3f);
		GameObject.Find("PlayerBloodSplat").GetComponent<ParticleSystem>().Play();
		SFXManager.Instance.PlaySound("Gun");
		yield return new WaitForSeconds(0.25f);
		sa.AnimationName = "idle";
		yield return new WaitForSeconds(1f);
		player.GetSkeleton().timeScale = 0f;
		UI.ShowDeath(s, span);
	}

	private void RunPiggyBank()
	{
		UI.OpenPiggyBank();
	}

	private void ShootPlayerLair()
	{
		ShootPlayer("You pressed the buttons in the wrong order.", "Apretaste los botones en el orden incorrecto.", true);
	}

	private void ShootPlayerLair(string s, string spa)
	{
		ShootPlayer(s, spa, true);
	}

	private IEnumerator WaitForPrincipalAppear(string s, string sp)
	{
		StartCoroutine(RunDialogueSection(s, sp, null, true));
		while (pEmptyOptions)
		{
			yield return null;
		}
		AppearInLair();
	}

	private void PressYellow()
	{
		GameObject.Find("TankLightBottom" + mCurrentTank).GetComponent<Light>().enabled = false;
		mCurrentTank++;
		SFXManager.Instance.PlaySound("LightSwitch");
		if (mCurrentTank == 4)
		{
			mCurrentTank = 1;
		}
		GameObject.Find("TankLightBottom" + mCurrentTank).GetComponent<Light>().enabled = true;
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			GameObject.Find("Computer").GetComponent<ObjectInteractable>().lastSelectedOption.DestinationID = -2;
			StartCoroutine(WaitForPrincipalAppear("ACTIVE TANK CHANGED TO " + mCurrentTank + ".", "UNIDAD ACTIVA " + mCurrentTank + "."));
		}
		else if (!EnvironmentController.Instance.isSpanish)
		{
			GameObject.Find("Computer").GetComponent<ObjectInteractable>().dialogue.Nodes[0].DialogueText = "ACTIVE TANK CHANGED TO " + mCurrentTank + ".";
		}
		else
		{
			GameObject.Find("Computer").GetComponent<ObjectInteractable>().dialogue.Nodes[0].DialogueText = "UNIDAD ACTIVA " + mCurrentTank + ".";
		}
	}

	private void PressRed()
	{
		player.ForceChangeDirection(true);
		GameObject.Find("TankLightBottom" + mCurrentTank).GetComponent<Light>().color = Color.red;
		Interactable interactable = player.GetInteractable();
		if (mCurrentTank == 2)
		{
			KillCreature(2);
			if (EnvironmentController.Instance.ActionsLeft() > 0)
			{
				interactable.lastSelectedOption.DestinationID = -2;
			}
			StartCoroutine(KillBilly());
		}
		else if (EnvironmentController.Instance.ActionsLeft() > 1)
		{
			if ((mKilledCreature1 && mCurrentTank == 1) || (mKilledCreature3 && mCurrentTank == 3))
			{
				if (!EnvironmentController.Instance.isSpanish)
				{
					interactable.GetDialogue().Nodes[0].DialogueText = "NEUTRALIZING CONTENTS OF TANK " + mCurrentTank + ".~ ERROR: CONTENTS OF TANK " + mCurrentTank + " ALREADY NEUTRALIZED.";
				}
				else
				{
					interactable.GetDialogue().Nodes[0].DialogueText = "MATANDO A " + mCurrentTank + ".~ ERROR.";
				}
			}
			else
			{
				if (!EnvironmentController.Instance.isSpanish)
				{
					interactable.GetDialogue().Nodes[0].DialogueText = "NEUTRALIZING CONTENTS OF TANK " + mCurrentTank + ".";
				}
				else
				{
					interactable.GetDialogue().Nodes[0].DialogueText = "MATANDO A " + mCurrentTank + ".";
				}
				KillCreature(mCurrentTank);
			}
		}
		else if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			interactable.lastSelectedOption.DestinationID = -2;
			if ((mKilledCreature1 && mCurrentTank == 1) || (mKilledCreature3 && mCurrentTank == 3))
			{
				StartCoroutine(WaitForPrincipalAppear("NEUTRALIZING CONTENTS OF TANK " + mCurrentTank + ".~ ERROR: CONTENTS OF TANK " + mCurrentTank + " ALREADY NEUTRALIZED.", "MATANDO A " + mCurrentTank + ".~ ERROR."));
			}
			else
			{
				StartCoroutine(WaitForPrincipalAppear("NEUTRALIZING CONTENTS OF TANK " + mCurrentTank + ".", "MATANDO A " + mCurrentTank + "."));
				KillCreature(mCurrentTank);
			}
		}
	}

	private void PressGreen()
	{
		player.ForceChangeDirection(true);
		Interactable interactable = player.GetInteractable();
		if (mCurrentTank == 2)
		{
			if (!mSavedBilly)
			{
				mSavedBilly = true;
				interactable.lastSelectedOption.DestinationID = -2;
				GameObject.Find("TankLightBottom" + mCurrentTank).GetComponent<Light>().color = Color.green;
				GameObject.Find("LifeSystem").GetComponent<ParticleSystem>().Play();
				GameObject.Find("VatGlass2").GetComponent<SpriteRenderer>().DOColor(Color.green, 3f)
					.SetDelay(0.5f)
					.OnComplete(PlaceBilly);
				StartCoroutine(FixBilly(interactable));
			}
			else if (EnvironmentController.Instance.ActionsLeft() != 1)
			{
				if (!EnvironmentController.Instance.isSpanish)
				{
					interactable.GetDialogue().Nodes[0].DialogueText = "REVERTING MUTATION PROCESS ON CONTENTS OF TANK " + mCurrentTank + ". ~~ERROR: CONTENTS OF TANK " + mCurrentTank + " CANNOT BE REVERTED.";
				}
				else
				{
					interactable.GetDialogue().Nodes[0].DialogueText = "REVERTIR " + mCurrentTank + ". ~~ERROR.";
				}
				interactable.lastSelectedOption.DestinationID = 0;
			}
			else
			{
				interactable.lastSelectedOption.DestinationID = -2;
				StartCoroutine(WaitForPrincipalAppear("REVERTING MUTATION PROCESS ON CONTENTS OF TANK " + mCurrentTank + ". ~~ERROR: CONTENTS OF TANK " + mCurrentTank + " CANNOT BE REVERTED.", "REVERTIR " + mCurrentTank + ". ~~ERROR."));
			}
		}
		else if (EnvironmentController.Instance.ActionsLeft() != 1)
		{
			if (!EnvironmentController.Instance.isSpanish)
			{
				interactable.GetDialogue().Nodes[0].DialogueText = "REVERTING MUTATION PROCESS ON CONTENTS OF TANK " + mCurrentTank + ". ~~ERROR: CONTENTS OF TANK " + mCurrentTank + " CANNOT BE REVERTED.";
			}
			else
			{
				interactable.GetDialogue().Nodes[0].DialogueText = "REVERTIR " + mCurrentTank + ". ~~ERROR.";
			}
			interactable.lastSelectedOption.DestinationID = 0;
		}
		else
		{
			interactable.lastSelectedOption.DestinationID = -2;
			StartCoroutine(WaitForPrincipalAppear("REVERTING MUTATION PROCESS ON CONTENTS OF TANK " + mCurrentTank + ". ~~ERROR: CONTENTS OF TANK " + mCurrentTank + " CANNOT BE REVERTED.", "REVERTIR " + mCurrentTank + ". ~~ERROR."));
		}
	}

	private void PressBlue()
	{
		StartCoroutine(ReleaseTanks());
	}

	private IEnumerator ReleaseTanks()
	{
		UnlockHint("Lily", 6);
		StartCoroutine(RunDialogueSection("RELEASING CONTENTS OF ALL VIABLE TANKS.", "LIBERANDO.", null, true));
		if (!mKilledCreature1)
		{
			GameObject.Find("TankLightBottom1").GetComponent<Light>().enabled = true;
			GameObject.Find("TankLightBottom1").GetComponent<Light>().color = Color.blue;
		}
		if (!mKilledCreature3)
		{
			GameObject.Find("TankLightBottom3").GetComponent<Light>().enabled = true;
			GameObject.Find("TankLightBottom3").GetComponent<Light>().color = Color.blue;
		}
		if (mSavedBilly)
		{
			GameObject.Find("TankLightBottom2").GetComponent<Light>().enabled = true;
			GameObject.Find("TankLightBottom2").GetComponent<Light>().color = Color.blue;
		}
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.inAnim = true;
		if (!mKilledCreature1)
		{
			Creature1BreakOut();
			UnlockHint("Lily", 7);
		}
		if (!mKilledCreature3)
		{
			Invoke("Creature3BreakOut", 0.5f);
		}
		if (mSavedBilly)
		{
			Invoke("BillyBreakOut", 0.75f);
		}
		if (!mKilledCreature1)
		{
			StartCoroutine(CreatureKillPlayer());
		}
		else if (!mKilledCreature3)
		{
			if (EnvironmentController.Instance.ActionsLeft() == 0)
			{
				StartCoroutine(TrueEnding());
			}
			else
			{
				StartCoroutine(Creature3KillPlayer());
			}
		}
		else if (!mSavedBilly)
		{
			player.inAnim = false;
			StartCoroutine(SomethingWrongWithBilly());
		}
	}

	private void Creature1BreakOut()
	{
		player.ForceChangeDirection(true);
		GameObject gameObject = GameObject.Find("Creature1Spine");
		SkeletonAnimation sk1 = gameObject.GetComponent<SkeletonAnimation>();
		SFXManager.Instance.PlaySound("GlassShatter");
		gameObject.GetComponent<MeshRenderer>().enabled = true;
		sk1.AnimationName = "burst";
		GameObject.Find("BubbleSystem1").GetComponent<ParticleSystem>().Stop();
		GameObject.Find("VatGlass1").GetComponent<SpriteRenderer>().sprite = brokenGlass;
		GameObject.Find("Creature1").GetComponent<SpriteRenderer>().enabled = false;
		gameObject.transform.DOLocalMove(new Vector3(-8.8f, -3.7f, -8.7f), 0.3f).SetEase(Ease.OutSine).OnComplete(delegate
		{
			sk1.loop = true;
			sk1.AnimationName = "idle";
			SFXManager.Instance.PlaySound("CreatureNoise");
		});
	}

	private void Creature3BreakOut()
	{
		player.ForceChangeDirection(true);
		GameObject gameObject = GameObject.Find("Creature3Spine");
		SkeletonAnimation sk1 = gameObject.GetComponent<SkeletonAnimation>();
		sk1.skeleton.flipX = true;
		SFXManager.Instance.PlaySound("GlassShatter");
		gameObject.GetComponent<MeshRenderer>().enabled = true;
		sk1.AnimationName = "burst";
		GameObject.Find("BubbleSystem3").GetComponent<ParticleSystem>().Stop();
		GameObject.Find("VatGlass3").GetComponent<SpriteRenderer>().sprite = brokenGlass;
		GameObject.Find("Creature3").GetComponent<SpriteRenderer>().enabled = false;
		gameObject.transform.DOLocalMove(new Vector3(8.34f, -3.7f, -8.7f), 0.3f).SetEase(Ease.OutSine).OnComplete(delegate
		{
			sk1.loop = true;
			sk1.AnimationName = "idle";
			SFXManager.Instance.PlaySound("CreatureNoise");
		});
	}

	private void BillyBreakOut()
	{
		Billy billy = UnityEngine.Object.FindObjectOfType<Billy>();
		billy.transform.DOKill();
		billy.transform.parent = null;
		Vector3 position = billy.transform.position;
		billy.transform.position = new Vector3(position.x, position.y, -2.95f);
		SFXManager.Instance.PlaySound("GlassShatter");
		GameObject.Find("BubbleSystem2").GetComponent<ParticleSystem>().Stop();
		GameObject.Find("VatGlass2").GetComponent<SpriteRenderer>().sprite = brokenGlass;
		billy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		billy.GetComponent<Rigidbody2D>().AddTorque(-50f);
	}

	private IEnumerator TrueEnding()
	{
		player.inAnim = false;
		StartCoroutine(RunDialogueSection("No!~ What have you done!?", "¡No!~ ¡¿Qué ha hecho!?", this));
		GameObject creature = GameObject.Find("Creature3Spine");
		SkeletonAnimation creatureAnim = creature.GetComponent<SkeletonAnimation>();
		while (pEmptyOptions)
		{
			yield return null;
		}
		SkeletonAnimation ps = GameObject.Find("PrincipalShooting").GetComponent<SkeletonAnimation>();
		ps.AnimationName = "shooting";
		yield return new WaitForSeconds(0.15f);
		SFXManager.Instance.PlaySound("Gun");
		yield return new WaitForSeconds(0.35f);
		ps.AnimationName = "idle";
		StartCoroutine(RunDialogueSection("Stay away from me!", "¡Aléjese de mi!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		creatureAnim.AnimationName = "walk";
		creature.transform.DOMove(ps.transform.position + new Vector3(-4f, 0f, 0.1f), 1f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(1f);
		creatureAnim.AnimationName = "pump";
		yield return new WaitForSeconds(0.5f);
		SFXManager.Instance.PlaySound("GoreSplat1");
		yield return new WaitForSeconds(3.5f);
		ExplodePrincipal();
		yield return new WaitForSeconds(2f);
		creatureAnim.AnimationName = "idle";
		yield return new WaitForSeconds(1f);
		creatureAnim.skeleton.flipX = false;
		creatureAnim.AnimationName = "walk";
		GameObject head = GameObject.Find("PrincipalHeadExplosion");
		creature.transform.DOMove(head.transform.position + new Vector3(1f, -0.5f, 0f), 2f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(2f);
		creatureAnim.AnimationName = "attack";
		yield return new WaitForSeconds(0.5f);
		SFXManager.Instance.PlaySound("GoreSplat2");
		head.GetComponent<BoneFollower>().boneName = "armback";
		yield return new WaitForSeconds(0.5f);
		creatureAnim.AnimationName = "walk";
		creature.transform.DOMove(new Vector3(3.3f, -4.56f, -5.15f), 2f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(2f);
		creatureAnim.AnimationName = "idle";
		yield return new WaitForSeconds(1f);
		creatureAnim.AnimationName = "burst";
		creature.transform.localPosition = new Vector3(creature.transform.localPosition.x, creature.transform.localPosition.y, -0.15f);
		creature.transform.DOLocalMove(new Vector3(-1f, -20f, -0.15f), 0.5f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(2f);
		Lily i = UnityEngine.Object.FindObjectOfType<Lily>();
		if (mSavedBilly)
		{
			Billy b = UnityEngine.Object.FindObjectOfType<Billy>();
			b.transform.position = new Vector3(b.transform.position.x, b.transform.position.y, -2.95f);
			i.SetDirection(false);
			i.WalkToPoint(new Vector3(1.82f, -2.65f, -2.64f), 1f);
			StartCoroutine(RunDialogueSection("Billy!~ Wake up!~ C'mon!", "¡Billy!~ ¡Despiértese!~ ¡Vamos!", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			b.ApplyTextureBlock(b.defaultTextureBlock);
			SFXManager.Instance.PlayMusic("BedroomTheme");
			StartCoroutine(RunDialogueSection("Huh?...~Wh-~where am I? What's going on Lily?", "¿Ah?...~Do-~¿dónde estoy? ¿Qué está pasando Lily?", b));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("He's alive!~ He's saved! We did it!", "¡Él está vivo!~ ¡está a salvo! ¡Lo logramos!", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			b.transform.DORotate(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.Linear);
			b.transform.DOMove(new Vector3(-0.21f, -2.96f, -2.95f), 0.5f);
			yield return new WaitForSeconds(0.5f);
			b.mySkeleton.timeScale = 1.5f;
			yield return new WaitForSeconds(0.5f);
			b.SetDirection(true);
			StartCoroutine(RunDialogueSection("Can...~can we get out of here?~ I want to go home.", "Podemos...~ ¿podemos irnos de aquí?~ Quiero irme a casa.", b));
			while (pEmptyOptions)
			{
				yield return null;
			}
			EnvironmentController.Instance.RingBell();
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(RunDialogueSection("Of course we can silly, but first we have to go to show and tell!", "Claro que podemos irnos tontico, ¡pero primero tenemos que ir a \u00b4mostrar y contar\u00b4!", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Let's get out of here.", "¡Vamonos de aquí!", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UI.CompleteMission(Mission.LilySaveBilly);
			EnvironmentController.Instance.ChangeEnvironment(Room.Classroom2);
			SFXManager.Instance.PlayMusic("ThemeMusic");
		}
		else
		{
			StartCoroutine(RunDialogueSection("Billy is still in there! What do we do?", "¡Billy todavía está ahí adentro! ¿Qué hacemos?", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("CLIENT NEUTRALIZED. DESTROYING EVIDENCE.", "DESTRUCCIÓN DE LA EVIDENCIA.", null, true));
			while (pEmptyOptions)
			{
				yield return null;
			}
			mCurrentTank = 2;
			PressRed();
		}
	}

	private IEnumerator SomethingWrongWithBilly()
	{
		yield return null;
		Lily i = UnityEngine.Object.FindObjectOfType<Lily>();
		i.SetDirection(false);
		StartCoroutine(RunDialogueSection("Oh no!~ Billy is still trapped in there!", "¡Oh no!~ ¡Billy todavía está atrapado allí!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			AppearInLair();
		}
		if (EnvironmentController.Instance.ActionsLeft() == 0)
		{
			UI.CollapseDialogue();
			ShootPlayer("You pressed the buttons in the wrong order.", "Apretaste los botones en el orden incorrecto.", true);
		}
	}

	private IEnumerator CreatureKillPlayer()
	{
		UI.CollapseDialogue();
		yield return new WaitForSeconds(3f);
		GameObject c1 = GameObject.Find("Creature1Spine");
		SkeletonAnimation sk1 = c1.GetComponent<SkeletonAnimation>();
		sk1.AnimationName = "walk";
		c1.transform.DOMove(new Vector3(player.transform.position.x + 4f, player.transform.position.y, player.transform.position.z + 0.1f), 1.5f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(1.5f);
		sk1.AnimationName = "pump";
		yield return new WaitForSeconds(0.3f);
		SFXManager.Instance.PlaySound("GoreSplat2");
		yield return new WaitForSeconds(4f);
		player.ExplodePlayer("Kill the creature near you before you hit the blue button.", "Mata a la criatura antes de pulsar el botón azul.", false);
		yield return new WaitForSeconds(1.6f);
		sk1.AnimationName = "idle";
	}

	private IEnumerator Creature3KillPlayer()
	{
		UI.CollapseDialogue();
		yield return new WaitForSeconds(3f);
		GameObject c1 = GameObject.Find("Creature3Spine");
		SkeletonAnimation sk1 = c1.GetComponent<SkeletonAnimation>();
		sk1.skeleton.FlipX = false;
		sk1.AnimationName = "walk";
		c1.transform.DOMove(new Vector3(player.transform.position.x + 4f, player.transform.position.y, player.transform.position.z + 0.1f), 3f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(3f);
		sk1.AnimationName = "pump";
		yield return new WaitForSeconds(0.3f);
		SFXManager.Instance.PlaySound("GoreSplat2");
		yield return new WaitForSeconds(4f);
		player.ExplodePlayer("Make sure the creature by the ladder has something to attack before setting it loose.", "Asegúrese de que la criatura de la escalera tenga algo que atacar antes de soltarla", false);
		yield return new WaitForSeconds(1.6f);
		sk1.AnimationName = "idle";
	}

	private void ExitComputer()
	{
		GameObject.Find("Computer").GetComponent<ObjectInteractable>().GetDialogue()
			.Nodes[0].DialogueText = "ENTER A COMMAND.";
	}

	private void PlaceBilly()
	{
		Transform transform = GameObject.Find("Creature2").transform;
		SFXManager.Instance.PlaySound("RevertBilly");
		transform.GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("VatGlass2").GetComponent<SpriteRenderer>().DOColor(new Color(1f, 1f, 1f, 0.25f), 3f);
		Billy billy = UnityEngine.Object.FindObjectOfType<Billy>();
		billy.GetComponent<Renderer>().enabled = true;
		billy.transform.parent = transform.transform.parent;
		billy.transform.localPosition = new Vector3(0f, -2f, 0.5f);
		billy.transform.DOLocalMoveY(0f, 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
	}

	private IEnumerator FixBilly(Interactable oi)
	{
		Lily i = UnityEngine.Object.FindObjectOfType<Lily>();
		i.SetDirection(false);
		StartCoroutine(RunDialogueSection("REVERTING MUTATION PROCESS ON CONTENTS OF TANK 2.", "REVERTIR 2.", null, true));
		while (pEmptyOptions)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		StartCoroutine(RunDialogueSection("It worked!~ You fixed him! Now just find a way to get him out of there!", "¡Funcionó! ~ ¡Lo arregló! ¡Ahora sáquelo de allí!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		if (!EnvironmentController.Instance.isSpanish)
		{
			oi.GetDialogue().Nodes[0].DialogueText = "ENTER A COMMAND.";
		}
		else
		{
			oi.GetDialogue().Nodes[0].DialogueText = "ENTRA UN COMANDO.";
		}
		if (EnvironmentController.Instance.ActionsLeft() > 1)
		{
			player.SetInteractable(oi);
			oi.Interact();
		}
		else
		{
			AppearInLair();
		}
	}

	private IEnumerator KillBilly()
	{
		Lily i = UnityEngine.Object.FindObjectOfType<Lily>();
		StartCoroutine(RunDialogueSection("NEUTRALIZING CONTENTS OF TANK 2.", "MATANDO A 2.", null, true));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("NO!!~ STOP!!~ WHAT HAVE YOU DONE?!", "¡¡NO!!~ ¡¡DETÉNGASE!~ ¡¿QUÉ HA HECHO?!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("You...~you killed him.", "Usted...~lo mató.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I failed him.", "Le fallé.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.showDeath = true;
		UI.CollapseDialogue();
		UI.ShowDeath("Don't blend Billy.", "No mates a Billy.");
	}

	private void KillCreature(int x)
	{
		SFXManager.Instance.StopSound("Blender");
		SFXManager.Instance.PlaySound("Blender");
		Transform target = GameObject.Find("Creature" + x).transform;
		target.DOKill();
		if (x != 2)
		{
			target.DOLocalMoveY(-1f, 2f).SetEase(Ease.Linear).OnComplete(delegate
			{
				GameObject.Find("BloodSystem" + x).GetComponent<ParticleSystem>().Play();
			});
			target.DOLocalMoveX(0.25f, 0.1f).SetEase(Ease.Linear).SetLoops(20, LoopType.Yoyo)
				.SetDelay(2f);
			target.DOLocalMoveY(-2f, 3f).SetEase(Ease.Linear).SetDelay(2f);
			if (x == 1)
			{
				mKilledCreature1 = true;
			}
			else if (x == 3)
			{
				mKilledCreature3 = true;
			}
		}
		else
		{
			target.DOLocalMoveY(-2f, 2f).SetEase(Ease.Linear).OnComplete(delegate
			{
				GameObject.Find("BloodSystem" + x).GetComponent<ParticleSystem>().Play();
			});
			target.DOLocalMoveX(0.15f, 0.1f).SetEase(Ease.Linear).SetLoops(20, LoopType.Yoyo)
				.SetDelay(2f);
			target.DOLocalMoveY(-3f, 3f).SetEase(Ease.Linear).SetDelay(2f);
		}
		GameObject.Find("VatGlass" + x).GetComponent<SpriteRenderer>().DOColor(Color.red, 3f)
			.SetDelay(2f)
			.OnComplete(delegate
			{
				SFXManager.Instance.StopSound("Blender");
			});
	}

	public void AppearInLair()
	{
		StartCoroutine(RunLightsOff());
	}

	private IEnumerator RunLightsOff()
	{
		player.inAnim = true;
		UI.showDeath = true;
		yield return new WaitForSeconds(1f);
		player.inAnim = false;
		RenderSettings.ambientIntensity = 0f;
		Light l1 = GameObject.Find("LairTopLight").GetComponent<Light>();
		Light l2 = GameObject.Find("LairBottomLight").GetComponent<Light>();
		l1.enabled = false;
		l2.enabled = false;
		Lily i = UnityEngine.Object.FindObjectOfType<Lily>();
		i.GetComponent<Renderer>().enabled = false;
		player.GetComponent<Renderer>().enabled = false;
		SFXManager.Instance.PlaySound("PowerDown");
		StartCoroutine(RunDialogueSection("AAAAHHH!! The lights went out!~ I can't see a thing!", "¡¡AAAAHHH!! ¡Se apagó la luz!~ ¡No veo nada!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		GameObject ps = GameObject.Find("PrincipalShooting");
		ps.GetComponent<MeshRenderer>().enabled = true;
		ps.GetComponent<SkeletonAnimation>().skeleton.flipX = false;
		ps.transform.position = new Vector3(18.54f, -4.37f, -4.475f);
		i.GetComponent<Renderer>().enabled = true;
		player.GetComponent<Renderer>().enabled = true;
		l1.enabled = true;
		l2.enabled = true;
		RenderSettings.ambientIntensity = 1.1f;
		SFXManager.Instance.PlaySound("PowerUp");
		i.SetDirection(true);
		player.ForceChangeDirection(true);
		StartCoroutine(RunDialogueSection("Get away from my experiments!!", "¡¡Aléjese de mis experimentos!!", this));
		UI.showDeath = false;
		yield return new WaitForSeconds(1f);
		i.SetDirection(true);
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I knew it!~ I knew you were behind all of this! You've had my brother the whole time!", "¡Lo sabía! ~ ¡Yo sabía que usted estaba detrás de todo ésto! ¡Usted ha tenido a mi hermano durante todo este tiempo!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Yes, Lily.~ You have been a very particular thorn in my side. I knew I shouldn't have chosen him.", "Sí, Lily.~ Usted ha sido una piedra en mi camino. Sabía que no lo debía haber elegido a él.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Chosen him for what? What is the meaning of all this?", "¿Elegirlo para qué? ¿Qué significa todo esto?", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Interact();
	}

	public void ExplodePrincipal()
	{
		StartCoroutine(ExplodePrincipalCoroutine());
	}

	private IEnumerator ExplodePrincipalCoroutine()
	{
		UnityEngine.Object.FindObjectOfType<Billy>().GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
		GameObject.Find("BillyCatcher").GetComponent<BoxCollider2D>().enabled = false;
		SFXManager.Instance.PlaySound("Bang");
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		Transform t = GameObject.Find("PrincipalExplosion").transform;
		Transform shooting = GameObject.Find("PrincipalShooting").transform;
		t.transform.position = shooting.transform.position + new Vector3(0.55f, 1.5f, 0f);
		shooting.GetComponent<Renderer>().enabled = false;
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
				if (i == 0)
				{
					r.AddForceAtPosition(new Vector2(-400f, 1000f), t.position + Vector3.up * 1.5f);
					r.AddTorque(-50f);
				}
				else if (i == 1)
				{
					r.AddForceAtPosition(new Vector2(UnityEngine.Random.Range(-400, -200), 200f), t.position + Vector3.up * 1.5f);
				}
				else if (i > 1)
				{
					r.AddForceAtPosition(new Vector2(UnityEngine.Random.Range(-400, -200), 400f), t.position + Vector3.up * 1.5f);
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
	}
}
