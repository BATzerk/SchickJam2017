using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour {
	// Components
	[SerializeField] private Transform tf_myDebris;
	// Properties

	// Getters
	public Transform tf_MyDebris { get { return tf_myDebris; } }


	// ----------------------------------------------------------------
	//  Reset
	// ----------------------------------------------------------------
	public void Reset () {
		GameUtils.DestroyAllChildren (tf_myDebris);
	}





}
