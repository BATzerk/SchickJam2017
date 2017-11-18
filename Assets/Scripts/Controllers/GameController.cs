using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	// Properties
	// References
	[SerializeField] private GameCameraController cameraController;
	[SerializeField] private Player player;
	private EventManager eventManager;

	// Getters / Setters
	public Player Player { get { return player; } }



	// ----------------------------------------------------------------
	//  Start / Destroy
	// ----------------------------------------------------------------
	private void Start () {
		// Set application values
		Application.targetFrameRate = GameProperties.TARGET_FRAME_RATE;

		// Reset things!
		eventManager = GameManagers.Instance.EventManager;
	}



	// ----------------------------------------------------------------
	//  Doers
	// ----------------------------------------------------------------
	private void ReloadScene () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (SceneNames.GameScene);
	}
	private void TogglePause () {
		Time.timeScale = Time.timeScale==0 ? 1 : 0;
	}



	// ----------------------------------------------------------------
	//  Events
	// ----------------------------------------------------------------



	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		RegisterButtonInput ();
	}
	private void RegisterButtonInput () {
		// ENTER = Start a new game
		if (Input.GetKeyDown (KeyCode.Return)) {
			ReloadScene ();
		}
		// Action Button
		else if (InputController.IsButtonDown_Action) {
			OnButtonDown_Action ();
		}
		// P = Toggle pause
		else if (Input.GetKeyDown (KeyCode.P)) {
			TogglePause ();
		}
	}

	private void OnButtonDown_Action () {
		
	}






}


