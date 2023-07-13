using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager Instance;

	public bool customControls;

	public string interactButtonKeyboard;

	public string plannerButtonKeyboard;

	public string actionButtonKeyboard;

	public string pauseButtonKeyboard;

	public string interactButtonController;

	public string plannerButtonController;

	public string actionButtonController;

	public string pauseButtonController;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		customControls = PlayerPrefs.GetInt("CustomControls", 0) == 1;
		if (customControls)
		{
			GetCustomInputs();
		}
	}

	public void GetCustomInputs()
	{
		interactButtonKeyboard = PlayerPrefs.GetString("InteractButtonKeyboard", "Space");
		plannerButtonKeyboard = PlayerPrefs.GetString("PlannerButtonKeyboard", "Backslash");
		actionButtonKeyboard = PlayerPrefs.GetString("ActionButtonKeyboard", "Return");
		pauseButtonKeyboard = PlayerPrefs.GetString("PauseButtonKeyboard", "Escape");
		interactButtonController = PlayerPrefs.GetString("InteractButtonController", "Button 0");
		plannerButtonController = PlayerPrefs.GetString("PlannerButtonController", "Button 1");
		actionButtonController = PlayerPrefs.GetString("ActionButtonController", "Button 2");
		pauseButtonController = PlayerPrefs.GetString("PauseButtonController", "Button 7");
	}

	public void ApplyCustomInputs()
	{
		PlayerPrefs.SetString("InteractButtonKeyboard", interactButtonKeyboard);
		PlayerPrefs.SetString("PlannerButtonKeyboard", plannerButtonKeyboard);
		PlayerPrefs.SetString("ActionButtonKeyboard", actionButtonKeyboard);
		PlayerPrefs.SetString("PauseButtonKeyboard", pauseButtonKeyboard);
		PlayerPrefs.SetString("InteractButtonController", interactButtonController);
		PlayerPrefs.SetString("PlannerButtonController", plannerButtonController);
		PlayerPrefs.SetString("ActionButtonController", actionButtonController);
		PlayerPrefs.SetString("PauseButtonController", pauseButtonController);
		PlayerPrefs.SetInt("CustomControls", 1);
	}

	private void Update()
	{
	}

	public string[] GetInputStrings()
	{
		return new string[8] { interactButtonKeyboard, interactButtonController, plannerButtonKeyboard, plannerButtonController, actionButtonKeyboard, actionButtonController, pauseButtonKeyboard, pauseButtonController };
	}

	public bool IsInteractPressed()
	{
		return Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), interactButtonKeyboard)) || Input.GetButtonDown(interactButtonController) || Input.GetMouseButtonDown(0);
	}

	public bool IsInteractHeld()
	{
		return Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), interactButtonKeyboard)) || Input.GetButton(interactButtonController) || Input.GetMouseButton(0);
	}

	public bool IsPlannerPressed()
	{
		return Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), plannerButtonKeyboard)) || Input.GetButtonDown(plannerButtonController);
	}

	public bool IsActionPressed()
	{
		return Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), actionButtonKeyboard)) || Input.GetButtonDown(actionButtonController);
	}

	public bool IsPausePressed()
	{
		return Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), pauseButtonKeyboard)) || Input.GetButtonDown(pauseButtonController);
	}

	public void ReconfigureInteractKeyboard(string x)
	{
		interactButtonKeyboard = x;
	}

	public void ReconfigurePlannerKeyboard(string x)
	{
		plannerButtonKeyboard = x;
	}

	public void ReconfigureActionKeyboard(string x)
	{
		actionButtonKeyboard = x;
	}

	public void ReconfigurePauseKeyboard(string x)
	{
		pauseButtonKeyboard = x;
	}

	public void ReconfigureInteractController(string x)
	{
		interactButtonController = x;
	}

	public void ReconfigurePlannerController(string x)
	{
		plannerButtonController = x;
	}

	public void ReconfigureActionController(string x)
	{
		actionButtonController = x;
	}

	public void ReconfigurePauseController(string x)
	{
		pauseButtonController = x;
	}
}
