using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DialogueTree;
using Spine.Unity;
using UnityEngine;

public class NPCBehavior : Interactable
{
	[Serializable]
	public class TextureBlock
	{
		public Texture2D bodyTexture;

		public Sprite face;

		public Sprite faceMO;

		public TextureBlock(Texture2D body, Sprite f, Sprite fMO)
		{
			bodyTexture = body;
			face = f;
			faceMO = fMO;
		}
	}

	public TextureBlock defaultTextureBlock;

	public TextureBlock currentTextureBlock;

	public Sprite face;

	public Sprite faceMO;

	public Color dialogueColor;

	protected Renderer pRend;

	protected Collider2D pCollider;

	public Dictionary<Room, Dialogue> conversations = new Dictionary<Room, Dialogue>();

	protected bool pEmptyOptions;

	public bool mEndDay;

	private PauseMenu mPauseMenu;

	[HideInInspector]
	public SkeletonAnimation mySkeleton;

	[HideInInspector]
	public bool removedFromGame;

	public void SetEndDay(bool x)
	{
		NPCBehavior[] array = UnityEngine.Object.FindObjectsOfType<NPCBehavior>();
		foreach (NPCBehavior nPCBehavior in array)
		{
			if (nPCBehavior.GetEndDay())
			{
				return;
			}
		}
		mEndDay = x;
	}

	public bool GetEndDay()
	{
		return mEndDay;
	}

	private void Awake()
	{
		mySkeleton = GetComponent<SkeletonAnimation>();
	}

