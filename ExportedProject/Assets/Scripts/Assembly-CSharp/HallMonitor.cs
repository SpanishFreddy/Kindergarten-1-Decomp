using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HallMonitor : NPCBehavior
{
	private bool mShowedBathroomPass;

	private bool mShowedHallPass;

	private bool mIsMovingBack;

	private bool mGaveCiggs;

	private bool mDontEndCafeteria;

	private bool mShowedPass
	{
		get
		{
			return mShowedBathroomPass || mShowedHallPass;
		}
	}

	public void CheckSmoking()
	{
		if (EnvironmentController.Instance.furthestRoom == Room.Cafeteria)
		{
			Object.FindObjectOfType<Janitor>().transform.position = new Vector3(0f, 50f, 0f);
			if (!mGaveCiggs)
			{
				GameObject.Find("BathroomBoxes").SetActive(false);
				base.transform.position = new Vector3(-10f, -6.15f, -9.9f);
				SetDepth();
				UnlockHint("General", 5);
				UnlockHint("Lily", 2);
				UnlockHint("Jerome", 5);
				StartCoroutine(StartBathroom());
			}
			else
			{
				base.transform.position = new Vector3(26.1f, -4.6f, -4.7f);
			}
		}
	}

	private IEnumerator StartBathroom()
	{
		Lily i = Object.FindObjectOfType<Lily>();
		player.inAnim = true;
		if (UI.IsMissionActive(Mission.LilyMeetInBathroom))
		{
			i.transform.position = new Vector3(-18.7f, -1f, 1.3f);
			i.SetDepth();
			i.SetDirection(true);
			SetCurrentConversation(5);
			CompleteMission(Mission.LilyMeetInBathroom);
			yield return new WaitForSeconds(1f);
			player.inAnim = false;
			StartCoroutine(RunDialogueSection("What the--?!~ Woah there little hombre! You're way too young to be bringing your little girlfriend into the bathroom with you!", "¿¡Qué está pa--?!~ ¡Hey hombrecito! ¡Está demasiado joven para llevar a tu novia al baño! ", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("Gotta wait 'til at least sixth grade for that. I gotta take you both to the principal.", "Espérese por lo menos hasta el sexto grado. Voy a llevarlos a los dos al director. ", this));
			while (pEmptyOptions)
			{
				yield return null;
			}
			StartCoroutine(RunDialogueSection("No! You don't understand!~ This is so important!~ You can't stop us! There must be something we can do!", "¡No! ¡Usted no entiende!~ ¡Esto es muy importante!~ ¡No puede detenernos! ¡Hay algo que tenemos que hacer!", i));
			while (pEmptyOptions)
			{
				yield return null;
			}
			Interact();
		}
		else
		{
			yield return new WaitForSeconds(1f);
			player.inAnim = false;
			Interact();
		}
	}

	private void GiveCigs()
	{
		player.UseItem(Item.Cigarettes);
		mGaveCiggs = true;
	}

	private void WalkToStallLily()
	{
		StartCoroutine(Object.FindObjectOfType<Lily>().MakePhoneCall());
		WalkToStall();
	}

	private void WalkToStall()
	{
		SetDirection(true);
		WalkToPoint(new Vector3(26.1f, -4.6f, -4.7f), 2f);
	}

	public void EndBathroom()
	{
		SetCurrentConversation(11);
		Interact();
	}

	public void StartFollowing()
	{
		if (EnvironmentController.Instance.furthestRoom == Room.Classroom1 || EnvironmentController.Instance.furthestRoom == Room.Office)
		{
			if (mShowedBathroomPass)
			{
				base.transform.position = new Vector3(9.5f, -5f, -4.7f);
				GameObject.Find("HallMonitorWallCollider").transform.position = new Vector3(9.5f, 0f, 0f);
			}
			StartCoroutine(FollowPlayer());
		}
		else
		{
			GameObject.Find("HallMonitorWallCollider").GetComponent<BoxCollider2D>().enabled = false;
			base.transform.position = new Vector3(0f, 50f, 0f);
		}
	}

	private IEnumerator FollowPlayer()
	{
		while (EnvironmentController.currentRoom == Room.Hallway && !mShowedHallPass)
		{
			yield return null;
			if (!Mathf.Approximately(base.transform.position.y, player.transform.position.y))
			{
				SetAnimation("walk");
				base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(base.transform.position.x, player.transform.position.y, 0f), 0.5f);
				SetDepth();
			}
			else if (!mIsMovingBack)
			{
				SetAnimation("idle");
			}
		}
		SetAnimation("idle");
	}

	private void ShowBathroomPass()
	{
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			lastSelectedOption.DestinationID = 8;
			return;
		}
		mIsMovingBack = true;
		mShowedBathroomPass = true;
		SetAnimation("walk");
		SetDirection(false);
		GameObject.Find("HallMonitorWallCollider").transform.position = new Vector3(9.5f, 0f, 0f);
		base.transform.DOMoveX(9.5f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetDirection(true);
			SetAnimation("idle");
			mIsMovingBack = false;
		});
	}

	private void ShowHallPass()
	{
		if (EnvironmentController.Instance.ActionsLeft() == 1)
		{
			lastSelectedOption.DestinationID = 8;
			return;
		}
		mShowedHallPass = true;
		GameObject.Find("HallMonitorWallCollider").GetComponent<BoxCollider2D>().enabled = false;
	}

	private void UnscrewVent()
	{
		GameObject gameObject = GameObject.Find("AirVent");
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		gameObject.transform.DOLocalMoveY(2f, 0.1f);
		gameObject.transform.DOLocalMoveX(-7.6f, 0.5f).SetDelay(0.1f);
		GameObject.Find("MonstermonCardAirVent").GetComponent<BoxCollider2D>().enabled = true;
	}

	private void GetMonstermonCard()
	{
		player.SetInteractable(null);
		GameObject.Find("MonstermonCardAirVent").SetActive(false);
		player.GetItem(Item.TornadoFly);
		UnlockHint("Monstermon", 11);
	}

	public void EndHallway()
	{
		SetCurrentConversation(6);
		Interact();
	}

	public void SetEndCafeteria()
	{
		mDontEndCafeteria = true;
	}

	public void EndHallwayCafeteria()
	{
		if (!mDontEndCafeteria)
		{
			base.transform.position = new Vector3(14.12f, -0.82f, 0f);
			SetDepth();
			SetCurrentConversation(7);
			Interact();
		}
	}

	private void HeadBackToClass()
	{
		EnvironmentController.Instance.SetSpawnPoint(1);
		EnvironmentController.Instance.ChangeEnvironment(Room.Classroom1);
	}

	private void ForceToRecess()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Recess);
	}

	private void SendToPrincipalBathroom()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Office);
		Object.FindObjectOfType<Principal>().SetBathroom();
	}

	private void SendToPrincipalBathroomLily()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Office);
		Object.FindObjectOfType<Principal>().SetBathroomLily();
	}

	private void SendToPrincipal()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Office);
		Object.FindObjectOfType<Principal>().SetMonitor();
	}
}
