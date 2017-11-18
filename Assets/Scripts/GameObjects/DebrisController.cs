using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisController : MonoBehaviour {
	// Properties
	private float spawnInterval = 0.2f; // in SECONDS.
	private float timeUntilSpawnDebri;
	// References
	[SerializeField] private Blob blob;
	[SerializeField] private GameObject prefabGO_debri;
	[SerializeField] private Transform tf_debris;


	// ----------------------------------------------------------------
	//  Reset
	// ----------------------------------------------------------------
	public void Reset () {
		// Destroy sh**.
		GameUtils.DestroyAllChildren (tf_debris);

		// Reset values.
		timeUntilSpawnDebri = spawnInterval;
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void SpawnDebri () {
		// Position and vel!
		float posAngle = Random.Range(-Mathf.PI, Mathf.PI);
		float velAngle = -posAngle;
		velAngle += Random.Range(-0.15f, 0.15f); // vary up the direction it's heading in a bit, so it's not 100% going to the center.
		float spawnRadius = blob.CachedRadius * 6f;
		const float absVel = 2.9f;
		Vector2 pos = new Vector2(Mathf.Cos(posAngle)*spawnRadius, Mathf.Sin(posAngle)*spawnRadius);
		Vector2 vel = new Vector2(-Mathf.Cos(velAngle)*absVel, Mathf.Sin(velAngle)*absVel);
		Debri newObj = Instantiate(prefabGO_debri).GetComponent<Debri>();


		float rad =  Random.Range(0,100);
		if(rad < 12) {
			newObj.Initialize (tf_debris, pos, vel, Debri.Types.Bad);
		}else{
			newObj.Initialize (tf_debris, pos, vel, Debri.Types.Good);
		}

	

		// Reset values
		timeUntilSpawnDebri = spawnInterval;
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		timeUntilSpawnDebri -= Time.deltaTime;
		if (timeUntilSpawnDebri <= 0) {
			SpawnDebri ();
		}
		// QQQ TESTING
		if (Input.GetKeyDown(KeyCode.Space)) {
			for (int i=0; i<10; i++) {
				SpawnDebri ();
			}
		}
	}


}