	public override void Start()
	{
		base.Start();
		pRend = GetComponent<Renderer>();
		pCollider = GetComponent<Collider2D>();
		mPauseMenu = UnityEngine.Object.FindObjectOfType<PauseMenu>();
		if (conversations.Count == 0)
		{
			defaultTextureBlock = new TextureBlock(pRend.material.mainTexture as Texture2D, face, faceMO);
			currentTextureBlock = defaultTextureBlock;
			if (!EnvironmentController.Instance.isSpanish)
			{
				IEnumerator enumerator = Enum.GetValues(typeof(Room)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Room key = (Room)enumerator.Current;
						TextAsset textAsset = Resources.Load("XML/" + base.transform.name + "/" + base.transform.name + key, typeof(TextAsset)) as TextAsset;
						if (textAsset != null)
						{
							conversations.Add(key, Dialogue.LoadDialogue(textAsset));
							conversations[key].Nodes.Sort();
						}
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
			else
			{
				IEnumerator enumerator2 = Enum.GetValues(typeof(Room)).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Room key2 = (Room)enumerator2.Current;
						TextAsset textAsset2 = Resources.Load("XMLSpanish/" + base.transform.name + "/" + base.transform.name + key2, typeof(TextAsset)) as TextAsset;
						if (textAsset2 != null)
						{
							conversations.Add(key2, Dialogue.LoadDialogue(textAsset2));
							conversations[key2].Nodes.Sort();
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = enumerator2 as IDisposable) != null)
					{
						disposable2.Dispose();
					}
				}
			}
		}
		else
		{
			ApplyTextureBlock(currentTextureBlock);
		}
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, GetComponent<Collider2D>().bounds.min.y);
	}

	public void SetAnimation(string a)
	{
		mySkeleton.AnimationName = a;
	}

	public void SetDirection(bool setFlip)
	{
		mySkeleton.skeleton.flipX = setFlip;
	}

	private void CheckTurnAround()
	{
		mySkeleton.skeleton.flipX = player.transform.position.x > base.transform.position.x;
	}

	public override void Interact()
	{
		base.Interact();
		CheckTurnAround();
		UI.StartDialogue(GetCurrentConversation().DialogueText, this);
		Invoke("UpdateOptions", 0.5f);
		player.inDialogue = true;
	}

	public override int GetOptionCount()
	{
		return GetCurrentConversation().Options.Count;
	}

	public void DoBaseFunction()
	{
		if (lastSelectedOption.DestinationID > -1)
		{
			conversations[EnvironmentController.currentRoom].currentConversation = lastSelectedOption.DestinationID;
			UI.StartDialogue(GetCurrentConversation().DialogueText, this);
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
				conversations[EnvironmentController.currentRoom].currentConversation = lastSelectedOption.ResolutionID;
			}
			else if (lastSelectedOption.ResolutionID == -1)
			{
				conversations[EnvironmentController.currentRoom].currentConversation = 0;
			}
		}
		StartCoroutine(UI.ShowOptions(false));
		Invoke("UpdateOptions", 0.5f);
	}

	public DialogueNode GetCurrentConversation()
	{
		Dialogue value;
		if (conversations.TryGetValue(EnvironmentController.currentRoom, out value))
		{
			return value.Nodes[value.currentConversation];
		}
		return new DialogueNode("Error: Text not found");
	}

	public void SetCurrentConversation(int x)
	{
		if (conversations.ContainsKey(EnvironmentController.currentRoom))
		{
			conversations[EnvironmentController.currentRoom].currentConversation = x;
		}
	}

	public override DialogueOption[] GetCurrentOptions()
	{
		return GetCurrentConversation().Options.ToArray();
	}

	public void SetDepth()
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, GetComponent<BoxCollider2D>().bounds.min.y);
	}

	public void ActivateMission(Mission m)
	{
		StartCoroutine(SetMissionStatus(m, 0));
	}

	public void ActivateMission(Mission m, Room r)
	{
		StartCoroutine(SetMissionStatus(m, 0, r));
	}

	public void CompleteMission(Mission m)
	{
		StartCoroutine(SetMissionStatus(m, 1));
	}

	public void FailMission(Mission m)
	{
		StartCoroutine(SetMissionStatus(m, 2));
	}

	private IEnumerator SetMissionStatus(Mission m, int x)
	{
		while (UI.IsDialogueRunning() || UI.transitioning)
		{
			yield return null;
		}
		switch (x)
		{
		case 0:
			UI.AddMission(m, this, Room.None);
			break;
		case 1:
			UI.CompleteMission(m);
			break;
		case 2:
			UI.FailMission(m);
			break;
		}
	}

	private IEnumerator SetMissionStatus(Mission m, int x, Room r)
	{
		yield return null;
		while (UI.IsDialogueRunning() || UI.transitioning)
		{
			yield return null;
		}
		switch (x)
		{
		case 0:
			UI.AddMission(m, this, r);
			break;
		case 1:
			UI.CompleteMission(m);
			break;
		case 2:
			UI.FailMission(m);
			break;
		}
	}

	protected IEnumerator RunDialogueSection(string d, string sp, NPCBehavior n)
	{
		pEmptyOptions = true;
		if ((bool)n)
		{
			if (!EnvironmentController.Instance.isSpanish)
			{
				UI.StartDialogue(d, n);
			}
			else
			{
				UI.StartDialogue(sp, n);
			}
		}
		else if (!EnvironmentController.Instance.isSpanish)
		{
			UI.StartDialogue(d);
		}
		else
		{
			UI.StartDialogue(sp);
		}
		yield return new WaitForSeconds(0.5f);
		SetEmptyOptions();
		while (UI.IsDialogueRunning())
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		while (!InputManager.Instance.IsInteractPressed() || player.inAnim || UI.showPlanner || mPauseMenu.IsOpen)
		{
			yield return null;
		}
		SFXManager.Instance.PlaySound("Confirm");
		pEmptyOptions = false;
	}

	protected IEnumerator RunDialogueSection(string d, string sp, NPCBehavior n, bool cpu)
	{
		pEmptyOptions = true;
		if ((bool)n)
		{
			if (!EnvironmentController.Instance.isSpanish)
			{
				UI.StartDialogue(d, n, cpu);
			}
			else
			{
				UI.StartDialogue(sp, n, cpu);
			}
		}
		else if (!EnvironmentController.Instance.isSpanish)
		{
			UI.StartDialogue(d, null, cpu);
		}
		else
		{
			UI.StartDialogue(sp, null, cpu);
		}
		yield return new WaitForSeconds(0.5f);
		SetEmptyOptions();
		while (UI.IsDialogueRunning())
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		while (!InputManager.Instance.IsInteractPressed() || player.inAnim || UI.showPlanner)
		{
			yield return null;
		}
		SFXManager.Instance.PlaySound("Confirm");
		pEmptyOptions = false;
	}

	public void RemoveFromGame()
	{
		removedFromGame = true;
		pRend.enabled = false;
		pCollider.enabled = false;
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
	}

	public void RemoveFromGame(bool x)
	{
		removedFromGame = x;
		pRend.enabled = !x;
		pCollider.enabled = !x;
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = !x;
	}

	public void ApplyTextureBlock(TextureBlock t)
	{
		if (!pRend)
		{
			pRend = GetComponent<Renderer>();
		}
		currentTextureBlock = t;
		pRend.material.mainTexture = t.bodyTexture;
		face = t.face;
		faceMO = t.faceMO;
	}

	public void WalkToPoint(Vector3 v, float d)
	{
		SetAnimation("walk");
		player.inAnim = true;
		pCollider.enabled = false;
		base.transform.DOMove(v, d).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetAnimation("idle");
			player.inAnim = false;
			pCollider.enabled = true;
		})
			.OnUpdate(SetDepth);
	}

	public void WalkToPoint(Vector3 v, float d, bool x)
	{
		SetAnimation("walk");
		pCollider.enabled = false;
		base.transform.DOMove(v, d).SetEase(Ease.Linear).OnComplete(delegate
		{
			SetAnimation("idle");
			pCollider.enabled = true;
			SetDepth();
		});
	}

	public void FacePlayer()
	{
		SetDirection(player.transform.position.x > base.transform.position.x);
	}

	public virtual IEnumerator EndDayCoroutine()
	{
		yield return null;
	}

	public void CloneConversations(Dictionary<Room, Dialogue> original)
	{
		conversations.Clear();
		foreach (KeyValuePair<Room, Dialogue> item in original)
		{
			conversations.Add(item.Key, Dialogue.Clone(original[item.Key]));
		}
	}
}
