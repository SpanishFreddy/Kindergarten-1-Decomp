namespace DialogueTree
{
	public class DialogueOption
	{
		public string OptionText;

		public int DestinationID;

		public int ResolutionID;

		public string FunctionCall;

		public float MoneyLock;

		public Mission MissionLock;

		public Item ItemToUse;

		public bool IsAvailable = true;

		public bool UsesAction;

		public bool isComplete;

		public DialogueOption()
		{
		}

		public DialogueOption(string t)
		{
			OptionText = t;
			DestinationID = -1;
		}

		public void SetAvailable()
		{
			IsAvailable = true;
		}

		public static DialogueOption Clone(DialogueOption original)
		{
			DialogueOption dialogueOption = new DialogueOption();
			dialogueOption.OptionText = original.OptionText;
			dialogueOption.DestinationID = original.DestinationID;
			dialogueOption.ResolutionID = original.ResolutionID;
			dialogueOption.FunctionCall = original.FunctionCall;
			dialogueOption.MoneyLock = original.MoneyLock;
			dialogueOption.MissionLock = original.MissionLock;
			dialogueOption.ItemToUse = original.ItemToUse;
			dialogueOption.IsAvailable = original.IsAvailable;
			dialogueOption.UsesAction = original.UsesAction;
			return dialogueOption;
		}
	}
}
