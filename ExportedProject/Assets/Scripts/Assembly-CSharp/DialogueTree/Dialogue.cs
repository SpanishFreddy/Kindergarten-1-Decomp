using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace DialogueTree
{
	public class Dialogue
	{
		public List<DialogueNode> Nodes;

		public int currentConversation;

		public Dialogue()
		{
			Nodes = new List<DialogueNode>();
		}

		public static Dialogue LoadDialogue(string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Dialogue));
			StreamReader textReader = new StreamReader(path);
			return (Dialogue)xmlSerializer.Deserialize(textReader);
		}

		public static Dialogue LoadDialogue(TextAsset ta)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Dialogue));
			StringReader textReader = new StringReader(ta.text);
			return (Dialogue)xmlSerializer.Deserialize(textReader);
		}

		public DialogueNode GetCurrentNode()
		{
			return Nodes[currentConversation];
		}

		public static Dialogue Clone(Dialogue original)
		{
			Dialogue dialogue = new Dialogue();
			dialogue.currentConversation = original.currentConversation;
			foreach (DialogueNode node in original.Nodes)
			{
				dialogue.Nodes.Add(DialogueNode.Clone(node));
			}
			return dialogue;
		}
	}
}
