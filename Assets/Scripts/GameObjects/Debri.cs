using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debri : MonoBehaviour {
	// Constants
	public enum Types { Good, Bad }
	// Components
	[SerializeField] private Rigidbody2D rigidbody;
	// Properties
	private Types type;

	// ----------------------------------------------------------------
	//  Initialize
	// ----------------------------------------------------------------
	public void Initialize (Transform _parentTransform, Vector2 _pos, Vector2 _vel, Types _type) {
		this.transform.SetParent (_parentTransform);
		this.transform.localScale = Vector3.one;
		this.transform.localEulerAngles = Vector3.zero;
		this.name = "Debri_" + _type;

		// TODO: Set sprite! And collider!

		type = _type;
		this.transform.localPosition = _pos;
		rigidbody.velocity = _vel;
	}
}
