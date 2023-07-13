using System.Collections;
using DG.Tweening;
using DialogueTree;
using UnityEngine;

public class Lily : NPCBehavior
{
	public enum LilyStatus
	{
		DontLeaveCafeteria = 0,
		HeadingToBathroom = 1,
		HeadingToOffice = 2,
		OutsideOffice = 3,
		None = 4
	}

	public TextureBlock Surprised;

	public TextureBlock Gum;

	public TextureBlock Bloody;

	public TextureBlock Embarassed;

	public LilyStatus mLilyStatus = LilyStatus.None;

	private void LoseGum()
	{
		player.UseItem(Item.Gum);
		FailMission(Mission.CindyPutGumInHair);
		Object.FindObjectOfType<Cindy>().conversations[Room.Classroom1].Nodes[1].Options[0].IsAvailable = true;
	}

	private void LilyCompliment()
	{
		conversations[Room.Classroom1].currentConversation = 3;
	}

	private void GetDonut()
	{
		player.GetItem(Item.Donut);
		UnlockHint("Nugget", 4);
	}

	private void GetMoney()
	{
		player.GetMoney(2f);
	}

	private void ActivateReadNoteMission()
	{
		ActivateMission(Mission.LilyReadNote, Room.SchoolYard);
	}

	private void ActivateGetSentToOffice()
	{
		UnlockHint("Lily", 4);
		CompleteMission(Mission.LilyTellNote);
		ActivateMission(Mission.LilyGetSentToOffice, Room.SchoolYard);
		conversations[Room.Classroom1].currentConversation = 7;
	}

	private void TurnOnSurprised()
	{
		ApplyTextureBlock(Surprised);
	}

	private void PutGumInHair()
	{
		player.UseItem(Item.Gum);
		mLilyStatus = LilyStatus.DontLeaveCafeteria;
		ApplyTextureBlock(Gum);
		CompleteMission(Mission.CindyPutGumInHair);
		Object.FindObjectOfType<Cindy>().ActivateMission(Mission.CindyTalkMorningTime, Room.Classroom1);
		Object.FindObjectOfType<Cindy>().conversations[Room.Classroom1].currentConversation = 2;
	}

	private void FailGiveLetter()
	{
		FailMission(Mission.NuggetGiveLoveLetter);
		DialogueNode currentConversation = Object.FindObjectOfType<Nugget>().GetCurrentConversation();
		currentConversation.Options[0].IsAvailable = true;
		currentConversation.Options[0].IsAvailable = false;
	}

	private void FailFindHatch()
	{
		FailMission(Mission.LilyLookForSomethingSuspicious);
	}

	private void TeacherReadLetter()
	{
		StartCoroutine(TeacherReadLetterCoroutine());
	}

