using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour {
	// Components
	[SerializeField] private Transform tf_myDebris;
	// Properties
	private float cachedRadius;
	// References
	[SerializeField] private GameObject prefabGO_destructiveBurst;
	[SerializeField] private GameCameraController cameraController;
	[SerializeField] private GameController gameController;
	[SerializeField] private PaddleController paddleController;
	[SerializeField] private Transform tf_destructiveBursts;
	private List<Debri> myDebri; // all the Schick that's stuck to me! Schickhead!

	public float life;
	const float maxLife = 100;

	// Getters
	public int NumDebri { get { return myDebri.Count; } }
	public Transform tf_MyDebris { get { return tf_myDebris; } }
	public float CachedRadius { get { return cachedRadius; } }
	private float CalculateRadius () {
		// Go through all my children and find who's the farthest from my center!
		float farthestDistance = 2; // have a minimum value.
		for (int i=0; i<myDebri.Count; i++) {
			if(myDebri[i]){
				float distance = myDebri[i].transform.localPosition.magnitude;

				if (farthestDistance < distance){
					farthestDistance = distance;
				}
			}
		}
		return farthestDistance;
	}


	// ----------------------------------------------------------------
	//  Reset
	// ----------------------------------------------------------------
	public void Reset () {
		GameUtils.DestroyAllChildren (tf_myDebris);
		GameUtils.DestroyAllChildren (tf_destructiveBursts);
		myDebri = new List<Debri>();
		cachedRadius = 1f;
		life = maxLife;
	}


	private void UpdateRadius (bool canShrinkDistance) {
		cachedRadius = CalculateRadius ();
		paddleController.UpdatePaddleDistanceTarget (cachedRadius, canShrinkDistance);
		cameraController.UpdateZoomScaleTarget (cachedRadius);
	}
	private void BlowUpDebriInArea (Vector2 pos, float radius) {
		for (int i=myDebri.Count-1; i>=0; --i) {
			float dist = Vector2.Distance (pos, myDebri[i].transform.localPosition);
			if (dist<radius) {
				RemoveDebri (myDebri[i], false);
			}
		}
	}


	// ----------------------------------------------------------------
	//  Events
	// ----------------------------------------------------------------
	public void OnDebriAdded (Debri _debri) {
		if (!myDebri.Contains(_debri)) {
			myDebri.Add (_debri);
			UpdateRadius (false);
		}
	}
	public void GetHitByDebri (Debri _debri) {
		// Who the fuck's fault is this. ...Jesus, Deb. This is why we can't have nice things.
		gameController.GuiltyPlayer = _debri.transform.localPosition.x<0 ? 0 : 1;
		AddDestructiveBurst (_debri.transform.localPosition);
//		Destroy (_debri.gameObject);
		life -= 100;
	}
	private void AddDestructiveBurst (Vector2 pos) {
		DestructiveBurst burst = Instantiate(prefabGO_destructiveBurst).GetComponent<DestructiveBurst>();
		float burstRadius = 0.5f;//cachedRadius * 0.6f; // the bigger I am, the bigger the blast radius is!
		burst.Initialize (tf_destructiveBursts, pos, burstRadius);
//		BlowUpDebriInArea (pos, burstRadius);
	}
	private void RemoveDebri (Debri _debri, bool doUpdateRadius) {
		if (!myDebri.Contains(_debri)) {
			Debug.LogError ("Trying to remove a Debri that's not in the Blob's list of Debri!");
			return;
		}
		// Destroy dis bad boy!
		_debri.BlowUp ();
		// Remove from my liiist.
		myDebri.Remove (_debri);
		if (doUpdateRadius) {
			UpdateRadius (true);
		}
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void FixedUpdate () {
		this.transform.localEulerAngles = new Vector3 (0, 0, Mathf.Sin (Time.time*1f)*20f);
	}





}
