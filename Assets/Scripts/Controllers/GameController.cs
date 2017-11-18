using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	// Properties
	// References
	[SerializeField] private Blob blob;
	[SerializeField] private DebrisController debrisController;
	[SerializeField] private GameCameraController cameraController;
	[SerializeField] private Paddle[] paddles;
	private EventManager eventManager;

	// Getters / Setters
//	public Player Player { get { return player; } }



	// ----------------------------------------------------------------
	//  Start / Destroy
	// ----------------------------------------------------------------
	private void Start () {
		// Set application values
		Application.targetFrameRate = GameProperties.TARGET_FRAME_RATE;

		// Reset things!
		eventManager = GameManagers.Instance.EventManager;

		StartNewGame ();
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
	private void StartNewGame () {
		blob.Reset ();
		debrisController.Reset ();
		for (int i=0; i<paddles.Length; i++) {
			paddles[i].Reset (i);
		}
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
//		// Action Button
//		else if (InputController.IsButtonDown_Action) {
//			OnButtonDown_Action ();
//		}
		// P = Toggle pause
		else if (Input.GetKeyDown (KeyCode.Escape)) {
			TogglePause ();
		}
	}






}


