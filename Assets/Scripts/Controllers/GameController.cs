﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public enum GameState { GAMEOVER, PLAYING }
	static public GameState gameState = GameState.GAMEOVER;
	// References
	[SerializeField] private Blob blob;
	[SerializeField] private DebrisController debrisController;
	[SerializeField] private GameCameraController cameraController;
	[SerializeField] private PaddleController paddleController;
	[SerializeField] private WinnerLoserUI winnerLoserUI;
	private EventManager eventManager;
	// Properties
	private bool canRestartGame;
	static float score;
	float highscore = 0;
	[SerializeField] private GameObject prefabGO_badBurst;
	[SerializeField] private GameObject viewTitle;
	[SerializeField] private GameObject viewControls;
	[SerializeField] private Text textScore;
	[SerializeField] private Text textHighScore;
	[SerializeField] private Transform tf_particleBursts;

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
		GameOver ();

		// Add event listeners!
		GameManagers.Instance.EventManager.AddParticleBurstEvent += AddParticleBurst;
	}
	private void OnDestroy () {
		// Remove event listeners!
		GameManagers.Instance.EventManager.AddParticleBurstEvent -= AddParticleBurst;
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
		GameUtils.DestroyAllChildren (tf_particleBursts);
		Hack_DestroyAllDebri ();
		AudioController.getSingleton().PlayBGSoundClip(SoundClipId.MUS_BACKGROUND_1, 0.8f);
		AudioController.getSingleton().PlaySFX(SoundClipId.SFX_GAME_START, 1.0f);
	}
	private void Hack_DestroyAllDebri () {
		Debri[] allDebri = GameObject.FindObjectsOfType<Debri>();
		for (int i=allDebri.Length-1; i>=0; --i) {
			GameObject.Destroy (allDebri[i].gameObject);
		}
	}


	/*
	 * Wait for player input to start
	 * If red hits core game overs
	 * game over has time out before next round starts 3 seconds.
	 * Highscore??
	 */
	public void GameOver () {
		gameState = GameState.GAMEOVER;
		winnerLoserUI.OnGameOver (GuiltyPlayer);
		DestroyAnyDriftingDebri ();

		canRestartGame = false;
		Invoke("ShowTitle", 1.5f);
		Invoke("AllowGameRestart", 3);
		// TODO: Delay then restart
		AudioController.getSingleton().PlayBGSoundClip(SoundClipId.MUS_BACKGROUND_1, 0.3f);
		AudioController.getSingleton ().PlaySFX(SoundClipId.SFX_GAME_OVER);
	}
	private void ShowTitle () {
		viewTitle.SetActive (true);
		viewControls.SetActive(true);
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

		if (blob.life <= 0){
			// GameOver!
			if (gameState == GameState.PLAYING) {
				GameOver();
			}
		}
		UpdateScoreView();
	}
	private void RegisterButtonInput () {
		// R = Start a new game
		if (Input.GetKeyDown (KeyCode.R)) {
			ReloadScene ();
		}
		//		// Action Button
		//		else if (InputController.IsButtonDown_Action) {
		//			OnButtonDown_Action ();
		//		}
		// P = Toggle pause
		else if (Input.GetKeyDown (KeyCode.P)) {
			TogglePause ();
		}
	}

	private void AllowGameRestart () {
		canRestartGame = true;
	}

	private void PlayerInput(){
		if(gameState == GameState.GAMEOVER && canRestartGame){
			StartNewGame ();

			viewTitle.SetActive (false);
			viewControls.SetActive( false);
			gameState = GameState.PLAYING;
		}
	}

	private void UpdateScoreView(){
		if(gameState != GameState.PLAYING){ return;};

		score = blob.NumDebri;

		if(score >= highscore) {

			if(textHighScore.color == Color.black){
				AudioController.getSingleton().PlaySFX(SoundClipId.SFX_HIGHSCORE, 1.0f);
			}

			highscore = score;
			textHighScore.color = new Color(255/255f, 200/255f, 60/255f);
			textScore.color = new Color(255/255f, 200/255f, 60/255f);


		}else{
			// not a new highscore
			textHighScore.color = Color.black;
			textScore.color = Color.black;
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




	private void AddParticleBurst (Vector2 pos) {
		GameObject go_particleBurst = Instantiate(prefabGO_badBurst);
		go_particleBurst.transform.SetParent (tf_particleBursts);
		go_particleBurst.transform.localPosition = pos;
		go_particleBurst.transform.localScale = Vector3.one;
	}

}


