using System.Collections;
using DialogueTree;
using UnityEngine;

public class UnlockableObject : ObjectInteractable
{
	private bool mViewed;

	public bool unlocked;

	public override void Start()
	{
		UI = Object.FindObjectOfType<UIController>();
		player = Object.FindObjectOfType<PlayerController>();
		TextAsset textAsset = (EnvironmentController.Instance.isSpanish ? (Resources.Load("XMLSpanish/Objects/UnlockableObjects/" + base.transform.name, typeof(TextAsset)) as TextAsset) : (Resources.Load("XML/Objects/UnlockableObjects/" + base.transform.name, typeof(TextAsset)) as TextAsset));
		if (textAsset != null)
		{
			dialogue = Dialogue.LoadDialogue(textAsset);
		}
		else if (startFunction.Length == 0)
		{
			Debug.LogWarning(base.transform.name + " does not have any functions or a valid xml file to read from.");
		}
		int @int = PlayerPrefs.GetInt("Unlock" + base.transform.name + EnvironmentController.Instance.GetFileModifier(), 0);
		if (@int == 0)
		{
			GetComponent<SpriteRenderer>().color = Color.black;
			GetComponent<Collider2D>().enabled = false;
		}
		if (@int == 1)
		{
			StartCoroutine(SpawnNewTextPrefab());
			mViewed = false;
			unlocked = true;
		}
		if (@int == 2)
		{
			mViewed = true;
			unlocked = true;
		}
	}

	private IEnumerator SpawnNewTextPrefab()
	{
		yield return null;
		GameObject g = Object.Instantiate(GameObject.Find("NewTextPrefab"));
		g.transform.parent = base.transform;
		g.transform.localPosition = new Vector3(0f, 1.65f, -0.01f);
	}

	public override void Interact()
	{
		base.Interact();
		if (!mViewed)
		{
			PlayerPrefs.SetInt("Unlock" + base.transform.name + EnvironmentController.Instance.GetFileModifier(), 2);
			Object.Destroy(base.transform.GetChild(0).gameObject);
			mViewed = true;
		}
	}
}
