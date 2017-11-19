using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public enum GameState { GAMEOVER, PLAYING, TRANSITION }
	static public GameState gameState = GameState.GAMEOVER;
	// References
	[SerializeField] private Blob blob;
	[SerializeField] private DebrisController debrisController;
	[SerializeField] private GameCameraController cameraController;
	[SerializeField] private PaddleController paddleController;
	[SerializeField] private WinnerLoserUI winnerLoserUI;
	private EventManager eventManager;
	// Properties
	static float score;
	float highscore = 0;
	[SerializeField] private GameObject viewTitle;
	[SerializeField] private Text textScore;
	[SerializeField] private Text textHighScore;

	// Getters
	public int GuiltyPlayer;
	public bool IsGameOver { get { return gameState==GameState.GAMEOVER; } }


	// ----------------------------------------------------------------
	//  Start / Destroy
	// ----------------------------------------------------------------
	private void Start () {
		// Set application values
		Application.targetFrameRate = GameProperties.TARGET_FRAME_RATE;

		// Reset things!
		eventManager = GameManagers.Instance.EventManager;

		StartNewGame ();
		GameOver();
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
		// Reset values.
		GuiltyPlayer = -1; // no one is guilty!!!!
		Time.timeScale = 1f;

		// Reset peeps.
		winnerLoserUI.Reset ();
		cameraController.Reset ();
		blob.Reset ();
		debrisController.Reset ();
		paddleController.Reset ();
		AudioController.getSingleton().PlayBGSoundClip(SoundClipId.MUS_BACKGROUND_1, 0.8f);
	}


	/*
	 * Wait for player input to start
	 * If red hits core game overs
	 * game over has time out before next round starts 3 seconds.
	 * Highscore??
	 */
	public void GameOver(){
		gameState = GameState.TRANSITION;
		viewTitle.SetActive( true);
		winnerLoserUI.OnGameOver (GuiltyPlayer);
		DestroyAnyDriftingDebri ();

		Invoke("DelayRestartComplete",2);
		// TODO: Delay then restart
		AudioController.getSingleton().PlayBGSoundClip(SoundClipId.MUS_BACKGROUND_1, 0.2f);
		AudioController.getSingleton ().PlaySFX(SoundClipId.SFX_GAME_OVER);
	}

	private void DestroyAnyDriftingDebri () {
		// Brute-force it!
		Debri[] allDebri = GameObject.FindObjectsOfType<Debri>();
		for (int i=allDebri.Length-1; i>=0; --i) {
			if (allDebri[i].CurrentState == Debri.States.Drifting) {
				Destroy (allDebri[i].gameObject);
			}
		}
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		RegisterButtonInput ();

		if(blob.life <= 0){
			// GameOver!
			if(gameState == GameState.PLAYING){
				GameOver();
			}
		}
		UpdateScoreView();
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

	private void DelayRestartComplete(){
		gameState = GameState.GAMEOVER;
	}

	private void PlayerInput(){
		if(gameState == GameState.GAMEOVER){
			StartNewGame();

			viewTitle.SetActive(false);
			gameState =  GameState.PLAYING;
		}
	}

	private void UpdateScoreView(){

		if(gameState != GameState.PLAYING){ return;};

		score = blob.numOfDebrisCollected;

		if(score >= highscore){
			highscore = score;
			textHighScore.color = Color.red;
			textScore.color = Color.red;
		}else{
			// not a new highscore
			textHighScore.color = Color.white;
			textScore.color = Color.white;
		}

		textScore.text = score.ToString("N0");
		textHighScore.text = highscore.ToString("N0");
	}

	void OnEnable()
	{
		Paddle.OnPlayerInput += PlayerInput;
	}


	void OnDisable()
	{
		Paddle.OnPlayerInput -= PlayerInput;
	}
}


