using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using DG.Tweening;
using DialogueTree;
using TextFx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	private const int cHighlightLength1 = 14;

	private const int cHighlightLength2 = 8;

	private const int cSizeLength1 = 9;

	private const int cSizeLength2 = 7;

	private const float cDialogueDelay = 0.65f;

	public Font crayawn;

	public Font gamer;

	public RectTransform dialoguePanel;

	public Text dialogueText;

	[HideInInspector]
	public bool dialogueTransitioning;

	public RectTransform optionsPanel;

	private bool mOptionsOut;

	[HideInInspector]
	public bool optionsTransitioning;

	private IEnumerator mDialogueCoroutine;

	private bool mDialogueRunning;

	public Image plannerBG;

	public RectTransform plannerPanel;

	public RectTransform plannerUpdatedPanel;

	public Text plannerUpdatedTitle;

	public Text plannerUpdatedText;

	public RectTransform hintUnlockedPanel;

	public Transform actionsPanel;

	public Text moneyText;

	public Image face;

	private int mOptionCount;

	[HideInInspector]
	public bool showPlanner;

	[HideInInspector]
	public int currentOption;

	private Transform[] mOptions;

	public Transform inventoryBar;

	private DialogueOption[] mCurrentOptions;

	private Dictionary<Mission, MissionUIBehavior> mMissionObjects = new Dictionary<Mission, MissionUIBehavior>();

	private Dictionary<Mission, MissionUIBehavior.MissionStatus> mStoredMissionStatus = new Dictionary<Mission, MissionUIBehavior.MissionStatus>();

	public GameObject missionPrefab;

	public GameObject deathPanel;

	public Text deathDescription;

	public GameObject DayCompletePanel;

	private NPCBehavior mLastUsedChar;

	public Sprite computerFace;

	public Sprite computerFaceMO;

	public bool showEndDay;

	public bool showDeath;

	public CanvasGroup mondayPanel;

	public Text mondayText;

	public Text mondayAgainText;

	private int mDaysComplete;

	private bool mPiggyBankOpen;

	private int mPlayerDeaths;

	public Transform TutorialPanel;

	private float mPiggyBankMoney;

	private bool mInTutorial;

	private AudioSource mDialoguePop;

	private bool mShowingPlannerUpdate;

	private bool mReturnHome;

	private bool mDialogueOut
	{
		get
		{
			return dialoguePanel.anchoredPosition.y < -80f;
		}
	}

	public bool transitioning
	{
		get
		{
			return dialogueTransitioning || optionsTransitioning;
		}
	}

	private void Start()
	{
		UpdateMoneyText(false);
		mOptions = new Transform[optionsPanel.transform.childCount];
		for (int i = 0; i < mOptions.Length; i++)
		{
			mOptions[i] = optionsPanel.transform.GetChild(i);
		}
		mDaysComplete = PlayerPrefs.GetInt("DaysComplete" + EnvironmentController.Instance.GetFileModifier(), 1);
		if (EnvironmentController.currentRoom == Room.Bedroom)
		{
			StartCoroutine(FadeMonday(mDaysComplete));
		}
		mPiggyBankMoney = PlayerPrefs.GetFloat("PiggyBank" + EnvironmentController.Instance.GetFileModifier(), 5f);
		mPlayerDeaths = PlayerPrefs.GetInt("PlayerDeaths" + EnvironmentController.Instance.GetFileModifier(), 0);
		mDialoguePop = GameObject.Find("DialoguePopQuiet3").GetComponent<AudioSource>();
	}

	public void StartTutorial()
	{
		StartCoroutine(RunTutorial());
	}

	public bool IsTutorialOpen()
	{
		return mInTutorial;
	}

	private IEnumerator RunTutorial()
	{
		mInTutorial = true;
		TutorialPanel.gameObject.SetActive(true);
		TutorialPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
		Transform[] panels = new Transform[TutorialPanel.childCount];
		for (int j = 0; j < TutorialPanel.childCount; j++)
		{
			panels[j] = TutorialPanel.GetChild(j);
		}
		yield return null;
		for (int i = 0; i < 5; i++)
		{
			if (i > 0)
			{
				panels[i - 1].DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				panels[i - 1].DOScale(0f, 0.1f).SetDelay(0.15f);
				yield return new WaitForSeconds(0.3f);
				panels[i - 1].gameObject.SetActive(false);
			}
			panels[i].gameObject.SetActive(true);
			panels[i].DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			panels[i].DOScale(1f, 0.1f).SetDelay(0.15f);
			while (!InputManager.Instance.IsInteractPressed())
			{
				yield return null;
			}
			SFXManager.Instance.PlaySound("Confirm");
		}
		panels[panels.Length - 1].DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		panels[panels.Length - 1].DOScale(0f, 0.1f).SetDelay(0.15f);
		yield return new WaitForSeconds(0.3f);
		panels[panels.Length - 1].gameObject.SetActive(false);
		TutorialPanel.GetComponent<CanvasGroup>().DOFade(0f, 0.5f);
		PlayerPrefs.SetInt("ShowTutorial", 1);
		yield return new WaitForSeconds(0.5f);
		mInTutorial = false;
		TutorialPanel.gameObject.SetActive(false);
	}

	private IEnumerator FadeMonday(int x)
	{
		PlayerController.instance.inAnim = true;
		mondayPanel.alpha = 1f;
		if (PlayerPrefs.GetInt("SetTuesday", 0) == 0)
		{
			if (x != 1)
			{
				if (x == 2)
				{
					if (EnvironmentController.Instance.isSpanish)
					{
						mondayAgainText.text = "(de nuevo)";
					}
					else
					{
						mondayAgainText.text = "(again)";
					}
				}
				else if (EnvironmentController.Instance.isSpanish)
				{
					mondayAgainText.text = "(de nuevo x " + (x - 1) + ")";
				}
				else
				{
					mondayAgainText.text = "(again x " + (x - 1) + ")";
				}
				mondayAgainText.DOFade(1f, 2f).SetEase(Ease.Linear).SetDelay(1f);
			}
			mondayText.DOFade(1f, 3f).SetEase(Ease.Linear);
			yield return new WaitForSeconds(3f);
			PlayerController.instance.inAnim = false;
			mondayPanel.DOFade(0f, 1f);
		}
		else
		{
			PlayerPrefs.SetInt("SetTuesday", 0);
			if (EnvironmentController.Instance.isSpanish)
			{
				mondayText.text = "Martes";
				mondayAgainText.text = "Estoy bromeando. Siempre es lunes.";
			}
			else
			{
				mondayText.text = "Tuesday";
				mondayAgainText.text = "Just kidding. It's Monday forever.";
			}
			mondayText.DOFade(1f, 3f).SetEase(Ease.Linear);
			yield return new WaitForSeconds(3f);
			mondayAgainText.DOFade(1f, 2f).SetEase(Ease.Linear);
			yield return new WaitForSeconds(5f);
			PlayerController.instance.inAnim = false;
			mondayPanel.DOFade(0f, 1f);
		}
	}

	private void Update()
	{
		if (EnvironmentController.currentRoom == Room.SchoolYardEndDay || PlayerController.roomTransition)
		{
			return;
		}
		if (InputManager.Instance.IsPlannerPressed() && !mDialogueRunning && !PlayerController.instance.inAnim && !transitioning && !UnityEngine.Object.FindObjectOfType<PauseMenu>().IsOpen && !showEndDay && !showDeath && !mInTutorial)
		{
			ShowPlanner();
		}
		if (!InputManager.Instance.IsPausePressed())
		{
			return;
		}
		if (!showPlanner)
		{
			if (!showEndDay && !showDeath && !mPiggyBankOpen && !mInTutorial)
			{
				UnityEngine.Object.FindObjectOfType<PauseMenu>().OpenPauseMenu();
			}
		}
		else
		{
			ShowPlanner();
		}
	}

	public bool IsDialogueOut()
	{
		return mDialogueOut;
	}

	public void StartDialogue(string dialogue)
	{
		PlayerController.instance.inDialogue = true;
		mDialogueCoroutine = DisplayDialogue(dialogue, null, false);
		StartCoroutine(mDialogueCoroutine);
	}

	public void StartDialogue(string dialogue, NPCBehavior t)
	{
		PlayerController.instance.inDialogue = true;
		if (!mDialogueRunning)
		{
			mDialogueCoroutine = DisplayDialogue(dialogue, t, false);
			StartCoroutine(mDialogueCoroutine);
		}
		else
		{
			MonoBehaviour.print("Tried to run dialogue : " + dialogue);
		}
	}

	public void StartDialogue(string dialogue, NPCBehavior t, bool cpu)
	{
		PlayerController.instance.inDialogue = true;
		if (!mDialogueRunning)
		{
			mDialogueCoroutine = DisplayDialogue(dialogue, t, cpu);
			StartCoroutine(mDialogueCoroutine);
		}
		else
		{
			MonoBehaviour.print("Tried to run dialogue : " + dialogue);
		}
	}

	private void SetDefaultFont()
	{
		dialogueText.font = crayawn;
		dialogueText.fontSize = 50;
		dialogueText.lineSpacing = 0.72f;
		dialogueText.alignByGeometry = false;
	}

	private void SetComputerFont()
	{
		dialogueText.font = gamer;
		dialogueText.fontSize = 70;
		dialogueText.lineSpacing = 0.7f;
		dialogueText.alignByGeometry = true;
	}

	private IEnumerator DisplayDialogue(string dialogue, NPCBehavior t, bool cpu)
	{
		bool open = false;
		bool hl = false;
		bool big = false;
		Sprite tFace;
		Sprite tFaceMO;
		if (t != null)
		{
			tFace = t.face;
			tFaceMO = t.faceMO;
		}
		else if (cpu)
		{
			tFace = computerFace;
			tFaceMO = computerFaceMO;
		}
		else
		{
			tFace = PlayerController.instance.face;
			tFaceMO = PlayerController.instance.faceMO;
		}
		if (!mDialogueOut)
		{
			SetFace(tFace);
			dialogueTransitioning = true;
			if ((bool)t)
			{
				dialoguePanel.GetComponent<Image>().color = t.dialogueColor;
				SetDefaultFont();
			}
			else
			{
				dialoguePanel.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 1f);
				if (cpu)
				{
					SetComputerFont();
				}
				else
				{
					SetDefaultFont();
				}
			}
			dialoguePanel.DOAnchorPosY(-86f, 0.2f);
			dialogueText.text = string.Empty;
			yield return new WaitForSeconds(0.4f);
			dialogueTransitioning = false;
			mLastUsedChar = t;
		}
		else if (mLastUsedChar != t)
		{
			if (t != null)
			{
				dialoguePanel.GetComponent<Image>().DOColor(t.dialogueColor, 0.25f);
				mLastUsedChar = t;
				face.transform.DORotate(new Vector3(0f, 90f, 0f), 0.1f);
				yield return new WaitForSeconds(0.1f);
				SetFace(tFace);
				face.transform.DORotate(Vector3.zero, 0.1f);
				yield return new WaitForSeconds(0.1f);
				SetDefaultFont();
			}
			else
			{
				dialoguePanel.GetComponent<Image>().DOColor(Color.white, 0.25f);
				mLastUsedChar = t;
				face.transform.DORotate(new Vector3(0f, 90f, 0f), 0.1f);
				yield return new WaitForSeconds(0.1f);
				SetFace(tFace);
				face.transform.DORotate(Vector3.zero, 0.1f);
				yield return new WaitForSeconds(0.1f);
				if (cpu)
				{
					SetComputerFont();
				}
				else
				{
					SetDefaultFont();
				}
			}
		}
		mDialogueRunning = true;
		dialogue = dialogue.Replace("\\h", "<color=yellow>");
		dialogue = dialogue.Replace("/h", "</color>");
		dialogue = dialogue.Replace("\\b", "<size=90>");
		dialogue = dialogue.Replace("/b", "</size>");
		StartCoroutine(InterruptDialogue(dialogue, tFace));
		for (int i = 0; i < dialogue.Length; i++)
		{
			if (dialogue[i] == '~')
			{
				SetFace(tFace);
				face.transform.DORotate(Vector3.zero, 0.1f);
				yield return new WaitForSeconds(0.65f);
				dialogue = dialogue.Remove(i, 1);
				i--;
				continue;
			}
			if (dialogue[i] == '<')
			{
				if (!big && !hl)
				{
					if (dialogue[i + 1] == 'c')
					{
						hl = !hl;
						i += 14;
					}
					else if (dialogue[i + 1] == 's')
					{
						big = !big;
						i += 9;
					}
					continue;
				}
				if (big)
				{
					i += 7;
					big = !big;
				}
				if (hl)
				{
					i += 8;
					hl = !hl;
				}
				continue;
			}
			dialogueText.text = dialogue.Substring(0, i + 1);
			if (hl)
			{
				dialogueText.text += "</color>";
			}
			if (big)
			{
				dialogueText.text += "</size>";
			}
			if (!mDialoguePop.isPlaying)
			{
				SFXManager.Instance.PlaySound("DialoguePopQuiet3");
			}
			if (i % 3 == 0)
			{
				open = !open;
				if (!open)
				{
					if (t != null)
					{
						SetFace(t.face);
					}
					else
					{
						SetFace(tFace);
					}
					face.transform.DORotate(new Vector3(0f, 0f, 3f), 0.1f);
				}
				else
				{
					if (t != null)
					{
						SetFace(t.faceMO);
					}
					else
					{
						SetFace(tFaceMO);
					}
					face.transform.DORotate(new Vector3(0f, 0f, 0f), 0.1f);
				}
			}
			yield return new WaitForSeconds(0.025f);
		}
		SetFace(tFace);
		face.transform.DORotate(Vector3.zero, 0.1f);
		mDialogueRunning = false;
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(ShowOptions(true));
	}

	private IEnumerator InterruptDialogue(string d, Sprite t)
	{
		while (mDialogueRunning)
		{
			yield return null;
			if (InputManager.Instance.IsInteractPressed() && !PlayerController.instance.inAnim)
			{
				StopCoroutine(mDialogueCoroutine);
				mDialogueRunning = false;
				d = d.Replace("~", string.Empty);
				dialogueText.text = d;
				SetFace(t);
				face.transform.DORotate(Vector3.zero, 0.1f);
				yield return new WaitForSeconds(0.5f);
				StartCoroutine(ShowOptions(true));
			}
		}
	}

	public void SetFace(Sprite s)
	{
		face.sprite = s;
	}

	public IEnumerator ShowOptions(bool show)
	{
		if (mOptionCount == 0)
		{
			PlayerController.instance.optionsMode = false;
			optionsPanel.DOAnchorPosY(10f, 0.1f);
			optionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
			yield break;
		}
		mOptionsOut = show;
		yield return new WaitForEndOfFrame();
		if (show)
		{
			optionsTransitioning = true;
			optionsPanel.DOAnchorPosY(0f - optionsPanel.rect.height - 10f, 0.5f).SetEase(Ease.OutExpo);
			SetOption();
			yield return new WaitForSeconds(0.5f);
			optionsTransitioning = false;
			PlayerController.instance.optionsMode = true;
			PlayerController.instance.inAnim = false;
			optionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
			yield break;
		}
		optionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		PlayerController.instance.optionsMode = false;
		optionsTransitioning = true;
		optionsPanel.DOAnchorPosY(10f, 0.5f);
		if (!PlayerController.instance.inAnim)
		{
			SFXManager.Instance.PlaySound("Confirm");
		}
		yield return new WaitForSeconds(0.5f);
		optionsTransitioning = false;
	}

	public void UpdateOptionsPanel(DialogueOption[] options)
	{
		mCurrentOptions = options;
		currentOption = 0;
		mOptionCount = options.Length;
		for (int i = 0; i < mOptions.Length; i++)
		{
			if (i < options.Length && IsOptionAvailable(options[i]))
			{
				mOptions[i].gameObject.SetActive(true);
				if (options[i].ItemToUse != 0)
				{
					mOptions[i].GetChild(1).GetComponent<Image>().enabled = true;
					mOptions[i].GetChild(1).GetComponent<Image>().sprite = Resources.Load("Items/" + options[i].ItemToUse, typeof(Sprite)) as Sprite;
				}
				else if (options[i].MoneyLock > 0f)
				{
					mOptions[i].GetChild(1).GetComponent<Image>().enabled = true;
					mOptions[i].GetChild(1).GetComponent<Image>().sprite = Resources.Load("Items/Money", typeof(Sprite)) as Sprite;
					if (!options[i].OptionText.Contains("$"))
					{
						DialogueOption obj = options[i];
						obj.OptionText = obj.OptionText + " (-" + options[i].MoneyLock.ToString("c2") + ")";
					}
				}
				else
				{
					mOptions[i].GetChild(1).GetComponent<Image>().enabled = false;
				}
				mOptions[i].GetChild(2).GetComponent<Text>().text = options[i].OptionText;
				mOptions[i].GetChild(3).GetComponent<Image>().enabled = options[i].UsesAction;
				mOptions[i].GetComponent<Image>().color = Color.white;
				if (PlayerController.instance.money < options[i].MoneyLock)
				{
					mOptions[i].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
				}
			}
			else
			{
				mOptions[i].gameObject.SetActive(false);
			}
		}
	}

	private bool IsOptionAvailable(DialogueOption o)
	{
		return !o.isComplete && o.IsAvailable && PlayerController.instance.HasItem(o.ItemToUse) && IsMissionActiveForOptions(o.MissionLock);
	}

	public void SetOption(int x)
	{
		if (x == currentOption || x < 0 || x >= optionsPanel.childCount)
		{
			return;
		}
		if (!mOptions[x].gameObject.activeSelf)
		{
			if (x >= currentOption)
			{
				while (!mOptions[x].gameObject.activeSelf)
				{
					x++;
					if (x >= optionsPanel.childCount)
					{
						return;
					}
				}
			}
			else
			{
				while (!mOptions[x].gameObject.activeSelf)
				{
					x--;
					if (x < 0)
					{
						return;
					}
				}
			}
		}
		if ((mCurrentOptions[currentOption].UsesAction && EnvironmentController.Instance.ActionsLeft() <= 0) || mCurrentOptions[currentOption].MoneyLock > PlayerController.instance.money)
		{
			mOptions[currentOption].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
		}
		else
		{
			mOptions[currentOption].GetComponent<Image>().color = Color.white;
		}
		currentOption = x;
		mOptions[currentOption].GetComponent<Image>().color = Color.yellow;
		if (mCurrentOptions[currentOption].MoneyLock > PlayerController.instance.money)
		{
			mOptions[currentOption].GetComponent<Image>().color = new Color(1f, 1f, 0f, 0.5f);
		}
		RectTransform component = mOptions[currentOption].GetComponent<RectTransform>();
		component.DOKill();
		component.localScale = Vector2.one;
		component.DOScale(1.1f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
		SFXManager.Instance.PlaySound("DialoguePopQuiet");
	}

	public void ShakeOption()
	{
		SFXManager.Instance.PlaySound("Failed");
		mOptions[currentOption].DOShakePosition(0.5f, 12f, 20, 10f).OnComplete(delegate
		{
			PlayerController.instance.optionsMode = true;
		});
	}

	public void SetOption()
	{
		int num = 0;
		if (!mOptions[num].gameObject.activeSelf)
		{
			while (!mOptions[num].gameObject.activeSelf)
			{
				num++;
				if (num >= optionsPanel.childCount)
				{
					return;
				}
			}
		}
		currentOption = num;
		mOptions[currentOption].GetComponent<Image>().color = Color.yellow;
		if (mCurrentOptions[currentOption].MoneyLock > PlayerController.instance.money)
		{
			mOptions[currentOption].GetComponent<Image>().color = new Color(1f, 1f, 0f, 0.5f);
		}
	}

	public void CollapseDialogue()
	{
		StartCoroutine(CloseDialogue());
		mLastUsedChar = null;
		if (!mDialogueRunning)
		{
			EnvironmentController.Instance.waitForNextTalk = false;
			PlayerController.instance.inDialogue = false;
		}
	}

	private IEnumerator CloseDialogue()
	{
		if (mOptionsOut)
		{
			optionsTransitioning = true;
			optionsPanel.DOAnchorPosY(10f, 0.25f);
			yield return new WaitForSeconds(0.25f);
			optionsTransitioning = false;
			mOptionsOut = false;
		}
		if (mDialogueOut && !mDialogueRunning)
		{
			dialogueTransitioning = true;
			yield return new WaitForSeconds(0.25f);
			dialoguePanel.DOAnchorPosY(86f, 0.25f);
			yield return new WaitForSeconds(0.25f);
			dialogueTransitioning = false;
		}
	}

	public void AddItem(int x, Item y)
	{
		SFXManager.Instance.PlaySound("GetItem");
		inventoryBar.GetChild(x).gameObject.SetActive(true);
		inventoryBar.GetChild(x).GetChild(0).GetComponent<Image>()
			.sprite = Resources.Load("Items/" + y, typeof(Sprite)) as Sprite;
		LayoutElement component = inventoryBar.GetChild(x).GetComponent<LayoutElement>();
		component.preferredWidth = 0f;
		component.preferredHeight = 0f;
		component.DOPreferredSize(new Vector2(77f, 77f), 0.35f).SetEase(Ease.InOutSine);
		inventoryBar.GetChild(x).DOScale(1.4f, 0.1f).SetLoops(2, LoopType.Yoyo)
			.SetDelay(0.2f);
	}

	public void HighlightItem(int newItem, int prevItem)
	{
		SFXManager.Instance.PlaySound("DialoguePop");
		inventoryBar.GetChild(prevItem).GetComponent<RectTransform>().DOKill();
		inventoryBar.GetChild(prevItem).GetComponent<RectTransform>().localScale = Vector3.one;
		inventoryBar.GetChild(newItem).GetComponent<RectTransform>().localScale = Vector3.one;
		inventoryBar.GetChild(prevItem).GetComponent<Image>().color = new Color(1f, 1f, 0f, 0.517f);
		inventoryBar.GetChild(newItem).GetComponent<RectTransform>().DOScale(1.2f, 0.1f)
			.SetLoops(2, LoopType.Yoyo);
		inventoryBar.GetChild(newItem).GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.517f);
	}

	public void UnHighlightAllItems()
	{
		IEnumerator enumerator = inventoryBar.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				transform.GetComponent<Image>().color = new Color(1f, 1f, 0f, 0.517f);
				transform.GetComponent<RectTransform>().localScale = Vector3.one;
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

	public void ShowShowAndTellPanel(bool x)
	{
		CanvasGroup component = GameObject.Find("ShowAndTellPanel").GetComponent<CanvasGroup>();
		if (x)
		{
			component.DOFade(1f, 0.5f);
			return;
		}
		component.DOFade(0f, 0.5f);
		UnHighlightAllItems();
	}

	public void RemoveItem(int x)
	{
		SFXManager.Instance.PlaySound("UseItem");
		LayoutElement i = inventoryBar.GetChild(x).GetComponent<LayoutElement>();
		i.DOPreferredSize(new Vector2(0f, 0f), 0.35f).SetEase(Ease.InOutSine).OnComplete(delegate
		{
			i.transform.SetAsLastSibling();
			i.gameObject.SetActive(false);
		});
	}

	public void ValidateInventory(List<Item> inventory)
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < inventory.Count; i++)
		{
			inventoryBar.GetChild(i).gameObject.SetActive(true);
			inventoryBar.GetChild(i).GetChild(0).GetComponent<Image>()
				.sprite = Resources.Load("Items/" + inventory[i], typeof(Sprite)) as Sprite;
			LayoutElement component = inventoryBar.GetChild(i).GetComponent<LayoutElement>();
			component.preferredWidth = 77f;
			component.preferredHeight = 77f;
		}
		for (int j = inventory.Count; j < inventoryBar.childCount; j++)
		{
			LayoutElement component2 = inventoryBar.GetChild(j).GetComponent<LayoutElement>();
			component2.preferredHeight = 0f;
			component2.preferredWidth = 0f;
			component2.gameObject.SetActive(false);
			list.Add(component2.transform);
		}
		foreach (Transform item in list)
		{
			item.SetAsLastSibling();
		}
	}

	public void UpdateMoneyText(bool playSound)
	{
		moneyText.text = PlayerController.instance.money.ToString("c2");
		if (playSound)
		{
			moneyText.rectTransform.DOKill();
			moneyText.rectTransform.localScale = Vector3.one;
			moneyText.rectTransform.anchoredPosition = new Vector2(105.7f, 0f);
			moneyText.rectTransform.DOScaleY(2f, 0.1f).SetEase(Ease.InOutBounce).SetLoops(2, LoopType.Yoyo);
			moneyText.rectTransform.DOLocalMoveY(20f, 0.1f).SetLoops(2, LoopType.Yoyo);
			SFXManager.Instance.PlaySound("ChaChing");
		}
	}

	public bool IsDialogueRunning()
	{
		return mDialogueRunning;
	}

	public void ShowPlanner()
	{
		showPlanner = !showPlanner;
		if (showPlanner)
		{
			SFXManager.Instance.PlaySound("DialoguePopQuiet");
			plannerBG.DOFade(0.5f, 0.25f);
			plannerPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			plannerPanel.DOScale(1f, 0.1f).SetDelay(0.15f);
			StartCoroutine(ControlPlanner());
		}
		else
		{
			SFXManager.Instance.PlaySound("Confirm");
			plannerBG.DOFade(0f, 0.25f);
			plannerPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			plannerPanel.DOScale(0f, 0.15f).SetDelay(0.15f);
		}
	}

	private IEnumerator ControlPlanner()
	{
		RectTransform r = GameObject.Find("MissionScrollRect").GetComponent<RectTransform>();
		int i = 0;
		int childcount = r.transform.childCount;
		r.DOAnchorPosY(0f, 0.1f);
		yield return new WaitForSeconds(0.1f);
		if (childcount <= 4)
		{
			yield break;
		}
		while (showPlanner)
		{
			float x = Input.GetAxisRaw("Vertical");
			if (x != 0f)
			{
				if (x > 0f && i < childcount - 5)
				{
					i++;
					SFXManager.Instance.PlaySound("DialoguePop");
				}
				if (x < 0f && i > 0)
				{
					i--;
					SFXManager.Instance.PlaySound("DialoguePop");
				}
				yield return new WaitForSeconds(0.1f);
				r.DOAnchorPosY(i * 130, 0.1f);
			}
			yield return null;
		}
	}

	public void AddMission(Mission m, NPCBehavior n, Room r)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(missionPrefab, plannerPanel.GetChild(2).GetChild(0));
		gameObject.transform.SetAsFirstSibling();
		mMissionObjects.Add(m, gameObject.GetComponent<MissionUIBehavior>());
		if (!EnvironmentController.Instance.isSpanish)
		{
			mMissionObjects[m].text.text = GetMissionDescription(m);
		}
		else
		{
			mMissionObjects[m].text.text = GetMissionDescriptionSpanish((MissionSpanish)m);
		}
		mMissionObjects[m].SetColor(n.dialogueColor);
		mMissionObjects[m].SetFace(n.defaultTextureBlock.face);
		mMissionObjects[m].environmentLock = r;
		mMissionObjects[m].mission = m;
		gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
		StartCoroutine(ShowUpdatePlanner(m, 0));
	}

	public void CompleteMission(Mission m)
	{
		if (mMissionObjects.ContainsKey(m))
		{
			mMissionObjects[m].transform.SetAsLastSibling();
			mMissionObjects[m].SetComplete();
			StartCoroutine(ShowUpdatePlanner(m, 1));
		}
	}

	public void FailMission(Mission m)
	{
		if (mMissionObjects.ContainsKey(m) && mMissionObjects[m].status == MissionUIBehavior.MissionStatus.Active)
		{
			mMissionObjects[m].transform.SetAsLastSibling();
			mMissionObjects[m].SetFailed();
			StartCoroutine(ShowUpdatePlanner(m, 2));
		}
	}

	public void CheckEnviornmentMissions()
	{
		foreach (MissionUIBehavior value in mMissionObjects.Values)
		{
			if (value.environmentLock == EnvironmentController.Instance.furthestRoom && value.status == MissionUIBehavior.MissionStatus.Active)
			{
				FailMission(value.mission);
			}
		}
	}

	private bool IsMissionActiveForOptions(Mission m)
	{
		if (m == Mission.None)
		{
			return true;
		}
		if (mMissionObjects.ContainsKey(m))
		{
			return mMissionObjects[m].status == MissionUIBehavior.MissionStatus.Active;
		}
		return false;
	}

	public bool ReceivedMission(Mission m)
	{
		return mMissionObjects.ContainsKey(m);
	}

	public bool IsMissionActive(Mission m)
	{
		if (mMissionObjects.ContainsKey(m))
		{
			return mMissionObjects[m].status == MissionUIBehavior.MissionStatus.Active;
		}
		return false;
	}

	public bool IsMissionComplete(Mission m)
	{
		if (mMissionObjects.ContainsKey(m))
		{
			return mMissionObjects[m].status == MissionUIBehavior.MissionStatus.Complete;
		}
		return false;
	}

	public bool IsMissionFailed(Mission m)
	{
		if (mMissionObjects.ContainsKey(m))
		{
			return mMissionObjects[m].status == MissionUIBehavior.MissionStatus.Failed;
		}
		return false;
	}

	private IEnumerator ShowUpdatePlanner(Mission m, int state)
	{
		while (mShowingPlannerUpdate)
		{
			yield return null;
		}
		if (!EnvironmentController.Instance.isSpanish)
		{
			switch (state)
			{
			case 0:
				plannerUpdatedTitle.text = "Planner Updated";
				plannerUpdatedTitle.color = Color.yellow;
				break;
			case 1:
				plannerUpdatedTitle.text = "Assignment Complete";
				plannerUpdatedTitle.color = Color.green;
				break;
			case 2:
				plannerUpdatedTitle.text = "Assignment Failed";
				plannerUpdatedTitle.color = Color.red;
				break;
			}
		}
		else
		{
			switch (state)
			{
			case 0:
				plannerUpdatedTitle.text = "Planificador Actualizado";
				plannerUpdatedTitle.color = Color.yellow;
				break;
			case 1:
				plannerUpdatedTitle.text = "Misión Completa";
				plannerUpdatedTitle.color = Color.green;
				break;
			case 2:
				plannerUpdatedTitle.text = "Misión Fallido";
				plannerUpdatedTitle.color = Color.red;
				break;
			}
		}
		if (!EnvironmentController.Instance.isSpanish)
		{
			plannerUpdatedText.text = GetMissionDescription(m);
		}
		else
		{
			plannerUpdatedText.text = GetMissionDescriptionSpanish((MissionSpanish)m);
		}
		mShowingPlannerUpdate = true;
		plannerUpdatedPanel.DOAnchorPosX(285f, 0.5f).SetEase(Ease.InOutSine);
		yield return new WaitForSeconds(1.5f);
		plannerUpdatedPanel.DOAnchorPosX(-300f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		mShowingPlannerUpdate = false;
	}

	public IEnumerator ShowUnlockHint()
	{
		while (mShowingPlannerUpdate)
		{
			yield return null;
		}
		mShowingPlannerUpdate = true;
		hintUnlockedPanel.DOAnchorPosX(285f, 0.5f).SetEase(Ease.InOutSine);
		yield return new WaitForSeconds(1.5f);
		hintUnlockedPanel.DOAnchorPosX(-300f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		mShowingPlannerUpdate = false;
	}

	public void SetActionsLeft(int x)
	{
		for (int i = 0; i < 6; i++)
		{
			if (i < 6 - x)
			{
				actionsPanel.GetChild(i).DOScale(Vector3.zero, 0.5f);
			}
			else
			{
				actionsPanel.GetChild(i).localScale = Vector3.one;
			}
		}
	}

	public string GetMissionDescription(Mission m)
	{
		MemberInfo[] member = typeof(Mission).GetMember(m.ToString());
		DescriptionAttribute[] array = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
		if (array.Length > 0)
		{
			return array[0].Description;
		}
		return "Description not found";
	}

	public string GetMissionDescriptionSpanish(MissionSpanish ms)
	{
		MemberInfo[] member = typeof(MissionSpanish).GetMember(ms.ToString());
		DescriptionAttribute[] array = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
		if (array.Length > 0)
		{
			return array[0].Description;
		}
		return "Description not found";
	}

	public string GetMonstermonName(MonsterMon m)
	{
		MemberInfo[] member = typeof(MonsterMon).GetMember(m.ToString());
		DescriptionAttribute[] array = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
		if (array.Length > 0)
		{
			return array[0].Description;
		}
		return "Description not found";
	}

	public void ShowDeath(string s, string span)
	{
		if (!EnvironmentController.Instance.isSpanish)
		{
			if (s == "Don't blend Billy.")
			{
				GameObject.Find("DeathTitleText").GetComponent<Text>().text = "Billy died.";
			}
			else
			{
				GameObject.Find("DeathTitleText").GetComponent<Text>().text = "You died.";
				mPlayerDeaths++;
				PlayerPrefs.SetInt("PlayerDeaths" + EnvironmentController.Instance.GetFileModifier(), mPlayerDeaths);
			}
		}
		else if (s == "No mates a Billy.")
		{
			GameObject.Find("DeathTitleText").GetComponent<Text>().text = "Billy muerto.";
		}
		else
		{
			GameObject.Find("DeathTitleText").GetComponent<Text>().text = "Estás muerto.";
			mPlayerDeaths++;
			PlayerPrefs.SetInt("PlayerDeaths" + EnvironmentController.Instance.GetFileModifier(), mPlayerDeaths);
		}
		SFXManager.Instance.StopMusic();
		deathPanel.SetActive(true);
		deathPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.5f).OnComplete(delegate
		{
			SFXManager.Instance.PlaySound("DeathTone");
		});
		TextFxUGUI component = GameObject.Find("DeathTitleText").GetComponent<TextFxUGUI>();
		component.AnimationManager.PlayAnimation();
		if (!EnvironmentController.Instance.isSpanish)
		{
			deathDescription.text = s;
		}
		else
		{
			deathDescription.text = span;
		}
		deathDescription.DOColor(new Color(0.34f, 0f, 0f, 1f), 0.5f).SetDelay(1f);
		StartCoroutine(WaitForDeathOption());
	}

	public void SetDeathOption(bool x)
	{
		if (x != mReturnHome)
		{
			RectTransform component = GameObject.Find("RestartRoomButton").GetComponent<RectTransform>();
			RectTransform component2 = GameObject.Find("RestartDayButton").GetComponent<RectTransform>();
			Image component3 = component.GetComponent<Image>();
			Image component4 = component2.GetComponent<Image>();
			if (x)
			{
				mReturnHome = true;
				component2.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
				component3.color = new Color(0f, 0f, 0f, 0f);
				component4.color = new Color(1f, 1f, 0f, 0.55f);
				SFXManager.Instance.PlaySound("DialoguePop");
			}
			else
			{
				mReturnHome = false;
				component.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
				component4.color = new Color(0f, 0f, 0f, 0f);
				component3.color = new Color(1f, 1f, 0f, 0.55f);
				SFXManager.Instance.PlaySound("DialoguePop");
			}
		}
	}

	private IEnumerator WaitForDeathOption()
	{
		RectTransform rr = GameObject.Find("RestartRoomButton").GetComponent<RectTransform>();
		RectTransform rd = GameObject.Find("RestartDayButton").GetComponent<RectTransform>();
		rr.localScale = Vector3.zero;
		rd.localScale = Vector3.zero;
		showDeath = true;
		yield return new WaitForSeconds(3f);
		deathPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		mReturnHome = false;
		Image rri = rr.GetComponent<Image>();
		Image rdi = rd.GetComponent<Image>();
		rr.DOScale(1f, 0.3f);
		rd.DOScale(1f, 0.3f).SetDelay(0.3f);
		SFXManager.Instance.PlaySound("DialoguePop");
		while (!InputManager.Instance.IsInteractPressed())
		{
			yield return null;
			if (mReturnHome)
			{
				if (Input.GetAxis("Vertical") > 0f)
				{
					mReturnHome = false;
					rr.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
					rdi.color = new Color(0f, 0f, 0f, 0f);
					rri.color = new Color(1f, 1f, 0f, 0.55f);
					SFXManager.Instance.PlaySound("DialoguePop");
				}
			}
			else if (Input.GetAxis("Vertical") < 0f)
			{
				mReturnHome = true;
				rd.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
				rri.color = new Color(0f, 0f, 0f, 0f);
				rdi.color = new Color(1f, 1f, 0f, 0.55f);
				SFXManager.Instance.PlaySound("DialoguePop");
			}
		}
		SFXManager.Instance.PlaySound("Confirm");
		rdi.color = new Color(0f, 0f, 0f, 0f);
		rri.color = new Color(1f, 1f, 0f, 0.55f);
		if (mReturnHome)
		{
			GameObject.Find("FaderPanel").GetComponent<Image>().DOFade(1f, 0.25f);
			yield return new WaitForSeconds(0.25f);
			SceneManager.LoadScene("Kindergarten");
		}
		else
		{
			EnvironmentController.Instance.RestartRoom();
			yield return new WaitForSeconds(0.5f);
		}
		deathPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		showDeath = false;
	}

	public void CompleteDay(Item i)
	{
		mDaysComplete++;
		PlayerPrefs.SetInt("DaysComplete" + EnvironmentController.Instance.GetFileModifier(), mDaysComplete);
		SFXManager.Instance.PlayMusic("EndDayTheme");
		DayCompletePanel.SetActive(true);
		StartCoroutine(CompleteDayCoroutine(i));
		PlayerController.instance.inAnim = true;
	}

	private IEnumerator CompleteDayCoroutine(Item i)
	{
		string d = "Unused name";
		Transform contButton = GameObject.Find("DayCompleteContinueButton").transform;
		DayCompletePanel.GetComponent<Image>().DOFade(0.8f, 0.5f);
		DayCompletePanel.transform.GetChild(0).DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		DayCompletePanel.transform.GetChild(0).DOScale(1f, 0.1f).SetDelay(0.15f);
		showEndDay = true;
		if (i != 0)
		{
			PlayerPrefs.SetInt("Unlock" + i.ToString() + EnvironmentController.Instance.GetFileModifier(), 1);
			switch (i)
			{
			case Item.BillyNote:
				d = "Billy's Note";
				break;
			case Item.KeyMold:
				d = "Key mold";
				break;
			case Item.LunchPass:
				d = "Lunch Pass";
				break;
			case Item.Phone:
				d = "Teacher's Phone";
				break;
			case Item.PrincipalKey:
				d = "Key to the Principal's Office";
				break;
			case Item.Flower:
				d = "Cindy's Flower";
				break;
			case Item.None:
				d = "Nothing";
				break;
			}
			GameObject.Find("DayCompleteItemIcon").GetComponent<Image>().sprite = Resources.Load("Items/" + i, typeof(Sprite)) as Sprite;
			GameObject.Find("UnlockedItemName").GetComponent<Text>().text = d + "!";
			yield return new WaitForSeconds(2f);
			SFXManager.Instance.PlaySound("Yay");
			Transform dcip = GameObject.Find("DayCompleteItemPanel").transform;
			dcip.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			dcip.DOScale(1f, 0.1f).SetDelay(0.15f);
			yield return new WaitForSeconds(1f);
			contButton.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			contButton.DOScale(1f, 0.1f).SetDelay(0.15f);
			SFXManager.Instance.PlaySound("DialoguePopQuiet");
			while (!InputManager.Instance.IsInteractPressed())
			{
				yield return null;
			}
			SFXManager.Instance.PlaySound("Confirm");
			dcip.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			dcip.DOScale(0f, 0.1f).SetDelay(0.15f);
			contButton.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			contButton.DOScale(0f, 0.1f).SetDelay(0.15f);
		}
		yield return new WaitForSeconds(1f);
		List<Item> mm = PlayerController.instance.MonstermonList();
		if (mm.Count > 0)
		{
			Transform monstermonPanel = GameObject.Find("MonstermonPanel").transform;
			monstermonPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			monstermonPanel.DOScale(1f, 0.1f).SetDelay(0.15f);
			yield return new WaitForSeconds(1f);
			int updateSteamCount = 0;
			int max = Mathf.Min(mm.Count, 5);
			for (int j = 0; j < max; j++)
			{
				Transform c = monstermonPanel.GetChild(j);
				c.gameObject.SetActive(true);
				c.transform.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				c.transform.DOScale(1f, 0.1f).SetDelay(0.15f);
				c.GetComponent<LayoutElement>().ignoreLayout = false;
				SFXManager.Instance.PlaySound("GetItem");
				c.GetChild(1).GetComponent<Image>().sprite = Resources.Load("Items/" + mm[j], typeof(Sprite)) as Sprite;
				c.GetChild(2).GetComponent<Text>().text = GetMonstermonName((MonsterMon)Enum.Parse(typeof(MonsterMon), mm[j].ToString()));
				if (PlayerPrefs.GetInt("Unlock" + mm[j].ToString() + EnvironmentController.Instance.GetFileModifier(), 0) == 0)
				{
					c.GetChild(4).GetComponent<Text>().text = "NEW!";
					PlayerPrefs.SetInt("Unlock" + mm[j].ToString() + EnvironmentController.Instance.GetFileModifier(), 1);
					updateSteamCount++;
				}
				yield return new WaitForSeconds(0.25f);
			}
			if (mm.Count > 5)
			{
				for (int k = 5; k < mm.Count; k++)
				{
					if (PlayerPrefs.GetInt("Unlock" + mm[k].ToString() + EnvironmentController.Instance.GetFileModifier(), 0) == 0)
					{
						PlayerPrefs.SetInt("Unlock" + mm[k].ToString() + EnvironmentController.Instance.GetFileModifier(), 1);
						updateSteamCount++;
					}
				}
			}
			yield return new WaitForSeconds(1f);
			contButton.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			contButton.DOScale(1f, 0.1f).SetDelay(0.15f);
			while (!InputManager.Instance.IsInteractPressed())
			{
				yield return null;
			}
			SFXManager.Instance.PlaySound("Confirm");
			monstermonPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			monstermonPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
			contButton.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			contButton.DOScale(0f, 0.1f).SetDelay(0.15f);
		}
		Transform moneyPanel = GameObject.Find("MoneyEarnedPanel").transform;
		Text me = GameObject.Find("MoneyEarnedAmount").GetComponent<Text>();
		me.text = PlayerController.instance.money.ToString("c2");
		Text pb = GameObject.Find("PiggyBankAmount").GetComponent<Text>();
		pb.text = mPiggyBankMoney.ToString("c2");
		yield return new WaitForSeconds(0.5f);
		moneyPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		moneyPanel.DOScale(1f, 0.1f).SetDelay(0.15f);
		yield return new WaitForSeconds(1.5f);
		me.text = 0.ToString("c2");
		float total = Mathf.Min(PlayerController.instance.money + mPiggyBankMoney, 10f);
		pb.text = total.ToString("c2");
		me.rectTransform.DOScaleY(2f, 0.1f).SetEase(Ease.InOutBounce).SetLoops(2, LoopType.Yoyo);
		me.rectTransform.DOLocalMoveY(20f, 0.1f).SetLoops(2, LoopType.Yoyo);
		pb.rectTransform.DOScaleY(2f, 0.1f).SetEase(Ease.InOutBounce).SetLoops(2, LoopType.Yoyo);
		pb.rectTransform.DOLocalMoveY(20f, 0.1f).SetLoops(2, LoopType.Yoyo);
		SFXManager.Instance.PlaySound("ChaChing");
		yield return new WaitForSeconds(1f);
		contButton.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		contButton.DOScale(1f, 0.1f).SetDelay(0.15f);
		SFXManager.Instance.PlaySound("DialoguePopQuiet");
		while (!InputManager.Instance.IsInteractPressed())
		{
			yield return null;
		}
		SFXManager.Instance.PlaySound("Confirm");
		GameObject.Find("FaderPanel").GetComponent<Image>().DOFade(1f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		PlayerPrefs.SetFloat("PiggyBank" + EnvironmentController.Instance.GetFileModifier(), total);
		SceneManager.LoadScene("Kindergarten");
	}

	public void OpenPiggyBank()
	{
		PlayerController.instance.inAnim = true;
		StartCoroutine(RunPiggyBank());
		GameObject.Find("PiggyBankPanel").GetComponent<CanvasGroup>().DOFade(1f, 0.25f);
	}

	private IEnumerator RunPiggyBank()
	{
		float startingPlayerMoney = PlayerController.instance.money;
		Text piggyBankText = GameObject.Find("PiggyBankMoney").GetComponent<Text>();
		piggyBankText.text = mPiggyBankMoney.ToString("c2");
		float currentMoney3 = mPiggyBankMoney;
		yield return null;
		mPiggyBankOpen = true;
		int streak = 0;
		while (mPiggyBankOpen)
		{
			float input = Input.GetAxisRaw("Vertical");
			if (input > 0f)
			{
				if ((double)PlayerController.instance.money > 0.1)
				{
					PlayerController.instance.money -= 0.1f;
					PlayerController.instance.money = Mathf.Round(PlayerController.instance.money * 100f) / 100f;
					currentMoney3 += 0.1f;
					currentMoney3 = Mathf.Round(currentMoney3 * 100f) / 100f;
					moneyText.text = PlayerController.instance.money.ToString("c2");
					piggyBankText.text = currentMoney3.ToString("c2");
					SFXManager.Instance.PlaySound("DialoguePop");
					if (streak < 10)
					{
						yield return new WaitForSeconds(0.1f);
					}
					else
					{
						yield return new WaitForSeconds(0.025f);
					}
					streak++;
				}
				else if (PlayerController.instance.money > 0f)
				{
					currentMoney3 += PlayerController.instance.money;
					PlayerController.instance.money = 0f;
					PlayerController.instance.money = Mathf.Round(PlayerController.instance.money * 100f) / 100f;
					currentMoney3 = Mathf.Round(currentMoney3 * 100f) / 100f;
					moneyText.text = PlayerController.instance.money.ToString("c2");
					piggyBankText.text = currentMoney3.ToString("c2");
					SFXManager.Instance.PlaySound("DialoguePop");
					if (streak < 10)
					{
						yield return new WaitForSeconds(0.1f);
					}
					else
					{
						yield return new WaitForSeconds(0.025f);
					}
					streak++;
				}
			}
			else if (input < 0f)
			{
				if ((double)currentMoney3 > 0.1)
				{
					PlayerController.instance.money += 0.1f;
					PlayerController.instance.money = Mathf.Round(PlayerController.instance.money * 100f) / 100f;
					currentMoney3 -= 0.1f;
					currentMoney3 = Mathf.Round(currentMoney3 * 100f) / 100f;
					moneyText.text = PlayerController.instance.money.ToString("c2");
					piggyBankText.text = currentMoney3.ToString("c2");
					SFXManager.Instance.PlaySound("DialoguePop");
					if (streak < 10)
					{
						yield return new WaitForSeconds(0.1f);
					}
					else
					{
						yield return new WaitForSeconds(0.025f);
					}
					streak++;
				}
				else if (currentMoney3 > 0f)
				{
					PlayerController.instance.money += currentMoney3;
					PlayerController.instance.money = Mathf.Round(PlayerController.instance.money * 100f) / 100f;
					currentMoney3 = 0f;
					currentMoney3 = Mathf.Round(currentMoney3 * 100f) / 100f;
					moneyText.text = PlayerController.instance.money.ToString("c2");
					piggyBankText.text = currentMoney3.ToString("c2");
					SFXManager.Instance.PlaySound("DialoguePop");
					if (streak < 10)
					{
						yield return new WaitForSeconds(0.1f);
					}
					else
					{
						yield return new WaitForSeconds(0.025f);
					}
					streak++;
				}
			}
			else
			{
				streak = 0;
			}
			if (InputManager.Instance.IsInteractPressed())
			{
				mPiggyBankMoney = currentMoney3;
				SFXManager.Instance.PlaySound("ChaChing");
				GameObject.Find("PiggyBankPanel").GetComponent<CanvasGroup>().DOFade(0f, 0.25f);
				mPiggyBankOpen = false;
			}
			if (InputManager.Instance.IsPlannerPressed())
			{
				GameObject.Find("PiggyBankPanel").GetComponent<CanvasGroup>().DOFade(0f, 0.25f);
				PlayerController.instance.money = startingPlayerMoney;
				moneyText.text = startingPlayerMoney.ToString("c2");
				mPiggyBankOpen = false;
			}
			yield return null;
		}
		PlayerController.instance.inAnim = false;
	}

	public void StoreUI()
	{
		mStoredMissionStatus.Clear();
		foreach (KeyValuePair<Mission, MissionUIBehavior> mMissionObject in mMissionObjects)
		{
			mStoredMissionStatus.Add(mMissionObject.Key, mMissionObject.Value.status);
		}
	}

	public void ResetUI()
	{
		List<Mission> list = new List<Mission>();
		foreach (KeyValuePair<Mission, MissionUIBehavior> mMissionObject in mMissionObjects)
		{
			if (!mStoredMissionStatus.ContainsKey(mMissionObject.Key))
			{
				UnityEngine.Object.Destroy(mMissionObject.Value.gameObject);
				list.Add(mMissionObject.Key);
			}
			else if (mStoredMissionStatus[mMissionObject.Key] == MissionUIBehavior.MissionStatus.Active)
			{
				mMissionObject.Value.SetInProgress();
				mMissionObject.Value.transform.SetAsFirstSibling();
			}
		}
		foreach (Mission item in list)
		{
			mMissionObjects.Remove(item);
		}
	}

	public void ForceResetUI()
	{
		StopAllCoroutines();
		face.transform.DOKill();
		optionsPanel.DOKill();
		dialoguePanel.DOKill();
		plannerUpdatedPanel.DOKill();
		PlayerController.instance.inDialogue = false;
		mDialogueRunning = false;
		mOptionsOut = false;
		dialogueTransitioning = false;
		optionsTransitioning = false;
		mDialogueCoroutine = null;
		deathPanel.GetComponent<CanvasGroup>().alpha = 0f;
		deathPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		deathDescription.color = new Color(0.34f, 0f, 0f, 0f);
		optionsPanel.anchoredPosition = new Vector2(-107f, 10f);
		dialoguePanel.anchoredPosition = new Vector2(0f, 86f);
		plannerUpdatedPanel.anchoredPosition = new Vector3(-300f, 160f);
	}
}
