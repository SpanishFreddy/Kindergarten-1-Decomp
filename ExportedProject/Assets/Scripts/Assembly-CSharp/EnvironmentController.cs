using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using TextFx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnvironmentController : MonoBehaviour
{
	[Serializable]
	public class RoomScenario
	{
		public string name;

		public Room room;

		public UnityEvent startFunction;

		public UnityEvent endFunction;
	}

	public bool isSpanish;

	public Transform spanishCanvas;

	[HideInInspector]
	public bool waitForNextAction;

	public bool waitForNextTalk;

	public static EnvironmentController Instance;

	public Room startRoom;

	public static Room currentRoom;

	public Room furthestRoom;

	private Image mFaderPanel;

	private Dictionary<Room, GameObject> mRooms = new Dictionary<Room, GameObject>();

	private Dictionary<Room, int> mActions = new Dictionary<Room, int>();

	public RoomScenario[] roomList;

	private UIController mUI;

	private int mSpawnPoint;

	private NPCBehavior[] mStoredNPC = new NPCBehavior[12];

	private GameObject[] mStoreEnvironments;

	private bool mRingBell;

	private string mFileModifier = string.Empty;

	private IEnumerator endRoomCoroutine;

	private bool mWaitingForEnd;

	public string GetFileModifier()
	{
		return mFileModifier;
	}

	private void Awake()
	{
		if (PlayerPrefs.GetInt("SaveFile", 1) == 2)
		{
			mFileModifier = "2";
		}
		else
		{
			mFileModifier = string.Empty;
		}
		if (isSpanish)
		{
			UnityEngine.Object.FindObjectOfType<UIController>().gameObject.SetActive(false);
			spanishCanvas.gameObject.SetActive(true);
		}
		Instance = this;
		mUI = UnityEngine.Object.FindObjectOfType<UIController>();
	}

	private void Start()
	{
		GetRooms();
		mFaderPanel = GameObject.Find("FaderPanel").GetComponent<Image>();
		mFaderPanel.color = Color.black;
		ChangeEnvironment(startRoom);
		mUI.SetActionsLeft(mActions[furthestRoom]);
	}

	public void GetRooms()
	{
		mRooms.Clear();
		InitializeActions();
		IEnumerator enumerator = Enum.GetValues(typeof(Room)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Room key = (Room)enumerator.Current;
				if ((bool)base.transform.Find(key.ToString()))
				{
					mRooms.Add(key, base.transform.Find(key.ToString()).gameObject);
					if (mRooms[key].activeSelf)
					{
						startRoom = key;
					}
				}
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

	private void InitializeActions()
	{
		mActions.Clear();
		mActions.Add(Room.Bedroom, 3);
		mActions.Add(Room.SchoolYard, 5);
		mActions.Add(Room.Classroom1, 5);
		mActions.Add(Room.Office, 5);
		mActions.Add(Room.ClassroomLunch, 2);
		mActions.Add(Room.Cafeteria, 5);
		mActions.Add(Room.Recess, 5);
		mActions.Add(Room.Lair, 5);
		mActions.Add(Room.Classroom2, 1);
	}

	public void ChangeEnvironment(string r)
	{
		Room cr = currentRoom;
		currentRoom = (Room)Enum.Parse(typeof(Room), r);
		if (cr == Room.None)
		{
			cr = currentRoom;
		}
		PlayerController.roomTransition = true;
		mFaderPanel.DOFade(1f, 1f).OnComplete(delegate
		{
			mRooms[cr].SetActive(false);
		});
		Invoke("SwitchRoom", 1.1f);
	}

	private void ChangeEnvironment()
	{
		ChangeEnvironment(furthestRoom);
		PlayerController.roomTransition = true;
		mFaderPanel.DOFade(1f, 1f).OnComplete(SwitchRoom);
		mUI.SetActionsLeft(mActions[furthestRoom]);
	}

	public void ChangeEnvironment(Room r)
	{
		if (r != 0)
		{
			ChangeEnvironment(r.ToString());
		}
	}

	private IEnumerator ChangeEnvironmentReset(Room r)
	{
		PlayerController.roomTransition = true;
		mFaderPanel.DOFade(1f, 1f);
		currentRoom = r;
		yield return new WaitForSeconds(1f);
		yield return null;
		GetRooms();
		yield return null;
		mRooms[r].SetActive(true);
		InitializeNPCs();
		InitializePlayerPos();
		mFaderPanel.DOFade(0f, 1f);
		yield return new WaitForSeconds(0.25f);
		PlayerController.roomTransition = false;
		RoomScenario[] array = roomList;
		foreach (RoomScenario roomScenario in array)
		{
			if (furthestRoom == roomScenario.room)
			{
				StartCoroutine(InvokeStartFunction(roomScenario));
			}
		}
	}

	private void SwitchRoom()
	{
		waitForNextAction = false;
		waitForNextTalk = false;
		if (!mRooms[currentRoom])
		{
			GetRooms();
		}
		mRooms[currentRoom].SetActive(true);
		InitializeNPCs();
		if (mSpawnPoint > 0)
		{
			InitializePlayerPos(mSpawnPoint);
		}
		else
		{
			InitializePlayerPos();
		}
		mFaderPanel.DOFade(0f, 1f);
		RoomScenario[] array = roomList;
		foreach (RoomScenario roomScenario in array)
		{
			if (currentRoom == roomScenario.room)
			{
				StartCoroutine(InvokeStartFunction(roomScenario));
			}
		}
	}

	private IEnumerator InvokeStartFunction(RoomScenario rs)
	{
		yield return null;
		rs.startFunction.Invoke();
		yield return null;
		PlayerController.roomTransition = false;
	}

	private void InitializeNPCs()
	{
		NPCBehavior[] array = UnityEngine.Object.FindObjectsOfType<NPCBehavior>();
		foreach (NPCBehavior nPCBehavior in array)
		{
			if (!nPCBehavior.removedFromGame && nPCBehavior.transform.name != "Billy")
			{
				Transform transform = mRooms[currentRoom].transform.Find("NPCPositions/" + nPCBehavior.name + "Pos");
				nPCBehavior.transform.position = transform.position;
				nPCBehavior.SetDirection(transform.localScale.x < 0f);
				nPCBehavior.SetDepth();
			}
			else
			{
				nPCBehavior.transform.position = new Vector3(0f, 50f, 0f);
			}
		}
	}

	private void InitializePlayerPos()
	{
		Transform transform = mRooms[currentRoom].transform.Find("NPCPositions/PlayerPos");
		PlayerController.instance.transform.position = transform.position;
		PlayerController.instance.ForceChangeDirection(transform.localScale.x > 0f);
		PlayerController.instance.SetDepth();
		PlayerController.instance.SetInteractable(null);
	}

	private void InitializePlayerPos(int x)
	{
		Transform transform = mRooms[currentRoom].transform.Find("NPCPositions/PlayerPos" + x);
		PlayerController.instance.transform.position = transform.position;
		PlayerController.instance.ForceChangeDirection(transform.localScale.x > 0f);
		PlayerController.instance.SetDepth();
		PlayerController.instance.SetInteractable(null);
		mSpawnPoint = 0;
	}

	public void DecreaseActions()
	{
		if (mActions[furthestRoom] > 0)
		{
			if (waitForNextAction)
			{
				waitForNextAction = false;
			}
			mActions[furthestRoom]--;
			mUI.SetActionsLeft(mActions[furthestRoom]);
			if (mActions[furthestRoom] == 1 && (currentRoom == Room.Hallway || currentRoom == Room.Closet || currentRoom == Room.Bathroom))
			{
				RingBell();
				if ((currentRoom == Room.Hallway || currentRoom == Room.Bathroom) && furthestRoom == Room.Classroom1)
				{
					endRoomCoroutine = InvokeEndFunction(currentRoom);
					StartCoroutine(endRoomCoroutine);
				}
			}
			if (mActions[furthestRoom] == 0)
			{
				endRoomCoroutine = InvokeEndFunction(currentRoom);
				StartCoroutine(endRoomCoroutine);
			}
		}
		else
		{
			endRoomCoroutine = InvokeEndFunction(currentRoom);
			StartCoroutine(endRoomCoroutine);
		}
	}

	private IEnumerator InvokeEndFunction(Room r)
	{
		mWaitingForEnd = true;
		while (PlayerController.instance.inDialogue || PlayerController.instance.inAnim || mUI.IsDialogueOut() || mUI.dialogueTransitioning)
		{
			yield return null;
		}
		PlayerController.instance.inAnim = true;
		yield return new WaitForSeconds(0.25f);
		PlayerController.instance.inAnim = false;
		if (!mWaitingForEnd)
		{
			yield break;
		}
		RoomScenario[] array = roomList;
		foreach (RoomScenario roomScenario in array)
		{
			if (roomScenario.room == r)
			{
				roomScenario.endFunction.Invoke();
				break;
			}
		}
	}

	public int ActionsLeft()
	{
		return mActions[furthestRoom];
	}

	public void SetSpawnPoint(int x)
	{
		mSpawnPoint = x;
	}

	public static void ShowHoleCover(bool x)
	{
		GameObject.Find("HoleCover").GetComponent<SpriteRenderer>().enabled = x;
	}

	public void RingBell()
	{
		if (!mRingBell)
		{
			SFXManager.Instance.PlaySound("Bell");
			mRingBell = true;
		}
	}

	public void UpdateFurthestRoom()
	{
		mRingBell = false;
		if (furthestRoom != currentRoom)
		{
			mUI.CheckEnviornmentMissions();
			furthestRoom = currentRoom;
			mUI.SetActionsLeft(mActions[furthestRoom]);
			CreateRestartPoint();
		}
	}

	public void StartBedroom()
	{
		SFXManager.Instance.PlayMusic("BedroomTheme");
		mUI.SetActionsLeft(mActions[furthestRoom]);
		PlayerController.instance.lockActions = true;
		StartCoroutine(BedroomCoroutine());
		UnlockableObject[] array = UnityEngine.Object.FindObjectsOfType<UnlockableObject>();
		int num = 0;
		UnlockableObject[] array2 = array;
		foreach (UnlockableObject unlockableObject in array2)
		{
			if (unlockableObject.transform.parent.name.Contains("Cards") && unlockableObject.unlocked)
			{
				num++;
			}
		}
		PlayerPrefs.SetInt("UnlockedCards" + mFileModifier, num);
		if (num == 25)
		{
			GameObject gameObject = GameObject.Find("MonstermonComplete");
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	private IEnumerator BedroomCoroutine()
	{
		yield return new WaitForSeconds(3f);
		if (PlayerPrefs.GetInt("ShowTutorial", 0) == 0)
		{
			PlayerController.instance.SetInteractable(GameObject.Find("Tutorial").GetComponent<ObjectInteractable>());
			GameObject.Find("Tutorial").GetComponent<ObjectInteractable>().Interact();
		}
		GameObject.Find("KindergartenTitle").GetComponent<TextFxUGUI>().AnimationManager.PlayAnimation();
		yield return new WaitForSeconds(1.5f);
		SFXManager.Instance.PlaySound("Yay");
	}

	public void StartSchoolyard()
	{
		PlayerController.instance.lockActions = false;
		if (PlayerController.instance.money > 3f)
		{
			UnityEngine.Object.FindObjectOfType<Buggs>().StealMoney();
		}
	}

	public void EndSchoolYard()
	{
		UnityEngine.Object.FindObjectOfType<Teacher>().EndSchoolYard();
	}

	public void StartCS1()
	{
		PlayerController.instance.lockActions = false;
		if (furthestRoom == Room.SchoolYard)
		{
			UpdateFurthestRoom();
		}
		UnityEngine.Object.FindObjectOfType<Teacher>().StartClassroom();
		UnityEngine.Object.FindObjectOfType<Jerome>().CheckTriedToLeave();
		UnityEngine.Object.FindObjectOfType<Buggs>().CheckCallOverPlayer();
		UnityEngine.Object.FindObjectOfType<Principal>().mPunishingBuggs = false;
		if (mUI.IsMissionActive(Mission.CindyPlayHouse) || mUI.IsMissionActive(Mission.CindyWashUp))
		{
			UnityEngine.Object.FindObjectOfType<Cindy>().transform.position = new Vector3(-19.65f, -12.1f, -12.09f);
		}
	}

	public void EndCS1()
	{
		UnityEngine.Object.FindObjectOfType<Teacher>().EndClassroom1();
	}

	public void StartCafeteria()
	{
		UpdateFurthestRoom();
		UnityEngine.Object.FindObjectOfType<Principal>().mPunishingBuggs = false;
		PlayerController.instance.lockActions = false;
		UnityEngine.Object.FindObjectOfType<LunchLady>().CheckSendToRecess();
		UnityEngine.Object.FindObjectOfType<Lily>().CheckLeaveCafeteria();
		UnityEngine.Object.FindObjectOfType<Jerome>().InterceptCafeteria();
		UnityEngine.Object.FindObjectOfType<Buggs>().CheckDeadInCafeteria();
	}

	public void EndCafeteria()
	{
		UnityEngine.Object.FindObjectOfType<Nugget>().CheckPoison();
	}

	public void StartRecess()
	{
		UpdateFurthestRoom();
		PlayerController.instance.lockActions = false;
		UnityEngine.Object.FindObjectOfType<Monty>().CheckDeath();
		UnityEngine.Object.FindObjectOfType<Janitor>().CheckAppearAtRecess();
		UnityEngine.Object.FindObjectOfType<Buggs>().CheckDeath();
		UnityEngine.Object.FindObjectOfType<Teacher>().StartRecess();
		UnityEngine.Object.FindObjectOfType<Nugget>().CheckEndWorld();
	}

	public void EndRecess()
	{
		if (mUI.IsMissionComplete(Mission.BuggsKillTeacher))
		{
			UnityEngine.Object.FindObjectOfType<Janitor>().EndRecess();
		}
		else
		{
			UnityEngine.Object.FindObjectOfType<Teacher>().EndRecess();
		}
	}

	public void StartCS2()
	{
		UpdateFurthestRoom();
		UnityEngine.Object.FindObjectOfType<Teacher>().StartClassRoom2();
	}

	public void StartOffice()
	{
		PlayerController.instance.lockActions = true;
		if (furthestRoom == Room.SchoolYard)
		{
			UpdateFurthestRoom();
		}
		UnityEngine.Object.FindObjectOfType<Principal>().StartPrincipal();
	}

	public void StartHallway()
	{
		UnityEngine.Object.FindObjectOfType<HallMonitor>().StartFollowing();
		UnityEngine.Object.FindObjectOfType<Lily>().CheckHallwayLilyStatus();
		UnityEngine.Object.FindObjectOfType<Buggs>().CheckDeadInHallway();
		PlayerController.instance.lockActions = true;
		if (ActionsLeft() == 1)
		{
			RingBell();
			if (furthestRoom == Room.Classroom1 || furthestRoom == Room.Office)
			{
				UnityEngine.Object.FindObjectOfType<HallMonitor>().EndHallway();
			}
		}
		if (furthestRoom == Room.Cafeteria)
		{
			GameObject.Find("JanitorHallwayDoor").GetComponent<ObjectInteractable>().startFunction = string.Empty;
			GameObject.Find("ClassRoomDoorHallway").GetComponent<ObjectInteractable>().startFunction = string.Empty;
			GameObject.Find("CafeteriaDoorHallway").GetComponent<ObjectInteractable>().startFunction = "EnterCafeteria";
		}
	}

	public void EndHallway()
	{
		if (furthestRoom == Room.Classroom1 || furthestRoom == Room.Office)
		{
			if (currentRoom == Room.Hallway)
			{
				UnityEngine.Object.FindObjectOfType<HallMonitor>().EndHallway();
			}
		}
		else if (UnityEngine.Object.FindObjectOfType<Nugget>().GetPoisonedPlayer())
		{
			UnityEngine.Object.FindObjectOfType<Nugget>().CheckPoison();
		}
		else if (currentRoom == Room.Hallway)
		{
			UnityEngine.Object.FindObjectOfType<HallMonitor>().EndHallwayCafeteria();
		}
	}

	public void StartCloset()
	{
		Transform transform = GameObject.Find("StolenStuff").transform;
		transform.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -18.45f);
		PlayerController.instance.lockActions = true;
		if (mUI.IsMissionActive(Mission.JeromeGoToCloset))
		{
			mUI.CompleteMission(Mission.JeromeGoToCloset);
			UnityEngine.Object.FindObjectOfType<Jerome>().ActivateMission(Mission.JeromeGetLaserFromBox, Room.Classroom1);
		}
	}

	public void EndCloset()
	{
		UnityEngine.Object.FindObjectOfType<Janitor>().AppearJanitor();
	}

	public void StartCSLunch()
	{
		PlayerController.instance.lockActions = false;
		UnityEngine.Object.FindObjectOfType<Teacher>().Interact();
		UpdateFurthestRoom();
		if (mUI.IsMissionActive(Mission.BuggsGoToLunchWithTeacher))
		{
			UnityEngine.Object.FindObjectOfType<Buggs>().CompleteMission(Mission.BuggsGoToLunchWithTeacher);
			UnityEngine.Object.FindObjectOfType<Buggs>().ActivateMission(Mission.BuggsKillTeacher);
		}
	}

	public void EndCSLunch()
	{
		UnityEngine.Object.FindObjectOfType<Teacher>().EndCSLunch();
	}

	public void StartBathroom()
	{
		UnityEngine.Object.FindObjectOfType<HallMonitor>().CheckSmoking();
		PlayerController.instance.lockActions = true;
		if (mUI.IsMissionComplete(Mission.JeromeHideLaser))
		{
			GameObject gameObject = GameObject.Find("GarbageBag");
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			gameObject.GetComponent<SpriteRenderer>().enabled = true;
		}
	}

	public void EndBathroom()
	{
		if (currentRoom != Room.Bathroom)
		{
			return;
		}
		if (furthestRoom == Room.Classroom1)
		{
			UnityEngine.Object.FindObjectOfType<Janitor>().EndBathroom();
		}
		else if (!UnityEngine.Object.FindObjectOfType<Nugget>().GetPoisonedPlayer())
		{
			if (mUI.IsMissionActive(Mission.JeromeGetLaserFromBathroom) || mUI.IsMissionComplete(Mission.JeromeGetLaserFromBathroom))
			{
				UnityEngine.Object.FindObjectOfType<Janitor>().EndBathroomCafeteria();
			}
			else
			{
				UnityEngine.Object.FindObjectOfType<HallMonitor>().EndBathroom();
			}
		}
		else
		{
			UnityEngine.Object.FindObjectOfType<Nugget>().CheckPoison();
		}
	}

	public void StartNuggetCave()
	{
		PlayerController.instance.lockActions = true;
		UnityEngine.Object.FindObjectOfType<Nugget>().CheckRemoveNuggetPiles();
	}

	public void EndNuggetCave()
	{
		UnityEngine.Object.FindObjectOfType<Nugget>().EndNuggetCave();
	}

	public void EndBedroom()
	{
		PlayerController.instance.SetInteractable(GameObject.Find("GoToSchool").GetComponent<ObjectInteractable>());
		PlayerController.instance.GetInteractable().GetDialogue().GetCurrentNode()
			.DialogueText = "If I don't leave now, I'm gonna be late for school.";
		PlayerController.instance.GetInteractable().GetCurrentOptions()[0].OptionText = "(Go to school.)";
		PlayerController.instance.GetInteractable().GetCurrentOptions()[1].IsAvailable = false;
		GameObject.Find("GoToSchool").GetComponent<ObjectInteractable>().Interact();
	}

	public void StartEndDay()
	{
		PlayerController.instance.inAnim = true;
		NPCBehavior[] array = UnityEngine.Object.FindObjectsOfType<NPCBehavior>();
		foreach (NPCBehavior nPCBehavior in array)
		{
			nPCBehavior.transform.position = new Vector3(nPCBehavior.transform.position.x, nPCBehavior.transform.position.y, 6f);
		}
		StartCoroutine(EndDayCoroutine());
	}

	public void StartLair()
	{
		UpdateFurthestRoom();
		PlayerController.instance.lockActions = true;
		SFXManager.Instance.PlayMusic("LairTheme");
		GameObject.Find("Creature2").transform.DOLocalMoveY(0.55f, 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
		GameObject.Find("Creature1").transform.DOLocalMoveY(-0.55f, 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear)
			.SetDelay(0.5f);
		GameObject.Find("Creature3").transform.DOLocalMoveY(-0.55f, 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear)
			.SetDelay(1f);
		UnityEngine.Object.FindObjectOfType<Lily>().StartLair();
		UnityEngine.Object.FindObjectOfType<Billy>().ResetLair();
	}

	public void EndLair()
	{
	}

	private IEnumerator EndDayCoroutine()
	{
		NPCBehavior[] kids = new NPCBehavior[6]
		{
			UnityEngine.Object.FindObjectOfType<Jerome>(),
			UnityEngine.Object.FindObjectOfType<Cindy>(),
			UnityEngine.Object.FindObjectOfType<Monty>(),
			UnityEngine.Object.FindObjectOfType<Buggs>(),
			UnityEngine.Object.FindObjectOfType<Nugget>(),
			UnityEngine.Object.FindObjectOfType<Lily>()
		};
		PlayerController p = PlayerController.instance;
		p.inAnim = true;
		NPCBehavior endKid = null;
		bool endDayScene = false;
		if (UnityEngine.Object.FindObjectOfType<Lily>().GetEndDay())
		{
			UnityEngine.Object.FindObjectOfType<Billy>().transform.position = UnityEngine.Object.FindObjectOfType<Lily>().transform.position;
			Teacher t = UnityEngine.Object.FindObjectOfType<Teacher>();
			StartCoroutine(WalkAway(t));
			yield return new WaitForSeconds(2f);
		}
		NPCBehavior[] array = kids;
		foreach (NPCBehavior i in array)
		{
			if (!i.removedFromGame)
			{
				StartCoroutine(WalkAway(i));
				if (i.GetEndDay())
				{
					endKid = i;
					endDayScene = true;
				}
				yield return new WaitForSeconds(0.5f);
			}
		}
		if (UnityEngine.Object.FindObjectOfType<Lily>().GetEndDay())
		{
			StartCoroutine(WalkAwayBilly());
			yield return new WaitForSeconds(0.5f);
		}
		p.transform.position = new Vector3(24.2f, 1.2f, 6f);
		p.ForceAnim(true);
		p.ForceChangeDirection(false);
		p.transform.DOMove(new Vector3(19f, 0f, 6f), 1f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(1f);
		p.transform.DOMove(new Vector3(16f, UnityEngine.Random.Range(-1.5f, -0.25f), -1.14f), 0.5f).SetEase(Ease.Linear);
		yield return new WaitForSeconds(0.5f);
		if (endDayScene)
		{
			p.transform.DOMove(new Vector3(9.8f, -7.21f, -7.2f), 2.5f).SetEase(Ease.Linear);
			yield return new WaitForSeconds(1.5f);
			StartCoroutine(endKid.EndDayCoroutine());
			yield return new WaitForSeconds(1f);
			p.inAnim = false;
			p.ForceAnim(false);
		}
		else
		{
			p.transform.DOMove(new Vector3(-36f, UnityEngine.Random.Range(-2f, -1.5f), -1.14f), 7f).SetEase(Ease.Linear);
			yield return new WaitForSeconds(2f);
			mUI.CompleteDay(Item.None);
		}
	}

	private IEnumerator WalkAway(NPCBehavior j)
	{
		j.SetDirection(false);
		j.WalkToPoint(new Vector3(19f, 0f, 6f), 0.75f, false);
		yield return new WaitForSeconds(0.75f);
		j.WalkToPoint(new Vector3(16f, UnityEngine.Random.Range(-1.5f, -0.25f), -1.14f), 0.5f, false);
		yield return new WaitForSeconds(0.5f);
		if (j.GetEndDay())
		{
			j.WalkToPoint(new Vector3(6.23f, -7.21f, -7.2f), 2.5f, false);
			yield return new WaitForSeconds(2.5f);
			j.SetDirection(true);
		}
		else
		{
			j.WalkToPoint(new Vector3(-36f, UnityEngine.Random.Range(-2f, -1.5f), -1.14f), 7f, false);
		}
	}

	private IEnumerator WalkAwayBilly()
	{
		Billy i = UnityEngine.Object.FindObjectOfType<Billy>();
		i.SetDirection(false);
		i.WalkToPoint(new Vector3(19f, 0f, 6f), 0.75f, false);
		yield return new WaitForSeconds(0.75f);
		i.WalkToPoint(new Vector3(16f, UnityEngine.Random.Range(-1.5f, -0.25f), -1.14f), 0.5f, false);
		yield return new WaitForSeconds(0.5f);
		i.WalkToPoint(new Vector3(7.58f, -6.67f, -6.49f), 2.5f, false);
		yield return new WaitForSeconds(2.5f);
		i.SetDirection(true);
	}

	private void CreateRestartPoint()
	{
		PlayerController.instance.StorePlayerValues();
		StoreNpcStatus();
		mUI.StoreUI();
		StoreEnvironment();
	}

	public void RestartRoom()
	{
		mWaitingForEnd = false;
		StartCoroutine(ChangeEnvironmentReset(furthestRoom));
		StartCoroutine(RunRestartRoom());
	}

	private IEnumerator RunRestartRoom()
	{
		mUI.ResetUI();
		yield return new WaitForSeconds(1f);
		GameObject ps = GameObject.Find("PrincipalShooting");
		ps.GetComponent<MeshRenderer>().enabled = false;
		ps.GetComponent<SkeletonAnimation>().AnimationName = "idle";
		GameObject.Find("PlayerBloodStream").GetComponent<ParticleSystem>().Stop();
		SFXManager.Instance.RestartMusic();
		mUI.ForceResetUI();
		PlayerController.instance.ResetPlayerValues();
		UnityEngine.Object.FindObjectOfType<CameraFollow>().overrideCamera = false;
		RestoreEnvironments();
		RestoreNPCs();
		yield return null;
		StoreEnvironment();
	}

	private void StoreEnvironment()
	{
		mStoreEnvironments = new GameObject[base.transform.childCount];
		for (int i = 0; i < base.transform.childCount; i++)
		{
			mStoreEnvironments[i] = UnityEngine.Object.Instantiate(base.transform.GetChild(i).gameObject);
			mStoreEnvironments[i].name = base.transform.GetChild(i).name;
			mStoreEnvironments[i].SetActive(false);
		}
	}

	private void RestoreEnvironments()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				UnityEngine.Object.Destroy(transform.gameObject);
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
		GameObject[] array = mStoreEnvironments;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(true);
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = new Vector3(0f, -3.6f, 0f);
			gameObject.SetActive(false);
		}
		GetRooms();
		mUI.SetActionsLeft(mActions[furthestRoom]);
	}

	private void RestoreNPCs()
	{
		NPCBehavior[] array = UnityEngine.Object.FindObjectsOfType<NPCBehavior>();
		foreach (NPCBehavior nPCBehavior in array)
		{
			UnityEngine.Object.Destroy(nPCBehavior.gameObject);
		}
		NPCBehavior[] array2 = mStoredNPC;
		foreach (NPCBehavior nPCBehavior2 in array2)
		{
			nPCBehavior2.gameObject.SetActive(true);
			NPCBehavior nPCBehavior3 = UnityEngine.Object.Instantiate(nPCBehavior2);
			nPCBehavior3.transform.name = nPCBehavior2.transform.name;
			nPCBehavior3.ApplyTextureBlock(nPCBehavior2.currentTextureBlock);
			nPCBehavior3.CloneConversations(nPCBehavior2.conversations);
			nPCBehavior2.gameObject.SetActive(false);
		}
	}

	private void StoreNpcStatus()
	{
		NPCBehavior[] array = UnityEngine.Object.FindObjectsOfType<NPCBehavior>();
		for (int i = 0; i < mStoredNPC.Length; i++)
		{
			if ((bool)mStoredNPC[i])
			{
				UnityEngine.Object.Destroy(mStoredNPC[i].gameObject);
			}
		}
		for (int j = 0; j < mStoredNPC.Length; j++)
		{
			mStoredNPC[j] = UnityEngine.Object.Instantiate(array[j]);
			mStoredNPC[j].CloneConversations(array[j].conversations);
			mStoredNPC[j].transform.name = array[j].transform.name;
			mStoredNPC[j].ApplyTextureBlock(array[j].currentTextureBlock);
			mStoredNPC[j].gameObject.SetActive(false);
		}
	}
}
