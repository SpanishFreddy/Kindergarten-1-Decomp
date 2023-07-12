using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Jerome : NPCBehavior
{
	private enum DuckColor
	{
		red = 0,
		yellow = 1,
		blue = 2
	}

	public bool mLeftClass;

	private bool mJeromeStuff;

	private int mDucks;

	private bool mWrongOrder;

	private DuckColor[] mSolution = new DuckColor[7]
	{
		DuckColor.yellow,
		DuckColor.yellow,
		DuckColor.blue,
		DuckColor.blue,
		DuckColor.red,
		DuckColor.yellow,
		DuckColor.blue
	};

	private void Bringmold()
	{
		GameObject gameObject = GameObject.Find("KeyMold");
		player.GetItem(Item.KeyMold);
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().color = Color.black;
	}

	public void TakeYoyo()
	{
		player.UseItem(Item.Yoyo);
		DeactivateNotCool();
		if (EnvironmentController.Instance.ActionsLeft() < 2)
		{
			lastSelectedOption.DestinationID = 24;
			ActivateNotEnoughTime();
		}
	}

	public void TakePill()
	{
		player.UseItem(Item.Pill);
	}

	public void TakePills()
	{
		player.UseItem(Item.Pills);
	}

	public void GetHallPass()
	{
		player.GetItem(Item.HallPass);
		UnlockHint("Teacher", 3);
		CompleteMission(Mission.TeacherTryToGetPass);
		if (UI.IsMissionComplete(Mission.TeacherTryToGetPass) || UI.IsMissionActive(Mission.TeacherTryToGetPass))
		{
			Object.FindObjectOfType<Teacher>().ActivateMission(Mission.TeacherGivePass, Room.Classroom1);
		}
	}

	private void ActivateNotCool()
	{
		Object.FindObjectOfType<Teacher>().conversations[Room.Classroom1].Nodes[11].Options[0].IsAvailable = true;
	}

	private void DeactivateNotCool()
	{
		Object.FindObjectOfType<Teacher>().conversations[Room.Classroom1].Nodes[11].Options[0].IsAvailable = false;
	}

	private void ActivateNotEnoughTime()
	{
		Object.FindObjectOfType<Teacher>().conversations[Room.Classroom1].Nodes[11].Options[1].IsAvailable = true;
	}

	private void TakeAllMoney()
	{
		UnlockHint("Monty", 2);
		player.GetMoney(0f - player.money);
	}

	public void JeromeDistract()
	{
		Teacher teacher = Object.FindObjectOfType<Teacher>();
		GetHallPass();
		SetDirection(true);
		pCollider.enabled = false;
		SetAnimation("walk");
		Vector3[] path = new Vector3[2]
		{
			new Vector3(9.67f, -6f, -4.73f),
			new Vector3(26.5f, -7.83f, -7.83f)
		};
		Object.FindObjectOfType<Teacher>().StoreConversation();
		ActivateMission(Mission.JeromeLeaveClassroom, Room.Classroom1);
		teacher.SetJeromeDistract(true);
		base.transform.DOPath(path, 1.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetDirection(false);
			SetDepth();
			SetAnimation("idle");
			pCollider.enabled = true;
			Object.FindObjectOfType<Teacher>().SetDirection(true);
			Object.FindObjectOfType<Teacher>().SetCurrentConversation(27);
			StartCoroutine(EndJeromeDistract());
		})
			.OnUpdate(base.SetDepth);
	}

	private IEnumerator EndJeromeDistract()
	{
		player.lockActions = true;
		EnvironmentController.Instance.waitForNextTalk = true;
		yield return null;
		while (EnvironmentController.Instance.waitForNextTalk && EnvironmentController.currentRoom == Room.Classroom1)
		{
			yield return null;
		}
		player.lockActions = false;
		Teacher t = Object.FindObjectOfType<Teacher>();
		t.SetJeromeDistract(false);
		t.ResetConversation();
		yield return null;
		if (EnvironmentController.currentRoom == Room.Classroom1)
		{
			FailMission(Mission.JeromeLeaveClassroom);
			player.inAnim = true;
			SetDirection(false);
			pCollider.enabled = false;
			SetAnimation("walk");
			Vector3[] path = new Vector3[2]
			{
				new Vector3(9.67f, -6f, -4.73f),
				new Vector3(9.67f, -3.68f, -3.67f)
			};
			t.SetDirection(false);
			SetCurrentConversation(17);
			base.transform.DOPath(path, 2f).SetEase(Ease.Linear).OnComplete(delegate
			{
				SetDepth();
				SetAnimation("idle");
				pCollider.enabled = true;
				SetDirection(base.transform.position.x < player.transform.position.x);
				StartCoroutine(PlayerFindWayToJerome());
				player.inAnim = false;
			})
				.OnUpdate(base.SetDepth);
		}
		player.lockActions = false;
	}

	private IEnumerator PlayerFindWayToJerome()
	{
		StartCoroutine(RunDialogueSection("GET OVER HERE KID!!", "¡FUERA DE AQUÍ NIÑO!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.inAnim = true;
		player.ForceAnim(true);
		Vector3 v = player.transform.position;
		if (v.x < 5f)
		{
			if (v.y > -6f)
			{
				player.ForceChangeDirection(true);
				player.transform.DOMove(new Vector3(6f, -3.5f, -5f), 1f).SetEase(Ease.Linear).OnUpdate(player.SetDepth)
					.OnComplete(delegate
					{
						player.ForceAnim(false);
						player.inAnim = false;
						Interact();
					});
			}
			else
			{
				Vector3[] path = new Vector3[3]
				{
					new Vector3(-5f, v.y, v.z),
					new Vector3(-5f, -3.5f, v.z),
					new Vector3(6f, -3.5f, -5f)
				};
				player.ForceChangeDirection(false);
				player.transform.DOPath(path, 2f).SetEase(Ease.Linear).OnWaypointChange(delegate(int x)
				{
					if (x == 2)
					{
						player.ForceChangeDirection(true);
					}
				})
					.OnUpdate(player.SetDepth)
					.OnComplete(delegate
					{
						player.ForceAnim(false);
						player.inAnim = false;
						Interact();
					});
			}
		}
		else if (v.x > 17f)
		{
			player.ForceChangeDirection(false);
			player.transform.DOMove(new Vector3(14f, -6.5f, -6.55f), 1f).SetEase(Ease.Linear).OnUpdate(player.SetDepth)
				.OnComplete(delegate
				{
					player.ForceAnim(false);
					player.inAnim = false;
					player.ForceChangeDirection(true);
					Interact();
				});
		}
		else if (v.y < -7f)
		{
			player.ForceChangeDirection(true);
			player.transform.DOMove(new Vector3(11f, -6f, -5f), 2f).SetEase(Ease.Linear).OnUpdate(player.SetDepth)
				.OnComplete(delegate
				{
					player.ForceAnim(false);
					player.inAnim = false;
					player.ForceChangeDirection(false);
					Interact();
				});
		}
		else
		{
			player.ForceChangeDirection(false);
			player.transform.DOMove(new Vector3(6f, -3.5f, -5f), 1f).SetEase(Ease.Linear).OnUpdate(player.SetDepth)
				.OnComplete(delegate
				{
					player.ForceAnim(false);
					player.inAnim = false;
					player.ForceChangeDirection(true);
					Interact();
				});
		}
		if (EnvironmentController.Instance.ActionsLeft() == 0)
		{
			while (player.inDialogue || player.inAnim)
			{
				yield return null;
			}
			Object.FindObjectOfType<Teacher>().EndClassroom1();
		}
	}

	public void SetTriedToLeave()
	{
		if (UI.IsMissionActive(Mission.JeromeLeaveClassroom))
		{
			mLeftClass = true;
			Object.FindObjectOfType<Teacher>().SetJeromeDistract(false);
			CompleteMission(Mission.JeromeLeaveClassroom);
			ActivateMission(Mission.JeromeGoToCloset, Room.Classroom1);
		}
	}

	public void CheckTriedToLeave()
	{
		if (mLeftClass)
		{
			mLeftClass = false;
			WalkToPoint(new Vector3(-15.5f, -1.8f, -3.67f), 2f);
			if (EnvironmentController.Instance.ActionsLeft() == 1)
			{
				StartCoroutine(TalkAtLunch());
				return;
			}
			SetCurrentConversation(20);
			Interact();
		}
	}

	private IEnumerator TalkAtLunch()
	{
		SetDirection(false);
		StartCoroutine(RunDialogueSection("Well? Did you get it?~ What--", "¿Entonces? ¿Entendiste?~ Que—", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Teacher t = Object.FindObjectOfType<Teacher>();
		t.WalkToPoint(new Vector3(14.24f, -6.94f, -6.91f), 2f);
		StartCoroutine(RunDialogueSection("Okay kids. We all heard the lunch bell! Let's go down to the cafeteria for a nutritious meal. I hear we're having slop today! How exciting!", "Está bien, niños. ¡Todos escuchamos la campana para el almuerzo! Bajemos a la cafetería a comer. ¡Escuché que tendremos papilla! ¡Qué emoción!", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetCurrentConversation(23);
		Interact();
	}

	private void GoToLunch()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Cafeteria);
	}

	private void GivePassBack()
	{
		player.UseItem(Item.HallPass);
	}

	private void WalkBackToStart()
	{
		player.UseItem(Item.HallPass);
		SetDirection(true);
		WalkToPoint(new Vector3(9.67f, -3.68f, -3.67f), 2f);
		FailMission(Mission.JeromeGetLaserFromBox);
		FailMission(Mission.JeromeGoToCloset);
	}

	public void CheckedJanitorDoor()
	{
		conversations[Room.Classroom1].Nodes[21].Options[1].IsAvailable = true;
	}

	public void InterceptCafeteria()
	{
		if (mJeromeStuff)
		{
			return;
		}
		Janitor janitor = Object.FindObjectOfType<Janitor>();
		if (player.HasItem(Item.HallPass))
		{
			SetDirection(true);
			SetCurrentConversation(2);
			StartCoroutine(WalkToJerome());
			if (janitor.GetUnscrewedShelf())
			{
				janitor.transform.position = new Vector3(0f, 50f, 0f);
			}
		}
		mJeromeStuff = true;
	}

	private IEnumerator WalkToJerome()
	{
		player.inAnim = true;
		yield return new WaitForSeconds(0.5f);
		player.inAnim = false;
		StartCoroutine(RunDialogueSection("Hey kid!~ Over here!", "¡Hey, niño! ~ ¡Ven acá!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.inAnim = true;
		player.ForceAnim(true);
		player.transform.DOMove(new Vector3(15.26f, -3.92f, -3.91f), 1f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(1f);
		player.inAnim = false;
		player.ForceAnim(false);
		Interact();
	}

	public void StartJanitorCafeteria()
	{
		StartCoroutine(StartCafeteria());
	}

	private IEnumerator StartCafeteria()
	{
		Janitor i = Object.FindObjectOfType<Janitor>();
		StartCoroutine(RunDialogueSection("Dude that's awesome! I just hope the janitor doesn't notice. He can be pretty territor--.", "¡Amigo, eso es maravilloso! Espero que el conserje no se de cuenta. Él puede volverse bastante territorial—", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		StartCoroutine(RunDialogueSection("WHICH ONE OF YOU DEAD LITTLE CHILDREN WENT INTO MY CLOSET!?!", "¡¿¡QUIÉN DE USTEDES NIÑITOS MUERTOS, ENTRÓ A MI ARMARIO!?!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		NPCBehavior[] array = Object.FindObjectsOfType<NPCBehavior>();
		foreach (NPCBehavior nPCBehavior in array)
		{
			if (nPCBehavior.transform.name != "Lily")
			{
				nPCBehavior.SetDirection(true);
			}
		}
		StartCoroutine(i.AppearAndSearch());
		SetCurrentConversation(3);
		Interact();
	}

	private void StartJanitorCafeteriaNoLaser()
	{
		StartCoroutine(StartCafeteriaNoLaser());
	}

	private IEnumerator StartCafeteriaNoLaser()
	{
		Janitor i = Object.FindObjectOfType<Janitor>();
		StartCoroutine(RunDialogueSection("Aww man. That's a shame.~ Looks like I'm just gonna have to deal with whatever punishmen--", "Ayyy, hombre. Qué pena.~ Parece que tendré que lidiar con cualquier castigo—", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		StartCoroutine(RunDialogueSection("WHICH ONE OF YOU DEAD LITTLE CHILDREN WENT INTO MY CLOSET!?!", "¡¿¡QUIÉN DE USTEDES NIÑITOS MUERTOS, ENTRÓ A MI ARMARIO!?!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		NPCBehavior[] array = Object.FindObjectsOfType<NPCBehavior>();
		foreach (NPCBehavior nPCBehavior in array)
		{
			if (nPCBehavior.transform.name != "Lily")
			{
				nPCBehavior.SetDirection(true);
			}
		}
		SetCurrentConversation(15);
		Interact();
	}

	private void CallAppearNow()
	{
		StartCoroutine(Object.FindObjectOfType<Janitor>().AppearAndSearch());
		StartCoroutine(WaitTurnOffAction());
	}

	private IEnumerator WaitTurnOffAction()
	{
		yield return null;
		EnvironmentController.Instance.waitForNextAction = false;
	}

	private void ActivateHideMission()
	{
		CompleteMission(Mission.JeromeBringLaser);
		ActivateMission(Mission.JeromeHideLaser);
		GameObject.Find("Blocker1").GetComponent<BoxCollider2D>().enabled = true;
		GameObject.Find("Blocker2").GetComponent<BoxCollider2D>().enabled = true;
	}

	private void HideLaser()
	{
		player.UseItem(Item.LaserPointer);
		CompleteMission(Mission.JeromeHideLaser);
	}

	private void ThrowAwaySlop()
	{
		player.UseItem(Item.Slop);
	}

	private void PayOffLunchLady()
	{
		StartCoroutine(WalkToLunchLady());
	}

	private IEnumerator WalkToLunchLady()
	{
		Vector3 v = base.transform.position;
		SetDirection(false);
		WalkToPoint(new Vector3(-17.3f, -2.8f, -2.79f), 2f);
		Object.FindObjectOfType<LunchLady>().SetDirection(true);
		Object.FindObjectOfType<LunchLady>().SetDistracted(true);
		yield return new WaitForSeconds(3f);
		SetDirection(false);
		WalkToPoint(v, 2f);
		Interact();
		yield return new WaitForSeconds(2f);
		FacePlayer();
	}

	private void ActivateGetLaserFromBathroom()
	{
		ActivateMission(Mission.JeromeGetLaserFromBathroom, Room.Cafeteria);
	}

	private void GetLaserPointerGarbage()
	{
		player.GetItem(Item.LaserPointer);
		CompleteMission(Mission.JeromeGetLaserFromBathroom);
		ActivateMission(Mission.JeromeBringLaserRecess, Room.Recess);
		GameObject.Find("GarbageBag").GetComponent<ObjectInteractable>().GetCurrentOptions()[0].IsAvailable = false;
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			SFXManager.Instance.PlaySound("Bell");
		}
	}

	private void JanitorJeromeKillRecess()
	{
		StartCoroutine(CheckPlayerForLaser());
	}

	private IEnumerator CheckPlayerForLaser()
	{
		Janitor i = Object.FindObjectOfType<Janitor>();
		UnlockHint("Jerome", 6);
		StartCoroutine(RunDialogueSection("Hey! What are you doing talking to him?~ Are you in on this? Empty your pockets!!", "¡Hey! ¿Por qué estás hablando con él?~ ¿Estás metido en ésto?  ¡¡Vacía tus bolcillos!!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		if (player.HasItem(Item.LaserPointer))
		{
			StartCoroutine(RunDialogueSection("The lasery thingy! You are in on this! You know what that means!", "¡La cosita de lasercito! ¡Estás metido en ésto! ¡Sabes lo que eso significa!", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			UI.CollapseDialogue();
			i.KillPlayer("Just do what Monty says next time.", "La próxima vez solo haz lo que dice Monty. ");
		}
		else
		{
			StartCoroutine(RunDialogueSection("Hmmmm...~I guess not. Now screw off so I can berate this brat a little more.", "Hmmmm...~Me imagino que no. Ahora vete a la mierda para que yo pueda seguir regañando a este mocoso.", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			SetCurrentConversation(2);
			Interact();
		}
	}

	private void JeromeSolveDucks()
	{
		ActivateMission(Mission.JeromeSolveRiddle, Room.Recess);
	}

	private void PressBlue()
	{
		SFXManager.Instance.PlaySound("Duck1");
		if (mDucks < 7)
		{
			if (mSolution[mDucks] != DuckColor.blue)
			{
				mWrongOrder = true;
			}
			mDucks++;
			if (mDucks == 7)
			{
				CheckDuckSolution();
			}
		}
	}

	private void PressYellow()
	{
		SFXManager.Instance.PlaySound("Duck3");
		if (mDucks < 7)
		{
			if (mSolution[mDucks] != DuckColor.yellow)
			{
				mWrongOrder = true;
			}
			mDucks++;
			if (mDucks == 7)
			{
				CheckDuckSolution();
			}
		}
	}

	private void PressRed()
	{
		SFXManager.Instance.PlaySound("Duck2");
		if (mDucks < 7)
		{
			if (mSolution[mDucks] != 0)
			{
				mWrongOrder = true;
			}
			mDucks++;
			if (mDucks == 7)
			{
				CheckDuckSolution();
			}
		}
	}

	private void CheckDuckSolution()
	{
		if (mWrongOrder)
		{
			player.ExplodePlayer("That was not the solution.", "Ésa no era la solución.", false);
			return;
		}
		SetCurrentConversation(10);
		CompleteMission(Mission.JeromeSolveRiddle);
		GameObject gameObject = GameObject.Find("MonstermonCardDucks");
		gameObject.GetComponent<BoxCollider2D>().enabled = true;
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
	}

	private void PickUpDuckCard()
	{
		player.GetItem(Item.CyclopsDuckling);
		UnlockHint("Monstermon", 13);
		GameObject gameObject = GameObject.Find("MonstermonCardDucks");
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}

	public override IEnumerator EndDayCoroutine()
	{
		yield return null;
		Interact();
	}

	private void UseLaser()
	{
		player.UseItem(Item.LaserPointer);
	}

	private void Getmold()
	{
		player.GetItem(Item.KeyMold);
	}

	private void JeromeEndDay()
	{
		SteamScript.UnlockAchievement("JeromeAchievement");
		UI.CompleteDay(Item.KeyMold);
		WalkToPoint(new Vector3(-36f, Random.Range(-2f, -1.5f), -1.14f), 7f, false);
		SetDirection(false);
		Object.FindObjectOfType<PauseMenu>().UnlockAllHints("Jerome");
	}
}
