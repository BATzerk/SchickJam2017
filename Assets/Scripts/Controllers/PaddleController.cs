using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {
	// Properties
	private float paddleDistance;
	private float paddleDistanceTarget;
	// References
	[SerializeField] private Blob blob;
	[SerializeField] private Paddle[] paddles;

	// Getters
	public float PaddleDistance { get { return paddleDistance; } }


	// ----------------------------------------------------------------
	//  Reset
	// ----------------------------------------------------------------
	public void Reset () {
		// Reset values!
		paddleDistance = paddleDistanceTarget = 2f;

		// Reset paddlez!
		for (int i=0; i<paddles.Length; i++) {
			paddles[i].Reset (i);
		}
	}


	// ----------------------------------------------------------------
	//  Doers
	// ----------------------------------------------------------------
	/** canShrinkDistance: Set to false if you don't want the paddles to be able to get *closer* to the center. We DO want the distance to shrink when we lose bits. */
	public void UpdatePaddleDistanceTarget (bool canShrinkDistance) {
		float blobRadius = blob.CalculateRadius ();
		blobRadius += 0.7f; // Make it bigga! Some wigga room!
		// If we CAN'T shrink the distance, but we WANT to, then do nothin'.
		if (!canShrinkDistance && blobRadius<paddleDistanceTarget) { }
		else {
			paddleDistanceTarget = blobRadius;
		}
	}

	private void Update () {
		// Ease paddleDistance!
		paddleDistance += (paddleDistanceTarget-paddleDistance) / 24f;
	}


}
