using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
	public Transform backgrounds;

	private Transform[] mBackgroundArray;

	public Image faderPanel;

	public RectTransform pauseMenu;

	private bool mPlayGame;

	private int mSelection;

	private bool mRunMainScreen;

	private int mSaveFileChoice;

	private bool mInSaveFile;

	private void Start()
	{
		int childCount = backgrounds.childCount;
		mBackgroundArray = new Transform[childCount];
		for (int i = 0; i < childCount; i++)
		{
			mBackgroundArray[i] = backgrounds.GetChild(i);
		}
		StartCoroutine(CycleBackgrounds());
		faderPanel.color = new Color(0f, 0f, 0f, 1f);
		faderPanel.DOFade(0f, 3f);
		pauseMenu.localScale = Vector3.zero;
		StartCoroutine(RunMainScreen(true));
	}

	private IEnumerator CycleBackgrounds()
	{
		int x = 0;
		while (!mPlayGame)
		{
			RectTransform t = mBackgroundArray[x].GetComponent<RectTransform>();
			mBackgroundArray[x].GetComponent<CanvasGroup>().DOFade(1f, 1f);
			t.anchoredPosition = new Vector2(200f, 44f);
			t.DOAnchorPosX(-200f, 7f).SetEase(Ease.Linear);
			yield return new WaitForSeconds(6f);
			mBackgroundArray[x].GetComponent<CanvasGroup>().DOFade(0f, 1f);
			x++;
			if (x == backgrounds.childCount)
			{
				x = 0;
			}
		}
	}

	public void SetOnPlay(int x)
	{
		if (mSelection != x)
		{
			Transform transform = GameObject.Find("PlayButton").transform;
			Transform transform2 = GameObject.Find("QuitButton").transform;
			Transform transform3 = GameObject.Find("CreditsButton").transform;
			Transform[] array = new Transform[3] { transform, transform3, transform2 };
			SFXManager.Instance.PlaySound("DialoguePop");
			UnHighlightWindow(array[mSelection]);
			mSelection = x;
			HighlightWindow(array[mSelection], 1.1f);
		}
	}

	private IEnumerator RunMainScreen(bool firstTime)
	{
		mRunMainScreen = true;
		if (firstTime)
		{
			yield return new WaitForSeconds(3f);
		}
		SFXManager.Instance.PlaySound("DialoguePop");
		pauseMenu.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		pauseMenu.DOScale(1f, 0.1f).SetDelay(0.15f);
		Transform pauseButton = GameObject.Find("PlayButton").transform;
		Transform quitButton = GameObject.Find("QuitButton").transform;
		Transform creditsButton = GameObject.Find("CreditsButton").transform;
		Transform[] buttons = new Transform[3] { pauseButton, creditsButton, quitButton };
		yield return new WaitForSeconds(0.25f);
		StartCoroutine(RunGetMainScreenConfirm());
		while (mRunMainScreen)
		{
			if (Input.GetAxis("Vertical") > 0f)
			{
				if (mSelection != 0)
				{
					SFXManager.Instance.PlaySound("DialoguePop");
					UnHighlightWindow(buttons[mSelection]);
					mSelection--;
					HighlightWindow(buttons[mSelection], 1.1f);
					yield return new WaitForSeconds(0.25f);
				}
			}
			else if (Input.GetAxis("Vertical") < 0f && mSelection != 2)
			{
				SFXManager.Instance.PlaySound("DialoguePop");
				UnHighlightWindow(buttons[mSelection]);
				mSelection++;
				HighlightWindow(buttons[mSelection], 1.1f);
				yield return new WaitForSeconds(0.25f);
			}
			yield return null;
		}
	}

	private IEnumerator RunGetMainScreenConfirm()
	{
		while (mRunMainScreen)
		{
			if (InputManager.Instance.IsInteractPressed())
			{
				mRunMainScreen = false;
				SFXManager.Instance.PlaySound("Confirm");
				switch (mSelection)
				{
				case 0:
					CloseButtons();
					yield return new WaitForSeconds(0.5f);
					StartCoroutine(RunPickFile());
					break;
				case 1:
					CloseButtons();
					yield return new WaitForSeconds(0.3f);
					StartCoroutine(RunCredits());
					break;
				case 2:
					Application.Quit();
					break;
				}
			}
			yield return null;
		}
	}

	public void SetSaveFileChoice(int x)
	{
		Transform transform = GameObject.Find("SaveFilePanels").transform;
		if (mSaveFileChoice != x)
		{
			UnHighlightWindow(transform.GetChild(mSaveFileChoice));
			mSaveFileChoice = x;
			HighlightWindow(transform.GetChild(mSaveFileChoice), 1.1f);
			SFXManager.Instance.PlaySound("DialoguePop");
		}
	}

	private IEnumerator GetSaveFileChoice()
	{
		while (!InputManager.Instance.IsInteractPressed())
		{
			yield return null;
		}
		mInSaveFile = false;
		GameObject.Find("SaveFilePanels").GetComponent<CanvasGroup>().blocksRaycasts = false;
		SFXManager.Instance.PlaySound("Confirm");
		Transform savefilePanels = GameObject.Find("SaveFilePanels").transform;
		savefilePanels.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		savefilePanels.DOScale(0f, 0.1f).SetDelay(0.15f);
		switch (mSaveFileChoice)
		{
		case 0:
			PlayerPrefs.SetInt("SaveFile", 1);
			faderPanel.DOFade(1f, 1f);
			yield return new WaitForSeconds(1f);
			mPlayGame = true;
			SceneManager.LoadScene("Kindergarten");
			break;
		case 1:
			PlayerPrefs.SetInt("SaveFile", 2);
			faderPanel.DOFade(1f, 1f);
			yield return new WaitForSeconds(1f);
			mPlayGame = true;
			SceneManager.LoadScene("Kindergarten");
			break;
		case 2:
			UnHighlightWindow(savefilePanels.GetChild(2));
			yield return new WaitForSeconds(0.3f);
			StartCoroutine(RunMainScreen(false));
			break;
		}
	}

	private IEnumerator RunPickFile()
	{
		Transform savefilePanels = GameObject.Find("SaveFilePanels").transform;
		savefilePanels.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		savefilePanels.DOScale(1f, 0.1f).SetDelay(0.15f);
		yield return new WaitForSeconds(0.3f);
		savefilePanels.GetComponent<CanvasGroup>().blocksRaycasts = true;
		Transform[] saveOptions = new Transform[3]
		{
			savefilePanels.GetChild(0),
			savefilePanels.GetChild(1),
			savefilePanels.GetChild(2)
		};
		mInSaveFile = true;
		mSaveFileChoice = 0;
		HighlightWindow(saveOptions[mSaveFileChoice], 1.1f);
		SFXManager.Instance.PlaySound("DialoguePop");
		StartCoroutine(GetSaveFileChoice());
		while (mInSaveFile)
		{
			if (Input.GetAxis("Vertical") < 0f)
			{
				if (mSaveFileChoice < 2)
				{
					UnHighlightWindow(saveOptions[mSaveFileChoice]);
					mSaveFileChoice++;
					HighlightWindow(saveOptions[mSaveFileChoice], 1.1f);
					SFXManager.Instance.PlaySound("DialoguePop");
					yield return new WaitForSeconds(0.25f);
				}
			}
			else if (Input.GetAxis("Vertical") > 0f && mSaveFileChoice > 0)
			{
				UnHighlightWindow(saveOptions[mSaveFileChoice]);
				mSaveFileChoice--;
				HighlightWindow(saveOptions[mSaveFileChoice], 1.1f);
				SFXManager.Instance.PlaySound("DialoguePop");
				yield return new WaitForSeconds(0.25f);
			}
			yield return null;
		}
	}

	private IEnumerator RunCredits()
	{
		Transform creditsPanel = GameObject.Find("Credits").transform;
		creditsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		creditsPanel.DOScale(1f, 0.1f).SetDelay(0.15f);
		yield return new WaitForSeconds(0.5f);
		while (!InputManager.Instance.IsInteractPressed())
		{
			yield return null;
		}
		SFXManager.Instance.PlaySound("Confirm");
		creditsPanel.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		creditsPanel.DOScale(0f, 0.1f).SetDelay(0.15f);
		yield return new WaitForSeconds(0.3f);
		StartCoroutine(RunMainScreen(false));
	}

	private void CloseButtons()
	{
		pauseMenu.DOScale(new Vector3(0.2f, 1.3f, 1f), 0.15f);
		pauseMenu.DOScale(0f, 0.1f).SetDelay(0.15f);
	}

	private void HighlightWindow(Transform t, float s)
	{
		Outline componentInChildren = t.GetComponentInChildren<Outline>();
		componentInChildren.enabled = true;
		componentInChildren.effectColor = Color.yellow;
		t.DOScale(s, 0.2f);
	}

	private void UnHighlightWindow(Transform t)
	{
		Outline componentInChildren = t.GetComponentInChildren<Outline>();
		componentInChildren.enabled = false;
		componentInChildren.effectColor = Color.yellow;
		t.DOScale(1f, 0.2f);
	}
}
