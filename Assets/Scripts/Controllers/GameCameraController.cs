using UnityEngine;
using System.Collections;

public class GameCameraController : MonoBehaviour {
	// Settables
	private float easing_zoom = 140f; // HIGHER is SLOWER. Lower is snappier.
	// Components
	[SerializeField] Camera camera;
	// Properties
	private float zoomScale;
	private float zoomScale_target;
//	private Vector2 pos_target;
	// References
	[SerializeField] private GameController gameControllerRef;

	// Getters
	private float frameTimeScaleUnscaled { get { return TimeController.FrameTimeScaleUnscaled; } }
//	private Vector3 playerPos { get { return gameControllerRef.Player.transform.localPosition; } }
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
	public void Reset () {
		zoomScale = zoomScale_target = 1;
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		UpdateZoom ();
		ApplyZoom ();
	}

	private void UpdateZoom () {
		zoomScale += (zoomScale_target-zoomScale) / easing_zoom;
	}
	private void ApplyZoom () {
		camera.orthographicSize = 4 + 5*zoomScale;
	}


	public void UpdateZoomScaleTarget (float blobRadius) {
		zoomScale_target = Mathf.Max (1, blobRadius*0.3f); // TODO balance this right
	}

}



