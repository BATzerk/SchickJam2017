using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	// References
	[SerializeField] private Blob blob;
	[SerializeField] private DebrisController debrisController;
	[SerializeField] private GameCameraController cameraController;
	[SerializeField] private PaddleController paddleController;
	private EventManager eventManager;



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
		cameraController.Reset ();
		blob.Reset ();
		debrisController.Reset ();
		paddleController.Reset ();
		AudioController.getSingleton().PlayBGSoundClip(SoundClipId.MUS_BACKGROUND_1);
	}




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


