using UnityEngine;
using System.Collections;

public class GameCameraController : MonoBehaviour {
	// Settables
	private float easing_pos = 2f; // HIGHER is SLOWER. Lower is snappier.
	// Properties
	private float zoomAmount;
	private float zoomAmount_target;
	private Vector2 pos_target;
	// References
	[SerializeField] private GameController gameControllerRef;

	// Getters
	private float frameTimeScaleUnscaled { get { return TimeController.FrameTimeScaleUnscaled; } }
	private Vector3 playerPos { get { return gameControllerRef.Player.transform.localPosition; } }
	// Getters / Setters
	private Vector2 pos {
		get { return this.transform.localPosition; }
		set { this.transform.localPosition = value; }
	}



	// ----------------------------------------------------------------
	//  Start / Destroy
	// ----------------------------------------------------------------
	private void Awake () {
		// Add event listeners!
//		GameManagers.Instance.EventManager.GameViewChangedEvent += OnGameViewChanged;
	}
	private void OnDestroy () {
		// Remove event listeners!
//		GameManagers.Instance.EventManager.GameViewChangedEvent -= OnGameViewChanged;
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		UpdatePosTarget ();
		UpdatePos ();
	}

	private void UpdatePosTarget () {
		pos_target = playerPos;
	}
	private void UpdatePos () {
		pos += (pos_target-pos) / easing_pos;
	}


}



