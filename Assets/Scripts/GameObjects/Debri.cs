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
	private Blob blob;

	// Getters
	public Types Type { get { return type; } }

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

	// ----------------------------------------------------------------
	//  Doers
	// ----------------------------------------------------------------

	// ----------------------------------------------------------------
	//  Collisions
	// ----------------------------------------------------------------
	private void OnCollisionEnter2D (Collision2D col) {
		// Debri hit me?
		if (col.gameObject.layer == LayerMask.NameToLayer(LayerNames.Blob)) {
			Blob blob = col.gameObject.GetComponent<Blob>();
			if (blob == null) { Debug.LogError ("Whoa! GO on Blob layer collided with Debri, but it doesn't have a Blob script!"); }
			if (type == Types.Good) {
				StickToBlob (blob);
			}
			// Bad??
			else {
				DamageBlob (blob);
			}
		}

		if (col.gameObject.layer == LayerMask.NameToLayer(LayerNames.Paddle)) {

			HitPaddle();
		}
	}
	private void StickToBlob (Blob blob = null) {
		this.gameObject.layer = LayerMask.NameToLayer(LayerNames.Blob);
		rigidbody.bodyType = RigidbodyType2D.Kinematic;
		rigidbody.velocity = Vector2.zero;
		rigidbody.angularVelocity = 0;
		//this.transform.SetParent (blob.tf_MyDebris);

		// TEMP!
		this.GetComponent<SpriteRenderer>().color = Color.green;
		AudioController.getSingleton().PlaySFX(SoundClipId.SFX_CORE_HIT, 0.6f);
	}
	private void DamageBlob (Blob blob) {
		// TODO: This
	}

	private void HitPaddle(){
		AudioController.getSingleton().PlaySFX(SoundClipId.SFX_PADDLE_HIT);
		Destroy(gameObject);
		/*this.gameObject.layer = LayerMask.NameToLayer(LayerNames.Blob);
		rigidbody.bodyType = RigidbodyType2D.Kinematic;
		rigidbody.velocity = rigidbody.velocity * -1 * 10f;
		rigidbody.angularVelocity = 0;
		*/

		/*Vector2 knockbackVelocity = 
		new Vector2((transform.position.x - player.transform.position.x) * forceMultiplier, (transform.position.y - player.transform.position.y) * forceMultiplier);
		rigidbody.GetComponent<Rigidbody2D>().velocity = -knockbackVelocity;*/
	}

}
