using DialogueTree;
using UnityEngine;

public class ObjectInteractable : Interactable
{
	public string startFunction;

	public Dialogue dialogue;

	public string NPC;

	public bool activateOnTouch;

	public bool manualSetDepth;

	public override void Start()
	{
		base.Start();
		TextAsset textAsset = (EnvironmentController.Instance.isSpanish ? (Resources.Load("XMLSpanish/Objects/" + base.transform.parent.parent.name + "/" + base.transform.name, typeof(TextAsset)) as TextAsset) : (Resources.Load("XML/Objects/" + base.transform.parent.parent.name + "/" + base.transform.name, typeof(TextAsset)) as TextAsset));
		if (textAsset != null)
		{
			dialogue = Dialogue.LoadDialogue(textAsset);
		}
		else if (startFunction.Length == 0)
		{
			Debug.LogWarning(base.transform.name + " does not have any functions or a valid xml file to read from.");
		}
		if (!manualSetDepth)
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, GetComponent<Collider2D>().bounds.min.y);
		}
	}

	public override bool ActivateOnTouch()
	{
		return activateOnTouch;
	}

	public override void Interact()
	{
		if (startFunction.Length > 0)
		{
			GameObject.Find(NPC).GetComponent<Interactable>().Invoke(startFunction, 0f);
			player.SetInteractable(null);
		}
		else
		{
			DoNormalInteract();
		}
	}

	public override void DoNormalInteract()
	{
		UI.StartDialogue(dialogue.GetCurrentNode().DialogueText);
		Invoke("UpdateOptions", 0.5f);
		player.inDialogue = true;
	}

	public override void ExecuteBaseEvent(int x)
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
		if (!string.IsNullOrEmpty(lastSelectedOption.FunctionCall))
		{
			GameObject gameObject = GameObject.Find(NPC);
			if (!gameObject)
			{
				Invoke(lastSelectedOption.FunctionCall, 0f);
			}
			else
			{
				gameObject.GetComponent<Interactable>().Invoke(lastSelectedOption.FunctionCall, 0f);
			}
		}
		Invoke("DoBaseFunction", 0f);
	}

	public virtual void DoBaseFunction()
	{
		if (lastSelectedOption.DestinationID > -1)
		{
			dialogue.currentConversation = lastSelectedOption.DestinationID;
			UI.StartDialogue(dialogue.GetCurrentNode().DialogueText);
		}
		else
		{
			if (lastSelectedOption.DestinationID != -1)
			{
				player.SetInteractable(null);
				StartCoroutine(UI.ShowOptions(false));
				return;
			}
			UI.CollapseDialogue();
			player.SetInteractable(null);
			if (lastSelectedOption.ResolutionID > 0)
			{
				dialogue.currentConversation = lastSelectedOption.ResolutionID;
			}
			else if (lastSelectedOption.ResolutionID == -1)
			{
				dialogue.currentConversation = 0;
			}
		}
		StartCoroutine(UI.ShowOptions(false));
		Invoke("UpdateOptions", 0.5f);
	}

	public override DialogueOption[] GetCurrentOptions()
	{
		return dialogue.GetCurrentNode().Options.ToArray();
	}

	public override int GetOptionCount()
	{
		try
		{
			return GetCurrentOptions().Length;
		}
		catch
		{
			return 0;
		}
	}

	private void StartGame()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.SchoolYard);
	}

	public override Dialogue GetDialogue()
	{
		return dialogue;
	}
}
