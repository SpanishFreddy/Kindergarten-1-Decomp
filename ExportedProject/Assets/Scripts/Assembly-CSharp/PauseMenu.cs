using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	public class Hint
	{
		public string NPC;

		public int Number;

		public string Description;

		public bool Unlocked;

		public bool Viewed;

		public bool UnlockedByDefault;
	}

	public class HintList
	{
		public Hint[] Hints;
	}

	private HintList mHints;

	public RectTransform pauseMenu;

	public RectTransform areYouSureMenu;

	public Text areYouSureText;

	public RectTransform areYouSureEraseData;

	public Transform pauseButtons;

	private bool mPauseOpen;

	private bool mAreYouSureOpen;

	public Transform yesButton;

	public Transform noButton;

	public Transform hintsPanel;

	public Transform hintGrid;

	public Text hintDescription;

	public Image hintBackground;

	public Text versionNumber;

	public Transform mainOptionsPanel;

	public Transform mainOptionsButtons;

	public Transform controlOptionsPanel;

	public Transform videoOptionsPanel;

	public Transform audioOptionsPanel;

	public Transform[] controlBoxes;

	private int mCurrentSelection;

	private int mOptionSelection;

	private bool mHintsOpen;

	private bool mMainOptionsOpen;

	private bool mAudioOptionsOpen;

	private bool mControlOptionsOpen;

	private bool mVideoOptionsOpen;

	private bool mAreYouSureData;

	private Event keyEvent;

	private List<Resolution> mAvailableResolutions = new List<Resolution>();

	public Sprite buggsFace;

	public Sprite monstermon;

	private Transform mMusicBox;

	private Transform mEffectsVolumeBox;

	private bool mYes;

	private int mHintSelection;

	private bool mWaitReconfigure;

	private int mControlSelection;

	private int mCurrentRes;

	private int mVideoSelection;

	private bool mIsFullScreen;

	private int mAudioSelection;

	private float mMusicVolume;

	private float mEffectsVolume;

	public bool IsOpen
	{
		get
		{
			return mPauseOpen || mAreYouSureOpen || mHintsOpen || mAudioOptionsOpen || mControlOptionsOpen || mVideoOptionsOpen || mMainOptionsOpen || mAreYouSureData;
		}
	}

	private void Start()
	{
		GetResolutions();
		StartCoroutine(InitializeHints());
		mMusicBox = GameObject.Find("MusicVolumeBox").transform;
		mEffectsVolumeBox = GameObject.Find("EffectsVolumeBox").transform;
	}

	private void OnGUI()
	{
		keyEvent = Event.current;
	}

	private void GetResolutions()
	{
		int num = 0;
		Resolution[] resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			Resolution item = resolutions[i];
			bool flag = true;
			foreach (Resolution mAvailableResolution in mAvailableResolutions)
			{
				if (item.width == mAvailableResolution.width && item.height == mAvailableResolution.height)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				continue;
			}
			if (num == 0)
			{
				mAvailableResolutions.Add(item);
			}
			else
			{
				bool flag2 = false;
				for (int j = 0; j < num; j++)
				{
					if (item.width < mAvailableResolutions[j].width && !flag2)
					{
						mAvailableResolutions.Insert(j, item);
						flag2 = true;
					}
				}
				if (!flag2)
				{
					mAvailableResolutions.Add(item);
				}
			}
			num++;
		}
	}

	private IEnumerator InitializeHints()
	{
		XmlSerializer ser = new XmlSerializer(typeof(HintList));
		TextAsset te = (EnvironmentController.Instance.isSpanish ? (Resources.Load("XMLSpanish/Hints", typeof(TextAsset)) as TextAsset) : (Resources.Load("XML/Hints", typeof(TextAsset)) as TextAsset));
		StringReader r = new StringReader(te.text);
		mHints = (HintList)ser.Deserialize(r);
		Hint[] hints = mHints.Hints;
		foreach (Hint hint in hints)
		{
			int @int = PlayerPrefs.GetInt(hint.NPC + hint.Number + EnvironmentController.Instance.GetFileModifier(), 0);
			if (@int == 0)
			{
				hint.Unlocked = false;
				hint.Viewed = false;
			}
			if (@int > 0 || hint.UnlockedByDefault)
			{
				hint.Unlocked = true;
			}
			if (@int > 1)
			{
				hint.Unlocked = true;
				hint.Viewed = true;
			}
		}
		r.Close();
		yield return null;
		Dictionary<string, Sprite> faces = new Dictionary<string, Sprite>
		{
			{
				"Lily",
				Object.FindObjectOfType<Lily>().defaultTextureBlock.face
			},
			{
				"Jerome",
				Object.FindObjectOfType<Jerome>().defaultTextureBlock.face
			},
			{
				"Cindy",
				Object.FindObjectOfType<Cindy>().defaultTextureBlock.face
			},
			{ "Buggs", buggsFace },
			{
				"Monty",
				Object.FindObjectOfType<Monty>().defaultTextureBlock.face
			},
			{
				"Nugget",
				Object.FindObjectOfType<Nugget>().defaultTextureBlock.face
			},
			{
				"Teacher",
				Object.FindObjectOfType<Teacher>().defaultTextureBlock.face
			},
			{
				"General",
				PlayerController.instance.face
			},
			{ "Monstermon", monstermon }
		};
		Dictionary<string, Color> colors = new Dictionary<string, Color>
		{
			{
				"Lily",
				Object.FindObjectOfType<Lily>().dialogueColor
			},
			{
				"Jerome",
				Object.FindObjectOfType<Jerome>().dialogueColor
			},
			{
				"Cindy",
				Object.FindObjectOfType<Cindy>().dialogueColor
			},
			{
				"Buggs",
				Object.FindObjectOfType<Buggs>().dialogueColor
			},
			{
				"Monty",
				Object.FindObjectOfType<Monty>().dialogueColor
			},
			{
				"Nugget",
				Object.FindObjectOfType<Nugget>().dialogueColor
			},
			{
				"Teacher",
				Object.FindObjectOfType<Teacher>().dialogueColor
			},
			{
				"General",
				Color.white
			},
			{
				"Monstermon",
				Color.cyan
			}
		};
		for (int j = 0; j < mHints.Hints.Length; j++)
		{
			Transform child = hintGrid.GetChild(j);
			child.GetChild(0).eulerAngles = new Vector3(0f, 0f, 90 * Random.Range(0, 4));
			if (mHints.Hints[j].Unlocked)
			{
				child.GetComponent<Image>().color = colors[mHints.Hints[j].NPC] - new Color(0f, 0f, 0f, 0.5f);
				child.GetChild(1).GetComponent<Image>().enabled = true;
				child.GetChild(1).GetComponent<Image>().sprite = faces[mHints.Hints[j].NPC];
				child.GetChild(2).GetComponent<Text>().text = mHints.Hints[j].Number.ToString();
				if (!mHints.Hints[j].Viewed)
				{
					child.GetChild(3).GetComponent<Text>().enabled = true;
				}
				child.GetChild(4).GetComponent<Text>().enabled = false;
			}
		}
	}

	public void UnlockHint(string npc, int u)
	{
		for (int i = 0; i < mHints.Hints.Length; i++)
		{
			if (mHints.Hints[i].NPC == npc && mHints.Hints[i].Number == u)
			{
				if (!mHints.Hints[i].Unlocked)
				{
					PlayerPrefs.SetInt(npc + u + EnvironmentController.Instance.GetFileModifier(), 1);
					mHints.Hints[i].Unlocked = true;
					StartCoroutine(Object.FindObjectOfType<UIController>().ShowUnlockHint());
					UnlockHint(npc, u, i);
				}
				break;
			}
		}
	}

	public void UnlockAllHints(string npc)
	{
		for (int i = 0; i < mHints.Hints.Length; i++)
		{
			if (mHints.Hints[i].NPC == npc && !mHints.Hints[i].Unlocked)
			{
				PlayerPrefs.SetInt(npc + mHints.Hints[i].Number + EnvironmentController.Instance.GetFileModifier(), 1);
				mHints.Hints[i].Unlocked = true;
				UnlockHint(npc, mHints.Hints[i].Number, i);
			}
		}
	}

	private void UnlockHint(string npc, int number, int index)
	{
		Transform child = hintGrid.GetChild(index);
		Color color;
		Sprite sprite;
		if (npc != "General" && npc != "Monstermon")
		{
			color = GameObject.Find(npc).GetComponent<NPCBehavior>().dialogueColor;
			sprite = GameObject.Find(npc).GetComponent<NPCBehavior>().defaultTextureBlock.face;
		}
		else
		{
			color = Color.white;
			sprite = ((!(npc == "Monstermon")) ? PlayerController.instance.face : monstermon);
		}
		child.GetComponent<Image>().color = color - new Color(0f, 0f, 0f, 0.5f);
		child.GetChild(1).GetComponent<Image>().enabled = true;
		child.GetChild(1).GetComponent<Image>().sprite = sprite;
		child.GetChild(2).GetComponent<Text>().text = number.ToString();
		child.GetChild(3).GetComponent<Text>().enabled = true;
		child.GetChild(4).GetComponent<Text>().enabled = false;
	}

	public void OpenPauseMenu()
	{
		if (mHintsOpen || mMainOptionsOpen || mVideoOptionsOpen || mAudioOptionsOpen || mControlOptionsOpen)
		{
			return;
		}
		versionNumber.enabled = true;
		if (!mPauseOpen)
		{
			if (mAreYouSureOpen)
			{
				SFXManager.Instance.PlaySound("Confirm");
				CloseAreYouSure(true);
				return;
			}
			mPauseOpen = true;
			GetComponent<Image>().DOFade(0.6f, 0.25f);
			pauseMenu.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
			pauseMenu.DOScale(1f, 0.1f).SetDelay(0.15f);
			StartCoroutine(RunPauseMenu());
		}
		else
		{
			SFXManager.Instance.PlaySound("Confirm");
			GetComponent<Image>().DOFade(0f, 0.25f);
			ClosePauseMenu();
		}
	}

	public void SetPauseSelection(int x)
	{
		if (mCurrentSelection != x)
		{
			SFXManager.Instance.PlaySound("DialoguePop");
			UnHighlightButton(mCurrentSelection);
			mCurrentSelection = x;
			HighlightButton(mCurrentSelection);
		}
	}

	private IEnumerator RunPauseMenu()
	{
		HighlightButton(0);
		mCurrentSelection = 0;
		yield return new WaitForSeconds(0.3f);
		for (int i = 0; i < 7; i++)
		{
			UnHighlightButton(i);
		}
		HighlightButton(0);
		pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
		StartCoroutine(WaitForConfirm());
		while (mPauseOpen)
		{
			float x = Input.GetAxis("Vertical");
			if (x > 0f)
			{
				if (mCurrentSelection > 0)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					UnHighlightButton(mCurrentSelection);
					mCurrentSelection--;
					HighlightButton(mCurrentSelection);
					yield return new WaitForSeconds(0.25f);
				}
			}
			else if (x < 0f && mCurrentSelection < 6)
			{
				SFXManager.Instance.PlaySound("DialoguePop");
				UnHighlightButton(mCurrentSelection);
				mCurrentSelection++;
				HighlightButton(mCurrentSelection);
				yield return new WaitForSeconds(0.25f);
			}
			yield return null;
		}
		pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
		UnHighlightButton(mCurrentSelection);
	}

	private IEnumerator WaitForConfirm()
	{
		while (mPauseOpen)
		{
			if (InputManager.Instance.IsInteractPressed())
			{
				pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
				ClosePauseMenu();
				SFXManager.Instance.PlaySound("Confirm");
				if (mCurrentSelection == 1 || mCurrentSelection == 2 || mCurrentSelection == 6)
				{
					yield return new WaitForSeconds(0.2f);
					StartCoroutine(ActivateAreYouSure(mCurrentSelection));
				}
				else if (mCurrentSelection == 3)
				{
					yield return new WaitForSeconds(0.2f);
					StartCoroutine(RunHintsMenu());
				}
				else if (mCurrentSelection == 4)
				{
					yield return new WaitForSeconds(0.2f);
					StartCoroutine(RunOptionsMainMenu());
				}
				else if (mCurrentSelection == 5)
				{
					GetComponent<Image>().DOFade(0f, 0.25f);
					Object.FindObjectOfType<UIController>().StartTutorial();
				}
				else
				{
					GetComponent<Image>().DOFade(0f, 0.25f);
				}
			}
			yield return null;
		}
	}

	private void ClosePauseMenu()
	{
		pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
		versionNumber.enabled = false;
		mPauseOpen = false;
		pauseMenu.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		pauseMenu.DOScale(0f, 0.1f).SetDelay(0.15f);
	}

	public void SetYes(bool x)
	{
		if (x)
		{
			mYes = false;
			SFXManager.Instance.PlaySound("DialoguePop");
			if (!mAreYouSureData)
			{
				noButton.DOScale(1.1f, 0.1f);
				yesButton.DOScale(1f, 0.1f);
				yesButton.GetComponentInChildren<Outline>().enabled = false;
				noButton.GetComponentInChildren<Outline>().enabled = true;
			}
			else
			{
				UnHighlightWindow(GameObject.Find("YesButtonData").transform);
				HighlightWindow(GameObject.Find("NoButtonData").transform);
			}
		}
		else
		{
			mYes = true;
			SFXManager.Instance.PlaySound("DialoguePop");
			if (!mAreYouSureData)
			{
				noButton.DOScale(1f, 0.2f);
				yesButton.DOScale(1.1f, 0.2f);
				yesButton.GetComponentInChildren<Outline>().enabled = true;
				noButton.GetComponentInChildren<Outline>().enabled = false;
			}
			else
			{
				HighlightWindow(GameObject.Find("YesButtonData").transform);
				UnHighlightWindow(GameObject.Find("NoButtonData").transform);
			}
		}
	}

	private IEnumerator RunAreYouSureEraseData()
	{
		mAreYouSureData = true;
		areYouSureEraseData.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		areYouSureEraseData.DOScale(1f, 0.1f).SetDelay(0.15f);
		Text questionText = GameObject.Find("QuestionDataText").GetComponent<Text>();
		if (!EnvironmentController.Instance.isSpanish)
		{
			questionText.text = "Are you sure you want to delete all progress on this file? You will lose all unlocked special items, Monstermon cards, and money in your piggy bank.";
		}
		else
		{
			questionText.text = "¿Seguro que quieres eliminar todo el progreso de este archivo? Perderás todos los artículos especiales desbloqueados, tarjetas Monstermon y  el dinero de tu alcancía.";
		}
		mYes = true;
		yield return new WaitForSeconds(0.5f);
		areYouSureEraseData.GetComponent<CanvasGroup>().blocksRaycasts = true;
		Transform eYesButton = GameObject.Find("YesButtonData").transform;
		Transform eNoButton = GameObject.Find("NoButtonData").transform;
		bool reallySure = false;
		mYes = false;
		HighlightWindow(eNoButton);
		UnHighlightWindow(eYesButton);
		while (mAreYouSureData)
		{
			if (Input.GetAxis("Horizontal") > 0f)
			{
				if (mYes)
				{
					mYes = false;
					HighlightWindow(eNoButton);
					UnHighlightWindow(eYesButton);
					SFXManager.Instance.PlaySound("DialoguePop");
				}
			}
			else if (Input.GetAxis("Horizontal") < 0f && !mYes)
			{
				mYes = true;
				HighlightWindow(eYesButton);
				UnHighlightWindow(eNoButton);
				SFXManager.Instance.PlaySound("DialoguePop");
			}
			if (InputManager.Instance.IsInteractPressed())
			{
				SFXManager.Instance.PlaySound("Confirm");
				if (mYes)
				{
					if (!reallySure)
					{
						if (!EnvironmentController.Instance.isSpanish)
						{
							questionText.text = "Seriously. If you press 'yes' again your data is gone. It's not coming back.";
						}
						else
						{
							questionText.text = "En serio. Si vuelves a presionar  'sí', tus datos desaparecerán. Por siempre.";
						}
						reallySure = true;
						yield return new WaitForSeconds(0.5f);
					}
					else
					{
						DeleteData(EnvironmentController.Instance.GetFileModifier());
					}
				}
				else
				{
					mAreYouSureData = false;
				}
			}
			yield return null;
		}
		areYouSureEraseData.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		areYouSureEraseData.DOScale(0f, 0.1f).SetDelay(0.15f);
		StartCoroutine(RunOptionsMainMenu());
		areYouSureEraseData.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	private void DeleteData(string x)
	{
		PlayerPrefs.SetFloat("PiggyBank" + EnvironmentController.Instance.GetFileModifier(), 5f);
		PlayerPrefs.SetInt("PlayerDeaths" + EnvironmentController.Instance.GetFileModifier(), 0);
		PlayerPrefs.SetInt("DaysComplete" + EnvironmentController.Instance.GetFileModifier(), 1);
		PlayerPrefs.SetInt("WorldEnder" + EnvironmentController.Instance.GetFileModifier(), 0);
		PlayerPrefs.SetInt("UnlockKeyMold" + x, 0);
		PlayerPrefs.SetInt("UnlockLunchPass" + x, 0);
		PlayerPrefs.SetInt("UnlockKingTowerbeetle" + x, 0);
		PlayerPrefs.SetInt("UnlockGiraffeSerpent" + x, 0);
		PlayerPrefs.SetInt("UnlockFaptainCalcon" + x, 0);
		PlayerPrefs.SetInt("UnlockMagicalAirship" + x, 0);
		PlayerPrefs.SetInt("UnlockBillyNote" + x, 0);
		PlayerPrefs.SetInt("UnlockReallyBrightStar" + x, 0);
		PlayerPrefs.SetInt("UnlockFlower" + x, 0);
		PlayerPrefs.SetInt("UnlockPrincipalKey" + x, 0);
		PlayerPrefs.SetInt("UnlockEvilThwarter" + x, 0);
		PlayerPrefs.SetInt("UnlockPhone" + x, 0);
		PlayerPrefs.SetInt("UnlockWizardWorm" + x, 0);
		PlayerPrefs.SetInt("UnlockHolyKnight" + x, 0);
		PlayerPrefs.SetInt("UnlockFreezyguyJim" + x, 0);
		PlayerPrefs.SetInt("UnlockTornadoFly" + x, 0);
		PlayerPrefs.SetInt("UnlockManWithLongArm" + x, 0);
		PlayerPrefs.SetInt("UnlockOhfakaTornado" + x, 0);
		PlayerPrefs.SetInt("UnlockEyeOfTheButtholder" + x, 0);
		PlayerPrefs.SetInt("UnlockMartianOrbMan" + x, 0);
		PlayerPrefs.SetInt("UnlockCactusOutlaw" + x, 0);
		PlayerPrefs.SetInt("UnlockUneatenCake" + x, 0);
		PlayerPrefs.SetInt("UnlockCyclopsDuckling" + x, 0);
		PlayerPrefs.SetInt("UnlockSneakySnake" + x, 0);
		PlayerPrefs.SetInt("UnlockOglebopGolem" + x, 0);
		PlayerPrefs.SetInt("UnlockWallOfCastle" + x, 0);
		PlayerPrefs.SetInt("UnlockShroomTurtle" + x, 0);
		PlayerPrefs.SetInt("UnlockLiterallyGrass" + x, 0);
		PlayerPrefs.SetInt("UnlockSpikeyFlimflam" + x, 0);
		PlayerPrefs.SetInt("UnlockDoomjelly" + x, 0);
		PlayerPrefs.SetInt("UnlockBlueEyesGoldDragon" + x, 0);
		PlayerPrefs.SetInt("UnlockKeyMold" + x, 0);
		Hint[] hints = mHints.Hints;
		foreach (Hint hint in hints)
		{
			PlayerPrefs.SetInt(hint.NPC + hint.Number + x, 0);
		}
		SceneManager.LoadScene("Kindergarten");
	}

	private IEnumerator ActivateAreYouSure(int x)
	{
		mAreYouSureOpen = true;
		areYouSureMenu.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		areYouSureMenu.DOScale(1f, 0.1f).SetDelay(0.15f);
		yield return null;
		switch (x)
		{
		case 1:
		{
			string text = string.Empty;
			if (!EnvironmentController.Instance.isSpanish)
			{
				switch (EnvironmentController.Instance.furthestRoom)
				{
				case Room.SchoolYard:
					text = "the school yard";
					break;
				case Room.Classroom1:
					text = "morning time";
					break;
				case Room.Office:
					text = "the principal's office";
					break;
				case Room.Cafeteria:
					text = "lunch";
					break;
				case Room.ClassroomLunch:
					text = "lunch";
					break;
				case Room.Recess:
					text = "recess";
					break;
				case Room.Lair:
					text = "the principal's lab";
					break;
				}
				areYouSureText.text = "Are you sure you want to reset your day to the start of " + text + "? All progress made since that point will be lost.";
			}
			else
			{
				switch (EnvironmentController.Instance.furthestRoom)
				{
				case Room.SchoolYard:
					text = "el patio de la escuela";
					break;
				case Room.Classroom1:
					text = "clases de la mañana";
					break;
				case Room.Office:
					text = "el oficina del director";
					break;
				case Room.Cafeteria:
					text = "almuerzo";
					break;
				case Room.ClassroomLunch:
					text = "almuerzo";
					break;
				case Room.Recess:
					text = "recreo";
					break;
				case Room.Lair:
					text = "el laboratorio del director";
					break;
				}
				areYouSureText.text = "¿Estás seguro que quieres restablecer el día al principio" + text + "? Todos los progresos realizados en el juego se perderán.";
			}
			break;
		}
		case 2:
			if (!EnvironmentController.Instance.isSpanish)
			{
				areYouSureText.text = "Are you sure you want to restart the day? You will lose all progress from today and start a new day from your bedroom.";
			}
			else
			{
				areYouSureText.text = "¿Estás seguro que quieres reiniciar el día? En este caso perderás todo el progreso del día y comenzarás un nuevo día desde tu dormitorio.";
			}
			break;
		case 6:
			if (EnvironmentController.currentRoom == Room.Bedroom)
			{
				if (!EnvironmentController.Instance.isSpanish)
				{
					areYouSureText.text = "Are you sure you want to return to the main menu?";
				}
				else
				{
					areYouSureText.text = "¿Estás seguro que quieres ir al menú principal?";
				}
			}
			else if (!EnvironmentController.Instance.isSpanish)
			{
				areYouSureText.text = "Are you sure you want to the main menu? All progress made during this day will be lost.";
			}
			else
			{
				areYouSureText.text = "¿Estás seguro que quieres ir al menú principal? Perderás todo el progreso de este día.";
			}
			break;
		}
		mYes = true;
		noButton.DOScale(1f, 0.1f);
		yesButton.DOScale(1.1f, 0.1f);
		yesButton.GetComponentInChildren<Outline>().enabled = true;
		noButton.GetComponentInChildren<Outline>().enabled = false;
		while (mAreYouSureOpen)
		{
			if (Input.GetAxis("Horizontal") > 0f)
			{
				if (mYes)
				{
					mYes = false;
					noButton.DOScale(1.1f, 0.1f);
					yesButton.DOScale(1f, 0.1f);
					SFXManager.Instance.PlaySound("DialoguePop");
					yesButton.GetComponentInChildren<Outline>().enabled = false;
					noButton.GetComponentInChildren<Outline>().enabled = true;
					yield return new WaitForSeconds(0.2f);
				}
			}
			else if (Input.GetAxis("Horizontal") < 0f && !mYes)
			{
				mYes = true;
				noButton.DOScale(1f, 0.2f);
				yesButton.DOScale(1.1f, 0.2f);
				SFXManager.Instance.PlaySound("DialoguePop");
				yesButton.GetComponentInChildren<Outline>().enabled = true;
				noButton.GetComponentInChildren<Outline>().enabled = false;
				yield return new WaitForSeconds(0.2f);
			}
			if (InputManager.Instance.IsInteractPressed())
			{
				SFXManager.Instance.PlaySound("Confirm");
				if (mYes)
				{
					CloseAreYouSure(false);
					switch (x)
					{
					case 1:
						if (EnvironmentController.currentRoom != Room.Bedroom)
						{
							EnvironmentController.Instance.RestartRoom();
							GetComponent<Image>().DOFade(0f, 0.2f);
						}
						else
						{
							GetComponent<Image>().DOFade(1f, 0.25f);
							yield return new WaitForSeconds(0.25f);
							SceneManager.LoadScene("Kindergarten");
						}
						break;
					case 2:
						GetComponent<Image>().DOFade(1f, 0.25f);
						yield return new WaitForSeconds(0.25f);
						SceneManager.LoadScene("Kindergarten");
						break;
					case 6:
						GetComponent<Image>().DOFade(1f, 0.25f);
						yield return new WaitForSeconds(0.25f);
						SceneManager.LoadScene("TitleScreen");
						break;
					}
				}
				else
				{
					CloseAreYouSure(true);
				}
			}
			yield return null;
		}
	}

	public void SetHint(int x)
	{
		if (mHintSelection == x)
		{
			return;
		}
		UnHighlightHint(mHintSelection);
		mHintSelection = x;
		HighlightHint(mHintSelection);
		hintDescription.transform.localScale = Vector3.zero;
		hintDescription.transform.DOScale(Vector3.one, 0.2f);
		hintBackground.DOColor(hintGrid.GetChild(mHintSelection).GetComponent<Image>().color, 0.2f);
		SFXManager.Instance.PlaySound("DialoguePop");
		if (mHints.Hints[mHintSelection].Unlocked)
		{
			hintDescription.text = mHints.Hints[mHintSelection].Description;
			if (!mHints.Hints[mHintSelection].Viewed)
			{
				Hint hint = mHints.Hints[mHintSelection];
				hint.Viewed = true;
				hintGrid.GetChild(mHintSelection).GetChild(3).GetComponent<Text>()
					.enabled = false;
				PlayerPrefs.SetInt(hint.NPC + hint.Number + EnvironmentController.Instance.GetFileModifier(), 2);
			}
		}
		else
		{
			hintDescription.text = "???";
		}
	}

	private IEnumerator RunHintsMenu()
	{
		mHintsOpen = true;
		hintsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		hintsPanel.DOScale(1f, 0.1f).SetDelay(0.15f).OnComplete(delegate
		{
			hintsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		});
		mHintSelection = 0;
		HighlightHint(0);
		hintDescription.text = mHints.Hints[mHintSelection].Description;
		while (mHintsOpen)
		{
			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");
			if (x != 0f || y != 0f)
			{
				UnHighlightHint(mHintSelection);
				if (x > 0f && mHintSelection < mHints.Hints.Length - 1)
				{
					mHintSelection++;
				}
				else if (x < 0f && mHintSelection > 0)
				{
					mHintSelection--;
				}
				else if (y > 0f && mHintSelection > 14)
				{
					mHintSelection -= 15;
				}
				else if (y < 0f && mHintSelection < mHints.Hints.Length - 15)
				{
					mHintSelection += 15;
				}
				HighlightHint(mHintSelection);
				hintDescription.transform.localScale = Vector3.zero;
				hintDescription.transform.DOScale(Vector3.one, 0.2f);
				hintBackground.DOColor(hintGrid.GetChild(mHintSelection).GetComponent<Image>().color, 0.2f);
				SFXManager.Instance.PlaySound("DialoguePop");
				if (mHints.Hints[mHintSelection].Unlocked)
				{
					hintDescription.text = mHints.Hints[mHintSelection].Description;
					if (!mHints.Hints[mHintSelection].Viewed)
					{
						Hint hint = mHints.Hints[mHintSelection];
						hint.Viewed = true;
						hintGrid.GetChild(mHintSelection).GetChild(3).GetComponent<Text>()
							.enabled = false;
						PlayerPrefs.SetInt(hint.NPC + hint.Number + EnvironmentController.Instance.GetFileModifier(), 2);
					}
				}
				else
				{
					hintDescription.text = "???";
				}
				yield return new WaitForSeconds(0.2f);
			}
			if (InputManager.Instance.IsInteractPressed() || InputManager.Instance.IsPausePressed())
			{
				hintsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
				UnHighlightHint(mHintSelection);
				SFXManager.Instance.PlaySound("Confirm");
				hintsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				hintsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
				yield return new WaitForSeconds(0.2f);
				mHintsOpen = false;
				OpenPauseMenu();
			}
			yield return null;
		}
		hintsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void SetMainOption(int x)
	{
		SFXManager.Instance.PlaySound("DialoguePop");
		UnHighlightWindow(mainOptionsButtons.GetChild(mOptionSelection));
		mOptionSelection = x;
		HighlightWindow(mainOptionsButtons.GetChild(mOptionSelection), 1.1f);
	}

	private IEnumerator RunOptionsMainMenu()
	{
		mMainOptionsOpen = true;
		mainOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		mainOptionsPanel.DOScale(1f, 0.1f).SetDelay(0.15f).OnComplete(delegate
		{
			mainOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		});
		StartCoroutine(WaitForOptionsConfirm());
		Transform[] oWindows = new Transform[5];
		for (int i = 0; i < 5; i++)
		{
			oWindows[i] = mainOptionsButtons.GetChild(i);
		}
		mOptionSelection = 0;
		HighlightWindow(oWindows[0]);
		while (mMainOptionsOpen)
		{
			float y = Input.GetAxis("Vertical");
			if (y > 0f && mOptionSelection > 0)
			{
				SFXManager.Instance.PlaySound("DialoguePop");
				UnHighlightWindow(oWindows[mOptionSelection]);
				mOptionSelection--;
				HighlightWindow(oWindows[mOptionSelection], 1.1f);
				yield return new WaitForSeconds(0.25f);
			}
			if (y < 0f && mOptionSelection < 4)
			{
				SFXManager.Instance.PlaySound("DialoguePop");
				UnHighlightWindow(oWindows[mOptionSelection]);
				mOptionSelection++;
				HighlightWindow(oWindows[mOptionSelection], 1.1f);
				yield return new WaitForSeconds(0.25f);
			}
			yield return null;
		}
		mainOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		UnHighlightWindow(oWindows[mOptionSelection]);
	}

	private IEnumerator WaitForOptionsConfirm()
	{
		while (mMainOptionsOpen)
		{
			if (InputManager.Instance.IsInteractPressed())
			{
				mainOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				mainOptionsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
				SFXManager.Instance.PlaySound("Confirm");
				mainOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
				yield return new WaitForSeconds(0.2f);
				mMainOptionsOpen = false;
				if (mOptionSelection == 0)
				{
					StartCoroutine(RunControlOptionsMenu());
				}
				else if (mOptionSelection == 1)
				{
					StartCoroutine(RunVideoOptionsMenu());
				}
				else if (mOptionSelection == 2)
				{
					StartCoroutine(RunAudioOptionsMenu());
				}
				else if (mOptionSelection == 3)
				{
					StartCoroutine(RunAreYouSureEraseData());
				}
				else
				{
					OpenPauseMenu();
				}
			}
			if (InputManager.Instance.IsPausePressed())
			{
				mainOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				mainOptionsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
				SFXManager.Instance.PlaySound("Confirm");
				mainOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
				yield return new WaitForSeconds(0.2f);
				mMainOptionsOpen = false;
				OpenPauseMenu();
			}
			yield return null;
		}
	}

	public void SetControlBox(int x)
	{
		if (!mWaitReconfigure)
		{
			SFXManager.Instance.PlaySound("DialoguePop");
			UnHighlightWindow(controlBoxes[mControlSelection]);
			mControlSelection = x;
			HighlightWindow(controlBoxes[mControlSelection], 1.1f);
		}
	}

	private IEnumerator RunControlOptionsMenu()
	{
		mControlOptionsOpen = true;
		controlOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		controlOptionsPanel.DOScale(1f, 0.1f).SetDelay(0.15f).OnComplete(delegate
		{
			controlOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		});
		mControlSelection = 0;
		HighlightWindow(controlBoxes[0]);
		string[] s = InputManager.Instance.GetInputStrings();
		for (int i = 0; i < 8; i++)
		{
			if (i % 2 == 0)
			{
				controlBoxes[i].GetComponentInChildren<Text>().text = s[i];
			}
			else
			{
				controlBoxes[i].GetComponentInChildren<Text>().text = s[i];
			}
		}
		while (mControlOptionsOpen)
		{
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");
			if (x > 0f && mControlSelection % 2 == 0 && mControlSelection != 8)
			{
				SFXManager.Instance.PlaySound("DialoguePop");
				UnHighlightWindow(controlBoxes[mControlSelection]);
				mControlSelection++;
				HighlightWindow(controlBoxes[mControlSelection], 1.1f);
				yield return new WaitForSeconds(0.2f);
			}
			if (x < 0f && mControlSelection % 2 == 1 && mControlSelection != 8)
			{
				SFXManager.Instance.PlaySound("DialoguePop");
				UnHighlightWindow(controlBoxes[mControlSelection]);
				mControlSelection--;
				HighlightWindow(controlBoxes[mControlSelection], 1.1f);
				yield return new WaitForSeconds(0.2f);
			}
			if (y > 0f && mControlSelection > 1)
			{
				SFXManager.Instance.PlaySound("DialoguePop");
				UnHighlightWindow(controlBoxes[mControlSelection]);
				mControlSelection -= 2;
				HighlightWindow(controlBoxes[mControlSelection], 1.1f);
				yield return new WaitForSeconds(0.2f);
			}
			if (y < 0f)
			{
				if (mControlSelection < 7)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					UnHighlightWindow(controlBoxes[mControlSelection]);
					mControlSelection += 2;
					HighlightWindow(controlBoxes[mControlSelection], 1.1f);
					yield return new WaitForSeconds(0.2f);
				}
				else if (mControlSelection == 7)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					UnHighlightWindow(controlBoxes[mControlSelection]);
					mControlSelection++;
					HighlightWindow(controlBoxes[mControlSelection], 1.1f);
					yield return new WaitForSeconds(0.2f);
				}
			}
			if (InputManager.Instance.IsInteractPressed() && !mWaitReconfigure)
			{
				if (mControlSelection % 2 == 0)
				{
					if (mControlSelection < 8)
					{
						mWaitReconfigure = true;
						StartCoroutine(WaitToReconfigureKey(mControlSelection));
						while (mWaitReconfigure)
						{
							yield return null;
						}
						yield return null;
					}
					else
					{
						SFXManager.Instance.PlaySound("Confirm");
						InputManager.Instance.ApplyCustomInputs();
						controlOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
						controlOptionsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
						yield return new WaitForSeconds(0.2f);
						mControlOptionsOpen = false;
						StartCoroutine(RunOptionsMainMenu());
					}
				}
				else
				{
					mWaitReconfigure = true;
					StartCoroutine(WaitToReconfigureButton(mControlSelection));
					while (mWaitReconfigure)
					{
						yield return null;
					}
					yield return null;
				}
			}
			if (InputManager.Instance.IsPausePressed())
			{
				InputManager.Instance.GetCustomInputs();
				SFXManager.Instance.PlaySound("Confirm");
				controlOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				controlOptionsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
				yield return new WaitForSeconds(0.2f);
				mControlOptionsOpen = false;
				StartCoroutine(RunOptionsMainMenu());
			}
			yield return null;
		}
		controlOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		UnHighlightWindow(controlBoxes[mControlSelection]);
	}

	private IEnumerator WaitToReconfigureKey(int sel)
	{
		HighlightWindowGreen(controlBoxes[sel], 1.1f);
		yield return null;
		while (InputManager.Instance.IsInteractHeld())
		{
			yield return null;
		}
		yield return null;
		while (keyEvent.keyCode == KeyCode.None)
		{
			yield return null;
		}
		string a = keyEvent.keyCode.ToString();
		switch (a)
		{
		default:
			if (!(a.ToUpper() == InputManager.Instance.interactButtonKeyboard.ToUpper()) && !(a.ToUpper() == InputManager.Instance.actionButtonKeyboard.ToUpper()) && !(a.ToUpper() == InputManager.Instance.plannerButtonKeyboard.ToUpper()) && !(a.ToUpper() == InputManager.Instance.pauseButtonKeyboard.ToUpper()))
			{
				switch (sel)
				{
				case 0:
					InputManager.Instance.ReconfigureInteractKeyboard(a);
					break;
				case 2:
					InputManager.Instance.ReconfigurePlannerKeyboard(a);
					break;
				case 4:
					InputManager.Instance.ReconfigureActionKeyboard(a);
					break;
				case 6:
					InputManager.Instance.ReconfigurePauseKeyboard(a);
					break;
				}
				SFXManager.Instance.PlaySound("GetItem");
				controlBoxes[sel].GetComponentInChildren<Text>().text = a;
				HighlightWindow(controlBoxes[sel], 1.1f);
				mWaitReconfigure = false;
				break;
			}
			goto case "W";
		case "W":
		case "S":
		case "A":
		case "D":
			SFXManager.Instance.PlaySound("Failed");
			HighlightWindow(controlBoxes[sel], 1.1f);
			mWaitReconfigure = false;
			yield return new WaitForSeconds(0.2f);
			break;
		}
	}

	private IEnumerator WaitToReconfigureButton(int sel)
	{
		HighlightWindowGreen(controlBoxes[sel], 1.1f);
		while (InputManager.Instance.IsInteractHeld())
		{
			yield return null;
		}
		yield return null;
		int x = -1;
		while (x == -1)
		{
			for (int i = 0; i < 16; i++)
			{
				if (Input.GetButtonDown("Button " + i))
				{
					x = i;
				}
			}
			if (keyEvent.keyCode != 0)
			{
				SFXManager.Instance.PlaySound("Failed");
				HighlightWindow(controlBoxes[sel], 1.1f);
				mWaitReconfigure = false;
				yield break;
			}
			yield return null;
		}
		string a = "Button " + x;
		if (a == InputManager.Instance.interactButtonController.ToUpper() || a == InputManager.Instance.actionButtonController.ToUpper() || a == InputManager.Instance.plannerButtonController.ToUpper() || a == InputManager.Instance.pauseButtonController.ToUpper())
		{
			SFXManager.Instance.PlaySound("Failed");
			HighlightWindow(controlBoxes[sel], 1.1f);
			mWaitReconfigure = false;
			yield break;
		}
		switch (sel)
		{
		case 1:
			InputManager.Instance.ReconfigureInteractController(a);
			break;
		case 3:
			InputManager.Instance.ReconfigurePlannerController(a);
			break;
		case 5:
			InputManager.Instance.ReconfigureActionController(a);
			break;
		case 7:
			InputManager.Instance.ReconfigurePauseController(a);
			break;
		}
		SFXManager.Instance.PlaySound("GetItem");
		controlBoxes[sel].GetComponentInChildren<Text>().text = a;
		HighlightWindow(controlBoxes[sel], 1.1f);
		yield return new WaitForSeconds(0.5f);
		mWaitReconfigure = false;
	}

	public void SetVideoOption(bool right)
	{
		Text component = GameObject.Find("ScreenSizeText").GetComponent<Text>();
		Text component2 = GameObject.Find("WindowedModeText").GetComponent<Text>();
		if (mVideoSelection == 0)
		{
			if (right)
			{
				if (mCurrentRes < mAvailableResolutions.Count - 1)
				{
					mCurrentRes++;
				}
			}
			else if (mCurrentRes > 0)
			{
				mCurrentRes--;
			}
			SFXManager.Instance.PlaySound("DialoguePop");
			component.text = mAvailableResolutions[mCurrentRes].width + " x " + mAvailableResolutions[mCurrentRes].height;
			component.transform.DOKill();
			component.transform.localPosition = new Vector3(0f, -10f, 0f);
			component.transform.DOLocalMoveY(0f, 0.1f);
			component.transform.DOLocalMoveY(-10f, 0.1f).SetDelay(0.1f);
			component.transform.parent.DOKill();
			component.transform.parent.localScale = Vector3.one;
			component.transform.parent.DOScale(1.05f, 0.1f);
			component.transform.parent.DOScale(1f, 0.1f).SetDelay(0.1f);
		}
		else
		{
			if (mVideoSelection != 1)
			{
				return;
			}
			if (right)
			{
				if (mIsFullScreen)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					mIsFullScreen = false;
					component2.text = "On";
					component2.transform.DOKill();
					component2.transform.localPosition = new Vector3(0f, -10f, 0f);
					component2.transform.DOLocalMoveY(0f, 0.1f);
					component2.transform.DOLocalMoveY(-10f, 0.1f).SetDelay(0.1f);
					component2.transform.parent.DOKill();
					component2.transform.parent.localScale = Vector3.one;
					component2.transform.parent.DOScale(1.05f, 0.1f);
					component2.transform.parent.DOScale(1f, 0.1f).SetDelay(0.1f);
				}
			}
			else
			{
				SFXManager.Instance.PlaySound("DialoguePop");
				mIsFullScreen = true;
				component2.text = "Off";
				component2.transform.DOKill();
				component2.transform.localPosition = new Vector3(0f, -10f, 0f);
				component2.transform.DOLocalMoveY(0f, 0.1f);
				component2.transform.DOLocalMoveY(-10f, 0.1f).SetDelay(0.1f);
				component2.transform.parent.DOKill();
				component2.transform.parent.localScale = Vector3.one;
				component2.transform.parent.DOScale(1.05f, 0.1f);
				component2.transform.parent.DOScale(1f, 0.1f).SetDelay(0.1f);
			}
		}
	}

	private IEnumerator RunVideoOptionsMenu()
	{
		mVideoOptionsOpen = true;
		videoOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		videoOptionsPanel.DOScale(1f, 0.1f).SetDelay(0.15f).OnComplete(delegate
		{
			videoOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		});
		mVideoSelection = 0;
		int startingRes = mCurrentRes;
		Text resText = GameObject.Find("ScreenSizeText").GetComponent<Text>();
		Text winText = GameObject.Find("WindowedModeText").GetComponent<Text>();
		resText.text = mAvailableResolutions[mCurrentRes].width + " x " + mAvailableResolutions[mCurrentRes].height;
		mIsFullScreen = Screen.fullScreen;
		if (Screen.fullScreen)
		{
			winText.text = "Off";
		}
		else
		{
			winText.text = "On";
		}
		Transform[] oWindows = new Transform[3];
		Transform oh = GameObject.Find("VideoOptionsHolder").transform;
		for (int i = 0; i < 3; i++)
		{
			oWindows[i] = oh.GetChild(i + 1);
		}
		HighlightWindow(oWindows[mVideoSelection]);
		while (mVideoOptionsOpen)
		{
			yield return null;
			float y = Input.GetAxis("Vertical");
			float x = Input.GetAxis("Horizontal");
			if (x > 0f)
			{
				if (mVideoSelection == 0)
				{
					if (mCurrentRes < mAvailableResolutions.Count - 1)
					{
						SFXManager.Instance.PlaySound("DialoguePop");
						mCurrentRes++;
						resText.text = mAvailableResolutions[mCurrentRes].width + " x " + mAvailableResolutions[mCurrentRes].height;
						resText.transform.DOKill();
						resText.transform.localPosition = new Vector3(0f, -10f, 0f);
						resText.transform.DOLocalMoveY(0f, 0.1f);
						resText.transform.DOLocalMoveY(-10f, 0.1f).SetDelay(0.1f);
						resText.transform.parent.DOKill();
						resText.transform.parent.localScale = Vector3.one;
						resText.transform.parent.DOScale(1.05f, 0.1f);
						resText.transform.parent.DOScale(1f, 0.1f).SetDelay(0.1f);
						yield return new WaitForSeconds(0.2f);
					}
				}
				else if (mVideoSelection == 1 && mIsFullScreen)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					mIsFullScreen = false;
					winText.text = "On";
					winText.transform.DOKill();
					winText.transform.localPosition = new Vector3(0f, -10f, 0f);
					winText.transform.DOLocalMoveY(0f, 0.1f);
					winText.transform.DOLocalMoveY(-10f, 0.1f).SetDelay(0.1f);
					winText.transform.parent.DOKill();
					winText.transform.parent.localScale = Vector3.one;
					winText.transform.parent.DOScale(1.05f, 0.1f);
					winText.transform.parent.DOScale(1f, 0.1f).SetDelay(0.1f);
					yield return new WaitForSeconds(0.2f);
				}
			}
			if (x < 0f)
			{
				if (mVideoSelection == 0)
				{
					if (mCurrentRes > 0)
					{
						SFXManager.Instance.PlaySound("DialoguePop");
						mCurrentRes--;
						resText.text = mAvailableResolutions[mCurrentRes].width + " x " + mAvailableResolutions[mCurrentRes].height;
						resText.transform.DOKill();
						resText.transform.localPosition = new Vector3(0f, -10f, 0f);
						resText.transform.DOLocalMoveY(0f, 0.1f);
						resText.transform.DOLocalMoveY(-10f, 0.1f).SetDelay(0.1f);
						resText.transform.parent.DOKill();
						resText.transform.parent.localScale = Vector3.one;
						resText.transform.parent.DOScale(1.05f, 0.1f);
						resText.transform.parent.DOScale(1f, 0.1f).SetDelay(0.1f);
						yield return new WaitForSeconds(0.2f);
					}
				}
				else if (mVideoSelection == 1 && !mIsFullScreen)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					mIsFullScreen = true;
					winText.text = "Off";
					winText.transform.DOKill();
					winText.transform.localPosition = new Vector3(0f, -10f, 0f);
					winText.transform.DOLocalMoveY(0f, 0.1f);
					winText.transform.DOLocalMoveY(-10f, 0.1f).SetDelay(0.1f);
					winText.transform.parent.DOKill();
					winText.transform.parent.localScale = Vector3.one;
					winText.transform.parent.DOScale(1.05f, 0.1f);
					winText.transform.parent.DOScale(1f, 0.1f).SetDelay(0.1f);
					yield return new WaitForSeconds(0.2f);
				}
			}
			if (y > 0f)
			{
				if (mVideoSelection > 0)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					UnHighlightWindow(oWindows[mVideoSelection]);
					mVideoSelection--;
					HighlightWindow(oWindows[mVideoSelection]);
				}
				yield return new WaitForSeconds(0.25f);
			}
			if (y < 0f)
			{
				if (mVideoSelection < oWindows.Length - 1)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					UnHighlightWindow(oWindows[mVideoSelection]);
					mVideoSelection++;
					HighlightWindow(oWindows[mVideoSelection]);
				}
				yield return new WaitForSeconds(0.25f);
			}
			if (InputManager.Instance.IsInteractPressed() && mVideoSelection == 2)
			{
				SFXManager.Instance.PlaySound("Confirm");
				videoOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				videoOptionsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
				if (startingRes != mCurrentRes)
				{
					Screen.SetResolution(mAvailableResolutions[mCurrentRes].width, mAvailableResolutions[mCurrentRes].height, true);
					Object.FindObjectOfType<CameraFollow>().RefreshScreen((float)mAvailableResolutions[mCurrentRes].width / (float)mAvailableResolutions[mCurrentRes].height);
				}
				Screen.fullScreen = mIsFullScreen;
				yield return new WaitForSeconds(0.2f);
				StartCoroutine(RunOptionsMainMenu());
				mVideoOptionsOpen = false;
				UnHighlightWindow(oWindows[mVideoSelection]);
			}
			if (InputManager.Instance.IsPausePressed() || InputManager.Instance.IsPlannerPressed())
			{
				SFXManager.Instance.PlaySound("Confirm");
				videoOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				videoOptionsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
				yield return new WaitForSeconds(0.2f);
				StartCoroutine(RunOptionsMainMenu());
				mVideoOptionsOpen = false;
				UnHighlightWindow(oWindows[mVideoSelection]);
			}
		}
		videoOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void SetVideoSelection(int x)
	{
		if (mVideoSelection != x)
		{
			Transform[] array = new Transform[3];
			Transform transform = GameObject.Find("VideoOptionsHolder").transform;
			for (int i = 0; i < 3; i++)
			{
				array[i] = transform.GetChild(i + 1);
			}
			SFXManager.Instance.PlaySound("DialoguePop");
			UnHighlightWindow(array[mVideoSelection]);
			mVideoSelection = x;
			HighlightWindow(array[mVideoSelection]);
		}
	}

	public void SetAudioSelection(int x)
	{
		if (mAudioSelection != x)
		{
			Transform[] array = new Transform[3];
			Transform transform = GameObject.Find("AudioOptionsHolder").transform;
			for (int i = 0; i < 3; i++)
			{
				array[i] = transform.GetChild(i + 1);
			}
			Transform transform2 = GameObject.Find("MusicVolumeBox").transform;
			Transform transform3 = GameObject.Find("EffectsVolumeBox").transform;
			SFXManager.Instance.PlaySound("DialoguePop");
			UnHighlightWindow(array[mAudioSelection]);
			mAudioSelection = x;
			HighlightWindow(array[mAudioSelection]);
		}
	}

	public void SetMusicVolume(bool x)
	{
		if (x)
		{
			if (mMusicVolume < 1f)
			{
				mMusicVolume += 0.1f;
				if (mMusicVolume > 1f)
				{
					mMusicVolume = 1f;
				}
				mMusicBox.localScale = new Vector3(mMusicVolume, 1f, 0f);
				SFXManager.Instance.SetVolumes(mMusicVolume, mEffectsVolume);
			}
		}
		else if (mMusicVolume > 0f)
		{
			mMusicVolume -= 0.1f;
			if (mMusicVolume < 0f)
			{
				mMusicVolume = 0f;
			}
			mMusicBox.localScale = new Vector3(mMusicVolume, 1f, 0f);
			SFXManager.Instance.SetVolumes(mMusicVolume, mEffectsVolume);
		}
	}

	public void SetEffectsVolume(bool x)
	{
		if (x)
		{
			if (mEffectsVolume < 2f)
			{
				mEffectsVolume += 0.1f;
				if (mEffectsVolume > 2f)
				{
					mEffectsVolume = 2f;
				}
				mEffectsVolumeBox.localScale = new Vector3(mEffectsVolume / 2f, 1f, 0f);
				SFXManager.Instance.SetVolumes(mMusicVolume, mEffectsVolume);
			}
		}
		else if (mEffectsVolume > 0f)
		{
			mEffectsVolume -= 0.1f;
			if (mEffectsVolume < 0f)
			{
				mEffectsVolume = 0f;
			}
			mEffectsVolumeBox.localScale = new Vector3(mEffectsVolume / 2f, 1f, 0f);
			SFXManager.Instance.SetVolumes(mMusicVolume, mEffectsVolume);
		}
		SFXManager.Instance.PlaySound("DialoguePop");
	}

	private IEnumerator RunAudioOptionsMenu()
	{
		mAudioOptionsOpen = true;
		audioOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		audioOptionsPanel.DOScale(1f, 0.1f).SetDelay(0.15f).OnComplete(delegate
		{
			audioOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		});
		mAudioSelection = 0;
		mMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		mEffectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1f);
		float storeMusicVolume = mMusicVolume;
		float storeEffectsVolume = mEffectsVolume;
		Transform[] oWindows = new Transform[3];
		Transform oh = GameObject.Find("AudioOptionsHolder").transform;
		mMusicBox.localScale = new Vector3(mMusicVolume, 1f);
		mEffectsVolumeBox.localScale = new Vector3(mEffectsVolume / 2f, 1f);
		for (int i = 0; i < 3; i++)
		{
			oWindows[i] = oh.GetChild(i + 1);
		}
		HighlightWindow(oWindows[0]);
		while (mAudioOptionsOpen)
		{
			yield return null;
			float y = Input.GetAxis("Vertical");
			float x = Input.GetAxis("Horizontal");
			if (x > 0f)
			{
				if (mAudioSelection == 0 && mMusicVolume < 1f)
				{
					mMusicVolume += 0.01f;
					mMusicBox.localScale = new Vector3(mMusicVolume, 1f, 0f);
					SFXManager.Instance.SetVolumes(mMusicVolume, mEffectsVolume);
					yield return new WaitForSeconds(0.025f);
				}
				if (mAudioSelection == 1 && mEffectsVolume < 2f)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					mEffectsVolume += 0.1f;
					mEffectsVolumeBox.localScale = new Vector3(mEffectsVolume / 2f, 1f, 0f);
					SFXManager.Instance.SetVolumes(mMusicVolume, mEffectsVolume);
					yield return new WaitForSeconds(0.1f);
				}
			}
			else if (x < 0f)
			{
				if (mAudioSelection == 0 && mMusicVolume > 0f)
				{
					mMusicVolume -= 0.01f;
					mMusicBox.localScale = new Vector3(mMusicVolume, 1f, 0f);
					SFXManager.Instance.SetVolumes(mMusicVolume, mEffectsVolume);
					yield return new WaitForSeconds(0.05f);
				}
				if (mAudioSelection == 1 && mEffectsVolume > 0f)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					mEffectsVolume -= 0.1f;
					mEffectsVolumeBox.localScale = new Vector3(mEffectsVolume / 2f, 1f, 0f);
					SFXManager.Instance.SetVolumes(mMusicVolume, mEffectsVolume);
					yield return new WaitForSeconds(0.15f);
				}
			}
			if (y > 0f)
			{
				if (mAudioSelection > 0)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					UnHighlightWindow(oWindows[mAudioSelection]);
					mAudioSelection--;
					HighlightWindow(oWindows[mAudioSelection]);
				}
				yield return new WaitForSeconds(0.25f);
			}
			if (y < 0f)
			{
				if (mAudioSelection < oWindows.Length - 1)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					UnHighlightWindow(oWindows[mAudioSelection]);
					mAudioSelection++;
					HighlightWindow(oWindows[mAudioSelection]);
				}
				yield return new WaitForSeconds(0.25f);
			}
			if (InputManager.Instance.IsInteractPressed() && mAudioSelection == 2)
			{
				SFXManager.Instance.PlaySound("Confirm");
				audioOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				audioOptionsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
				if (mMusicVolume != storeMusicVolume)
				{
					PlayerPrefs.SetFloat("MusicVolume", mMusicVolume);
				}
				if (mEffectsVolume != storeEffectsVolume)
				{
					PlayerPrefs.SetFloat("EffectsVolume", mEffectsVolume);
				}
				yield return new WaitForSeconds(0.2f);
				StartCoroutine(RunOptionsMainMenu());
				mAudioOptionsOpen = false;
				UnHighlightWindow(oWindows[mAudioSelection]);
			}
			if (InputManager.Instance.IsPausePressed() || InputManager.Instance.IsPlannerPressed())
			{
				SFXManager.Instance.PlaySound("Confirm");
				audioOptionsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
				audioOptionsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
				SFXManager.Instance.SetVolumes(storeMusicVolume, storeEffectsVolume);
				yield return new WaitForSeconds(0.2f);
				StartCoroutine(RunOptionsMainMenu());
				mAudioOptionsOpen = false;
				UnHighlightWindow(oWindows[mAudioSelection]);
			}
		}
		audioOptionsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	private int GetCurrentResIndex()
	{
		for (int i = 0; i < mAvailableResolutions.Count; i++)
		{
			if (mAvailableResolutions[i].width == Screen.width && mAvailableResolutions[i].height == Screen.height)
			{
				return i;
			}
		}
		return 0;
	}

	private void CloseAreYouSure(bool backToMain)
	{
		mAreYouSureOpen = false;
		areYouSureMenu.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		areYouSureMenu.DOScale(0f, 0.1f).SetDelay(0.15f);
		if (backToMain)
		{
			Invoke("OpenPauseMenu", 0.2f);
		}
	}

	private void HighlightWindow(Transform t)
	{
		t.GetComponentInChildren<Outline>().enabled = true;
		t.DOScale(1.01f, 0.2f);
	}

	private void HighlightWindow(Transform t, float s)
	{
		Outline componentInChildren = t.GetComponentInChildren<Outline>();
		componentInChildren.enabled = true;
		componentInChildren.effectColor = Color.yellow;
		t.DOScale(s, 0.2f);
	}

	private void HighlightWindowGreen(Transform t, float s)
	{
		Outline componentInChildren = t.GetComponentInChildren<Outline>();
		componentInChildren.enabled = true;
		componentInChildren.effectColor = Color.green;
		t.DOScale(s, 0.2f);
	}

	private void UnHighlightWindow(Transform t)
	{
		Outline componentInChildren = t.GetComponentInChildren<Outline>();
		componentInChildren.enabled = false;
		componentInChildren.effectColor = Color.yellow;
		t.DOScale(1f, 0.2f);
	}

	private void UnHighlightButton(int x)
	{
		Transform child = pauseButtons.GetChild(x);
		child.GetComponentInChildren<Outline>().enabled = false;
		child.DOScale(1f, 0.2f);
	}

	private void HighlightButton(int x)
	{
		Transform child = pauseButtons.GetChild(x);
		child.DOScale(1.1f, 0.2f);
		child.GetComponentInChildren<Outline>().enabled = true;
	}

	private void UnHighlightHint(int x)
	{
		Transform child = hintGrid.GetChild(x);
		child.GetComponentInChildren<Outline>().enabled = false;
		child.DOScale(1f, 0.2f);
	}

	private void HighlightHint(int x)
	{
		Transform child = hintGrid.GetChild(x);
		child.DOScale(1.1f, 0.2f);
		child.GetComponentInChildren<Outline>().enabled = true;
	}
}
