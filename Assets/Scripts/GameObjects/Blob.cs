using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour {
	// Components
	[SerializeField] private Transform tf_myDebris;
	// References
	[SerializeField] private PaddleController paddleController;
	private List<Debri> myDebri; // all the Schick that's stuck to me! Schickhead!

	// Getters
	public Transform tf_MyDebris { get { return tf_myDebris; } }
	public float CalculateRadius () {
		// Go through all my children and find who's the farthest from my center!
		float farthestDistance = 2; // have a minimum value.
		for (int i=0; i<myDebri.Count; i++) {
			float distance = myDebri[i].transform.localPosition.magnitude;
			if (farthestDistance < distance) {
				farthestDistance = distance;
			}
		}
		return farthestDistance;
	}


	// ----------------------------------------------------------------
	//  Reset
	// ----------------------------------------------------------------
	public void Reset () {
		GameUtils.DestroyAllChildren (tf_myDebris);
		myDebri = new List<Debri>();
	}


	// ----------------------------------------------------------------
	//  Events
	// ----------------------------------------------------------------
	public void OnDebriAdded (Debri _debri) {
		myDebri.Add (_debri);
		paddleController.UpdatePaddleDistanceTarget (false);
	}
	public void GetHitByDebri (Debri _debri) {
		// TODO: This.
		//		OnDebriRemoved (this);
		paddleController.UpdatePaddleDistanceTarget (true);
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void FixedUpdate () {
		this.transform.localEulerAngles = new Vector3 (0, 0, Time.time*5f);
	}





}
