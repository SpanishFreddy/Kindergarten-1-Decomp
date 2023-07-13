using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DialogueTree
{
	public class DialogueNode : IComparable<DialogueNode>
	{
		[XmlAttribute("name")]
		public string name;

		public int NodeID = -1;

		public string DialogueText;

		public List<DialogueOption> Options;

		public DialogueNode()
		{
			Options = new List<DialogueOption>();
		}

		public DialogueNode(string text)
		{
			DialogueText = text;
			Options = new List<DialogueOption>();
		}

		public int CompareTo(DialogueNode x)
		{
			return NodeID.CompareTo(x.NodeID);
		}

		public static DialogueNode Clone(DialogueNode original)
		{
			DialogueNode dialogueNode = new DialogueNode();
			dialogueNode.name = original.name;
			dialogueNode.NodeID = original.NodeID;
			dialogueNode.DialogueText = original.DialogueText;
			foreach (DialogueOption option in original.Options)
			{
				dialogueNode.Options.Add(DialogueOption.Clone(option));
			}
			return dialogueNode;
		}
	}
}
