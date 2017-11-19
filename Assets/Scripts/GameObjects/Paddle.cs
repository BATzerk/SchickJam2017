using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Paddle : MonoBehaviour {
	// Constants
	private const float INPUT_FORCE = 0.008f; // Higher is faster.
	// Components
	[SerializeField] private BoxCollider2D handCollider; // TODO: Andrew. Make these curved. ( ( ( ( ( ( ( ( ( like that, not like |, like ) not |. <3
	[SerializeField] private SpriteRenderer handSprite;
	[SerializeField] private BoxCollider2D armCollider;
	[SerializeField] private SpriteRenderer armSprite;

	// References
	[SerializeField] private PaddleController paddleController;
	// Properties
	private float loc; // from 0 to 1.
	private float locVel;
	private float angleStart,angleEnd; // we lerp between these two with loc.
	private int index; // 0 or 1. Determines which joystick axis affects me.
	private int inputDir; // -1 or 1, depending on my index.

	// Getters
	private float distFromCenter { get { return paddleController.PaddleDistance; } }

	// Event
	public static Action OnPlayerInput;

	// ----------------------------------------------------------------
	//  Reset
	// ----------------------------------------------------------------
	public void Reset (int _index) {
		index = _index;

		loc = 0.5f;
		locVel = 0;
		if (index==0) {
			angleStart = -Mathf.PI*0.5f;
			angleEnd = Mathf.PI*0.5f;
		}
		else {
			angleStart = Mathf.PI*1.5f;
			angleEnd = Mathf.PI*0.5f;
		}

		// Set my properties, kween!
		SetSize (new Vector2(0.2f, 1.4f));
	}


	// ----------------------------------------------------------------
	//  Doers
	// ----------------------------------------------------------------
	/** Sizes the collider and sprite. */
	private void SetSize (Vector2 _size) {
		handCollider.size = _size;
//		GameUtils.SizeSprite (bodySprite, _size.x,_size.y);
		handSprite.transform.localScale = _size * 100f; // Hack. Idk why it's too small.
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		if (InputController.JoystickAxes == null) { return; } // So we can recompile without an error.
		UpdateLocVel ();
		ApplyLocVel ();
		ApplyBounds ();

		WaitingForPlayerInput();
	}
	private void UpdateLocVel () {
		// What's our joystick look like?
		Vector2 inputAxis = InputController.JoystickAxes[index];
		// Make u-turns faster!
		if (inputAxis.y!=0 && GameMathUtils.Sign(inputAxis.y) != GameMathUtils.Sign(locVel)) {
			locVel *= 0f;
		}
		// Hit me!
		float sizeSpeedScale = (7+distFromCenter)*0.125f; // slow down the movement ever so slightly the bigger we get.
		locVel += inputAxis.y * INPUT_FORCE / sizeSpeedScale;

	}
	private void ApplyLocVel () {
		loc += locVel;
		// Friction! Different if input or not.
//		Vector2 inputAxis = InputController.JoystickAxes[index];
//		if (inputAxis.y != 0) {
//			locVel *= 0.92f;
//		}
//		else {
			locVel *= 0.8f;
//		}
		// Great! Position/rotate me!
		float angle = Mathf.Lerp(angleStart, angleEnd, loc);
//		Vector2 handPos = new Vector3 (Mathf.Cos(angle)*distFromCenter, Mathf.Sin(angle)*distFromCenter);
		Vector2 armSize = new Vector2(distFromCenter, 0.2f);

		handSprite.transform.localPosition = new Vector3(distFromCenter, 0, 0);
		handCollider.transform.localPosition = new Vector3(distFromCenter, 0, 0);
		armSprite.transform.localPosition = new Vector3(distFromCenter*0.5f, 0, 0);
		armCollider.transform.localPosition = new Vector3(distFromCenter*0.5f, 0, 0);
		GameUtils.SizeSprite (armSprite, armSize.x,armSize.y);
		armCollider.size = armSize;

		this.transform.localEulerAngles = new Vector3 (0, 0, angle*Mathf.Rad2Deg);
	}
	private void ApplyBounds () {
		if (loc<0) {
			loc = 0;
			locVel *= -0.9f;
		}
		else if (loc>1) {
			loc = 1;
			locVel *= -0.9f;
		}
	}

	private bool WaitingForPlayerInput(){

		if (InputController.JoystickAxes == null) {  return false; } // So we can recompile without an error.
		Vector2 inputAxis = InputController.JoystickAxes[index];

		if(Mathf.Abs( inputAxis.y )  > 0.8f){
			OnPlayerInput();
		}

		return false;
	}
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {
	// Constants
	private const float POS_EASING = 5f; // Higher is slower.
	// Components
	[SerializeField] private BoxCollider2D bodyCollider; // TODO: Andrew. Make these curved. ( ( ( ( ( ( ( ( ( like that, not like |, like ) not |. <3
	[SerializeField] private SpriteRenderer bodySprite;
	// Properties
	private float distFromCenter;
	private float angleFromCenter;
	private float targetAngleFromCenter;
	private int index; // 0 or 1. Determines which joystick axis affects me.



	// ----------------------------------------------------------------
	//  Reset
	// ----------------------------------------------------------------
	public void Reset (int _index) {
		index = _index;

		angleFromCenter = targetAngleFromCenter = Mathf.PI*_index; // just default these to some angle.

		// Set my properties, kween!
		SetSize (new Vector2(0.2f, 1.4f));
		distFromCenter = 2 + index*0.3f;
	}


	// ----------------------------------------------------------------
	//  Doers
	// ----------------------------------------------------------------
	/** Sizes the collider and sprite. * /
	private void SetSize (Vector2 _size) {
		bodyCollider.size = _size;
		//		GameUtils.SizeSprite (bodySprite, _size.x,_size.y);
		bodySprite.transform.localScale = _size * 100f; // Hack. Idk why it's too small.
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		AcceptInput ();
		UpdatePosition ();
		UpdateRotation ();
	}
	private void AcceptInput () {
		if (InputController.JoystickAxes == null) { return; } // So we can recompile without an error.
		// What's our joystick look like?
		Vector2 inputAxis = InputController.JoystickAxes[index];
		// Are we pushing at least a little??
		if (inputAxis.magnitude > 0.2f) {
			float joystickAngle = Mathf.Atan2(inputAxis.y, inputAxis.x);
			// Good! Ease our target angle to where the input isss!
			if (Mathf.Abs(targetAngleFromCenter-joystickAngle) > Mathf.PI) { // Loop the loop, I mean angle!! Hashtag smooth easing!
				if (targetAngleFromCenter<0) { targetAngleFromCenter += Mathf.PI*2; }
				else { targetAngleFromCenter -= Mathf.PI*2; }
			}
			// Ease to target!
			targetAngleFromCenter += (joystickAngle-targetAngleFromCenter) / POS_EASING;
		}
	}
	private void UpdatePosition () {
		// Loop the loop, I mean angle!! Hashtag smooth easing!
		if (Mathf.Abs(angleFromCenter-targetAngleFromCenter) > Mathf.PI) {
			if (angleFromCenter<0) { angleFromCenter += Mathf.PI*2; }
			else { angleFromCenter -= Mathf.PI*2; }
		}
		// Ease to target!
		angleFromCenter += (targetAngleFromCenter-angleFromCenter) / POS_EASING;
		// Great! Position/rotate me!
		this.transform.localPosition = new Vector3 (Mathf.Cos(angleFromCenter)*distFromCenter, Mathf.Sin(angleFromCenter)*distFromCenter);
	}
	private void UpdateRotation () {
		this.transform.localEulerAngles = new Vector3 (0, 0, angleFromCenter*Mathf.Rad2Deg);
	}


	}


*/