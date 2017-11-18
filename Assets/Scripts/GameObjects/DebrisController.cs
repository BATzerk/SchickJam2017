using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisController : MonoBehaviour {
	// Properties
//	private float spawnInterval = 0.2f; // in SECONDS.
	private float timeUntilSpawnGood;
	private float timeUntilSpawnBad;
	// References
	[SerializeField] private Blob blob;
	[SerializeField] private GameController gameController;
	[SerializeField] private GameObject prefabGO_debri;
	[SerializeField] private Transform tf_debris;


	// ----------------------------------------------------------------
	//  Reset
	// ----------------------------------------------------------------
	public void Reset () {
		// Destroy sh**.
		GameUtils.DestroyAllChildren (tf_debris);

		// Reset values.
		timeUntilSpawnBad = 3f;
		timeUntilSpawnGood = 0f;
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void SpawnDebri (Debri.Types type) {
		// Position and vel!
		float posAngle = Random.Range(-Mathf.PI, Mathf.PI);
		float velAngle = -posAngle;
		velAngle += Random.Range(-0.15f, 0.15f); // vary up the direction it's heading in a bit, so it's not 100% going to the center.
		float spawnRadius = blob.CachedRadius * 6f;
		const float absVel = 2.9f;
		Vector2 pos = new Vector2(Mathf.Cos(posAngle)*spawnRadius, Mathf.Sin(posAngle)*spawnRadius);
		Vector2 vel = new Vector2(-Mathf.Cos(velAngle)*absVel, Mathf.Sin(velAngle)*absVel);
		Debri newObj = Instantiate(prefabGO_debri).GetComponent<Debri>();

		newObj.Initialize (tf_debris, pos, vel, type);

		// Reset values
		if (type==Debri.Types.Good) {
			timeUntilSpawnGood = 0.2f;
		}
		else {
			timeUntilSpawnBad = Random.Range (0.5f, 2f);
		}
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		// Game over? Don't spawn no more.
		if (gameController.IsGameOver) { return; }

		timeUntilSpawnGood -= Time.deltaTime;
		timeUntilSpawnBad -= Time.deltaTime;
		if (timeUntilSpawnGood <= 0) {
			SpawnDebri (Debri.Types.Good);
		}
		if (timeUntilSpawnBad <= 0) {
			SpawnDebri (Debri.Types.Bad);
		}

		// TEMP TESTING
		if (Input.GetKeyDown(KeyCode.Space)) {
			for (int i=0; i<10; i++) {
				SpawnDebri (Debri.Types.Good);
			}
		}
	}


}
