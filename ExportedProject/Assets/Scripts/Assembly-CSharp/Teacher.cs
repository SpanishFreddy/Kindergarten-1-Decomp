using System;
using System.Collections;
using DG.Tweening;
using DialogueTree;
using UnityEngine;
using UnityEngine.UI;

public class Teacher : NPCBehavior
{
	public TextureBlock blood1;

	public TextureBlock blood2;

	private int mStoredIndex;

	public bool mGotRidOfBuggs;

	public bool mIntroduction;

	public bool mBathroom;

	public bool mOffice;

	private bool mJeromeDistract;

	private bool mTookAPill;

	private bool mWarnedAboutCubby;

	private int mRecessStars;

	private void BringLunchPass()
	{
		GameObject gameObject = GameObject.Find("LunchPass");
		player.GetItem(Item.LunchPass);
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().color = Color.black;
	}

	private void GetStar()
	{
		player.GetItem(Item.GoldStar);
	}

	private void GetBugsStar()
	{
		player.GetItem(Item.GoldStar);
	}

	private void BuildCharacter()
	{
		lastSelectedOption.isComplete = true;
		if (!EnvironmentController.Instance.isSpanish)
		{
			GetCurrentConversation().DialogueText = "It'll build character!";
		}
		else
		{
			GetCurrentConversation().DialogueText = "Te hará más fuerte.";
		}
	}

	private void IsThisLegal()
	{
		lastSelectedOption.isComplete = true;
		if (!EnvironmentController.Instance.isSpanish)
		{
			GetCurrentConversation().DialogueText = "Probably not, but if this is how I can start my underground kid fighting ring then it's a risk I'm willing to take!";
		}
		else
		{
			GetCurrentConversation().DialogueText = "No, pero será divertido.";
		}
	}

	private void WhatsInIt()
	{
		CompleteMission(Mission.TeacherTellOnBuggs);
		lastSelectedOption.isComplete = true;
		if (!EnvironmentController.Instance.isSpanish)
		{
			GetCurrentConversation().DialogueText = "I'll give you a gold star! We all love gold stars, don't we?";
		}
		else
		{
			GetCurrentConversation().DialogueText = "¡Te daré una estrella!";
		}
		GetCurrentConversation().Options[3].IsAvailable = true;
		GetCurrentConversation().Options[4].IsAvailable = true;
	}

	private void WatchingBugs()
	{
		UnityEngine.Object.FindObjectOfType<Buggs>().SetCallOverPlayer(false);
		DialogueOption dialogueOption = UnityEngine.Object.FindObjectOfType<Buggs>().conversations[Room.SchoolYard].Nodes[4].Options[1];
		ActivateMission(Mission.TeacherStartFightWithBuggs, Room.SchoolYard);
		dialogueOption.DestinationID = -1;
		dialogueOption.FunctionCall = "SavedByTeacher";
	}

	private void WontRat()
	{
		UnityEngine.Object.FindObjectOfType<Buggs>().SetCallOverPlayer(true);
		conversations[Room.ClassroomLunch].Nodes[0].Options[0].IsAvailable = false;
		conversations[Room.ClassroomLunch].Nodes[0].Options[1].IsAvailable = true;
	}

	public void ExpelBugs()
	{
		CompleteMission(Mission.TeacherStartFightWithBuggs);
		player.SetInteractable(null);
		StartCoroutine(ExpelBuggsCoroutine());
	}

