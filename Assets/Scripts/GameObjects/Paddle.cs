﻿using System.Collections;
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
		SetSize (new Vector2(20, 140));
		distFromCenter = 2 + index*0.3f;
	}


	// ----------------------------------------------------------------
	//  Doers
	// ----------------------------------------------------------------
	/** Sizes the collider and sprite. */
	private void SetSize (Vector2 _size) {
		bodyCollider.size = _size;
//		GameUtils.SizeSprite (bodySprite, _size.x,_size.y);
		bodySprite.transform.localScale = _size;
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

