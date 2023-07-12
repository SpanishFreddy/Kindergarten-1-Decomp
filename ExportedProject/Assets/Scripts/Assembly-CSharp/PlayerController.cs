using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class PlayerController : MonoBehaviour
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

	public static PlayerController instance;

	public float moveSpeed;

	public bool lockActions;

	private SkeletonAnimation mSkeleton;

	[HideInInspector]
	public bool inDialogue;

	[HideInInspector]
	public bool optionsMode;

	private UIController mUI;

	private List<Item> mInventory = new List<Item>();

	private Interactable mInteractable;

	private GameObject mInteractableArrow;

	private float previousInput;

	private BoxCollider2D mCollider;

	private Rigidbody2D mRigidBody;

	public float money = 1.5f;

	public static bool roomTransition;

	[HideInInspector]
	public bool inAnim;

	private PauseMenu mPauseMenu;

	private bool mOverrideAnim;

	private bool mDisableGravity;

	public Sprite face;

	public Sprite faceMO;

	public TextureBlock defaultTextureBlock;

	public TextureBlock beaten;

	public TextureBlock bloody;

	public TextureBlock bruised;

	public TextureBlock bruisedBloody;

	public Texture bruisedTexture;

	public Texture holeInHeadTexture;

	public Texture bloodyTexture;

	public Texture bloodTexture;

	public Texture pukingTexture;

	public Texture stabbed;

	private TextureBlock mCurrentTextureBlock;

	private TextureBlock mStoreTextureBlock;

	private float mStoreMoney;

	private List<Item> mStoreInventory = new List<Item>();

	private int mShowAndTellSelection;

	private bool mInShowAndTell;

	private bool mIsMoving
	{
		get
		{
			return (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f) && !inDialogue && !roomTransition;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		mCollider = GetComponent<BoxCollider2D>();
		mSkeleton = GetComponent<SkeletonAnimation>();
		mRigidBody = GetComponent<Rigidbody2D>();
		mSkeleton.skeleton.FlipX = true;
		mUI = UnityEngine.Object.FindObjectOfType<UIController>();
		mPauseMenu = UnityEngine.Object.FindObjectOfType<PauseMenu>();
		mInteractableArrow = GameObject.Find("InteractableArrow");
		mCurrentTextureBlock = defaultTextureBlock;
		SetDepth();
	}

	public void SetShowAndTell(Transform t)
	{
		int siblingIndex = t.GetSiblingIndex();
		if (siblingIndex != mShowAndTellSelection && mInShowAndTell)
		{
			mUI.HighlightItem(siblingIndex, mShowAndTellSelection);
			mShowAndTellSelection = siblingIndex;
		}
	}

	public IEnumerator SelectShowAndTellItem()
	{
		inAnim = true;
		mInShowAndTell = true;
		previousInput = 0f;
		mShowAndTellSelection = 0;
		int inventorCount = mInventory.Count;
		mUI.HighlightItem(0, 0);
		mUI.ShowShowAndTellPanel(true);
		while (inAnim)
		{
			if (!mPauseMenu.IsOpen)
			{
				previousInput = Input.GetAxisRaw("Horizontal");
				if (previousInput > 0f)
				{
					if (mShowAndTellSelection < inventorCount - 1)
					{
						mShowAndTellSelection++;
						mUI.HighlightItem(mShowAndTellSelection, mShowAndTellSelection - 1);
						yield return new WaitForSeconds(0.25f);
					}
				}
				else if (previousInput < 0f && mShowAndTellSelection > 0)
				{
					mShowAndTellSelection--;
					mUI.HighlightItem(mShowAndTellSelection, mShowAndTellSelection + 1);
					yield return new WaitForSeconds(0.25f);
				}
				if (InputManager.Instance.IsInteractPressed())
				{
					inAnim = false;
					SFXManager.Instance.PlaySound("Confirm");
				}
			}
			yield return null;
		}
		mInShowAndTell = false;
		mUI.ShowShowAndTellPanel(false);
		StartCoroutine(UnityEngine.Object.FindObjectOfType<Teacher>().PlayShowAndTell(mInventory[mShowAndTellSelection]));
	}

	public void SetDepth()
	{
		if ((bool)mCollider)
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, mCollider.bounds.min.y);
		}
		else
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, GetComponent<BoxCollider2D>().bounds.min.y);
		}
	}

	private void Update()
	{
		UpdateAnimator();
		if (roomTransition)
		{
			mInteractableArrow.SetActive(false);
		}
		else if (!inDialogue && !inAnim && !roomTransition)
		{
			HandleWalkState();
		}
		else
		{
			HandleOptionsState();
		}
	}

	private void FixedUpdate()
	{
		if (mIsMoving && !roomTransition && !mUI.showPlanner && !inAnim && !mPauseMenu.IsOpen && !mUI.IsTutorialOpen())
		{
			HandleMovement();
		}
		else if (!mDisableGravity)
		{
			mRigidBody.velocity = Vector3.zero;
		}
	}

	public void HandleWalkState()
	{
		if (InputManager.Instance.IsInteractPressed() && (bool)mInteractable && !mUI.transitioning && !mUI.showPlanner && !mPauseMenu.IsOpen && !mUI.IsTutorialOpen())
		{
			mSkeleton.AnimationName = "idle";
			mSkeleton.skeleton.flipX = base.transform.position.x < mInteractable.transform.position.x;
			mInteractable.Interact();
		}
		if (InputManager.Instance.IsActionPressed() && !mUI.transitioning && !mUI.showPlanner && !mPauseMenu.IsOpen && !inDialogue && !inAnim && !mUI.IsDialogueOut() && !mUI.IsTutorialOpen())
		{
			if (!lockActions)
			{
				GetMoney(0.25f);
				EnvironmentController.Instance.DecreaseActions();
			}
			else
			{
				SFXManager.Instance.PlaySound("Failed");
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((bool)collision.gameObject.GetComponent<Interactable>() && !inDialogue && !inAnim && !inDialogue)
		{
			mInteractable = collision.gameObject.GetComponent<Interactable>();
			mInteractableArrow.SetActive(true);
			if ((bool)mInteractable.GetComponent<Renderer>())
			{
				mInteractableArrow.transform.position = new Vector3(mInteractable.transform.position.x, mInteractable.GetComponent<Renderer>().bounds.max.y, -25f);
			}
			else
			{
				mInteractableArrow.transform.position = new Vector3(mInteractable.transform.position.x, mInteractable.GetComponent<Collider2D>().bounds.max.y, -25f);
			}
			if (collision.transform.name == "HallMonitor" && EnvironmentController.currentRoom == Room.Hallway)
			{
				StartCoroutine(MoveExclamationPointToMonitor());
			}
		}
	}

	private IEnumerator MoveExclamationPointToMonitor()
	{
		while ((bool)mInteractable)
		{
			if (mInteractable.transform.name == "HallMonitor")
			{
				bool activeSelf = mInteractableArrow.activeSelf;
				if (Input.GetAxis("Vertical") != 0f)
				{
					if (activeSelf)
					{
						mInteractableArrow.SetActive(false);
					}
				}
				else if (!activeSelf)
				{
					mInteractableArrow.SetActive(true);
					mInteractableArrow.transform.position = new Vector3(mInteractable.transform.position.x, mInteractable.GetComponent<Renderer>().bounds.max.y, -25f);
				}
			}
			yield return null;
		}
	}

	private void OnCollisionExit2D(Collision2D c)
	{
		if (mInteractable == c.gameObject.GetComponent<Interactable>())
		{
			SetInteractable(null);
		}
	}

	private void OnTriggerEnter2D(Collider2D c)
	{
		if (!c.gameObject.GetComponent<Interactable>() || inDialogue || inAnim || inDialogue)
		{
			return;
		}
		mInteractable = c.gameObject.GetComponent<Interactable>();
		if (mInteractable.ActivateOnTouch())
		{
			mInteractable.Interact();
			return;
		}
		mInteractableArrow.SetActive(true);
		if ((bool)mInteractable.GetComponent<MeshRenderer>())
		{
			mInteractableArrow.transform.position = new Vector3(mInteractable.transform.position.x, mInteractable.GetComponent<MeshRenderer>().bounds.max.y, -9f);
		}
		else
		{
			mInteractableArrow.transform.position = new Vector3(mInteractable.transform.position.x, mInteractable.GetComponent<Collider2D>().bounds.max.y, -9f);
		}
	}

	private void OnTriggerExit2D(Collider2D c)
	{
		if (mInteractable == c.gameObject.GetComponent<Interactable>() && !InputManager.Instance.IsInteractPressed())
		{
			SetInteractable(null);
		}
	}

	private void HandleOptionsState()
	{
		if (!optionsMode || mUI.showPlanner || mPauseMenu.IsOpen || mUI.IsTutorialOpen())
		{
			return;
		}
		if (previousInput != Input.GetAxisRaw("Vertical"))
		{
			if ((previousInput > 0f && Input.GetAxisRaw("Vertical") > 0f) || (previousInput < 0f && Input.GetAxisRaw("Vertical") < 0f))
			{
				return;
			}
			previousInput = Input.GetAxisRaw("Vertical");
			if (previousInput != 0f)
			{
				if (previousInput > 0f)
				{
					mUI.SetOption(mUI.currentOption - 1);
				}
				else
				{
					mUI.SetOption(mUI.currentOption + 1);
				}
			}
		}
		if (InputManager.Instance.IsInteractPressed() && !mUI.transitioning && !inAnim && mInteractable != null)
		{
			optionsMode = false;
			mInteractable.ExecuteBaseEvent(mUI.currentOption);
		}
	}

	private void HandleMovement()
	{
		float axisRaw = Input.GetAxisRaw("Horizontal");
		float axisRaw2 = Input.GetAxisRaw("Vertical");
		mRigidBody.velocity = new Vector3(axisRaw, axisRaw2) * moveSpeed;
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, mCollider.bounds.min.y);
		if (axisRaw != 0f && mIsMoving)
		{
			mSkeleton.skeleton.flipX = axisRaw > 0f;
		}
	}

	private void UpdateAnimator()
	{
		if (!inAnim && !mOverrideAnim)
		{
			if (!mIsMoving || mUI.showPlanner || mPauseMenu.IsOpen || mUI.IsTutorialOpen())
			{
				mSkeleton.AnimationName = "idle";
			}
			else
			{
				mSkeleton.AnimationName = "run";
			}
		}
	}

	public void ForceAnim(bool walk)
	{
		if (!walk)
		{
			mSkeleton.AnimationName = "idle";
		}
		else
		{
			mSkeleton.AnimationName = "run";
		}
	}

	public void ForceChangeDirection(bool right)
	{
		mSkeleton.skeleton.flipX = right;
	}

	public bool HasItem(Item item)
	{
		return mInventory.Contains(item) || item == Item.None;
	}

	public int ItemCount()
	{
		return mInventory.Count;
	}

	public List<Item> MonstermonList()
	{
		List<Item> list = new List<Item>();
		foreach (Item item in mInventory)
		{
			IEnumerator enumerator2 = Enum.GetValues(typeof(MonsterMon)).GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					if (((MonsterMon)enumerator2.Current).ToString() == item.ToString())
					{
						list.Add(item);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator2 as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
		return list;
	}

	public void UseItem(Item item)
	{
		if (mInventory.Contains(item))
		{
			mUI.RemoveItem(mInventory.IndexOf(item));
			mInventory.Remove(item);
		}
		else
		{
			Debug.LogError("Tried to use item " + item.ToString() + " but it was not in player inventory.");
		}
	}

	public void GetItem(Item item)
	{
		mUI.AddItem(mInventory.Count, item);
		mInventory.Add(item);
	}

	public void SetInteractable(Interactable i)
	{
		mInteractable = i;
		if (i == null)
		{
			mInteractableArrow.SetActive(false);
		}
	}

	public Interactable GetInteractable()
	{
		return mInteractable;
	}

	public void PlayAnimation(string s)
	{
		mSkeleton.AnimationName = s;
	}

	public void CancelOverride()
	{
		mOverrideAnim = false;
	}

	public void EnableOverride()
	{
		mOverrideAnim = true;
	}

	public void GetMoney(float s)
	{
		money += s;
		mUI.UpdateMoneyText(true);
	}

	public void DisableGravity(bool x)
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -7.15f);
		base.gameObject.layer = 8;
		mDisableGravity = true;
		mRigidBody.gravityScale = 1f;
		mRigidBody.freezeRotation = false;
		if (x)
		{
			mRigidBody.AddTorque(60f);
		}
		else
		{
			mRigidBody.AddTorque(-60f);
		}
		mCollider.offset = new Vector2(0f, 1.25f);
		mCollider.size = new Vector2(1.3f, 2.5f);
	}

	public void ResetGravity()
	{
		base.gameObject.layer = 0;
		mDisableGravity = false;
		mRigidBody.gravityScale = 0f;
		mRigidBody.freezeRotation = true;
		base.transform.rotation = Quaternion.identity;
		mCollider.offset = new Vector2(0f, 0.25f);
		mCollider.size = new Vector2(1.2693f, 0.487f);
	}

	public int GetItemCount(Item item)
	{
		int num = 0;
		foreach (Item item2 in mInventory)
		{
			if (item2 == item)
			{
				num++;
			}
		}
		return num;
	}

	public SkeletonAnimation GetSkeleton()
	{
		return mSkeleton;
	}

	public void ActivateHoleInHead()
	{
		ApplyTexture(holeInHeadTexture);
	}

	public void ActivateStabbed()
	{
		ApplyTexture(stabbed);
	}

	public void ActivateBloody()
	{
		ApplyTexture(bloodyTexture);
	}

	public void ActivateBlood()
	{
		GetComponent<Renderer>().material.mainTexture = beaten.bodyTexture;
		face = beaten.face;
		faceMO = beaten.faceMO;
	}

	public void ActivatePuking()
	{
		ApplyTexture(pukingTexture);
	}

	public void ApplyTexture(Texture t)
	{
		GetComponent<Renderer>().material.mainTexture = t;
	}

	public void ApplyTextureBlock(TextureBlock t)
	{
		if (t != null)
		{
			mCurrentTextureBlock = t;
			GetComponent<Renderer>().material.mainTexture = t.bodyTexture;
			face = t.face;
			faceMO = t.faceMO;
		}
	}

	public void ActivateBloodStream()
	{
		GameObject.Find("PlayerBloodStream").GetComponent<ParticleSystem>().Play();
	}

	public void ActivateBloodStream(Vector3 pos)
	{
		GameObject.Find("PlayerBloodStream").transform.localPosition = pos;
		GameObject.Find("PlayerBloodStream").GetComponent<ParticleSystem>().Play();
	}

	public void ActivateBloodSplat()
	{
		GameObject.Find("PlayerBloodSplat").GetComponent<ParticleSystem>().Play();
	}

	public void ActivateBloodSplat(Vector3 pos)
	{
		GameObject.Find("PlayerBloodSplat").transform.localPosition = pos;
		GameObject.Find("PlayerBloodSplat").GetComponent<ParticleSystem>().Play();
	}

	public void ExplodePlayer(string s, string spa, bool beep)
	{
		StartCoroutine(ExplodeChar(s, spa, beep));
	}

	public void ExplodePlayer(string s, string spa)
	{
		StartCoroutine(ExplodeChar(s, spa, true));
	}

	private IEnumerator ExplodeChar(string s, string spa, bool beep)
	{
		inAnim = true;
		ForceAnim(false);
		mUI.showDeath = true;
		if (beep)
		{
			SFXManager.Instance.PlaySound("Beep");
			yield return new WaitForSeconds(0.5f);
			SFXManager.Instance.PlaySound("Beep");
			yield return new WaitForSeconds(0.5f);
			SFXManager.Instance.PlaySound("Beep");
			yield return new WaitForSeconds(0.5f);
		}
		SFXManager.Instance.PlaySound("Bang");
		if (HasItem(Item.Bug))
		{
			UseItem(Item.Bug);
		}
		UnityEngine.Object.FindObjectOfType<CameraFollow>().CameraShake(0.5f);
		GameObject explosion = UnityEngine.Object.Instantiate(GameObject.Find("CharacterExplosion"));
		Transform t = explosion.transform;
		t.transform.parent = GameObject.Find(EnvironmentController.currentRoom.ToString()).transform;
		t.transform.position = base.transform.position;
		GetComponent<Renderer>().enabled = false;
		GameObject.Find("Player/Shadow").GetComponent<SpriteRenderer>().enabled = false;
		t.GetComponent<ParticleSystem>().Play();
		int i = 0;
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				Rigidbody2D r = transform.GetComponent<Rigidbody2D>();
				r.isKinematic = false;
				r.gravityScale = 2f;
				switch (i)
				{
				case 0:
					transform.GetComponent<SpriteRenderer>().flipX = mSkeleton.skeleton.flipX;
					r.AddForceAtPosition(new Vector2(UnityEngine.Random.Range(-400, 400), 1000f), t.position + Vector3.up * 1.5f);
					r.AddTorque(UnityEngine.Random.Range(-100, 100));
					break;
				case 1:
					r.AddForceAtPosition(new Vector2(UnityEngine.Random.Range(-100, 100), 200f), t.position + Vector3.up * 1.5f);
					break;
				default:
					r.AddForceAtPosition(new Vector2(UnityEngine.Random.Range(-100, 100), 400f), t.position + Vector3.up * 1.5f);
					break;
				}
				i++;
			}
		}
		finally
		{
			IDisposable disposable;
			IDisposable disposable2 = (disposable = enumerator as IDisposable);
			if (disposable != null)
			{
				disposable2.Dispose();
			}
		}
		yield return new WaitForSeconds(3f);
		mUI.ShowDeath(s, spa);
	}

	public void StorePlayerValues()
	{
		mStoreTextureBlock = mCurrentTextureBlock;
		mStoreMoney = money;
		mStoreInventory.Clear();
		foreach (Item item in mInventory)
		{
			mStoreInventory.Add(item);
		}
	}

	public void ResetPlayerValues()
	{
		money = mStoreMoney;
		mUI.UpdateMoneyText(false);
		mInventory.Clear();
		foreach (Item item in mStoreInventory)
		{
			mInventory.Add(item);
		}
		base.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Renderer>().enabled = true;
		GetComponent<Renderer>().material.color = Color.white;
		GameObject.Find("Player/Shadow").GetComponent<SpriteRenderer>().enabled = true;
		mUI.ShowShowAndTellPanel(false);
		StopAllCoroutines();
		mUI.ValidateInventory(mInventory);
		inAnim = false;
		inDialogue = false;
		optionsMode = false;
		mOverrideAnim = false;
		mSkeleton.AnimationName = "idle";
		ResetGravity();
		SetInteractable(null);
		mSkeleton.timeScale = 1.5f;
		ApplyTextureBlock(mStoreTextureBlock);
	}
}