	private IEnumerator ExpelBuggsCoroutine()
	{
		Buggs b = UnityEngine.Object.FindObjectOfType<Buggs>();
		player.inAnim = true;
		SetDirection(false);
		SetAnimation("walk");
		StartCoroutine(RunDialogueSection("Buggs!!~~~~~ How dare you attack another student unprovoked again! That's the last straw. I'm sending you to the principal's office so he can finally expel you!", "¡¡Buggs!!~~~~~ ¡Cómo te atreves a atacar a otro estudiante sin razón! Esta es la gota que derramó el vaso. ¡Vete a la oficina del director a ver si finalmente puede expulsarte!", this));
		base.transform.DOMove(new Vector3(-16.1f, -4.45f, -6f), 3f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.PlayAnimation("idle");
			b.SetAnimation("idle");
			SetAnimation("idle");
			player.inAnim = false;
			SetDepth();
			player.CancelOverride();
			player.inDialogue = true;
			player.GetSkeleton().timeScale = 1.5f;
		});
		while (pEmptyOptions)
		{
			yield return null;
		}
		b.SetDirection(true);
		StartCoroutine(RunDialogueSection("No! Please! Don't!~ He started it! He's a liar! He said my dad was never coming back!", "¡No! Por favor! ¡No lo hagas!~ ¡Él empezó! ¡Es un mentiroso! ¡Él dijo que mi papá jamás regresaría!", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Well he's right...~ and after this little incident neither are you.~ Now go to the principal's office or I'll have the janitor carry you there!", "Pues, él tiene razón...~ y después de este pequeño incidente tampoco tú. ~ ¡Ahora ve a la oficina del director o haré que te lleve el conserje!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("But...~but...", "Pero...~pero...", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("\\bGO!!/b", "\\b¡¡VE!!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Stupid rat...~\\hwe coulda been friends if you didn't rat me out./h", "Estúpido traidor...~\\h hubiésemos podido ser amigos sino me hubieses delatado./h", b));
		UnlockHint("Buggs", 2);
		b.GetComponent<Collider2D>().enabled = false;
		b.SetAnimation("walk");
		player.inAnim = true;
		player.ForceChangeDirection(true);
		b.transform.DOMove(new Vector3(18.8f, -0.5f, -1.26f), 4f).SetEase(Ease.Linear).OnComplete(delegate
		{
			b.RemoveFromGame();
			player.inAnim = false;
			b.SetAnimation("idle");
		})
			.OnUpdate(b.SetDepth);
		while (pEmptyOptions)
		{
			yield return null;
		}
		UnityEngine.Object.FindObjectOfType<Nugget>().conversations[Room.Cafeteria].Nodes[12].Options[0].IsAvailable = false;
		UnityEngine.Object.FindObjectOfType<Nugget>().conversations[Room.Cafeteria].Nodes[12].Options[1].IsAvailable = true;
		player.ForceChangeDirection(true);
		UnityEngine.Object.FindObjectOfType<Principal>().SetPunishingBuggs(true);
		SetCurrentConversation(14);
		mGotRidOfBuggs = true;
		Interact();
	}

	private void ActivateJeromeMission()
	{
		ActivateMission(Mission.TeacherTalkMorningTimeJerome, Room.Classroom1);
	}

	public void EndSchoolYard()
	{
		if (EnvironmentController.currentRoom == Room.SchoolYard)
		{
			mIntroduction = true;
			SFXManager.Instance.PlaySound("Bell");
			SetCurrentConversation(17);
			Interact();
		}
	}

	private void GoToClassroom()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom1);
	}

	private void NuggetSendToPrincipal()
	{
		UI.CollapseDialogue();
		UnityEngine.Object.FindObjectOfType<Principal>().SetNugget();
		CompleteMission(Mission.LilyGetSentToOffice);
		EnvironmentController.Instance.ChangeEnvironment(Room.Office);
		conversations[Room.Classroom1].currentConversation = 33;
	}

	public void StartClassroom()
	{
		if (mIntroduction)
		{
			Interact();
			if (mGotRidOfBuggs)
			{
				UnityEngine.Object.FindObjectOfType<Buggs>().RemoveFromGame();
			}
			mIntroduction = false;
		}
		else if (mBathroom)
		{
			TakePassBack();
			mBathroom = false;
		}
		else if (mOffice)
		{
			BackFromOffice();
			mOffice = false;
		}
	}

	public void DisableBlockCubbies()
	{
		GameObject.Find("Classroom1/Interactables/CubbyBuggs").GetComponent<ObjectInteractable>().startFunction = string.Empty;
		GameObject.Find("Classroom1/Interactables/CubbyNugget").GetComponent<ObjectInteractable>().startFunction = string.Empty;
		GameObject.Find("Classroom1/Interactables/CubbyMonty").GetComponent<ObjectInteractable>().startFunction = string.Empty;
		GameObject.Find("Classroom1/Interactables/CubbyCindy").GetComponent<ObjectInteractable>().startFunction = string.Empty;
		GameObject.Find("Classroom1/Interactables/CubbyLily").GetComponent<ObjectInteractable>().startFunction = string.Empty;
		GameObject.Find("Classroom1/Interactables/CubbyJerome").GetComponent<ObjectInteractable>().startFunction = string.Empty;
	}

	public void EnableBlockCubbies()
	{
		if (mTookAPill)
		{
			GameObject.Find("Classroom1/Interactables/CubbyBuggs").GetComponent<ObjectInteractable>().startFunction = "BlockCubbyPills";
			GameObject.Find("Classroom1/Interactables/CubbyNugget").GetComponent<ObjectInteractable>().startFunction = "BlockCubbyPills";
			GameObject.Find("Classroom1/Interactables/CubbyMonty").GetComponent<ObjectInteractable>().startFunction = "BlockCubbyPills";
			GameObject.Find("Classroom1/Interactables/CubbyCindy").GetComponent<ObjectInteractable>().startFunction = "BlockCubbyPills";
			GameObject.Find("Classroom1/Interactables/CubbyLily").GetComponent<ObjectInteractable>().startFunction = "BlockCubbyPills";
			GameObject.Find("Classroom1/Interactables/CubbyJerome").GetComponent<ObjectInteractable>().startFunction = "BlockCubbyPills";
		}
		else
		{
			GameObject.Find("Classroom1/Interactables/CubbyBuggs").GetComponent<ObjectInteractable>().startFunction = "BlockCubby";
			GameObject.Find("Classroom1/Interactables/CubbyNugget").GetComponent<ObjectInteractable>().startFunction = "BlockCubby";
			GameObject.Find("Classroom1/Interactables/CubbyMonty").GetComponent<ObjectInteractable>().startFunction = "BlockCubby";
			GameObject.Find("Classroom1/Interactables/CubbyCindy").GetComponent<ObjectInteractable>().startFunction = "BlockCubby";
			GameObject.Find("Classroom1/Interactables/CubbyLily").GetComponent<ObjectInteractable>().startFunction = "BlockCubby";
			GameObject.Find("Classroom1/Interactables/CubbyJerome").GetComponent<ObjectInteractable>().startFunction = "BlockCubby";
		}
	}

	private void GetNuggetFromCubby()
	{
		Nugget nugget = UnityEngine.Object.FindObjectOfType<Nugget>();
		player.GetItem(Item.Nugget);
		GameObject.Find("NuggetInCubby").SetActive(false);
		nugget.conversations[Room.Classroom1].Nodes[8].Options[0].IsAvailable = true;
		if (UI.IsMissionActive(Mission.NuggetGetCubbyNugget))
		{
			CompleteMission(Mission.NuggetGetCubbyNugget);
			nugget.ActivateMission(Mission.NuggetStartThirdNugget, Room.Classroom1);
		}
		else if (UI.IsMissionComplete(Mission.NuggetStartCollectNuggets))
		{
			nugget.ActivateMission(Mission.NuggetStartThirdNugget, Room.Classroom1);
		}
	}

	private void StealMoney()
	{
		GameObject.Find("MoneyInCubby").GetComponent<SpriteRenderer>().enabled = false;
		player.GetMoney(2f);
	}

	private void GetCardFromCubby()
	{
		GameObject.Find("CardInCubby").GetComponent<SpriteRenderer>().enabled = false;
		player.GetItem(Item.FreezyguyJim);
		UnlockHint("Monstermon", 2);
	}

	private void GetKnifeFromCubby()
	{
		player.GetItem(Item.Knife);
		GameObject.Find("KnifeInCubby").GetComponent<SpriteRenderer>().enabled = false;
	}

	private void GetBugFromCubby()
	{
		player.GetItem(Item.Bug);
		GameObject.Find("BugInCubby").GetComponent<SpriteRenderer>().enabled = false;
	}

	private void GetMagGlassFromCubby()
	{
		player.GetItem(Item.MagnifyingGlass);
		GameObject.Find("MagGlassInCubby").GetComponent<SpriteRenderer>().enabled = false;
	}

	public void SetJeromeDistract(bool x)
	{
		mJeromeDistract = x;
		if (!x)
		{
			if (mTookAPill)
			{
				if (mWarnedAboutCubby)
				{
					DisableBlockCubbies();
				}
				else
				{
					EnableBlockCubbies();
				}
			}
			else
			{
				EnableBlockCubbies();
			}
		}
		else
		{
			DisableBlockCubbies();
		}
	}

	private void GetCindyStar()
	{
		player.GetItem(Item.GoldStar);
	}

	private void TakeStar()
	{
		player.UseItem(Item.GoldStar);
	}

	private void UnlockLunchPassHint()
	{
		UnlockHint("Buggs", 4);
	}

	public void SetComingFromOffice(bool expelled, bool buggsInRoom, string n)
	{
		mIntroduction = false;
		mOffice = true;
		if (n == "Cindy")
		{
			if (buggsInRoom)
			{
				if (expelled)
				{
					conversations[Room.Classroom1].currentConversation = 29;
				}
				else
				{
					conversations[Room.Classroom1].currentConversation = 14;
				}
			}
			else if (expelled)
			{
				conversations[Room.Classroom1].currentConversation = 29;
				conversations[Room.Classroom1].Nodes[29].Options[0].IsAvailable = false;
				conversations[Room.Classroom1].Nodes[29].Options[1].IsAvailable = true;
				conversations[Room.Classroom1].Nodes[16].Options[0].IsAvailable = false;
				conversations[Room.Classroom1].Nodes[16].Options[1].IsAvailable = true;
			}
			else
			{
				conversations[Room.Classroom1].currentConversation = 32;
			}
		}
		if (n == "Nugget" && buggsInRoom)
		{
			conversations[Room.Classroom1].currentConversation = 33;
		}
	}

	public void StopPlayerFromLeaving()
	{
		if (!mJeromeDistract)
		{
			BoxCollider2D dc = GameObject.Find("ClassroomDoor").GetComponent<BoxCollider2D>();
			dc.enabled = false;
			SetAnimation("walk");
			StoreConversation();
			conversations[Room.Classroom1].currentConversation = 2;
			player.inAnim = true;
			Interact();
			SetDirection(false);
			Vector3[] path = new Vector3[3]
			{
				new Vector3(7.6f, -7f, -7f),
				new Vector3(4.45f, -2.5f, -2.5f),
				new Vector3(-12.5f, -2.5f, 2.5f)
			};
			player.ForceChangeDirection(true);
			base.transform.DOPath(path, 3f).SetEase(Ease.Linear).OnComplete(delegate
			{
				SetDepth();
				player.inAnim = false;
				SetAnimation("idle");
				dc.enabled = true;
			})
				.OnUpdate(base.SetDepth);
		}
		else
		{
			EnvironmentController.Instance.ChangeEnvironment(Room.Hallway);
			UnityEngine.Object.FindObjectOfType<Jerome>().SetTriedToLeave();
		}
	}

	public void TakePassBack()
	{
		GameObject.Find("ClassroomDoor").GetComponent<Collider2D>().enabled = false;
		SetAnimation("walk");
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			SetCurrentConversation(57);
		}
		else
		{
			SetCurrentConversation(28);
		}
		player.inAnim = true;
		Interact();
		SetDirection(false);
		Vector3[] path = new Vector3[3]
		{
			new Vector3(7.6f, -7f, -7f),
			new Vector3(4.45f, -2.5f, -2.5f),
			new Vector3(-12.5f, -2.5f, 2.5f)
		};
		player.ForceChangeDirection(true);
		base.transform.DOPath(path, 3f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetDepth();
			player.inAnim = false;
			SetAnimation("idle");
			GameObject.Find("ClassroomDoor").GetComponent<Collider2D>().enabled = true;
			player.UseItem(Item.BathroomPass);
		})
			.OnUpdate(base.SetDepth);
	}

	private void GoToLunchFromDoor()
	{
	}

	public void BackFromOffice()
	{
		SetAnimation("walk");
		player.inAnim = true;
		Interact();
		Vector3[] path = new Vector3[3]
		{
			new Vector3(7.6f, -7f, -7f),
			new Vector3(4.45f, -2.5f, -2.5f),
			new Vector3(-12.5f, -2.5f, 2.5f)
		};
		SetDirection(false);
		base.transform.DOPath(path, 3f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetDepth();
			player.inAnim = false;
			mySkeleton.AnimationName = "idle";
			GameObject.Find("ClassroomDoor").GetComponent<Collider2D>().enabled = true;
		})
			.OnUpdate(base.SetDepth);
	}

	public void WalkBackToStart()
	{
		if (player.inDialogue)
		{
			StartCoroutine(WaitWalkBackToStart());
			return;
		}
		GameObject.Find("ClassroomDoor").GetComponent<Collider2D>().enabled = false;
		SetDirection(true);
		SetAnimation("walk");
		if (mStoredIndex > 0)
		{
			ResetConversation();
		}
		Vector3[] path = new Vector3[3]
		{
			new Vector3(4.45f, -2.5f, -2.5f),
			new Vector3(7.6f, -7f, -7f),
			new Vector3(25.6f, -7f, -7f)
		};
		base.transform.DOPath(path, 3f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetDirection(false);
			SetAnimation("idle");
			GameObject.Find("ClassroomDoor").GetComponent<Collider2D>().enabled = true;
		})
			.OnUpdate(base.SetDepth);
	}

	private IEnumerator WaitWalkBackToStart()
	{
		while (player.inDialogue)
		{
			yield return null;
		}
		WalkBackToStart();
	}

	public void GetBathroomPass()
	{
		player.GetItem(Item.BathroomPass);
	}

	public void GoToHallway()
	{
		mBathroom = true;
		EnvironmentController.Instance.ChangeEnvironment(Room.Hallway);
	}

	private void WhyDontYouGo()
	{
		if (!EnvironmentController.Instance.isSpanish)
		{
			conversations[Room.Classroom1].Nodes[7].DialogueText = "So will you stop laughing and help me?";
		}
		else
		{
			conversations[Room.Classroom1].Nodes[7].DialogueText = "Por lo que va a ayudar?";
		}
		conversations[Room.Classroom1].Nodes[7].Options[0].isComplete = true;
	}

	public void ExpelForThat()
	{
		DialogueNode dialogueNode = conversations[Room.Classroom1].Nodes[7];
		if (!EnvironmentController.Instance.isSpanish)
		{
			dialogueNode.DialogueText = "I've tried to have him expelled, but the principal wants me to give him proof that it's happening!";
		}
		else
		{
			dialogueNode.DialogueText = "Lo he intentado, pero necesito pruebas!";
		}
		dialogueNode.Options[1].isComplete = true;
		dialogueNode.Options[2].SetAvailable();
	}

	public void RatOnJerome()
	{
		FailMission(Mission.JeromeLeaveClassroom);
		StartCoroutine(RatOnJeromeCoroutine());
		player.lockActions = false;
		UnityEngine.Object.FindObjectOfType<Jerome>().StopAllCoroutines();
		SetJeromeDistract(false);
		CompleteMission(Mission.TeacherGivePass);
		player.UseItem(Item.HallPass);
		UnlockHint("Teacher", 4);
	}

	private IEnumerator RatOnJeromeCoroutine()
	{
		Jerome i = UnityEngine.Object.FindObjectOfType<Jerome>();
		StartCoroutine(RunDialogueSection("I knew it!", "¡Lo sabía!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(true);
		StartCoroutine(RunDialogueSection("Jerome, you little cockroach, we've been over this! You can't just steal your dad's hall passes!", "Jerome, pequeña cucaracha, ¡hemos terminado con ésto! ¡No puedes robar los pases de tu padre!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("What?! You ratted me out to the teacher? Dude!", "¡¿Qué?! ¿Me delataste con la maestra? ¡Socio!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetCurrentConversation(22);
		UpdateOptions();
		Interact();
	}

	private void RatOnJerome2()
	{
		StartCoroutine(RatOnJerome2Coroutine());
	}

	private IEnumerator RatOnJerome2Coroutine()
	{
		Jerome i = UnityEngine.Object.FindObjectOfType<Jerome>();
		StartCoroutine(RunDialogueSection("No. Cause that's what you are, Teacher's SPECIAL~ LITTLE~ RAT.~ Just like Jerome is Teacher's special little suspended hooligan.", "No. Porque eso es lo que eres , EL PEQUEÑO TRAIDOR ESPECIAL de la profesora. ~ Igual que Jerome es el pequeño vándalo suspendido especial de la profesora.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Awww c'mon! Bro, I thought you were cool!", "¡Es en serio, hermano!?, pensé que eras \u00b4cool!\u00b4", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.GetItem(Item.GoldStar);
		StartCoroutine(RunDialogueSection("Well he's not...~and because he's so uncool, he gets one of my special gold stars. Now why don't you go take that pass back to your dad? I'm sure he'd love to know where it went.", "Pues no, él no es cool...~y por no ser \u00b4cool\u00b4 él recibe mis estrellas especiales de oro. ¿Y ahora,  por qué no vas y le devuelves este pase a tu papá? Estoy segura de que a él le encantaría saber a donde estaba.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Vector3[] v = new Vector3[3]
		{
			new Vector3(7.5f, -6.7f, -6.69f),
			new Vector3(4.2f, -2.9f, -2.89f),
			new Vector3(-17.8f, -0.5f, -0.5f)
		};
		i.SetAnimation("walk");
		i.GetComponent<BoxCollider2D>().enabled = false;
		player.inAnim = true;
		i.transform.DOPath(v, 4f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.inAnim = false;
			i.RemoveFromGame();
		});
		StartCoroutine(RunDialogueSection("Man...~stupid rat...~getting me in trouble...~My dad is gonna kill me.", "Carajo...~estúpido traidor...~me estás metiendo en problemas...~Mi papá me va a matar.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetCurrentConversation(25);
		Interact();
	}

	private void ActivateGetPassFromJeromeMission()
	{
		CompleteMission(Mission.TeacherTalkMorningTimeJerome);
		ActivateMission(Mission.TeacherTryToGetPass, Room.Classroom1);
	}

	private void GetDollar()
	{
		player.GetMoney(1f);
		TookAPill();
	}

	private void TookAPill()
	{
		mTookAPill = true;
		EnableBlockCubbies();
		if (player.HasItem(Item.Pill))
		{
			player.UseItem(Item.Pill);
		}
		UnlockHint("Nugget", 3);
	}

	public void BlockCubby()
	{
		UnityEngine.Object.FindObjectOfType<Nugget>().conversations[Room.Classroom1].Nodes[10].Options[1].IsAvailable = true;
		StartCoroutine(StopInspectingCubby());
	}

	private IEnumerator StopInspectingCubby()
	{
		player.ForceChangeDirection(true);
		StartCoroutine(RunDialogueSection("Hey!~ Don't go snooping around in other people's cubbies!", "¡Hey!~ ¡No husmees por las oficinas ajenas!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
	}

	public void BlockCubbyPills()
	{
		StartCoroutine(AllowInspectingCubby());
	}

	private IEnumerator AllowInspectingCubby()
	{
		player.ForceChangeDirection(true);
		mWarnedAboutCubby = true;
		StartCoroutine(RunDialogueSection("Hey!~ Don't go snooping around in...~you know what? I don't really care. Do what you want.", "¡Hey!~ No husmees por...~¿sabes que..? A mí no me importa. Haz lo que te de la gana.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		DisableBlockCubbies();
		UI.CollapseDialogue();
	}

	private void UseLunchPass()
	{
		player.UseItem(Item.LunchPass);
		if (UI.IsMissionActive(Mission.BuggsGoToLunchWithTeacher))
		{
			CompleteMission(Mission.BuggsGoToLunchWithTeacher);
			UnityEngine.Object.FindObjectOfType<Buggs>().ActivateMission(Mission.BuggsKillTeacher);
		}
		StartCoroutine(GoToTeacherLunch());
	}

	private IEnumerator GoToTeacherLunch()
	{
		SFXManager.Instance.PlaySound("Confirm");
		StartCoroutine(RunDialogueSection("The rest of you run along to the cafeteria.", "El resto de ustedes corran a la cafetería.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		EnvironmentController.Instance.ChangeEnvironment(Room.ClassroomLunch);
	}

	public void EndClassroom1()
	{
		SFXManager.Instance.PlaySound("Bell");
		SetCurrentConversation(36);
		Interact();
		Principal principal = UnityEngine.Object.FindObjectOfType<Principal>();
		principal.StartCoroutine(principal.DesaturateCamera());
	}

	private void GoToLunch()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Cafeteria);
	}

	private void ActivateLunchMissions()
	{
		ActivateMission(Mission.TeacherNuggetGainTrust, Room.Cafeteria);
		if (!UnityEngine.Object.FindObjectOfType<Buggs>().removedFromGame)
		{
			ActivateMission(Mission.TeacherGetBuggsInTrouble, Room.Recess);
		}
		ActivateMission(Mission.TeacherGetLilyInTrouble, Room.Recess);
		ActivateMission(Mission.TeacherGetMontyInTrouble, Room.Recess);
	}

	public void StartRecess()
	{
		player.lockActions = false;
		if (UI.IsMissionActive(Mission.TeacherTellGainedTrust) || UI.IsMissionActive(Mission.TeacherNuggetGainTrust) || UI.IsMissionFailed(Mission.TeacherNuggetGainTrust))
		{
			int num = 22;
			UnlockHint("Teacher", 5);
			if (UnityEngine.Object.FindObjectOfType<Lily>().removedFromGame)
			{
				num += 4;
				mRecessStars++;
			}
			if (UnityEngine.Object.FindObjectOfType<Buggs>().removedFromGame)
			{
				num += 2;
				mRecessStars++;
			}
			if (UnityEngine.Object.FindObjectOfType<Monty>().removedFromGame)
			{
				num++;
				mRecessStars++;
			}
			SetCurrentConversation(num);
			Interact();
		}
		else if (UI.IsMissionComplete(Mission.BuggsKillTeacher))
		{
			base.transform.position = new Vector3(0f, 50f, 0f);
		}
	}

	private void GetRecessStars()
	{
		for (int i = 0; i < mRecessStars; i++)
		{
			player.GetItem(Item.GoldStar);
		}
	}

	private void TellAboutBuggs()
	{
		player.GetItem(Item.GoldStar);
		conversations[Room.Recess].Nodes[0].Options[0].IsAvailable = false;
	}

	private void StartNuggetCave()
	{
		UnityEngine.Object.FindObjectOfType<Nugget>().SetCurrentConversation(31);
		CompleteMission(Mission.TeacherTellGainedTrust);
		ActivateMission(Mission.TeacherCollectEvidence, Room.Classroom2);
	}

	private void SendToOfficeStatue()
	{
		UnityEngine.Object.FindObjectOfType<Lily>().RemoveFromGame(false);
		UnityEngine.Object.FindObjectOfType<Principal>().SetStatue();
		EnvironmentController.Instance.ChangeEnvironment(Room.Office);
	}

	public IEnumerator WhatHappenedToLily()
	{
		Lily j = UnityEngine.Object.FindObjectOfType<Lily>();
		Nugget k = UnityEngine.Object.FindObjectOfType<Nugget>();
		Cindy c = UnityEngine.Object.FindObjectOfType<Cindy>();
		player.inDialogue = true;
		player.inAnim = true;
		SetDirection(false);
		mySkeleton.AnimationName = "walk";
		base.transform.DOMove(new Vector3(10.25f, -7f, -4.3f), 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetDepth();
			mySkeleton.AnimationName = "idle";
			SetEmptyOptions();
			player.inAnim = false;
		});
		player.ForceChangeDirection(true);
		StartCoroutine(RunDialogueSection("Oh dear! Has anyone seen Lily? I don't see her anywhere!", "¡Por Dios! ¿Alguien ha visto a Lily? ¡No la puedo encontrar!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		k.SetDirection(true);
		StartCoroutine(RunDialogueSection("Nugget saw where the girl went!~ Nugget knows!", "¡Nugget vió a donde se fue la niña!~ ¡Nugget sabe!", k));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Shut up Nugget! You didn't see anything.", "¡Cállate, Nugget! Tú no has visto nada.", c));
		while (pEmptyOptions)
		{
			yield return null;
		}
		mySkeleton.AnimationName = "walk";
		base.transform.DOMove(new Vector3(-5f, -10f, -10f), 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			mySkeleton.AnimationName = "idle";
			SetDepth();
			player.inAnim = false;
		});
		StartCoroutine(RunDialogueSection("Nugget, I swear to whatever god your messed up family believes in, if Lily fell down that hole you dug I'm pushing you in after her!", "Nugget, ¡Juro por cualquier dios que tu desastrosa familia crea: si Lily se cayó en ese agujero que excavaste, tú vas a ir tras de ella!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Nugget will not give teacher the satisfaction. Teacher will never take Nugget alive!", "Nugget no le dará ese placer a la profesora. ¡La profesora no atrapará a Nugget vivo!", k));
		while (pEmptyOptions)
		{
			yield return null;
		}
		BoxCollider2D b = k.GetComponent<BoxCollider2D>();
		Rigidbody2D r = k.gameObject.AddComponent<Rigidbody2D>();
		k.transform.position = new Vector3(k.transform.position.x, k.transform.position.y, -7.15f);
		k.gameObject.layer = 9;
		b.offset = new Vector2(0f, 1.25f);
		b.size = new Vector2(1.26f, 2.5f);
		r.AddTorque(-100f);
		r.AddForce(Vector2.up * 20f);
		yield return new WaitForSeconds(1.5f);
		k.RemoveFromGame();
		yield return new WaitForSeconds(1.5f);
		SFXManager.Instance.PlaySound("Thud");
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		yield return new WaitForSeconds(1f);
		StartCoroutine(RunDialogueSection("Wow that hole is deep. I can't even see the bottom! We're gonna have to get the janitor out here with the ladder to get them out.", "Wow este agujero es profundo. ¡Ni siquiera puedo ver el fondo! ¡Hay que llamar al conserje para que venga con su escalera y los saque de aquí!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(true);
		StartCoroutine(RunDialogueSection("\\bOH MR.JANITOOOOOR!/b", "\\b¡OH SEÑOR CONSERJE!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Janitor i = UnityEngine.Object.FindObjectOfType<Janitor>();
		i.mySkeleton.AnimationName = "walk";
		i.transform.DOMoveX(4f, 4f).SetEase(Ease.Linear).OnComplete(delegate
		{
			i.mySkeleton.AnimationName = "idle";
			player.inAnim = false;
		});
		StartCoroutine(RunDialogueSection("I'm comin.' I'm comin.'~~~ What seems to be the problem there Ms. Jigglytits.", "Voy. Voy.'~~~ Parece que aquí tenemos un problema, Señora Tetasbailarinas.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("It's Applegate...~and I have two of my students stuck at the bottom of this hole!", "Es Applegate...~ ¡además tengo dos estudiantes atascados en el fondo del agujero!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("You want me to put the rest of 'em in the hole?", "¿Quieres que ponga a los demás en el agujero?", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("No! I want you to get the ones that are IN the hole, OUT of the hole. Can you do that?", "¡No! ! Yo quiero que saques los que están EN el agujero. ¿Puedes hacerlo?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Oh...~well that's less fun, but I guess I can do that. It might take awhile though. Might be best if you brought the ones that aren't in the hole back inside so I don't get confused.", "Oh...~está bien, aunque no es tan divertido, pero creo que puedo hacerlo.  Sin embargo, puede que tome tiempo. Lo mejor sería que llevaras adentro a los demás niños que no están en el agujero, así no me confundo.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Very well. Okay children, we're gonna cut recess a little short today! Let's head inside for show and tell.", "Perfecto. ¡Está bien niños, vamos a cortar un poco el tiempo del recreo hoy! Entremos al salón para empezar \u00b4mostrar y contar\u00b4.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		j.RemoveFromGame();
		k.RemoveFromGame();
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom2);
	}

	private void WalkBackRecess()
	{
		SetDirection(true);
		SetAnimation("walk");
		base.transform.DOMoveX(26.6f, 1.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetAnimation("idle");
			SetDirection(false);
		});
	}

	private void GoToClassroom2()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom2);
	}

	private void ShowDog()
	{
		UnlockHint("Teacher", 7);
		player.UseItem(Item.CindyDog);
		StartCoroutine(ShowDogCoroutine());
	}

	private IEnumerator ShowDogCoroutine()
	{
		StartCoroutine(RunDialogueSection("Oh my dearie me!~ Is that--~is that--", "¡Oh, queridita!~ Este es--~Este es—", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("\\bIs that Cindy's dog?!/b", "\\b¡¿Este es el perro de Cindy?!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.ForceChangeDirection(false);
		SetDirection(false);
		Cindy c = UnityEngine.Object.FindObjectOfType<Cindy>();
		if (c.removedFromGame)
		{
			Nugget i = UnityEngine.Object.FindObjectOfType<Nugget>();
			StartCoroutine(RunDialogueSection("\\bNUGGET!!/b", "\\b¡¡NUGGET!!/b", this));
			i.SetDirection(true);
			WalkToPoint(new Vector3(2.09f, -10.3f, -10.3f), 3f);
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("What is it large teacher lady?", "¿Qué es esta cosa, profesora?", i));
			i.WalkToPoint(new Vector3(-6f, -10.3f, -10.3f), 1f);
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Did you kill Cindy's dog?!", "¡¿Mataste al perro de Cindy?¡", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("What?!~ No! Nugget would never--~ well not NEVER, but Nugget is relatively harmless!", "¡¿Qué?!~ ¡No! Nugget jamás--~ esta bien, no JAMÁS ¡pero Nugget es relativamente inofensivo!", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Don't lie to me!", "¡No me mientas!", this));
			WalkToPoint(new Vector3(-2.25f, -10.3f, -10.3f), 1f);
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Nugget is not lying! Nugget knows it was the janitor who killed the little puppy!", "¡Nugget no miente! Nugget sabe que el conserje fue quien mató al pobre cachorrito", i));
			i.WalkToPoint(new Vector3(-7.85f, -10.3f, -10.3f), 1f);
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I know it was you!!", "¡¡Yo sé que fuiste tú!!", this));
			WalkToPoint(new Vector3(-6.5f, -10.3f, -10.3f), 0.5f);
			yield return new WaitForSeconds(0.5f);
			EnvironmentController.ShowHoleCover(true);
			BoxCollider2D b = i.GetComponent<BoxCollider2D>();
			Rigidbody2D r = i.gameObject.AddComponent<Rigidbody2D>();
			i.gameObject.layer = 9;
			b.offset = new Vector2(0f, 1.25f);
			b.size = new Vector2(1.26f, 2.5f);
			r.AddTorque(100f);
			r.AddForce(new Vector2(-200f, 20f));
			i.transform.position = new Vector3(i.transform.position.x, i.transform.position.y, -7.15f);
			yield return new WaitForSeconds(0.51f);
			player.inAnim = true;
			yield return new WaitForSeconds(2.49f);
			player.inAnim = false;
			EnvironmentController.ShowHoleCover(false);
			i.RemoveFromGame();
			SFXManager.Instance.PlaySound("Thud");
			UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Oh my!~ That was unfortunate.", "¡Ay, Dios! ~ Ésto es lamentable.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			SetDirection(true);
			WalkToPoint(new Vector3(26.6f, -4.4f, -4.37f), 2.5f);
			yield return new WaitForSeconds(2.5f);
			FacePlayer();
			player.ForceChangeDirection(base.transform.position.x > player.transform.position.x);
			SetCurrentConversation(34);
			Interact();
		}
		else
		{
			UnlockHint("Teacher", 2);
			c.SetDirection(true);
			player.inAnim = true;
			UI.StartDialogue("\\bWhat?!/b", c);
			c.WalkToPoint(player.transform.position - Vector3.right, 3f);
			yield return new WaitForSeconds(3f);
			player.inAnim = false;
			StartCoroutine(RunDialogueSection("You were the one who kidnapped my dog!? Oh my God! She's dead!", "¿¡Fuiste tú quien secuestró a mi perro!? ¡Oh, Dios mío! ¡Ella está muerta!", c));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Cindy, I think what he was trying to tell me is that Nugget--", "Cindy, creo que lo que él está tratando de decirme es que Nugget—", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			player.ForceChangeDirection(false);
			StartCoroutine(RunDialogueSection("You shut your fat ugly mouth! I have to make this right!", "¡Cierra tu asquerosa boca! ¡Tengo que hacer esto bien!", c));
			while (pEmptyOptions)
			{
				yield return null;
			}
			c.StabNoDeath();
			StartCoroutine(RunDialogueSection("Cindy no! You can't just--~ actually it would be one less kid for me to deal with.", "¡Cindy no! No puedes solamente--~ De hecho es un niño menos con el cual debo lidiar.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Good job Cindy!~ You've earned a gold star!", "¡Buen trabajo Cindy!~ ¡Te has ganado una estrella de oro!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UI.showDeath = true;
			UI.ShowDeath("Cindy might be a little upset to hear about her dog.", "Cindy está enojada por su perro.");
		}
	}

	private void GetNuggetStar()
	{
		player.GetItem(Item.GoldStar);
		if (player.GetItemCount(Item.GoldStar) == 6)
		{
			lastSelectedOption.DestinationID = 35;
		}
	}

	private void EndDay()
	{
		player.inAnim = true;
		player.ForceChangeDirection(false);
		player.ForceAnim(true);
		player.transform.DOMoveX(-36f, 7f).SetEase(Ease.Linear);
		pCollider.enabled = false;
		SetDirection(true);
		SetAnimation("walk");
		base.transform.DOMoveX(38f, 1.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			UI.CompleteDay(Item.LunchPass);
			SteamScript.UnlockAchievement("TeacherAchievement");
			UnityEngine.Object.FindObjectOfType<PauseMenu>().UnlockAllHints("Teacher");
		});
	}

	private void GetReallyBrightStar()
	{
		player.GetItem(Item.ReallyBrightStar);
		UnlockHint("Monstermon", 9);
	}

	private void BribeWithDonut()
	{
		player.UseItem(Item.Donut);
	}

	private void PickUpRecessCard()
	{
		player.GetItem(Item.LiterallyGrass);
		UnlockHint("Monstermon", 7);
		GameObject.Find("MonstermonCardPlayground").GetComponent<BoxCollider2D>().enabled = false;
	}

	public void EndRecess()
	{
		if (!UnityEngine.Object.FindObjectOfType<Monty>().inRecessScene)
		{
			SFXManager.Instance.PlaySound("Bell");
			SetCurrentConversation(44);
			Interact();
		}
	}

	private void GoToCS2()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom2);
	}

	public void StartClassRoom2()
	{
		if (UI.IsMissionComplete(Mission.BuggsKillTeacher))
		{
			Principal principal = UnityEngine.Object.FindObjectOfType<Principal>();
			principal.transform.position = base.transform.position + Vector3.right * -7f;
			StartCoroutine(principal.StartCS2());
			SetDirection(true);
			base.transform.position = new Vector3(15.25f, -5.9f, -7.2f);
			base.transform.eulerAngles = new Vector3(0f, 0f, -90f);
		}
		else
		{
			StartCoroutine(BeginShowAndTell());
		}
	}

	private IEnumerator BeginShowAndTell()
	{
		if (!UI.IsMissionComplete(Mission.LilySaveBilly))
		{
			if (UI.IsMissionFailed(Mission.JeromeSaveFromJanitor))
			{
				Monty j = UnityEngine.Object.FindObjectOfType<Monty>();
				UnityEngine.Object.FindObjectOfType<Jerome>().RemoveFromGame();
				StartCoroutine(RunDialogueSection("I hope everyone had a good recess! Let's all get ready for show--", "¡Espero que todos hayan tenido un buen recreo! Preparémonos para 'Mostrar--", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Ummm...Ms. Applegate?~ Where's Jerome?", "Ummm...¿Señora Applegate? ~ ¿Dónde está Jerome?", j));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("The janitor insisted that Jerome stay behind after recess to answer some questions about the mess in his closet.", "El conserje insistió en que Jerome se quedara después del recreo para contestar algunas preguntas sobre el desorden en su armario.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("He's dead isn't he?", "¿Él está muerto, verdad?", j));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("It'd be a safe bet.", "Sería una apuesta segura.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Anyway...~let's begin show and tell.~ Why don't you go first? What did you bring?", "Como sea...~vamos a empezar 'Mostrar y Contar.~ ¿Por qué no vas tu primero? ¿Qué trajiste?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("I hope everyone had a good recess! Let's all get ready for show and tell.~ Why don't you go first? What did you bring?", "¡Espero que todos hayan tenido un buen recreo! Preparense para \u00b4Mostrar y Contar\u00b4.~ ¿Por qué no vas tu primero? ¿Qué trajiste?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			if (player.ItemCount() > 0)
			{
				StartCoroutine(player.SelectShowAndTellItem());
				yield break;
			}
			StartCoroutine(RunDialogueSection("You didn't bring anything?~ You realize that this is pretty much the only thing you have to do in my class?", "¿No trajiste nada?~ ¿Te has dado cuenta de que ésto es casi lo único que tienes que hacer en mi clase?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			Interact();
			yield break;
		}
		Lily i = UnityEngine.Object.FindObjectOfType<Lily>();
		Billy b = UnityEngine.Object.FindObjectOfType<Billy>();
		Nugget k = UnityEngine.Object.FindObjectOfType<Nugget>();
		b.transform.position = new Vector3(-22.78f, -10.37f, -10.36f);
		b.SetDirection(true);
		b.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
		StartCoroutine(RunDialogueSection("Well look who's back from skipping recess!", "Bueno, ¡miren quien ha regresado del recreo!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Nugget's friend has returned! Billy is free from the evil principal!", "¡El amigo de Nugget regresó! ¡Billy se liberó del malvado director!", k));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Shut it weirdo. I hope you all have a good excuse!", "Cierra la boca bicho raro. ¡Espero que todos tengan una buena excusa! ", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I'm sorry Ms. Applegate, but we found Billy!", "Perdón Señora Applegate, pero encontramos a Billy.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I was in a vat under the school.", "Estaba en el túnel debajo de la escuela.", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I see...~but that's no excuse for leaving a supervised area! Go to the principal's office!", "Ok...~¡pero esa no es una excusa válida para salir del área que está bajo supervisión! ¡Ve a la oficina del director!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Ummmm...~the principal kinda...~exploded.", "Ummmm...~el director mas o menos... explotó.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("So there's no one around to sign my checks?", "¿Así que no hay nadie para firmar mis cheques?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I don't think so.", "No lo creo.", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Okay then...~bye!", "Entonces...~¡adios!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		WalkToPoint(new Vector3(-18.3f, -0.36f, -2.7f), 1f);
		UI.CollapseDialogue();
		player.inAnim = true;
		yield return new WaitForSeconds(1f);
		base.transform.position = new Vector3(0f, 50f, 0f);
		yield return new WaitForSeconds(2f);
		StartCoroutine(RunDialogueSection("I guess we can leave too then.", "Creo que nosotros también podemos irnos.", b));
		yield return new WaitForSeconds(0.5f);
		player.inAnim = false;
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		i.SetEndDay(true);
		EnvironmentController.Instance.ChangeEnvironment(Room.SchoolYardEndDay);
	}

	private void GoToOfficeNoItems()
	{
		UnityEngine.Object.FindObjectOfType<Principal>().SetNoItems();
		EnvironmentController.Instance.ChangeEnvironment(Room.Office);
	}

	public IEnumerator PlayShowAndTell(Item i)
	{
		yield return null;
		Lily k = UnityEngine.Object.FindObjectOfType<Lily>();
		Monty l = UnityEngine.Object.FindObjectOfType<Monty>();
		Buggs b = UnityEngine.Object.FindObjectOfType<Buggs>();
		Nugget m = UnityEngine.Object.FindObjectOfType<Nugget>();
		Jerome j = UnityEngine.Object.FindObjectOfType<Jerome>();
		Cindy c = UnityEngine.Object.FindObjectOfType<Cindy>();
		PlayerPrefs.SetInt("Showed" + i.ToString() + EnvironmentController.Instance.GetFileModifier(), 1);
		switch (i)
		{
		case Item.BillyNote:
			StartCoroutine(RunDialogueSection("What is that?~ Some kind of note?", "¿Qué es esto?~ Parece a una nota.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Your handwriting is truly awful. If I were grading that I would give you an F.", "Su letra es realmente horrible. Si estuviera calificándolo le daría una F.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!l.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("Eh...~\\hI could probably figure it out./h", "Eh...~\\hTal vez podría descifrarlo./h", l));
				while (pEmptyOptions)
				{
					yield return null;
				}
				UnlockHint("Monty", 3);
				if (!b.removedFromGame)
				{
					StartCoroutine(RunDialogueSection("Shut up nerd. No one asked you.", "Cállate nerdo. Nadie te preguntó.", b));
					while (pEmptyOptions)
					{
						yield return null;
					}
				}
				else
				{
					StartCoroutine(RunDialogueSection("Good for you Monty.", "Bien por ti Monty.", this));
					while (pEmptyOptions)
					{
						yield return null;
					}
				}
			}
			StartCoroutine(RunDialogueSection("Anyway...~who's next? It won't be hard to beat that.", "Como sea...~ ¿quién sigue? Eso no será difícil de superar.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.BillyCode:
			StartCoroutine(RunDialogueSection("What is that?~ Some kind of note?", "¿Qué es esto?~ Parece a una nota.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Your handwriting is truly awful. If I were grading that I would give you an F.", "Su letra es verdaderamente horrible. Si lo estuviera calificando  le daría una F.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Anyway...~who's next? It won't be hard to beat that.", "Como sea...~ ¿quién sigue? Eso no será difícil de superar.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.BiscuitBall:
			StartCoroutine(RunDialogueSection("What is that?~ Oh it's one of the Janitor's...~mystery meat balls.", "¿Qué es esto? ~Oh, esto es una de las...~ misteriosas galletas de carne del conserje.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I wouldn't eat that if I were you.", "Si fuera tú yo no lo comería.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!c.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("Is it vegan? It looks vegan.", "¿Es vegetariano? Pareciera.", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("*Sigh* No, Cindy. It's a meatball.", "*Suspira* No, Cindy. Es una galleta de carne.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				UnlockHint("Cindy", 4);
			}
			StartCoroutine(RunDialogueSection("Okay. Who's next? Someone with something a little less...~gross.", "Okay. ¿Quién sigue? Alguien con algo menos…~asqueroso.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Breathalyzer:
			StartCoroutine(RunDialogueSection("Oh my! Why on earth would you have something like this?", "¡Oh Dios! ¿Por qué demonios tendrías algo así?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Your parents need to be more responsible! You shouldn't have to use one of these until you're at least 16!", "¡Tus padres necesitan ser más responsables! ¡Tú no deberías usar estas cosas sino hasta que tengas por lo menos 16 años!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Who's next? Hopefully someone who doesn't need their BAC checked on the regular.", "¿Quién sigue? Esperemos que alguien que no necesita su BAC comprobado en el regular.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.BucketBlood:
			StartCoroutine(RunDialogueSection("That is disgusting!!~ Why would you bring that into my classroom?! Put it away!", "¡¡Qué asco!!~ ¿Por qué traerías eso a mi salón? ¡Sácalo de aquí!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I have half a mind to send you to the principal for that, but I have this sinking feeling you got that from the janitor's closet.", "Tengo casi decidido mandarte donde el director por ésto, pero tengo el presentimiento de que lo has sacado del armario del conserje.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I don't feel like filling out paperwork for this right now so I'm gonna let it slide.", "No tengo ganas de llenar papeles ahora, así que voy a dejarlo pasar.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!c.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("Hehehe...~that would look really good on Lily.", "Jejeje...~Eso se vería muy bien en Lily.", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				UnlockHint("Cindy", 5);
				if (!k.removedFromGame)
				{
					StartCoroutine(RunDialogueSection("It would not!~ Stop being mean Cindy!", "¡No!~ ¡Deja de portarte como loca, Cindy!", k));
					while (pEmptyOptions)
					{
						yield return null;
					}
				}
			}
			StartCoroutine(RunDialogueSection("Someone else...~anyone else, please show what you brought today.", "Alguien más...~alguien más, por favor muestren lo que han traído hoy.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Bug:
			StartCoroutine(RunDialogueSection("Oh! That looks like one of the principal's little devices. Why would you have one?", "¡Oh! Se parece a uno de los aparaticos del director. ¿Por qué lo tendrías?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			player.ExplodePlayer("Don't show the principal's device.", "No muestres eso.");
			UI.CollapseDialogue();
			yield break;
		case Item.ChocolateBar:
			StartCoroutine(RunDialogueSection("Oh a chocolate bar! Those are hard to find in a public school with the new government policies targeted at Buggs!", "¡Oh, una barra de chocolate! ¡Esos son difíciles de encontrar en una escuela pública con las nuevas políticas gubernamentales dirigidas a Buggs!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!b.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("You're one to talk!", "¡Tu turno para hablar!", b));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			StartCoroutine(RunDialogueSection("Hey kid! You gotta hook me up with that tomorrow. I pay big for chocolate.", "¡Hey, niño! Ayúdame mañana a ponerme en contacto con la persona que los vende. Yo pago mucho por chocolates.", l));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Yes yes, we all want chocolate. Okay, who's next?", "Sí, sí, todos queremos chocolate. ¿Ok, quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Cigarettes:
			player.UseItem(Item.Cigarettes);
			StartCoroutine(RunDialogueSection("Drugs?! In school?! How horrible! Give those to me right now! Now I don't have to pick some up after school.", "¡¿Narcóticos?! ¡¿En la escuela?! ¡Es horrible! ¡Dámelos! Así ya no tengo que recoger las mías después de la escuela.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Oh...~I'm also sending you to the principal's office. We have a some tolerance policy on violence, but a zero tolerance policy on drugs.", "Oh...~ A ti también te voy a mandar a la oficina del director. Tenemos una política de tolerancia para actos violentos, pero una política de cero tolerancia hacia el consumo de drogas.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnityEngine.Object.FindObjectOfType<Principal>().SetCigs();
			EnvironmentController.Instance.ChangeEnvironment(Room.Office);
			UI.CollapseDialogue();
			yield break;
		case Item.CindyDog:
			StartCoroutine(RunDialogueSection("Oh dear God! What is that?!", "¡Oh, por Dios Santo! ¡¿Qué es esto?!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Is that Cindy's dog!?", "¡¿Es el perro de Cindy!?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!c.removedFromGame)
			{
				c.ApplyTextureBlock(c.angry);
				c.SetDirection(true);
				StartCoroutine(RunDialogueSection("\\bWHAT?!?!/b", "\\b¡¿QUÉ?!/b", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				c.WalkToPoint(new Vector3(-15.7f, -7.2f, -4.64f), 0.5f);
				StartCoroutine(RunDialogueSection("You had my dog this whole time?! You killed her!", "¿Durante todo este tipo tu has tenido a mi perro?! ¡Lo mataste!", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				c.StabPlayerDog();
				yield break;
			}
			m.SetDirection(true);
			StartCoroutine(RunDialogueSection("Nugget told you this would bring trouble!", "¡Nugget te dijo que ésto podría causarte problemas!", m));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("You knew about this Nugget?", "¿Tu sabías de ésto Nugget?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Well...~Nugget knew...~but it was not Nugget who killed the poor puppy.", "Pues...~Nugget sabía...~pero no fue Nugget quien mató al pobre cachorrito.", m));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I don't believe that for a second! Go to the principal's office!", "¡No te creo ni por un segundo! ¡Ve a la oficina del director!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			m.WalkToPoint(new Vector3(-17.5f, -0.7f, -10.14f), 2f);
			StartCoroutine(RunDialogueSection("Nugget was wrong to trust his new friend.~ Nugget will have vengeance.", "Nugget cometió un error al confiar en su nuevo nuevo amigo.~ Nugget se vengará.", this));
			yield return new WaitForSeconds(2f);
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (UI.IsMissionActive(Mission.TeacherShowEvidence))
			{
				StartCoroutine(RunDialogueSection("Well done!~ That's another gold star for you!", "¡Bien hecho!~ ¡Otra estrella de oro para ti!", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				player.GetItem(Item.GoldStar);
			}
			else
			{
				StartCoroutine(RunDialogueSection("That was cryptic. Anyway...~who's next?", "Eso fue misterioso. Como sea...~ ¿quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			break;
		case Item.CindyShoe:
			StartCoroutine(RunDialogueSection("Oh this is Cindy's shoe, isn't it? Yes...~what happened to her was so...~teehee...~tragic.", "¡Oh, este es el zapato de Cindy, verdad? Sí...~lo que sucedió con ella fue tan...~jijijiji..~trágico.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Thank you so much for reminding me of the wonderfu--~tragic...~tragic events of this afternoon.", "Muchas gracias por recordarme la maravillosa--~trage...~ tragedia de esta tarde.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Donut:
			StartCoroutine(RunDialogueSection("Oh a donut! I do so love sweets.~ They make my job so much easier. I might even let you off the hook sometimes if you give me one.", "¡Oh, una rosquilla! A mi me gustan mucho los dulces. ~ Ellos hacen mi trabajo mucho más fácil. Incluso podría bajarte del gancho a veces si me das uno.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnlockHint("Nugget", 5);
			break;
		case Item.Flask:
			StartCoroutine(RunDialogueSection("A flask? I think I'm required by law to ask you if it's filled with anything...~not suited for your age.", "¿Un frasco? Creo que la ley me exige que te pregunte si está lleno de algo... ~ no adecuado para tu edad.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("You know what? I'm not gonna worry about it. If anyone asks, it's just water.", "¿Sabes qué? No voy a preocuparme por ésto. Si alguien pregunta, solo es agua.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Who's next? Someone with something a little less incriminating.", "¿Quién sigue? Alguien con algo un poco menos incriminatorio.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Flower:
			StartCoroutine(RunDialogueSection("A flower? That's pretty boring. What's there to even tell about that?", "¿Una flor? Eso es muy aburrido. ¿Acaso puedes decir algo al respecto?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (UI.IsMissionComplete(Mission.CindyDumpBucket) || UI.IsMissionComplete(Mission.CindyTellPaper))
			{
				c.SetShowFlower(true);
				c.SetDirection(true);
				StartCoroutine(RunDialogueSection("It is not boring! I gave him that cause he's been a good boyfriend all day!", "¡No es aburrido! ¡Se la dí porque él se portó como un buen novio todo el día!", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				UnlockHint("Cindy", 6);
				if (!b.removedFromGame)
				{
					StartCoroutine(RunDialogueSection("No. It's definitely wimpy. I'm probably gonna kick his butt for being so whipped.", "No. Eso es definitivamente cobarde. Tal vez le de una paliza por ser tan cobarde.", b));
					while (pEmptyOptions)
					{
						yield return null;
					}
					c.SetDirection(false);
					StartCoroutine(RunDialogueSection("Don't you dare Buggs! No one is allowed to kick my boyfriend's butt except me!", "¡No te atrevas, Buggs! ¡Nadie aparte de mi le puede pegar a mi novio!", c));
					while (pEmptyOptions)
					{
						yield return null;
					}
					b.mySkeleton.AnimationName = "walk";
					player.inAnim = true;
					b.transform.DOMove(new Vector3(-16f, -7f, 0f), 2f).SetEase(Ease.Linear).OnUpdate(base.SetDepth)
						.OnComplete(delegate
						{
							player.inAnim = false;
							b.mySkeleton.AnimationName = "idle";
							c.SetDirection(true);
							b.StartPunching();
						});
					StartCoroutine(RunDialogueSection("We'll see about that.", "Ya verémos.", b));
					while (pEmptyOptions)
					{
						yield return null;
					}
					StartCoroutine(RunDialogueSection("Oh my God! Ms. Applegate! Stop him!", "¡Oh por Dios! ¡Señora. Applegate! ¡Deténgalo!", c));
					while (pEmptyOptions)
					{
						yield return null;
					}
					StartCoroutine(RunDialogueSection("I would...~but damn if I don't love a good kiddie fight!", "Lo haría...~¡Maldita sea!, si no me gustaran tanto las peleas entre niñitos!", this));
					while (pEmptyOptions)
					{
						yield return null;
					}
					StartCoroutine(b.PunchPlayerToDeath("Don't show the flower if you're gonna get beat up for it.", "No muestres la flor si sabes que te pego por ella."));
					UI.CollapseDialogue();
					yield break;
				}
				StartCoroutine(RunDialogueSection("Trust me. It's boring. We're just gonna skip you and move onto someone more interesting.", "Créeme. Es aburrido. Vamos tan solo a ignorarte y pasar a alguien más interesante.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Okay who's next? It's not gonna be hard to beat that so don't be shy.", "¡Ok, quién sigue? Esto no va a ser difícil de vencer, así que no sean tímidos.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				break;
			}
			if (!c.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("I have a flower just like that one reserved for my boyfriends.", "Tengo una flor igual a esa reservada para mis novios.", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Good for you Cindy.", "Bien por ti, Cindy.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("Try to show something a little less...~girly next time.", "La próxima vez trata de mostrar algo menos...~femenino.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			StartCoroutine(RunDialogueSection("Okay who's next? It's not gonna be hard to beat that so don't be shy.", "¡Ok, quién sigue? Esto no va a ser difícil de vencer, así que no sean tímidos.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.GoldStar:
			StartCoroutine(RunDialogueSection("Oh you just love being my little rat, don't you? Showing off your gold star to the rest of these imbeciles.", "Oh, te encanta ser mi pequeño traidor, ¿no? Presumiendo tu estrella de oro a los demás  imbéciles.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("That's very good. Thank you for sharing!~ It's gonna be hard to beat that!", "Está  muy bien. ¡Gracias por compartir! ~ ¡Éste será dificil de superar!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Gum:
			StartCoroutine(RunDialogueSection("Ewww what is that?~ Is that chewed gum? That's not a very good thing to show and I'm sure there's not much to tell about it.", "¿Ewww qué es eso?~ ¿Es un chicle masticado? No es una cosa muy buena para mostrar y estoy seguro de que no hay mucho de que hablar al respecto.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!c.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("He was supposed to put it in Lily's hair!~ Way to drop the ball on that one.", "¡Se suponía que él debía ponerlo en el cabello de Lily!", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				if (!k.removedFromGame)
				{
					StartCoroutine(RunDialogueSection("Well thanks for not doing it. I appreciate that.", "Pues, gracias por no hacerlo. Te lo agradezco mucho.", k));
					while (pEmptyOptions)
					{
						yield return null;
					}
					UnlockHint("Nugget", 2);
				}
			}
			StartCoroutine(RunDialogueSection("We'll just move on. Try not to be so weird next time. Okay...~who's next?", "Ok, seguimos. Trata de no ser tan raro la próxima vez. Okay...~¿quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.HallPass:
			StartCoroutine(RunDialogueSection("Hey that's one of the principal's hall passes! He said one was missing this morning.", "¡Esto es uno de los pases del director! Esta mañana él dijo que uno se le había perdido.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I can't believe you stole it. Go to the principal and tell him what you did!", "No puedo creer que tu lo robaste. ¡Ve donde el director y dile que fuiste tú!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnityEngine.Object.FindObjectOfType<Principal>().SetPass();
			EnvironmentController.Instance.ChangeEnvironment(Room.Office);
			UI.CollapseDialogue();
			yield break;
		case Item.KeyMold:
			StartCoroutine(RunDialogueSection("A key mold? What's the key for?", "¿Un molde de llave? ¿Para qué es esta llave?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!j.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("It's not for anything. Don't worry about it.", "No es para nada. No te preocupes.", j));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Yeah, I wasn't that interested. I was just being polite. Sheesh.", "Sip, yo no estaba tan interesada. Solo trataba ser amable. Uiiishh.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("You know what? Keep it a secret. I'm sure it's a key to something great.", "¿Sabes qué? Mantenlo en secreto. Estoy segura que es una llave para algo genial.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			StartCoroutine(RunDialogueSection("Okay. Who's next?", "Ok. ¿Quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Knife:
			StartCoroutine(RunDialogueSection("What is that?~ OH MY! NO NO NO!!! You aren't allowed to have knives in school! That's very very dangerous! Go to the principal's office right now!", "¿Qué es esto?~ ¡OH DIOS! ¡¡¡NO NO NO!!! ¡Tú no estás autorizado para tener cuchillos en el colegio! ¡Es muy muy peligroso! ¡Ve a la oficina del director ahora mismo!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnityEngine.Object.FindObjectOfType<Principal>().SetKnife();
			EnvironmentController.Instance.ChangeEnvironment(Room.Office);
			UI.CollapseDialogue();
			yield break;
		case Item.LaserPointer:
			StartCoroutine(RunDialogueSection("Ah yes...~Jerome's silly laser pointer. Such a shame what happened to him.", "Ah sí...~Es el estúpido puntero laser de Jerome. Qué lástima lo que le pasó a él.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("What?! You had that all along?~ Man that is cold.", "¡¿Qué?! ¿Lo has tenido todo este tiempo?~ Amigo eso es astuto. ", l));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Get over it Monty. Who's next?", "Supéralo, Monty. ¿Quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.JanitorKey:
			StartCoroutine(RunDialogueSection("An old grey key?~ It looks similar to the one the janitor uses for his closet.", "¿Una vieja llave gris?~ Se parece a la que el conserje usa para su armario.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("He only keeps it locked during lunch anyway. Don't know what that's all about.", "Él solo mantiene su armario cerrado durante el almuerzo. No sé de que se trata todo esto.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Anyway, who's next?", "Como sea ¿quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.LoveLetter:
			StartCoroutine(RunDialogueSection("Ooooooh!~ Is that a love letter?", "¡Ooooooh!~ ¿Es una carta de amor?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!m.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("WHAT?!~ Nugget told you to give that to the pretty Lily!", "¡¿QUÉ?!~ ¡Nugget te pidió entregársela a la hermosa Lily!", m));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Nugget knows where Billy is!!!", "¡¡¡Nugget sabe donde está Billy!!!", m));
				while (pEmptyOptions)
				{
					yield return null;
				}
				player.ExplodePlayer("Don't show the love letter.", "No muestres eso.");
				yield break;
			}
			StartCoroutine(RunDialogueSection("I'm not gonna bother reading it. Young love is so fleeting as it is.", "No me voy a molestar leyéndola. El amor joven es tan fugaz.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Anyway, who's next?", "Como sea ¿quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.LunchPass:
			StartCoroutine(RunDialogueSection("Oh yes! One of my lunch passes! Getting to eat special lunch with teacher alone is a very special treat! You should try it tomorrow!", "¡Oh sí! ¡Uno de mis pases para el almuerzo! ¡Tener un almuerzo especial solo con la maestra es un regalo muy especial! ¡Debes probarlo mañana!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnlockHint("Buggs", 5);
			StartCoroutine(RunDialogueSection("What a fun thing to show at show and tell. Thank you. Who's next?", "Qué cosa tan divertida para mostrar en \u00b4Mostrar y Contar\u00b4. Gracias. ¿Quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.MagnifyingGlass:
			StartCoroutine(RunDialogueSection("A magnifying glass? I think the lunch lady could use that. She lost her glasses, but this could be a good substitute.", "¿Una lupa? Creo que la cocinera podría usarla. Ella perdió sus gafas, pero ésto podría sustituirlos bastante bien.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnlockHint("General", 4);
			if (!m.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("No! Nugget could use that glass of magnifying during morning time tomorrow. Bring it to Nugget and you will be rewarded!", "¡No! Nugget podría usar la lupa durante las clases de la mañana. ¡Si se lo entregas a Nugget serás recompensado!", m));
				while (pEmptyOptions)
				{
					yield return null;
				}
				UnlockHint("Monstermon", 4);
				StartCoroutine(RunDialogueSection("Shut it weirdo.", "Cállate, bicho raro.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			StartCoroutine(RunDialogueSection("Okay!~ Who's next?", "¡Ok!~ ¿Quién es el siguiente?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.McGlobs:
			StartCoroutine(RunDialogueSection("A silly meal? Normally you would just eat that at lunch, not bring it to show and tell.", "¿Una estúpida comida? Normalmente tu deberías comertela no traerla a 'Mostrar y Contar.\u00b4", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("It looks like you left the Monstermon card that goes with it inside the box. Wouldn't want to leave that behind now would we?", "Parece que has dejado la tarjeta Monstermon que va dentro de la caja. No quisiéramos olvidarlo ahora, ¿verdad?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			player.GetItem(Item.MagicalAirship);
			UnlockHint("Monstermon", 24);
			StartCoroutine(RunDialogueSection("Okay! Who's next?", "¡Ok!~ ¿Quién es el siguiente?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.MontyGlasses:
			StartCoroutine(RunDialogueSection("Awww Monty's glasses. I heard from the nurse that Monty is only partially paralyzed from the...~incident. So that's some good news.", "Awww las gafas de Monty. Yo escuché a la enfermera diciendo que Monty sólo está parcialmente paralizado desde el ... ~ incidente. Así que son buenas noticias.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Thank you for reminding us all to think of Monty...~and for reminding me that I need a new pill dealer.", "Gracias por hacernos pensar en Monty ... ~ y por recordarme que necesito un nuevo distribuidor de pastillas.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I suppose you could give these to the lunch lady. She was complaining earlier about how she lost her glasses.", "Supongo que podrías darle ésto a la cocinera. Hace un rato ella se  estaba quejando  porque perdió sus lentes.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnlockHint("General", 4);
			StartCoroutine(RunDialogueSection("Okay! Who's next?", "¡Ok!~ ¿Quién es el siguiente?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.MonstermonCardComplete:
			StartCoroutine(RunDialogueSection("Oh my! That's a lot of Monstermon cards! It looks like you have all 25!", "¡Por Dios! ¡Cuántas tarjetas  Monstermon! ¡Parece que tienes todas las 25!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!m.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("You have 25 talismans?!", "¿Tienes 25 talismanes?!", m));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("They're just cards Nugget. Don't be such a nutcase.", "Son solo tarjetas Nugget. No seas tan demente.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("But-~ but-~ \\hthe sign in the true cave!/h", "Pero-~ pero-~ \\h¡la señal en la verdadera cueva!/h", m));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Oh, knock it off! Anyway, who's next?", "¡Oh, ya basta! Como sea, ¿quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("That's very impressive. \\hI could have sworn I heard Nugget mumbling about 25 somethings earlier today./h", "Es muy impresionante. \\h Podría haber jurado que oí a Nugget hace un rato, murmurando sobre 25 cositas./h", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("So who's next?", "¿Así que, quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			break;
		case Item.Mop:
			StartCoroutine(RunDialogueSection("Ah yes.~ The janitor's mop. A true icon of our school.", "Ah, sí. ~ El trapeador del conserje. Un verdadero icono de nuestra escuela.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("It's cleaned so many messes. Killed so many children. I heard it's been passed down for generations.", "Este trapeador ha limpiado tantos desastres. Ha matado a tantos niños. He escuchado que ha pasado por generaciones.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Some say it once belonged to Napoleon. How fascinating. That's a very good show and tell. Thank you.", "Algunos dicen que perteneció a Napoleón. ¡Es fascinante! Éste es un buen ejemplo para \u00b4Mostrar y Contar\u00b4. Gracias.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("So who's next?", "¿Así que, quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Nugget:
			StartCoroutine(RunDialogueSection("Is that a chicken nugget? Why are you showing that?", "¿Es un nugget de pollo? ¿Por qué estás mostrando ésto?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!m.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("It is one of Nugget's nuggets! It means this is one of Nugget's friends.", "¡Es un nugget de Nugget! Esto significa que es uno de los amigos de Nugget.", m));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Right...~I hope you know what you're getting into. Being Nugget's friend can be...~challenging.", "Correcto...~Espero que sepas en lo que te estás metiendo. Ser amigo de Nugget puede ser...~ complicado.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("I'm sure if Nugget was here he would have some weird comment to make about it.", "Estoy seguro que si Nugget estuviera aquí, haría comentarios extraños sobre ésto.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			StartCoroutine(RunDialogueSection("Anyway, who's next? Someone with something more interesting than a nugget please.", "Como sea, ¿Quién sigue? Alguien con un objeto más interesante que un nugget, por favor.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Phone:
			StartCoroutine(RunDialogueSection("Is that my phone?!~ How the heck did you get that, and how dare you steal my things!", "¡¿Es mi teléfono?!~ ¡Cómo carambas lo conseguiste, y cómo te atreves a robar mis cosas!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("You're in kindergarten to learn how to share not steal! Go to the principal's office right now!", "¡Estás en el kindergarten para aprender a compartir, no a robar! ¡Te vas ya para la oficina del director!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnityEngine.Object.FindObjectOfType<Principal>().SetPhone();
			EnvironmentController.Instance.ChangeEnvironment(Room.Office);
			UI.CollapseDialogue();
			yield break;
		case Item.Pill:
			StartCoroutine(RunDialogueSection("Oh you know what teacher likes, don't you! I think I'll SHOW this to my mouth and TELL you all to not tell anyone about this.", "¿Oh, tú sabes lo que le gusta a la profesora, verdad? Creo que le voy a MOSTRAR esto a mi boca y les voy a CONTAR a ustedes para que guarden el secreto.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			player.UseItem(Item.Pill);
			if (!c.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("I can think of a couple of uses for it too. It'll make playing house more fun!", "Yo también puedo imaginar un par de usos que le puedes dar a ésto. ¡Ésto puede hacer el juego de la casita más divertido.", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				UnlockHint("Monstermon", 6);
				StartCoroutine(RunDialogueSection("No one cares Cindy! Who's next?", "¡A nadie importa, Cindy! ¿Quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("My favorite thing for show and tell by far! It's gonna be hard to beat that!", "¡Hasta el momento, es mi objeto favorito en 'Mostrar y Contar'! ¡Será difícil de superar!", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Okay, who's next?", "Como sea ¿quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			break;
		case Item.Pills:
			StartCoroutine(RunDialogueSection("Woah there! A whole bottle?! You must have really been up in the principal's business today, huh?", "¡Wow!  ¡¿Una botella completa?! Debes haber estado realmente en el negocio del director hoy, ¿eh? ", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!l.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("Hey, kid! You ever want someone to take those off your hands, I got a pretty penny for you.", "¡Hey, niño! Si alguna vez te quieres deshacer de esos, Tengo una linda monedita para ti.", l));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Hands off Monty! I called it first!", "¡Quítale las manos de encima, Monty! ¡Yo lo vi primero!", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Aww man!", "¡Aww, hombre!", l));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Now that that's settled, who wants to go next?", "Ahora que todo se ha resuelto, ¿quién quiere ser el siguiente?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("Feel free to leave those in your cubby...~you know...~for safe keeping.", "No te preocupes, puedes dejarlos en el casillero...~ya sabes...~ para mantenerlos seguros.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			StartCoroutine(RunDialogueSection("So who's next?", "Como sea ¿quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.PoisonNugget:
			StartCoroutine(RunDialogueSection("Is that a chicken nugget? Why are you showing that?", "¿Es un nugget de pollo? ¿Por qué está mostrando ésto?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!m.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("It is one of Nugget's nuggets! It is very...~special.", "¡Es un nugget de Nugget! Es muy...~especial.", m));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("What's so special about it?", "¿Y por qué es tan especial?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Perhaps teacher could eat it and find out!", "¡Tal vez la profesora podría comerlo y descubrirlo por sí misma!", m));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("No thank you, Nugget. Anyway, who's next?", "No, gracias, Nugget. ¿Quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("I'm sure if Nugget was here he would have some weird comment to make about it.", "Estoy seguro que si Nugget estuviera aquí, haría comentarios extraños sobre ésto.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Anyway, who's next? Someone with something more interesting than a nugget please.", "¿Quién sigue? Alguien con algo más interesante que un nugget, por favor.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			break;
		case Item.PrincipalKey:
			StartCoroutine(RunDialogueSection("Well that's a nice fancy gold key!~ It looks just like the one the principal has for his office.", "¡Pues, es una bonita y extravagante llave de oro!~ Se parece a la de la oficina del director.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("But that couldn't be the case. There's only one key to his office.", "Pero eso no puede ser posible. Sólo hay una llave de su oficina.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("That's a very nice thing to show for show and tell. Thank you.~ Who's next?", "Es una cosa muy agradable de mostrar en \u00b4Mostrar y Contar\u00b4. Gracias.~ ¿Quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Recipe:
			StartCoroutine(RunDialogueSection("It's a little note.~ Let's see what it says.", "Es una pequeña nota. ~ A ver que dice.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Oh. It appears to be a recipe of some sort...", "Oh. Parece que es una receta de...", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Oh dear God!!! The janitor is eating Cindy's dog!", "¡¡¡Oh,, Dios Santo!!! ¡El conserje se está comiendo el perro de Cindy!", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!c.removedFromGame)
			{
				c.SetDirection(true);
				c.ApplyTextureBlock(c.angry);
				StartCoroutine(RunDialogueSection("\\bWhat?!/b", "\\b¡¿Qué?!/b", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				c.SetDirection(false);
				c.WalkToPoint(new Vector3(-18.3f, -0.31f, -0.31f), 1.5f);
				StartCoroutine(RunDialogueSection("That old man is dead meat!", "¡Ese viejo es hombre muerto!", c));
				yield return new WaitForSeconds(1.5f);
				c.RemoveFromGame();
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Cindy stop!~ Oh who am I kidding? Let's just move on to the next kid for show and tell.", "¡Cindy detente!~ ¿Oh, a quién engaño? Vamos a pasar al siguiente chico para \u00b4Mostrar y Contar\u00b4.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("But now that I think about it, it's probably better for you kids than that hideous slop they normally serve.", "Pero ahora que lo pienso, probablemente eso es mejor para ustedes, que esa horrible porquería que normalmente sirven.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Anyway, who's next?", "Como sea ¿quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			break;
		case Item.Remote:
			StartCoroutine(RunDialogueSection("What is that? Some kind of remote?~ What happens if you press the button?", "¿Qué es esto? ? ¿Algún tipo de control remoto?~ ¿Qué pasa si aprietas el botón?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (UI.IsMissionComplete(Mission.BuggsPlaceDevice))
			{
				SFXManager.Instance.PlaySound("Bang");
				UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
				SetDirection(true);
				StartCoroutine(RunDialogueSection("Woah! What was that?", "¡Wow! ¿Qué fue esto?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("I guess it's nothing.", "Nada, creo", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Anyway...~ who's next?", "Como sea ¿quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				break;
			}
			player.ExplodePlayer("Don't show the remote if you haven't placed the device.", "No muestres eso.", false);
			yield break;
		case Item.Salad:
			UnlockHint("Cindy", 4);
			StartCoroutine(RunDialogueSection("What is that? It's a bunch of leafy greens in a bowl.", "¿Qué es esto? Es un manojo de hojas verdes en un tazón.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!b.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("It's a salad you cow.", "Es una ensalada, vaca.", b));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("OH!~ So that's what a salad looks like. My doctor keeps saying I should--~what did you just call me?", "¡OH!~ Entonces así son las ensaladas. Mi doctor me ha dicho que yo tengo que--~ ¿cómo me acabas de llamar?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Nothing.", "Nada.", b));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Oh good. Cause you're no model yourself.", "Está bien. Porque tú tampoco eres una modelo.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Anyway...~who's next?", "Como sea ¿quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("That's not a very interesting show and tell. We'll just move on to someone else.", "Este \u00b4Mostrar y contar\u00b4 no ha sido muy interesante. Vamos con la siguiente persona.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			break;
		case Item.Screwdriver:
			StartCoroutine(RunDialogueSection("Oh a screwdriver!~ That's my favorite drink.", "¡Oh, un destornillador!~ Es mi bebida favorita.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("The janitor was mentioning how he wishes he hadn't sold his to Monty. \\hApparently there's a loose shelf in his closet or something./h", "El conserje mencionó cómo deseaba no haber vendido el suyo a Monty. \\h Aparentemente hay un estante vacío en su armario o algo así./h", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnlockHint("Jerome", 3);
			StartCoroutine(RunDialogueSection("Okay. Who's next?", "Como sea ¿quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.SeveredFinger:
			StartCoroutine(RunDialogueSection("Oh is that a fake severed finger?", "¿Oh, es un dedo falso amputado?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I'm going to assume it's fake so I can go home some time today.", "Voy a asumir que este dedo es falso, así que por fin puedo ir a casa hoy.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Very good! Who's next?", "¡Muy bien! ¿Quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Shoe:
			StartCoroutine(RunDialogueSection("That's a shoe.~ Why are you showing us a shoe?", "Es un zapato. ~ ¿Por qué nos muestras un zapato?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!k.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("Hey that's my brother's shoe!~ Where did you find it?!", "¡Es el zapato de mi hermano!~ ¡¿Dónde lo encontraste?!", k));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Oh no no no Lily. We're not getting into this again.", "Oh, no, no, no, Lily. No vamos a empezar con esto de nuevo.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("But you don't understand! The principal! He--", "¡Pero ustedes no entienden! ¡El director! Él—", k));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("You know...~if you're so obsessed with the principal maybe you should go see him!", "Sabes...~ ¡si estás tan obsesionada con el director, quizás deberías ir a verlo!", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("But-- but I-", "Pero-- pero yo-", k));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Bye Lily!", "¡Adiós, Lily!", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Darn it!~ I'm getting closer though.", "¡Maldita sea!  Sin embargo, me estoy acercando.", k));
				k.WalkToPoint(new Vector3(-20.4f, -0.5f, -0.5f), 3f);
				yield return new WaitForSeconds(3f);
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Now that that nonsense has been taken care of, who wants to go next?", "Ahora que esa tontería se ha resuelto, ¿quién quiere ser el siguiente?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("You know what? I don't really care. We're just gonna skip you. Who's next?", "¿Sabes qué? A mí realmente ni me importa. Tan solo te vamos a omitir. ¿Quién sigue?", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			break;
		case Item.Slop:
			StartCoroutine(RunDialogueSection("I can't actually believe we make you kids eat that stuff.", "De hecho, no puedo creer que le hagamos comer a los niños esas cosas.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("I'm pretty sure you wouldn't be able to pass a drug test after eating that.", "Estoy completamente segura de que tu no pasarías una prueba de drogas, después de comer eso.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Anyway, who's next?", "Como sea ¿quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.VoiceRecorder:
			StartCoroutine(RunDialogueSection("A voice recorder? \\hThis little thing is Cindy's worst nightmare./h", "¿Un grabador de voz? \\hEsta cosita es la peor pesadilla de Cindy./h", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UnlockHint("Teacher", 2);
			if (!c.removedFromGame)
			{
				c.SetDirection(true);
				StartCoroutine(RunDialogueSection("Hey!~ What did I do?", "¡Hey! ~ ¿Qué hice?", c));
				while (pEmptyOptions)
				{
					yield return null;
				}
				StartCoroutine(RunDialogueSection("Oh please!~ Like you haven't accused people of things they didn't do before! Get over yourself.", "¡Ay, por favor!~ ¡Como si nunca hubieras acusado a la gente de cosas que no hicieron! Supéralo.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("That girl falsely accuses so many people of things they didn't do, I'm surprised someone hasn't done something about it yet.", "Esa chica ha acusado falsamente a tantas personas de cosas que no han hecho, que me sorprende que nadie haya hecho nada al respecto hasta el momento.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			StartCoroutine(RunDialogueSection("Thank you for showing this. I would hold onto it if you ever talk to her again.", "Graciaspor mostrarnos esto. Yo me aferraría a esto si vuelves a hablar con ella.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Okay. Who is next?", "Como sea ¿quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.Yoyo:
			StartCoroutine(RunDialogueSection("A yo-yo?", "¿Un yo-yo?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			if (!j.removedFromGame)
			{
				StartCoroutine(RunDialogueSection("Dude...~I love yo-yos. \\hWe could totally hang out if you get me one of those!/h", "Amigo...~Me gustan yo-yos. \\hPodríamos pasar un rato jugando si me das uno!/h", j));
				while (pEmptyOptions)
				{
					yield return null;
				}
				UnlockHint("Jerome", 2);
				StartCoroutine(RunDialogueSection("That's great Jerome. We all know you wouldn't bother being anyone's friend unless they are cool enough for you anyway.", "Eso es genial Jerome. Todos sabemos que no te molestarías en ser amigo de nadie a menos de que no sean lo suficientemente \u00b4cool\u00b4 para ti.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			else
			{
				StartCoroutine(RunDialogueSection("Can you do any cool tricks with it?~ Actually don't worry about it, I'm sure no one wants to see that anyway.", "¿Sabes hacer trucos con él?~ De hecho no te preocupes, estoy segura de que nadie quiere verlos de todos modos.", this));
				while (pEmptyOptions)
				{
					yield return null;
				}
			}
			StartCoroutine(RunDialogueSection("So who's next?", "Como sea ¿quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		default:
			StartCoroutine(RunDialogueSection("A Monstermon card?~ Oh come on. You have to be more creative than that.", "¿Una tarjeta de Monstermon?~ Ay por favor!. Deberías de ser más creativo.", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("We're gonna skip you. Who's next?", "Te vamos a omitir. ¿Quién sigue?", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			break;
		case Item.BathroomPass:
		case Item.BloodyKnife:
		case Item.Bucket:
		case Item.Calculator:
		case Item.CD:
		case Item.Collar:
		case Item.GoldKey:
			break;
		}
		StartCoroutine(EndShowAndTell());
	}

	private void EndShow()
	{
		StartCoroutine(EndShowAndTell());
	}

	private IEnumerator EndShowAndTell()
	{
		Image fader = GameObject.Find("FaderPanel").GetComponent<Image>();
		fader.DOFade(1f, 0.5f);
		yield return new WaitForSeconds(0.75f);
		fader.DOFade(0f, 0.5f);
		StartCoroutine(RunDialogueSection("Well children, that was a fun little show and tell.", "Bien niños, este fue un corto y divertido \u00b4Mostrar y Contar\u00b4.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SFXManager.Instance.PlaySound("Bell");
		player.inAnim = true;
		yield return new WaitForSeconds(0.5f);
		player.inAnim = false;
		StartCoroutine(RunDialogueSection("Oh! There's the bell! I hope you all had a fun day today. I'll see you all tomorrow!", "¡Oh! ¡Aquí está la campana! Espero que todos se hayan divertido el día de hoy. ¡Nos vemos mañana!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		EnvironmentController.Instance.ChangeEnvironment(Room.SchoolYardEndDay);
	}

	private void ShowCigs()
	{
		StartCoroutine(UI.ShowOptions(false));
		CancelInvoke("DoBaseFunction");
		UnityEngine.Object.FindObjectOfType<Monty>().Interact();
	}

	private IEnumerator ShowCigsCoroutine()
	{
		yield return null;
		UnityEngine.Object.FindObjectOfType<Monty>().Interact();
	}

	private void CheckStars()
	{
		StartCoroutine(CheckStarsCoroutine());
	}

	private IEnumerator CheckStarsCoroutine()
	{
		int x = player.GetItemCount(Item.GoldStar);
		if (x < 5)
		{
			StartCoroutine(RunDialogueSection("I would give you a gold star, but it's the end of the day and \\hyou would still be " + (5 - x) + " short of getting my special surprise. Maybe try to be a better rat tomorrow./h", "Te daría una estrella de oro, pero el día ya se acabó y \\hte falta todavía un poco para conseguir mi sorpresa especial. Tal vez trata de ser un mejor soplón mañana./h", this));
		}
		else
		{
			player.GetItem(Item.GoldStar);
			StartCoroutine(RunDialogueSection("Congratulations! You've managed to rat out the rest of your class and collect all six gold stars! This is truly a special occasion! You've earned a very special treat. Here you go!", "¡Felicidades! ¡Has logrado delatar al resto de tu clase y recoger las seis estrellas de oro! ¡Esta es una ocasión realmente especial Te has ganado un regalo muy especial. ¡Aquí tienes!", this));
		}
		yield return new WaitForSeconds(0.5f);
		SetEmptyOptions();
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetCurrentConversation(3);
		Interact();
		player.GetItem(Item.LunchPass);
	}

	private void GetLunchPass()
	{
		player.GetItem(Item.LunchPass);
	}

	private void ActivateDistraction()
	{
		player.UseItem(Item.Remote);
		if (UI.IsMissionComplete(Mission.BuggsPlaceDevice))
		{
			StartCoroutine(DetonateDistraction());
		}
		else
		{
			player.ExplodePlayer("You shouldn't detonate an explosive device in your pocket.", "Usted explotó.");
		}
	}

	private IEnumerator DetonateDistraction()
	{
		yield return new WaitForSeconds(0.5f);
		SFXManager.Instance.PlaySound("Bang");
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.25f);
		player.inAnim = true;
		yield return new WaitForSeconds(0.5f);
		SetDirection(true);
		yield return new WaitForSeconds(0.5f);
		mySkeleton.AnimationName = "walk";
		player.inAnim = false;
		base.transform.DOMoveX(13.5f, 1f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.inAnim = false;
			mySkeleton.AnimationName = "idle";
			SetEmptyOptions();
		});
		StartCoroutine(RunDialogueSection("Oh my my my my!~ What in the dickens was that?! Did it come from in here?", "¡Oh, por Dios!~ ¿Qué demonios fue eso? ¿Vino de aquí?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Transform t = GameObject.Find("CaughtTriggers").transform;
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform != t)
				{
					transform.GetComponent<BoxCollider2D>().enabled = true;
				}
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
		UI.CollapseDialogue();
	}

	private void CatchTryingToLeave()
	{
		SetDirection(false);
		player.ForceChangeDirection(true);
		SetCurrentConversation(7);
		Interact();
	}

	private void CatchTryingToStab()
	{
		FacePlayer();
		SetCurrentConversation(13);
		Interact();
	}

	public void EndCSLunch()
	{
		if (!player.inDialogue)
		{
			SetDirection(false);
			SetCurrentConversation(15);
			Interact();
		}
	}

	public void StartKillTeacher()
	{
		StartCoroutine(KillTeacher());
		GameObject.Find("KillTrigger").SetActive(false);
		player.SetInteractable(null);
		player.UseItem(Item.Knife);
	}

	private void DetonatePlayerLunch()
	{
		PlayerController.instance.ExplodePlayer("Go for the kill on the teacher. Don't hesitate!", "Mátala rápidamente.");
	}

	private IEnumerator KillTeacher()
	{
		EnvironmentController.Instance.DecreaseActions();
		player.inDialogue = true;
		ApplyTextureBlock(blood1);
		GameObject i = GameObject.Find("BloodyKnifeLunch");
		i.GetComponent<SpriteRenderer>().enabled = true;
		i.transform.SetParent(base.transform);
		i.transform.localPosition = new Vector3(i.transform.localPosition.x, i.transform.localPosition.y, 0.25f);
		base.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
		base.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
		player.ApplyTextureBlock(player.bloody);
		SFXManager.Instance.PlaySound("GoreSplat2");
		StartCoroutine(RunDialogueSection("\\bAAAAAAHHHHH!/b", "\\b¡AAAAAAHHHHH!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("You little bastard! I'm gonna...~oh dear...~you--", "¡Pequeño bastardo! Voy a...~Oh por dios...~tú—", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		base.transform.GetChild(0).gameObject.SetActive(false);
		mySkeleton.timeScale = 0f;
		Rigidbody2D r = GetComponent<Rigidbody2D>();
		base.gameObject.layer = 8;
		r.isKinematic = false;
		r.AddForce(Vector2.up * 45f);
		r.AddTorque(-300f);
		CompleteMission(Mission.BuggsKillTeacher);
		yield return new WaitForSeconds(0.1f);
		pCollider.offset = new Vector2(0f, 2.5f);
		GetComponent<BoxCollider2D>().size = new Vector2(3.25f, 5.1f);
		Buggs b = UnityEngine.Object.FindObjectOfType<Buggs>();
		b.transform.position = new Vector3(-18.6f, -0.5f, -9.2f);
		yield return new WaitForSeconds(1.5f);
		ApplyTextureBlock(blood2);
		StartCoroutine(RunDialogueSection("I heard her scream.~ So it's done, right? Wow. I can't believe you actually did it. Take the knife out of her, it's got our fingerprints all over it.", "Escuché tu grito. ~Así que se acabó ¿verdad? Wow. No puedo creer que de hecho lo hiciste. Quítale el cuchillo, tiene nuestras huellas por todas partes.", b));
		b.SetAnimation("walk");
		player.inAnim = true;
		b.transform.DOMove(new Vector3(5f, -3f, -9f), 3f).OnUpdate(b.SetDepth).SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				b.SetAnimation("idle");
				player.inAnim = false;
			});
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		r.isKinematic = true;
		player.inAnim = true;
		player.ForceAnim(true);
		Vector3 v = player.transform.position;
		player.transform.DOMove(new Vector3(14.34f, -6.7f, -6.8f), 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			i.GetComponent<SpriteRenderer>().enabled = false;
			player.GetItem(Item.BloodyKnife);
			player.ForceChangeDirection(false);
		});
		yield return new WaitForSeconds(0.5f);
		player.transform.DOMove(v, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.ForceAnim(false);
			SetDepth();
		});
		b.SetAnimation("walk");
		b.transform.DOMove(new Vector3(14.17f, -6.72f, -2.9f), 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			b.SetAnimation("idle");
		})
			.OnUpdate(b.SetDepth);
		StartCoroutine(RunDialogueSection("She's probably got a phone on her too.~~ I think I'll take that.", "Ella probablemente tiene un teléfono también. ~~ Creo que voy a tomarlo.", b));
		yield return new WaitForSeconds(1.5f);
		EnvironmentController.Instance.RingBell();
		player.ForceChangeDirection(true);
		yield return new WaitForSeconds(0.5f);
		player.inAnim = false;
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("That's the bell. Let's get out of here. Recess is about to start and I don't want anyone to notice we were missing at lunch. Come see me at recess and I'll help you hide that knife.", "La campana. Larguémonos de aquí. El recreo está a punto de comenzar y no quiero que alguien se de cuenta de que no estamos en el almuerzo. Buscame en el recreo y te ayudaré a esconder ese cuchillo.", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UnityEngine.Object.FindObjectOfType<Buggs>().ActivateMission(Mission.BuggsDisposeOfKnife, Room.Recess);
		UI.CollapseDialogue();
		b.conversations[Room.Recess].currentConversation = 1;
		EnvironmentController.Instance.ChangeEnvironment(Room.Recess);
		base.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
		base.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
		player.inAnim = false;
	}

	private void GetMonstermonSillyMeal()
	{
		player.GetItem(Item.FaptainCalcon);
		UnlockHint("Monstermon", 22);
	}

	private void WaitForBell()
	{
		player.inAnim = true;
		Image component = GameObject.Find("FaderPanel").GetComponent<Image>();
		component.DOFade(1f, 0.25f);
		component.DOFade(0f, 0.25f).SetDelay(1f).OnComplete(delegate
		{
			player.inAnim = false;
			EnvironmentController.Instance.RingBell();
			Interact();
		});
	}

	private void GoToRecess()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Recess);
	}

	private void KillPlayerPoisoned()
	{
		StartCoroutine(PoisonPlayer());
	}

	private IEnumerator PoisonPlayer()
	{
		player.inAnim = true;
		Renderer r = player.GetComponent<Renderer>();
		UI.showDeath = true;
		while (r.material.color.r > 0.8f)
		{
			r.material.color -= new Color(0.025f, 0f, 0.025f, 0f);
			yield return new WaitForSeconds(0.1f);
		}
		player.PlayAnimation("fallover");
		yield return new WaitForSeconds(1f);
		player.GetSkeleton().timeScale = 0f;
		UI.ShowDeath("Don't go to lunch with the teacher after befriending Buggs unless you have a plan.", "Necesitas un plan mejor.");
	}

	private void EnterClassroom()
	{
		EnvironmentController.Instance.SetSpawnPoint(1);
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom1);
	}

	public void StoreConversation()
	{
		mStoredIndex = conversations[Room.Classroom1].currentConversation;
	}

	public void ResetConversation()
	{
		conversations[Room.Classroom1].currentConversation = mStoredIndex;
	}
}
