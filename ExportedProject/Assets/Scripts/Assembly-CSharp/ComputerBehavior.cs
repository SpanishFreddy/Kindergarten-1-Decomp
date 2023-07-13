public class ComputerBehavior : ObjectInteractable
{
	public override void DoNormalInteract()
	{
		UI.StartDialogue(dialogue.GetCurrentNode().DialogueText, null, true);
		Invoke("UpdateOptions", 0.5f);
		player.inDialogue = true;
	}

	public override void DoBaseFunction()
	{
		if (lastSelectedOption.DestinationID > -1)
		{
			dialogue.currentConversation = lastSelectedOption.DestinationID;
			UI.StartDialogue(dialogue.GetCurrentNode().DialogueText, null, true);
		}
		else
		{
			UI.CollapseDialogue();
			player.SetInteractable(null);
			if (lastSelectedOption.ResolutionID > 0)
			{
				dialogue.currentConversation = lastSelectedOption.ResolutionID;
			}
		}
		StartCoroutine(UI.ShowOptions(false));
		Invoke("UpdateOptions", 0.5f);
	}
}
