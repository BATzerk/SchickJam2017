using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	// Properties
//	static private bool isPlayerAxisInput; // true if playerAxisInput's magnitude isn't 0!
	static private Vector2[] joystickAxes;

	// Getters
//	static public bool IsPlayerAxisInput { get { return isPlayerAxisInput; } }
	static public Vector2[] JoystickAxes { get { return joystickAxes; } }




	// ----------------------------------------------------------------
	//  Awake
	// ----------------------------------------------------------------
	private void Awake () {
		joystickAxes = new Vector2[2];
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		RegisterButtonInputs ();
	}

	private void RegisterButtonInputs () {
		if (joystickAxes == null) { return; } // So we can recompile without breaking.
		joystickAxes[0] = new Vector2 (Input.GetAxisRaw ("Horizontal0"), Input.GetAxisRaw ("Vertical0"));
		joystickAxes[1] = new Vector2 (Input.GetAxisRaw ("Horizontal1"), Input.GetAxisRaw ("Vertical1"));
//		isPlayerAxisInput = playerAxisInput.x!=0 || playerAxisInput.y!=0;
//
//		// Scale playerAxisInput so it's normalized, and easier to control. :)
//		if (playerAxisInput != Vector2.zero) {
//			// Get the length of the directon vector and then normalize it
//			// Dividing by the length is cheaper than normalizing when we already have the length anyway
//			float directionLength = playerAxisInput.magnitude;
//			playerAxisInput = playerAxisInput / directionLength;
//
//			// Make sure the length is no bigger than 1
//			directionLength = Mathf.Min (1, directionLength);
//
//			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
//			// This makes it easier to control slow speeds when using analog sticks
//			directionLength = directionLength * directionLength;
//
//			// Multiply the normalized direction vector by the modified length
//			playerAxisInput = playerAxisInput * directionLength;
//		}
	}


}


