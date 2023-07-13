using System.Collections;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class Cindy : NPCBehavior
{
	public TextureBlock crying;

	public TextureBlock bloody;

	public TextureBlock screaming;

	public TextureBlock angry;

	public TextureBlock angryBlood;

	public SpriteRenderer breathalyzer;

	public GameObject janitorHead;

	private bool mShowedFlower;

	public void SetShowFlower(bool x)
	{
		mShowedFlower = x;
	}

	public bool GetShowedFlower()
	{
		return mShowedFlower;
	}

	private void BringFlower()
	{
		GameObject gameObject = GameObject.Find("Flower");
		player.GetItem(Item.Flower);
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().color = Color.black;
	}

	private void GetGum()
	{
		player.GetItem(Item.Gum);
	}

	private void TakeGum()
	{
		player.UseItem(Item.Gum);
	}

	private void ActivateGumMission()
	{
		ActivateMission(Mission.CindyPutGumInHair, Room.Classroom1);
		ShutDownCindy(true);
		conversations[Room.Classroom1].currentConversation = 1;
	}

	private void CallRape()
	{
		player.SetInteractable(null);
		Object.FindObjectOfType<CameraFollow>().CameraShake();
		ApplyTextureBlock(screaming);
		StartCoroutine(CallRape2());
	}

	private IEnumerator CallRape2()
	{
		StartCoroutine(RunDialogueSection("\\bRAAAAAAPE!!/b", "\\b¡¡VIOLACIÓN!!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		ApplyTextureBlock(defaultTextureBlock);
		Teacher t = Object.FindObjectOfType<Teacher>();
		t.SetDirection(false);
		t.mySkeleton.AnimationName = "walk";
		player.inAnim = true;
		StartCoroutine(RunDialogueSection("Oh dearie me!~~ What on earth is going on over here? I heard someone cry 'rape!'", "¡Ay Dios mío!~~ ¿Pero qué está pasando aquí? ¡Escuché a alguien gritar 'violación!'", t));
		t.FacePlayer();
		t.transform.DOMove(new Vector3(3.25f, -8.45f, -8.45f), 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			t.FacePlayer();
			t.mySkeleton.AnimationName = "idle";
			SetDirection(true);
			player.ForceChangeDirection(true);
			player.inAnim = false;
		})
			.OnUpdate(delegate
			{
				t.SetDepth();
			});
		yield return null;
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Yes Ms. Applegate. It was me. This boy tried to rape me.", "Sí, Señora Applegate. Fui yo. Este chico intentó violarme.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("*Sigh*~ Cindy...~we've been over this. Boys not wanting to be your boyfriend does not count as rape, but as per the school policy, I have to send you both to the principal.", "*Suspiro*~ Cindy...~ya hemos hablado de esto. Que los chicos no quieran ser tus novios, no significa que quieran violarte, pero de acuerdo con la política de la escuela, tengo que enviarlos a los dos con el director.", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("What?! You're punishing me for almost being raped?!", "¿Qué? ¡¿Me está castigando a mi por casi ser violada?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Now you listen to me you little parasite.~ You are going to the principal's office. End of story.", "Ahora escúchame pequeño parásito. ~ Ve a la oficina del director. Fin de la historia.", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("This is so unfair!", "¡Esto es tan injusto!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Yeah, yeah.~ Tell it to the principal.", "Sí, sí. ~Cuéntale al director.", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		Object.FindObjectOfType<Principal>().SetCindy();
		EnvironmentController.Instance.ChangeEnvironment(Room.Office);
	}

	private void GoToDollHouse()
	{
		CompleteMission(Mission.CindyTalkMorningTime);
		ActivateMission(Mission.CindyPlayHouse, Room.Classroom1);
		WalkToPoint(new Vector3(-19.65f, -12.1f, -12.09f), 1.75f);
		SetDirection(false);
	}

	private void ActivateWashUp()
	{
		CompleteMission(Mission.CindyTalkMorningTime);
		ActivateMission(Mission.CindyWashUp, Room.Classroom1);
		WalkToPoint(new Vector3(-19.65f, -12.1f, -12.09f), 1.75f);
		SetDirection(false);
	}

	private void TurnOnTears()
	{
		ApplyTextureBlock(crying);
	}

	private void TurnOnBreathalyzer()
	{
		breathalyzer.enabled = true;
	}

	private void HideBreathalyzer()
	{
		breathalyzer.enabled = false;
	}

	private void GetBreathalyzer()
	{
		player.GetItem(Item.Breathalyzer);
		breathalyzer.enabled = false;
	}

	private void GetFlask()
	{
		player.GetItem(Item.Flask);
	}

	private void CantLeave()
	{
		UnlockHint("Cindy", 3);
		UnlockHint("Cindy", 2);
		StabPlayer("You're drunk. You can't leave. Drinking and driving is bad.", "Estás borracho. No puedes salir. Si bebiste, no conduzcas.");
	}

	private void ShowKnife()
	{
		base.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
	}

	private void HideKnife()
	{
		base.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
	}

	private void StabPlayer(string x, string span)
	{
		Transform child = player.transform.GetChild(3);
		HideKnife();
		child.GetComponent<SpriteRenderer>().enabled = true;
		player.ActivateBloodStream(new Vector3(0f, 1.75f, -3.6f));
		if (player.GetSkeleton().skeleton.flipX)
		{
			child.GetComponent<SpriteRenderer>().flipX = true;
			child.GetComponent<BoneFollower>().offset = new Vector3(-0.46f, 1.24f, -0.24f);
			child.rotation = Quaternion.Euler(0f, 0f, -267f);
		}
		else
		{
			child.GetComponent<SpriteRenderer>().flipX = false;
			child.GetComponent<BoneFollower>().offset = new Vector3(-0.4f, 1.3f, -0.24f);
			child.rotation = Quaternion.Euler(0f, 0f, -109f);
		}
		SFXManager.Instance.PlaySound("GoreSplat2");
		player.inAnim = true;
		player.ActivateStabbed();
		StartCoroutine(StabCoroutine(x, span));
	}

	private void StabPlayerNoMoney()
	{
		UnlockHint("Cindy", 2);
		UnlockHint("Cindy", 3);
		StabPlayer("You're going to need money if Cindy starts getting hostile.", "Necesitarás dinero si Cindy se vuelve hostil.");
	}

	public void StabPlayerDog()
	{
		StabPlayer("It's best not to show the dog when Cindy is around.", "Es mejor cuidar al perro mientras Cindy anda cerca.");
	}

	private IEnumerator StabCoroutine(string x, string span)
	{
		UI.showDeath = true;
		while (player.GetSkeleton().timeScale > 0f)
		{
			player.GetSkeleton().timeScale -= 0.01f;
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		if (player.GetSkeleton().skeleton.flipX)
		{
			player.transform.DORotate(new Vector3(0f, 0f, 90f), 0.25f);
		}
		else
		{
			player.transform.DORotate(new Vector3(0f, 0f, -90f), 0.25f);
		}
		yield return new WaitForSeconds(2f);
		UI.ShowDeath(x, span);
	}

	public void StabNoDeath()
	{
		Transform child = player.transform.GetChild(3);
		HideKnife();
		child.GetComponent<SpriteRenderer>().enabled = true;
		player.ActivateBloodStream(new Vector3(0f, 1.75f, -3.6f));
		if (player.GetSkeleton().skeleton.flipX)
		{
			child.GetComponent<SpriteRenderer>().flipX = true;
			child.GetComponent<BoneFollower>().offset = new Vector3(-0.46f, 1.24f, -0.24f);
			child.rotation = Quaternion.Euler(0f, 0f, -267f);
		}
		else
		{
			child.GetComponent<SpriteRenderer>().flipX = false;
			child.GetComponent<BoneFollower>().offset = new Vector3(-0.4f, 1.3f, -0.24f);
			child.rotation = Quaternion.Euler(0f, 0f, -109f);
		}
		SFXManager.Instance.PlaySound("GoreSplat2");
		player.ActivateStabbed();
		StartCoroutine(StabCoroutineNoDeath());
	}

	private IEnumerator StabCoroutineNoDeath()
	{
		while (player.GetSkeleton().timeScale > 0f)
		{
			player.GetSkeleton().timeScale -= 0.005f;
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		if (player.GetSkeleton().skeleton.flipX)
		{
			player.transform.DORotate(new Vector3(0f, 0f, 90f), 0.25f);
		}
		else
		{
			player.transform.DORotate(new Vector3(0f, 0f, -90f), 0.25f);
		}
	}

	private void ResetTexture()
	{
		ApplyTextureBlock(defaultTextureBlock);
	}

	private void EnableLunch()
	{
		conversations[Room.Cafeteria].currentConversation = 4;
		HideBreathalyzer();
		CompleteMission(Mission.CindyPlayHouse);
		ActivateMission(Mission.CindyEatLunch, Room.Cafeteria);
	}

	private void GetCardHouse()
	{
		player.GetItem(Item.GiraffeSerpent);
		UnlockHint("Monstermon", 6);
	}

	private void UsePill()
	{
		player.UseItem(Item.Pill);
	}

	private void UseSlop()
	{
		player.UseItem(Item.Slop);
	}

	private void UseSalad()
	{
		player.UseItem(Item.Salad);
	}

	private void UseDonut()
	{
		player.UseItem(Item.Donut);
	}

	private void UseBiscuitBall()
	{
		player.UseItem(Item.BiscuitBall);
	}

	private void FeedBiscuitBall()
	{
		player.UseItem(Item.BiscuitBall);
		Object.FindObjectOfType<Janitor>().conversations[Room.Cafeteria].Nodes[24].Options[0].IsAvailable = true;
		CompleteMission(Mission.CindyFindVegan);
	}

	private void FeedSalad()
	{
		player.UseItem(Item.Salad);
		CompleteMission(Mission.CindyFindVegan);
	}

	private void UseNugget()
	{
		player.UseItem(Item.Nugget);
	}

	private void GetKey()
	{
		player.GetItem(Item.JanitorKey);
	}

	private void DistractLunchLady()
	{
		StartCoroutine(PayOffLunchLady());
	}

	private void ActivateGumMissionLunch()
	{
		player.GetItem(Item.Gum);
		ActivateMission(Mission.CindyPutGumLunch, Room.Recess);
	}

	private void GetDoomJelly()
	{
		player.GetItem(Item.Doomjelly);
		UnlockHint("Monstermon", 10);
		CompleteMission(Mission.CindyTellPutGumLunch);
		conversations[Room.Recess].currentConversation = 23;
	}

	private IEnumerator PayOffLunchLady()
	{
		LunchLady ll = Object.FindObjectOfType<LunchLady>();
		player.inAnim = true;
		pCollider.enabled = false;
		ActivateMission(Mission.CindyGoToCloset, Room.Cafeteria);
		SetDirection(false);
		SetAnimation("walk");
		ll.SetDistracted(true);
		ShortcutExtensions.DOPath(path: new Vector3[2]
		{
			new Vector3(1.05f, -7.71f, -7.7f),
			new Vector3(-15.85f, -2.58f, -2.57f)
		}, target: base.transform, duration: 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetAnimation("idle");
			ll.SetDirection(true);
		});
		yield return new WaitForSeconds(3f);
		SetAnimation("walk");
		SetDirection(true);
		ShortcutExtensions.DOPath(path: new Vector3[2]
		{
			new Vector3(1.05f, -7.71f, -7.7f),
			new Vector3(6.34f, -7.71f, -7.703f)
		}, target: base.transform, duration: 2f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(2f);
		pCollider.enabled = true;
		FacePlayer();
		player.inAnim = false;
		pCollider.enabled = true;
		SetAnimation("idle");
		SetCurrentConversation(24);
		Interact();
		conversations[Room.Recess].currentConversation = 1;
	}

	private void StartVegan()
	{
		CompleteMission(Mission.CindyEatLunch);
		ActivateMission(Mission.CindyFindVegan, Room.Cafeteria);
	}

	private void CompleteVegan()
	{
		CompleteMission(Mission.CindyFindVegan);
		if (player.HasItem(Item.BiscuitBall))
		{
			player.UseItem(Item.BiscuitBall);
		}
		else
		{
			player.UseItem(Item.Salad);
		}
	}

	private void WashBlood()
	{
		player.ApplyTextureBlock(player.bruised);
		CompleteMission(Mission.CindyWashUp);
		ActivateMission(Mission.CindyPlayHouse, Room.Classroom1);
		conversations[Room.Classroom1].currentConversation = 4;
	}

	private void CheckActionsLeft()
	{
		if (EnvironmentController.Instance.ActionsLeft() == 0)
		{
			lastSelectedOption.DestinationID = 29;
		}
	}

	private void GetInPosition()
	{
		CompleteMission(Mission.CindyShowBucket);
		ActivateMission(Mission.CindyDumpBucket);
		if (EnvironmentController.Instance.ActionsLeft() == 0)
		{
			lastSelectedOption.DestinationID = 29;
			return;
		}
		player.inAnim = true;
		player.ForceAnim(true);
		player.inDialogue = true;
		player.lockActions = true;
		bool flag = player.transform.position.x > base.transform.position.x;
		player.ForceChangeDirection(flag);
		ShortcutExtensions.DOPath(path: (!flag) ? new Vector3[4]
		{
			new Vector3(-25.21f, -3.61f, 0f),
			new Vector3(-24.56f, -2.32f, 0f),
			new Vector3(-24.56f, 0.87f, 0f),
			new Vector3(-22.21f, 0.87f, 0f)
		} : new Vector3[4]
		{
			new Vector3(-10.75f, -3.61f, 0f),
			new Vector3(-11.16f, -2.32f, 0f),
			new Vector3(-11.16f, 0.87f, 0f),
			new Vector3(-13.34f, 0.87f, 0f)
		}, target: player.transform, duration: 3f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.ForceAnim(false);
			player.inAnim = false;
			player.inDialogue = false;
			SetDirection(true);
			StartCoroutine(CallLilyOver());
		})
			.OnWaypointChange(CheckTurnMB)
			.OnUpdate(delegate
			{
				player.SetDepth();
			});
	}

	private void CheckTurnMB(int x)
	{
		if (x == 1)
		{
			player.ForceChangeDirection(player.transform.position.x < base.transform.position.x);
		}
		if (x == 2)
		{
			if (player.transform.position.y < 0f)
			{
				GameObject.Find("monkeybars").transform.localPosition = new Vector3(-17.5f, -0.8f, -3f);
			}
			else
			{
				GameObject.Find("monkeybars").transform.localPosition = new Vector3(-17.5f, -0.8f, -7.458f);
			}
		}
	}

	private IEnumerator CallLilyOver()
	{
		Lily i = Object.FindObjectOfType<Lily>();
		SetEmptyOptions();
		yield return null;
		StartCoroutine(RunDialogueSection("Hey! Lily!~ Come over here! I want to show you something!", "¡Hey! ¡Lily!~ ¡Ven! ¡Quiero mostrarte algo!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		i.SetDirection(false);
		StartCoroutine(RunDialogueSection("Why can't you just leave me alone Cindy?~ I'm so sick of you harassing me!", "¿Por qué no me dejas en paz, Cindy? ~¡Estoy harto de que me acoses!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I'm not gonna harass you!~ We can be friends! Just come over here.", "¡No voy a acosarte! ~ ¡Podemos ser amigos! Sólo ven acá.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.inAnim = true;
		i.mySkeleton.AnimationName = "walk";
		i.transform.DOMove(new Vector3(-16.93f, -3.57f, -3.62f), 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.inAnim = false;
			i.mySkeleton.AnimationName = "idle";
			i.SetDepth();
		});
		StartCoroutine(RunDialogueSection("Fine...~what is it?", "Está bien...~ ¿Dime?", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
	}

	public void DumpBloodOnCindy()
	{
		player.UseItem(Item.BucketBlood);
		FailMission(Mission.CindyDumpBucket);
		ApplyTextureBlock(bloody);
		StartCoroutine(DumpedOnCindy());
	}

	private IEnumerator DumpedOnCindy()
	{
		Teacher t = Object.FindObjectOfType<Teacher>();
		GameObject.Find("CindyShoeCollider").GetComponent<BoxCollider2D>().enabled = true;
		GameObject bucket = GameObject.Find("Bucket");
		bucket.GetComponent<SpriteRenderer>().enabled = true;
		bucket.transform.position = new Vector3(-20f, 2.17f, -10f);
		bucket.GetComponentInChildren<ParticleSystem>().Play();
		player.inDialogue = true;
		player.SetInteractable(null);
		yield return new WaitForSeconds(0.25f);
		bucket.GetComponent<Rigidbody2D>().simulated = true;
		StartCoroutine(RunDialogueSection("Wha--?! ~No...no...no...nooooooooo.", "¡¿Qué--?! ~No...no...no...nooooooooo.", this));
		yield return new WaitForSeconds(0.5f);
		SFXManager.Instance.PlaySound("BucketClank");
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetAnimation("walk");
		base.transform.DOMoveX(-36f, 1f).SetEase(Ease.Linear).OnComplete(delegate
		{
			RemoveFromGame();
		});
		SetDirection(false);
		StartCoroutine(RunDialogueSection("\\bNOOOOOOOOO!!!/b", "\\b¡¡¡NOOOOOOOOO!!!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		t.SetAnimation("walk");
		t.SetDirection(false);
		t.transform.DOMoveX(-14f, 3f).SetEase(Ease.Linear).OnComplete(delegate
		{
			t.mySkeleton.AnimationName = "idle";
		});
		player.inAnim = true;
		StartCoroutine(RunDialogueSection("What's going on?~ Cindy, come back! Don't go running into the street! You'll get hit by a--~~~ I keep telling the principal we need to put a fence there!~ Sheesh.", "¿Qué está pasando?~ ¡Cindy, regresa! ¡No corras a la calle! Te van a atropellar--~~~ ¡Le he estado diciendo al director que tenemos que poner una reja allí! ~ Sheesh.", t));
		yield return new WaitForSeconds(2.5f);
		SFXManager.Instance.PlaySound("CarHorn");
		yield return new WaitForSeconds(1f);
		SFXManager.Instance.PlaySound("CarCrash");
		yield return new WaitForSeconds(0.25f);
		Object.FindObjectOfType<CameraFollow>().CameraShake();
		Rigidbody2D s = GameObject.Find("CindyShoe").GetComponent<Rigidbody2D>();
		s.isKinematic = false;
		s.AddForce(new Vector2(320f, 600f));
		s.AddTorque(-150f);
		while (UI.IsDialogueRunning())
		{
			yield return null;
		}
		player.inAnim = false;
		while (pEmptyOptions)
		{
			yield return null;
		}
		s.isKinematic = true;
		s.gameObject.layer = 0;
		s.transform.position = new Vector3(s.transform.position.x, s.transform.position.y, s.GetComponent<Collider2D>().bounds.min.y);
		t.SetDirection(true);
		StartCoroutine(RunDialogueSection("Okay children! There's been an accident that I don't want to get blamed for. We're going to cut recess a little short today. Everyone head inside.", "¡Okay, niños! Ha pasado un accidente del cual no quiero ser culpable. Hoy vamos a cortar el recreo. Todo el mundo para adentro.", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SendNPCOffScreen(Object.FindObjectOfType<Buggs>());
		SendNPCOffScreen(Object.FindObjectOfType<Lily>());
		SendNPCOffScreen(Object.FindObjectOfType<Monty>());
		SendNPCOffScreen(Object.FindObjectOfType<Nugget>());
		SendNPCOffScreen(Object.FindObjectOfType<Jerome>());
		yield return new WaitForSeconds(2f);
		t.SetDirection(false);
		StartCoroutine(RunDialogueSection("Come on down from there. I'd like to have a little chat with you before we go back inside.", "Bájate de allí. Me gustaría hablar contigo antes de que entremos.", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.inAnim = true;
		player.ForceAnim(true);
		player.ForceChangeDirection(true);
		ShortcutExtensions.DOPath(path: new Vector3[3]
		{
			new Vector3(-13.34f, 0.87f, 0f),
			new Vector3(-11.16f, 0.87f, 0f),
			new Vector3(-11.16f, -2.32f, 0f)
		}, target: player.transform, duration: 2.5f).SetEase(Ease.Linear).OnWaypointChange(CheckTurnMB)
			.OnComplete(delegate
			{
				player.inAnim = false;
			})
			.OnUpdate(delegate
			{
				player.SetDepth();
			});
		t.SetCurrentConversation(1);
		bucket.GetComponent<Rigidbody2D>().simulated = false;
		bucket.transform.position = new Vector3(bucket.transform.position.x, bucket.transform.position.y, bucket.GetComponent<Collider2D>().bounds.min.y);
		GameObject.Find("CindyShoeCollider").GetComponent<BoxCollider2D>().enabled = false;
		UI.CollapseDialogue();
	}

	private void SendNPCOffScreen(NPCBehavior n)
	{
		n.SetDirection(true);
		n.mySkeleton.AnimationName = "walk";
		n.transform.DOMoveX(60f, 6f).SetEase(Ease.Linear).OnComplete(delegate
		{
			n.mySkeleton.AnimationName = "idle";
		});
	}

	public IEnumerator ClimbDown()
	{
		player.inAnim = true;
		player.ForceAnim(true);
		player.ForceChangeDirection(true);
		UI.CollapseDialogue();
		ShortcutExtensions.DOPath(path: new Vector3[5]
		{
			new Vector3(-13.34f, 0.87f, 0f),
			new Vector3(-11.16f, 0.87f, 0f),
			new Vector3(-11.16f, -2.32f, 0f),
			new Vector3(-10.75f, -3.61f, 0f),
			new Vector3(-17.6f, -3.61f, 0f)
		}, target: player.transform, duration: 3.5f).SetEase(Ease.Linear).OnWaypointChange(CheckTurnMB)
			.OnComplete(delegate
			{
				player.inAnim = false;
			})
			.OnUpdate(delegate
			{
				player.SetDepth();
			});
		yield return new WaitForSeconds(3.5f);
		SetCurrentConversation(13);
		Interact();
	}

	private void GetShoe()
	{
		player.GetItem(Item.CindyShoe);
		GameObject.Find("CindyShoe").transform.position = new Vector3(50f, 50f, 0f);
	}

	private void GetMop()
	{
		player.GetItem(Item.Mop);
		GameObject.Find("Mop").transform.position = new Vector3(50f, 50f, 0f);
	}

	private void TakeFlower()
	{
		Teacher teacher = Object.FindObjectOfType<Teacher>();
		teacher.StartCoroutine(teacher.WhatHappenedToLily());
	}

	private void GetFlower()
	{
		SetEndDay(true);
		player.GetItem(Item.Flower);
	}

	private void ActivateReadNote()
	{
		CompleteMission(Mission.CindyShowPaper);
		ActivateMission(Mission.CindyReadPaper);
	}

	private void GetOhFaka()
	{
		UnlockHint("Monstermon", 8);
		player.GetItem(Item.OhfakaTornado);
	}

	private void KillJanitor()
	{
		player.UseItem(Item.Recipe);
		CompleteMission(Mission.CindyTellPaper);
		StartCoroutine(KillJanitorCoroutine());
	}

	private IEnumerator KillJanitorCoroutine()
	{
		Janitor i = Object.FindObjectOfType<Janitor>();
		Vector3 startPos = base.transform.position;
		ApplyTextureBlock(screaming);
		SetDirection(true);
		StartCoroutine(RunDialogueSection("\\bJANITOOOOOOR!!/b", "\\b¡¡CONSERJE!!/b", this));
		Object.FindObjectOfType<CameraFollow>().CameraShake(0.25f);
		while (pEmptyOptions)
		{
			yield return null;
		}
		ApplyTextureBlock(angry);
		i.SetDirection(false);
		i.WalkToPoint(new Vector3(12.28f, -4.54f, -4.64f), 2f);
		WalkToPoint(new Vector3(7.45f, -4.64f, -4.63f), 3f);
		StartCoroutine(RunDialogueSection("What's goin' on there little girl? How can the janitor help you?", "¿Qué está pasando aquí pequeña? ¿Cómo puede el conserje ayudarte?", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("YOU KILLED MY DOG!!", "¡¡MATASTE A MI PERRO!!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("What breed was it? The meat is quite tasty.", "¿Qué raza era? La carne es muy sabrosa.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		i.SetDirection(true);
		i.WalkToPoint(new Vector3(42.28f, -4.54f, -4.64f), 2f);
		WalkToPoint(new Vector3(37.45f, -4.64f, -4.63f), 2f);
		StartCoroutine(RunDialogueSection("I'm going to kill you with my bare hands! Come here!", "¡Te voy a matar con mis propias manos! ¡Ven acá!", this));
		player.inAnim = true;
		yield return new WaitForSeconds(2f);
		player.inAnim = false;
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Wow! You're faster than I thou--OW! Hey stop! Okay I get it! I'll put all the pieces of your dog back!", "¡Wow! Eres más rápida de lo que pensé--¡OH! ¡Hey, pára! ¡Okay, entendido! ¡Voy a poner todas las partes de tu perro en su lugar!", i));
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(BeatUpSounds());
		while (pEmptyOptions)
		{
			yield return null;
		}
		GameObject.Find("CindyShoeCollider").GetComponent<BoxCollider2D>().enabled = true;
		Rigidbody2D s = GameObject.Find("Mop").GetComponent<Rigidbody2D>();
		s.isKinematic = false;
		s.AddForce(new Vector2(-620f, 600f));
		s.AddTorque(150f);
		StartCoroutine(RunDialogueSection("My mop! I need that!", "¡Mi trapeador! ¡Lo necesito!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		i.ApplyTextureBlock(i.ear);
		ApplyTextureBlock(angryBlood);
		StartCoroutine(RunDialogueSection("\\bMY EAR!!~ YOU BIT OFF MY EAR!!/b", "\\b¡¡MI OREJA!!~ ¡¡MORDISTE MI OREJA!!/b", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("That's not all you're about to lose!!", "¡¡Eso no es lo único que vas a perder!!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("\\bAAAAAHHHHHH!!!/b", "\\b¡¡¡AAAAAHHHHHH!!!/b", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SFXManager.Instance.PlaySound("RemoveHead");
		UI.CollapseDialogue();
		player.inAnim = true;
		s.isKinematic = true;
		s.gameObject.layer = 0;
		s.transform.position = new Vector3(s.transform.position.x, s.transform.position.y, s.GetComponent<Collider2D>().bounds.min.y);
		GameObject.Find("CindyShoeCollider").GetComponent<BoxCollider2D>().enabled = false;
		SetDirection(false);
		janitorHead.SetActive(true);
		yield return new WaitForSeconds(2f);
		WalkToPoint(startPos, 7f);
		mySkeleton.timeScale = 1f;
		yield return new WaitForSeconds(7f);
		mySkeleton.timeScale = 1.5f;
		Interact();
	}

	private IEnumerator BeatUpSounds()
	{
		for (int i = 0; i < 3; i++)
		{
			SFXManager.Instance.PlaySound("BasicHit");
			Object.FindObjectOfType<CameraFollow>().CameraShake(0.25f);
			yield return new WaitForSeconds(1f);
		}
	}

	private void PickUpMop()
	{
		player.GetItem(Item.Mop);
		GameObject.Find("Mop").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("Mop").GetComponent<BoxCollider2D>().enabled = false;
	}

	private void GetMoneyForLily()
	{
		player.GetMoney(3f);
	}

	public override IEnumerator EndDayCoroutine()
	{
		if (!mShowedFlower)
		{
			ApplyTextureBlock(angry);
			Interact();
		}
		else
		{
			SetCurrentConversation(4);
			Interact();
		}
		yield return null;
	}

	private void LoseFlower()
	{
		SetDirection(false);
		WalkToPoint(new Vector3(-36f, Random.Range(-2f, -1.5f), -1.14f), 7f, false);
		player.UseItem(Item.Flower);
		UI.CompleteDay(Item.None);
	}

	private void EndCindyWithFlower()
	{
		SetDirection(false);
		WalkToPoint(new Vector3(-36f, Random.Range(-2f, -1.5f), -1.14f), 7f, false);
		UI.CompleteDay(Item.Flower);
		Object.FindObjectOfType<PauseMenu>().UnlockAllHints("Cindy");
		SteamScript.UnlockAchievement("CindyAchievement");
	}

	private void BreakUp()
	{
		ShutDownCindy(true);
	}

	public void ShutDownCindy(bool breakup)
	{
		if (breakup)
		{
			conversations[Room.Classroom1].currentConversation = 55;
			conversations[Room.Cafeteria].currentConversation = 33;
			conversations[Room.Recess].currentConversation = 27;
		}
		else
		{
			conversations[Room.Cafeteria].currentConversation = 34;
			conversations[Room.Classroom1].currentConversation = 56;
			conversations[Room.Recess].currentConversation = 28;
		}
	}
}
