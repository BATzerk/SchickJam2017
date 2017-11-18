using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisController : MonoBehaviour {
	// Properties
	private float spawnInterval = 1.2f; // in SECONDS.
	private float timeUntilSpawnDebri;
	// References
	[SerializeField] private Transform tf_debris;
	[SerializeField] private GameObject prefabGO_debri;


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
//		velAngle += Random.Range(-0.3f, 0.3f); // QQQ vary up the direction it's heading in a bit, so it's not 100% going to the center.
		const float spawnRadius = 10f;
		const float absVel = 2f;
		Vector2 pos = new Vector2(Mathf.Cos(posAngle)*spawnRadius, Mathf.Sin(posAngle)*spawnRadius);
		Vector2 vel = new Vector2(-Mathf.Cos(velAngle)*absVel, Mathf.Sin(velAngle)*absVel);
		Debri newObj = Instantiate(prefabGO_debri).GetComponent<Debri>();


		float rad =  Random.Range(0,100);
		if(rad < 40){
			newObj.Initialize (tf_debris, pos, vel, Debri.Types.Good);
		}else{
			newObj.Initialize (tf_debris, pos, vel, Debri.Types.Bad);
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
	}


}
