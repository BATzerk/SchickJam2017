﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public enum GameState { GAMEOVER, PLAYING }
	static public GameState gameState = GameState.GAMEOVER;

	// References
	[SerializeField] private Blob blob;
	[SerializeField] private DebrisController debrisController;
	[SerializeField] private GameCameraController cameraController;
	[SerializeField] private PaddleController paddleController;
	private EventManager eventManager;

	static float score;
	float highscore = 0;

	[SerializeField] private GameObject viewTitle;
	[SerializeField] private TextMesh textScore;
	[SerializeField] private TextMesh textHighScore;

	// Getters
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


	/*
	 * Wait for player input to start
	 * If red hits core game overs
	 * game over has time out before next round starts 3 seconds.
	 * Highscore??
	 */
	public void GameOver(){

		gameState = GameState.GAMEOVER;
		viewTitle.SetActive( true);
		// Delay then restart
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

	private void PlayerInput(){

		if(gameState == GameState.GAMEOVER){
			StartNewGame();
			viewTitle.SetActive(false);
			gameState =  GameState.PLAYING;
		}
	}

	private void UpdateScoreView(){

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


