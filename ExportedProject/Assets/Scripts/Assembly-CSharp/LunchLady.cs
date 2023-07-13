using UnityEngine;

public class LunchLady : NPCBehavior
{
	private int mStoreConvo;

	private bool mDistracted;

	private int mSlops;

	private void GetSlop()
	{
		if (player.HasItem(Item.Slop))
		{
			if (!EnvironmentController.Instance.isSpanish)
			{
				conversations[Room.Cafeteria].Nodes[1].DialogueText = "Woah woah woah there kiddo! You haven't finished the last helping of slop I gave you! \\hYou can come get more once you've eaten what you already have!/h";
			}
			else
			{
				conversations[Room.Cafeteria].Nodes[1].DialogueText = "Aún no has terminado. ¡Vuelve cuando tengas!";
			}
			conversations[Room.Cafeteria].Nodes[1].Options[0].OptionText = "Oh.";
		}
		else if (mSlops == 0)
		{
			mSlops++;
			if (!EnvironmentController.Instance.isSpanish)
			{
				conversations[Room.Cafeteria].Nodes[1].DialogueText = "There ya go buddy boy, and don't be shy about coming back for seconds. Plenty of slop to go around!";
				conversations[Room.Cafeteria].Nodes[1].Options[0].OptionText = "Thanks.";
				conversations[Room.Cafeteria].Nodes[0].DialogueText = "How you doing there, sonny? Want some more tasty nutritious slop for lunch?";
			}
			else
			{
				conversations[Room.Cafeteria].Nodes[1].DialogueText = "¡Ahí tienes! ¡Vuelve por unos segundos!";
				conversations[Room.Cafeteria].Nodes[1].Options[0].OptionText = "Gracias.";
				conversations[Room.Cafeteria].Nodes[0].DialogueText = "¿Cómo estás, hijito? ¿Gustas un poco deliciosa y nutritiva basura para el almuerzo?";
			}
			player.GetItem(Item.Slop);
		}
		else if (mSlops == 1)
		{
			mSlops++;
			if (!EnvironmentController.Instance.isSpanish)
			{
				conversations[Room.Cafeteria].Nodes[1].DialogueText = "I'm flattered you want seconds! There you go! Come back once you've finished that if you want!";
				conversations[Room.Cafeteria].Nodes[1].Options[0].OptionText = "Thanks.";
			}
			else
			{
				conversations[Room.Cafeteria].Nodes[1].DialogueText = "¡Estoy feliz de que quieras más! ¡Aqui tienes!";
				conversations[Room.Cafeteria].Nodes[1].Options[0].OptionText = "Gracias!";
			}
			player.GetItem(Item.Slop);
		}
		else
		{
			if (!EnvironmentController.Instance.isSpanish)
			{
				conversations[Room.Cafeteria].Nodes[1].DialogueText = "My word...~even Buggs doesn't eat my slop with such enthusiasm.";
				conversations[Room.Cafeteria].Nodes[1].Options[0].OptionText = "It's delicious.";
			}
			else
			{
				conversations[Room.Cafeteria].Nodes[1].DialogueText = "¡Guau! Usted realmente ama mi comida!";
				conversations[Room.Cafeteria].Nodes[1].Options[0].OptionText = "Es bueno.";
			}
			conversations[Room.Cafeteria].Nodes[1].Options[0].FunctionCall = "GetCakeCard";
			conversations[Room.Cafeteria].Nodes[1].Options[0].DestinationID = 24;
			player.GetItem(Item.Slop);
		}
	}

	public void GetCakeCard()
	{
		player.GetItem(Item.UneatenCake);
		UnlockHint("Monstermon", 20);
	}

	public void SetDistracted(bool x)
	{
		mDistracted = x;
	}

	public void StopPlayerLeaving()
	{
		if (mDistracted || UI.IsMissionActive(Mission.LilyMeetInBathroom))
		{
			EnvironmentController.Instance.SetSpawnPoint(2);
			EnvironmentController.Instance.ChangeEnvironment(Room.Hallway);
		}
		else
		{
			mStoreConvo = conversations[Room.Cafeteria].currentConversation;
			SetCurrentConversation(9);
			Interact();
		}
	}

	private void ResetConvo()
	{
		SetCurrentConversation(mStoreConvo);
	}

	public void CheckSendToRecess()
	{
		if (EnvironmentController.Instance.ActionsLeft() != 1)
		{
			return;
		}
		NPCBehavior[] array = Object.FindObjectsOfType<NPCBehavior>();
		foreach (NPCBehavior nPCBehavior in array)
		{
			if (nPCBehavior != this)
			{
				nPCBehavior.transform.position += new Vector3(0f, 50f, 0f);
				continue;
			}
			nPCBehavior.transform.position = new Vector3(-1.9f, -5.6f, 0f);
			nPCBehavior.SetDepth();
		}
		if (UI.IsMissionActive(Mission.CindyGoBackToCafeteria))
		{
			CompleteMission(Mission.CindyGoBackToCafeteria);
			Object.FindObjectOfType<Cindy>().ActivateMission(Mission.CindyShowBucket, Room.Recess);
		}
		SetCurrentConversation(11);
		Interact();
	}

	private void GiveGlasses()
	{
		player.UseItem(Item.MontyGlasses);
		conversations[Room.Cafeteria].Nodes[0].Options[3].isComplete = true;
		UnlockHint("General", 4);
		mDistracted = true;
	}

	private void GiveMagnifyingGlass()
	{
		player.UseItem(Item.MagnifyingGlass);
		conversations[Room.Cafeteria].Nodes[0].Options[3].isComplete = true;
		mDistracted = true;
	}

	private void GetChocolate()
	{
		player.GetItem(Item.ChocolateBar);
		conversations[Room.Cafeteria].Nodes[0].Options[2].IsAvailable = false;
		conversations[Room.Cafeteria].Nodes[18].Options[0].IsAvailable = false;
		conversations[Room.Cafeteria].Nodes[20].Options[0].IsAvailable = false;
		conversations[Room.Cafeteria].Nodes[21].Options[0].IsAvailable = false;
	}

	private void PaidOff()
	{
		mDistracted = true;
		conversations[Room.Cafeteria].Nodes[0].Options[3].isComplete = true;
	}

	private void GoToRecess()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Recess);
	}

	public void EnterCafeteria()
	{
		EnvironmentController.Instance.ChangeEnvironment(Room.Cafeteria);
	}
}