	private IEnumerator TeacherReadLetterCoroutine()
	{
		UI.CollapseDialogue();
		mLilyStatus = LilyStatus.DontLeaveCafeteria;
		player.UseItem(Item.LoveLetter);
		Teacher t = Object.FindObjectOfType<Teacher>();
		Nugget i = Object.FindObjectOfType<Nugget>();
		SetDirection(true);
		SetAnimation("walk");
		pCollider.enabled = false;
		Vector3[] v2 = new Vector3[3]
		{
			new Vector3(10.56f, -0.56f, -0.55345f),
			new Vector3(18.26f, -0.56f, -0.55345f),
			new Vector3(22.44f, -6.92f, -6.91345f)
		};
		player.inAnim = true;
		base.transform.DOPath(v2, 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetAnimation("idle");
			t.SetDirection(false);
		});
		yield return new WaitForSeconds(2f);
		StartCoroutine(RunDialogueSection("Ummm...~Ms. Applegate?", "Ummm...~¿Señora Applegate?", this));
		player.inAnim = false;
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Yes, Lily? What is it?", "¿Sí, Lily? ¿Qué es ésto?", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Nugget just gave me this love letter and I was wondering--", "Nugget me acaba de entregar esta carta de amor y me estaba preguntando—", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("\\bOOOOOOOH!!/b", "\\b¡¡OOOOOOOH!!/b", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.inAnim = true;
		t.SetAnimation("walk");
		t.transform.DOMoveX(15.63f, 1.5f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(1.5f);
		SetDirection(false);
		t.SetAnimation("idle");
		player.inAnim = false;
		StartCoroutine(RunDialogueSection("Gather round children!~ Nugget wrote Lily a love letter!", "¡Vengan todos, niños!~ ¡Nugget escribió a  Lily una carta de amor!", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		i.SetDirection(true);
		StartCoroutine(RunDialogueSection("Oh no no no. Nugget's letter is only to be read by the pretty Lily!", "Oh, no, no, no. ¡La carta de Nugget debe ser leída sólo por la preciosa Lily!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("\"Nugget likes the pretty Lily. He also misses her brother Billy.\"", "\"A Nugget le gusta la preciosa Lily. Además extraña a su hermano Billy.\"", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		ApplyTextureBlock(Embarassed);
		StartCoroutine(RunDialogueSection("Please do not continue!", "¡No sigas, por favor!", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("\"Nugget likes the Lily's hair. Nugget likes to smell her...~chair.\"", "\"A Nugget le gusta su cabello. A Nugget le gusta oler su...~cuello.\"", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Nugget do we need to schedule another trip to the school counselor? This is getting pretty weird.", "Nugget ¿necesitamos programar otra visita al consejero de la escuela? Esto se está poniendo muy raro.", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Nugget thinks you should stop then.", "Nugget cree que deberías parar.", i));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Maybe just a few more lines...", "Tal vez sólo unos renglones más...", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("\\bNUGGET SAID NO!!/b", "\\b¡¡NUGGET DIJO NO!!/b", i));
		yield return new WaitForSeconds(0.25f);
		Object.FindObjectOfType<CameraFollow>().CameraShake(1f);
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Okay...okay. Fine. Weirdo.~ As you were children.", "Okay...okay. Está bien. Bicho raro. ~ Como si fueran niños.", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		player.inAnim = true;
		t.SetDirection(true);
		t.SetAnimation("walk");
		t.transform.DOMoveX(25.65f, 1.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			t.SetAnimation("idle");
			t.SetDirection(false);
		});
		v2 = new Vector3[3]
		{
			new Vector3(18.26f, -0.56f, -0.55345f),
			new Vector3(10.56f, -0.56f, -0.55345f),
			new Vector3(10.56f, -1.65f, -1.64345f)
		};
		SetAnimation("walk");
		SetDirection(false);
		base.transform.DOPath(v2, 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetAnimation("idle");
			SetDirection(true);
			player.inAnim = false;
			player.SetInteractable(this);
			pCollider.enabled = true;
			Interact();
		});
		conversations[Room.Cafeteria].currentConversation = 1;
		i.SetCurrentConversation(19);
		CompleteMission(Mission.NuggetGiveLoveLetter);
		i.ActivateMission(Mission.NuggetDeliveredLetter);
		while (player.inDialogue)
		{
			yield return null;
		}
	}

	private void CheckForBug()
	{
		if (player.HasItem(Item.Bug))
		{
			lastSelectedOption.DestinationID = -1;
			player.ExplodePlayer("Don't talk to Lily if you have a device.", "No hables con Lily si tienes un dispositivo.");
		}
		else
		{
			CompleteMission(Mission.LilyTellAboutTheHatch);
		}
	}

	private void ActivateJanitorMission()
	{
		UnlockHint("Lily", 5);
		ActivateMission(Mission.LilyFindEvidence, Room.Classroom1);
		mLilyStatus = LilyStatus.DontLeaveCafeteria;
		conversations[Room.Cafeteria].currentConversation = 7;
	}

	private void UseShoe()
	{
		player.UseItem(Item.Shoe);
	}

	private void UseFinger()
	{
		player.UseItem(Item.SeveredFinger);
	}

	private void GoForwardWithPlan()
	{
		CompleteMission(Mission.LilyFindEvidence);
		ActivateMission(Mission.LilyTalkAtLunch, Room.Cafeteria);
		conversations[Room.Cafeteria].currentConversation = 11;
	}

	public IEnumerator MakePhoneCall()
	{
		player.inAnim = true;
		yield return new WaitForSeconds(1f);
		player.inAnim = false;
		Principal p = Object.FindObjectOfType<Principal>();
		SetDirection(false);
		StartCoroutine(RunDialogueSection("Good thinking bringing those cigarettes. Now give me the phone. I have a very important call to make to the principal.", "Bien pensado traer esos cigarrillos. Ahora dame el teléfono. Tengo una llamada muy importante que hacer al director.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.UseItem(Item.Phone);
		SFXManager.Instance.PlaySound("DialSound");
		yield return new WaitForSeconds(1f);
		StartCoroutine(RunDialogueSection("Ummm...~hello? Mr. Principal?", "Mmm...~¿hola? ¿Señor Director?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Lily?~ Is that you? How are you calling me with your teacher's phone? Put her on.", "¿Lily?~¿es usted? ¿Cómo me está llamando desde el teléfono de la profesora? Pásemela.", p));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("No, but if you want to save her life, you better get over to the classroom now!", "No, pero si quiere salvarle la vida, ¡lo mejor es que vaya al salón de clases ahora mismo!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("What's the meaning of this?!~ Lily!?~ Answer me young lady!", "¡¿Qué significa todo esto?!~ ¿¡Lily!?~ ¡Contésteme, señorita!", p));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("*Click*~ That should do it...~yup. I can hear him heading down the hall. Let's go!", "*Clic*~ Debías hacerlo...~sip. Puedo oírlo bajando por el pasillo. ¡Vámonos!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		player.inAnim = true;
		pCollider.enabled = true;
		SetAnimation("walk");
		base.transform.DOMove(new Vector3(-23.1f, -1f, -0.89f), 1f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.inAnim = false;
			SetAnimation("idle");
			base.transform.position = new Vector3(0f, 50f, 0f);
		});
		p.SetOutOfOffice();
		mLilyStatus = LilyStatus.HeadingToOffice;
	}

	private void GoForwardWithPlanLunch()
	{
		CompleteMission(Mission.LilyShowEvidence);
	}

	public void CheckLeaveCafeteria()
	{
		if (mLilyStatus == LilyStatus.None)
		{
			StartCoroutine(WaitLeaveCafeteria());
		}
		else if (mLilyStatus != 0)
		{
			base.transform.position = new Vector3(0f, 50f, 0f);
		}
	}

	private IEnumerator WaitLeaveCafeteria()
	{
		EnvironmentController.Instance.waitForNextAction = true;
		while (EnvironmentController.Instance.waitForNextAction || UI.IsDialogueOut() || player.inAnim)
		{
			yield return null;
		}
		LeaveCafeteria();
	}

	public void LeaveCafeteria()
	{
		if (mLilyStatus == LilyStatus.DontLeaveCafeteria)
		{
			return;
		}
		CameraFollow c = Object.FindObjectOfType<CameraFollow>();
		c.overrideCamera = true;
		c.transform.DOMove(new Vector3(c.maxPosX, c.minPosY, -30f), 0.5f);
		player.inAnim = true;
		Vector3[] path = new Vector3[2]
		{
			new Vector3(32f, -14.44f, -14.43f),
			new Vector3(23.5f, -0.5f, -0.52f)
		};
		SetDirection(true);
		SetAnimation("walk");
		pCollider.enabled = false;
		if (player.GetInteractable() == this)
		{
			player.SetInteractable(null);
		}
		base.transform.DOPath(path, 2.5f).SetEase(Ease.Linear).OnWaypointChange(delegate(int x)
		{
			if (x == 1)
			{
				SetDirection(false);
			}
		})
			.OnComplete(delegate
			{
				player.inAnim = false;
				SetAnimation("idle");
				pCollider.enabled = true;
				base.transform.position = new Vector3(50f, 0f, 0f);
				if (mLilyStatus == LilyStatus.None)
				{
					mLilyStatus = LilyStatus.OutsideOffice;
				}
				StartCoroutine(MoveCameraBack(c));
			});
	}

	private IEnumerator MoveCameraBack(CameraFollow c)
	{
		for (int i = 0; i < 60; i++)
		{
			c.transform.position = Vector3.Slerp(c.transform.position, new Vector3(Mathf.Clamp(player.transform.position.x, c.minPosX, c.maxPosX), Mathf.Clamp(player.transform.position.y, c.minPosY, c.maxPosY), -30f), 0.1f);
			yield return null;
		}
		c.overrideCamera = false;
	}

	private void MeetLilyInBathroom()
	{
		CompleteMission(Mission.LilyTalkAtLunch);
		CompleteMission(Mission.LilyShowCode);
		ActivateMission(Mission.LilyMeetInBathroom, Room.Cafeteria);
		mLilyStatus = LilyStatus.HeadingToBathroom;
		LeaveCafeteria();
	}

	public void CheckHallwayLilyStatus()
	{
		switch (mLilyStatus)
		{
		case LilyStatus.OutsideOffice:
			base.transform.position = new Vector3(-24.5f, -6.4f, -6.39f);
			GameObject.Find("PrincipalDoor").GetComponent<ObjectInteractable>().dialogue.Nodes[0].Options[0].IsAvailable = true;
			UnlockHint("Teacher", 6);
			SetDepth();
			break;
		case LilyStatus.HeadingToOffice:
			player.lockActions = true;
			base.transform.position = new Vector3(15f, -1.7f, -1.7f);
			SetDepth();
			SetAnimation("walk");
			SetDirection(false);
			pCollider.enabled = false;
			GameObject.Find("BathroomDoor").GetComponent<Collider2D>().enabled = false;
			GameObject.Find("JanitorHallwayDoor").GetComponent<Collider2D>().enabled = false;
			GameObject.Find("CafeteriaDoorHallway").GetComponent<Collider2D>().enabled = false;
			base.transform.DOMove(new Vector3(-27f, -6.4f, -6.39f), 3f).SetEase(Ease.Linear).OnComplete(delegate
			{
				pCollider.enabled = true;
				SetAnimation("idle");
			});
			break;
		case LilyStatus.HeadingToBathroom:
			SetCurrentConversation(4);
			player.lockActions = true;
			base.transform.position = new Vector3(-13f, -1f, -1.2f);
			SetAnimation("walk");
			SetDirection(true);
			pCollider.enabled = false;
			GameObject.Find("JanitorHallwayDoor").GetComponent<Collider2D>().enabled = false;
			GameObject.Find("CafeteriaDoorHallway").GetComponent<Collider2D>().enabled = false;
			base.transform.DOMoveX(15f, 2f).SetEase(Ease.Linear).OnComplete(delegate
			{
				SetAnimation("idle");
				pCollider.enabled = true;
				base.transform.position = new Vector3(0f, 50f, 0f);
			});
			break;
		}
	}

	private void KnockOnDoor()
	{
		StartCoroutine(KnockOnDoorWithLily());
	}

	private IEnumerator KnockOnDoorWithLily()
	{
		Principal p = Object.FindObjectOfType<Principal>();
		StartCoroutine(RunDialogueSection("What the--?! It's lunch time! Who's knocking on my door!?", "¡¿Qué carajos--?! ¡Es la hora de almuerzo! ¿¡Quién está tocando a mi puerta?", p));
		FacePlayer();
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("What are you doing?! You're gonna get us both in trouble!", "¡¿Qué estás haciendo?! ¡Vas a meternos en problemas!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.ForceAnim(true);
		player.ForceChangeDirection(true);
		player.inAnim = true;
		player.transform.DOMoveX(-19f, 1f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.ForceChangeDirection(false);
			player.ForceAnim(false);
			player.inAnim = false;
			player.SetInteractable(p);
		});
		p.transform.position = new Vector3(-27.9f, -7.94f, -7.91f);
		p.SetDepth();
		p.Interact();
		p.SetDirection(true);
	}

	private void EnterOffice()
	{
		EnvironmentController.Instance.SetSpawnPoint(1);
		EnvironmentController.Instance.ChangeEnvironment(Room.Office);
		if (mLilyStatus != LilyStatus.HeadingToOffice)
		{
			Object.FindObjectOfType<Principal>().mReason = Principal.OfficeReason.UsedKey;
		}
	}

	private void CompleteGumLunch()
	{
		player.UseItem(Item.Gum);
		ApplyTextureBlock(Gum);
		CompleteMission(Mission.CindyPutGumLunch);
		Object.FindObjectOfType<Cindy>().ActivateMission(Mission.CindyTellPutGumLunch);
	}

	public void DumpBloodOnLily()
	{
		player.UseItem(Item.BucketBlood);
		CompleteMission(Mission.CindyDumpBucket);
		ApplyTextureBlock(Bloody);
		StartCoroutine(DumpedOnLily());
	}

	private IEnumerator DumpedOnLily()
	{
		Cindy c = Object.FindObjectOfType<Cindy>();
		GameObject.Find("CindyShoeCollider").GetComponent<BoxCollider2D>().enabled = true;
		SetEmptyOptions();
		player.inDialogue = true;
		player.SetInteractable(null);
		GameObject bucket = GameObject.Find("Bucket");
		bucket.GetComponent<SpriteRenderer>().enabled = true;
		bucket.GetComponentInChildren<ParticleSystem>().Play();
		yield return new WaitForSeconds(0.25f);
		bucket.GetComponent<Rigidbody2D>().simulated = true;
		StartCoroutine(RunDialogueSection("\\bAAAAHHHHH!!/b", "\\b¡¡AAAAHHHHH!!/b", this));
		yield return new WaitForSeconds(0.5f);
		SFXManager.Instance.PlaySound("BucketClank");
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("What did you just--~Oh...~you and your awful boyfriend set me up again. I'm so done with this. You've made me miserable for so long.", "Qué has hecho--~Oh...~tú y tu horrible novio me engañaron otra vez. Ya no puedo más con ésto. Ustedes me han humillado por tanto tiempo.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Awww c'mon. I think it's an improvement!", "¡Ayyyyy! ¡Creo que ya se está mejorando!", c));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("You're just so mean. I can't take it anymore.", "Eres tan malo. No puedo soportarlo más.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		mySkeleton.AnimationName = "walk";
		SetDirection(true);
		GameObject.Find("HoleCover").GetComponent<SpriteRenderer>().enabled = true;
		base.transform.DOMove(new Vector3(-14f, -10f, -20f), 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			mySkeleton.AnimationName = "idle";
			SetDepth();
		});
		yield return new WaitForSeconds(2f);
		StartCoroutine(RunDialogueSection("Goodbye cruel world!", "¡Adiós mundo cruel!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		bucket.GetComponent<Rigidbody2D>().simulated = false;
		bucket.transform.position = new Vector3(bucket.transform.position.x, bucket.transform.position.y, bucket.GetComponent<Collider2D>().bounds.min.y);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -7.15f);
		Rigidbody2D r = GetComponent<Rigidbody2D>();
		base.gameObject.layer = 9;
		pCollider.offset = new Vector2(0f, 1.25f);
		GetComponent<BoxCollider2D>().size = new Vector2(1.26f, 2.5f);
		r.isKinematic = false;
		r.AddForce(new Vector2(200f, 0f));
		r.AddTorque(-150f);
		yield return new WaitForSeconds(1.5f);
		RemoveFromGame();
		yield return new WaitForSeconds(1.5f);
		SFXManager.Instance.PlaySound("Thud");
		Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		yield return new WaitForSeconds(1f);
		StartCoroutine(RunDialogueSection("Wow!~ That hole is deep! Do you think she died?~ Nevermind. I don't care. You should come down from there.", "¡Wow!~ ¡Este agujero es profundo! ¿Crees que está muerta?~ No importa. Tu deberías bajar ahí.", c));
		while (pEmptyOptions)
		{
			yield return null;
		}
		c.StartCoroutine(c.ClimbDown());
	}

	private void BlowUpStatue()
	{
		if (UI.IsMissionComplete(Mission.NuggetPlaceDevice))
		{
			SetDirection(true);
			GameObject.Find("Statue").SetActive(false);
			GameObject.Find("StatueBroken").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("StatueBroken").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Find("StatueExplosion").GetComponent<ParticleSystem>().Play();
			SFXManager.Instance.PlaySound("Bang");
			ApplyTextureBlock(Surprised);
			CompleteMission(Mission.NuggetTalkToLily);
			Object.FindObjectOfType<Nugget>().ActivateMission(Mission.NuggetGetLastNugget, Room.Recess);
			Object.FindObjectOfType<Principal>().SetStatue();
		}
		else
		{
			lastSelectedOption.DestinationID = -1;
			player.ExplodePlayer("Place the device before talking to Lily.", "Coloque el dispositivo antes de hablar con Lily.");
		}
	}

	private void CallOverTeacher()
	{
		StartCoroutine(TeacherStatueBlewUp());
	}

	private IEnumerator TeacherStatueBlewUp()
	{
		Teacher t = Object.FindObjectOfType<Teacher>();
		player.ForceChangeDirection(true);
		t.SetAnimation("walk");
		t.SetDirection(false);
		t.transform.DOMoveX(5.3f, 1.5f).SetEase(Ease.Linear).SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				t.SetAnimation("idle");
			});
		StartCoroutine(RunDialogueSection("Oh my good golly gosh!~ Why the heck did you two blow up the statue of the principal!? He's going to be furious!", "¡Dios mio!~ ¿Por qué carajos volaron la estatua del director? ¡Va a estar furioso!", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(true);
		StartCoroutine(RunDialogueSection("What?! You think I did this?! It was all him! He blew up the statue!", "¡¿Qué?! ¡¿Crees que fui yo?! ¡Él fue quien lo hizo! ¡Él voló la estatua!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("You can't expect me to believe that can you, Lily? You spend all day out here staring at the silly thing and your feelings toward the principal aren't exactly...~kind.", "¿Esperas que yo te crea eso Lily? Te la pasas todo el día aquí mirando tonterías y tus sentimientos hacia el director no son exactamente... ~ buenos.", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("No! This is so unfair! I was just in the wrong place at the wrong time!", "¡No! ¡Ésto es totalmente injusto! ¡Yo solo estaba en el momento y lugar equivocado!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("I'm sorry Lily, but you're gonna have to get the principal to believe you. Go to the office.", "Perdón Lily, pero vas a tener que convencer al director para que te crea. Ve a la oficina.", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("But...~but I--", "Pero...~pero yo—", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("\\bGO!!/b", "\\b¡¡VETE!!/b", t));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetAnimation("walk");
		base.transform.DOMoveX(35f, 2f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetAnimation("idle");
		});
		Object.FindObjectOfType<Nugget>().SetCurrentConversation(30);
		t.SetCurrentConversation(17);
		t.Interact();
		yield return new WaitForSeconds(2f);
		UnlockHint("Nugget", 5);
		RemoveFromGame();
	}

	private void CompleteGumRecess()
	{
		player.UseItem(Item.Gum);
		ApplyTextureBlock(Gum);
		CompleteMission(Mission.CindyPutGumLunch);
		Object.FindObjectOfType<Cindy>().ActivateMission(Mission.CindyTellPutGumLunch);
		Object.FindObjectOfType<Cindy>().SetCurrentConversation(25);
	}

	public IEnumerator StartOffice()
	{
		player.lockActions = true;
		player.inAnim = true;
		base.transform.position = new Vector3(-20.2f, -1.4f, -0.9f);
		SetDepth();
		yield return new WaitForSeconds(0.5f);
		player.inAnim = false;
		SetDirection(true);
		Interact();
	}

	private void WalkToHatch()
	{
		GameObject.Find("HatchObject").GetComponent<BoxCollider2D>().enabled = true;
		SetDirection(true);
		SetAnimation("walk");
		player.inAnim = true;
		base.transform.DOMove(new Vector3(18.2f, -11.7f, -0.9f), 3f).SetEase(Ease.Linear).OnUpdate(base.SetDepth)
			.OnComplete(delegate
			{
				SetDepth();
				SetAnimation("idle");
				StartCoroutine(ItsAKey());
				player.inAnim = false;
			});
	}

	private void OpenHatch()
	{
		player.inAnim = true;
		player.ForceAnim(true);
		player.transform.DOMoveY(-8.75f, 1f).SetEase(Ease.Linear).OnComplete(delegate
		{
			player.inAnim = false;
			player.ForceAnim(false);
			SetCurrentConversation(2);
			Interact();
		});
	}

	private void GoDownHatch()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Lair);
	}

	private IEnumerator ItsAKey()
	{
		GameObject.Find("Rug").GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find("RugSprite").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("RugFlippedSprite").GetComponent<SpriteRenderer>().enabled = true;
		StartCoroutine(RunDialogueSection("What the--?!~ There's no place to enter the code! This lock takes a key! What are we gonna do?", "¡¿Qué demonios--?!~ ¡Aquí no hay lugar para escribir el código! ¡Esta cerradura necesita una llave! ¿Qué vamos a hacer?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
	}

	private void GetHatchKey()
	{
		player.GetItem(Item.GoldKey);
	}

	public void StartLair()
	{
		ActivateMission(Mission.LilySaveBilly);
		StartCoroutine(StartLairCoroutine());
	}

	private IEnumerator StartLairCoroutine()
	{
		StartCoroutine(RunDialogueSection("Wow...~I can't believe this is under the--~", "Wow...~No puedo creer que ésto está debajo--~", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		WalkToPoint(new Vector3(2.65f, -1.78f, -1.77f), 1.5f);
		StartCoroutine(RunDialogueSection("\\bBILLY!!!/b", "\\b¡¡¡BILLY!!!/b", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("What--~ what has he done to you?", "Que--~ ¿qué te ha hecho él?", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		SetDirection(true);
		StartCoroutine(RunDialogueSection("We have to find a way to release him! Hurry!", "¡Tenemos que encontrar la forma de liberarlo! ¡Dáte prisa!", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
	}

	public override IEnumerator EndDayCoroutine()
	{
		Billy b = Object.FindObjectOfType<Billy>();
		StartCoroutine(RunDialogueSection("I don't know if I'll ever be able to come up with the words to thank you for what you did today.", "No sé si podré encontrar las palabras para agradecerte por todo lo que has hecho hoy.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Yeah. You saved my life. There's nothing I can do that would be fitting.", "Sí. Me salvaste la vida. No hay nada que pueda hacer para compensarte ", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Well maybe there is one thing.", "Tal vez hay una cosa.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("You're right.~ So here it is.", "Tienes razón. ~ Aquí está.", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		player.GetItem(Item.BlueEyesGoldDragon);
		StartCoroutine(RunDialogueSection("This is an ultra rare Monstermon card. Our dad works at the company that makes them. This is the only one in existence.", "Esta es una tarjeta Monstermon poco común. Nuestro papá trabajaba en la fábrica que las hacía. Este es el único ejemplar.", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("You can also have this collector's guide. It tells you where you can find Monstermon cards!", "También te regalo esta guía de coleccionista, la cual te dice donde puedes encontrar tarjetas Monstermon!", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Thanks for everything you've done today.", "Gracias por todo lo que has hecho hoy.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("We should get home...~I'm sure our parents are wondering where I am right about now.", "Tenemos que irnos a casa...~Estoy segura que nuestros padres se deben estar preguntando dónde estoy en estos momentos.", b));
		while (pEmptyOptions)
		{
			yield return null;
		}
		StartCoroutine(RunDialogueSection("Goodbye.", "Adiós.", this));
		while (pEmptyOptions)
		{
			yield return null;
		}
		UI.CollapseDialogue();
		SteamScript.UnlockAchievement("LilyAchievement");
		SetDirection(false);
		b.SetDirection(false);
		WalkToPoint(new Vector3(-36f, -2f, -1.14f), 7f, false);
		b.WalkToPoint(new Vector3(-36f, -1f, -1.14f), 7f, false);
		UI.CompleteDay(Item.None);
		Object.FindObjectOfType<PauseMenu>().UnlockAllHints("Lily");
		Object.FindObjectOfType<PauseMenu>().UnlockAllHints("Monstermon");
	}

	private void StartTutorial()
	{
		Object.FindObjectOfType<UIController>().StartTutorial();
	}

	private void SetTutorial()
	{
		PlayerPrefs.SetInt("ShowTutorial", 1);
	}
}
