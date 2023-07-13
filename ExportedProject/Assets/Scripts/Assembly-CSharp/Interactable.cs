using DialogueTree;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	[HideInInspector]
	public UIController UI;

	[HideInInspector]
	public PlayerController player;

	public DialogueOption lastSelectedOption;

	public virtual void Start()
	{
		UI = Object.FindObjectOfType<UIController>();
		player = Object.FindObjectOfType<PlayerController>();
	}

	public virtual int GetOptionCount()
	{
		return 0;
	}

	public virtual void ExecuteBaseEvent(int x)
	{
		lastSelectedOption = GetCurrentOptions()[x];
		if (lastSelectedOption.UsesAction)
		{
			if (EnvironmentController.Instance.ActionsLeft() <= 0)
			{
				UI.ShakeOption();
				return;
			}
			if (player.money >= lastSelectedOption.MoneyLock)
			{
				EnvironmentController.Instance.DecreaseActions();
			}
		}
		if (lastSelectedOption.MoneyLock > 0f)
		{
			if (player.money < lastSelectedOption.MoneyLock)
			{
				UI.ShakeOption();
				return;
			}
			player.GetMoney(0f - lastSelectedOption.MoneyLock);
		}
		if (!string.IsNullOrEmpty(lastSelectedOption.FunctionCall))
		{
			Invoke(lastSelectedOption.FunctionCall, 0f);
		}
		Invoke("DoBaseFunction", 0f);
	}

	public virtual void Interact()
	{
		player.SetInteractable(this);
	}

	public virtual DialogueOption[] GetCurrentOptions()
	{
		return new DialogueOption[0];
	}

	public void UpdateOptions()
	{
		UI.UpdateOptionsPanel(GetCurrentOptions());
	}

	public void SetEmptyOptions()
	{
		UI.UpdateOptionsPanel(new DialogueOption[0]);
	}

	public virtual Dialogue GetDialogue()
	{
		return null;
	}

	public virtual void DoNormalInteract()
	{
	}

	public virtual bool ActivateOnTouch()
	{
		return false;
	}

	public void UnlockHint(string n, int x)
	{
		Object.FindObjectOfType<PauseMenu>().UnlockHint(n, x);
	}
}
