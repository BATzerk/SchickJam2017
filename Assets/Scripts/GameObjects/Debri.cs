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
	private Blob myBlob;

	// Getters
	public Types Type { get { return type; } }

	// ----------------------------------------------------------------
	//  Initialize
	// ----------------------------------------------------------------
	public void Initialize (Transform _parentTransform, Vector2 _pos, Vector2 _vel, Types _type) {
		this.transform.SetParent (_parentTransform);
		this.transform.localScale = Vector3.one;
		this.transform.localEulerAngles = Vector3.zero + new Vector3(0,0,Random.Range(0,360));
	
		this.name = "Debri_" + _type;

		// TODO: Set sprite! And collider!

		type = _type;
		this.transform.localPosition = _pos;
		rigidbody.velocity = _vel;

		Sprite spt;
		string[]imgs;
		int index;

		if(type == Types.Bad){
			//this.GetComponent<SpriteRenderer>().color = Color.red;
			 imgs = new string[] {"Shapes/bad1"};
		}else{
			// pick random good sprit
			imgs = new string[] {"Shapes/good1", "Shapes/good2", "Shapes/good3", "Shapes/good4", "Shapes/good5"};

		}
		index = Random.Range(0, imgs.Length);
		spt = Resources.Load <Sprite> (imgs[index]);
		this.GetComponent<SpriteRenderer>().sprite = spt;

		gameObject.transform.localScale = this.transform.localScale * Random.Range(0,3f);
		//Vector3.one * 0.5f  * t
	}

	// ----------------------------------------------------------------
	//  Doers
	// ----------------------------------------------------------------

	// ----------------------------------------------------------------
	//  Collisions
	// ----------------------------------------------------------------
	private Blob GetBlobFromGO(GameObject go) {
		// The thing itself is a Blob!
		Blob blob = go.GetComponent<Blob>();
		if (blob != null) { return blob; }
		// Ah. Then it's totes another Debri!
		Debri debri = go.GetComponent<Debri>();
		if (debri == null) { Debug.LogError ("Whoa! A Debri was hit by a thing that isn't a Blob NOR a Debri: " + go.name); return null; }
		return debri.myBlob;
	}
	private void OnCollisionEnter2D (Collision2D col) {
		GameObject go = col.gameObject;
		// Did I hit a Debris or a Blob?
		if (go.layer == LayerMask.NameToLayer(LayerNames.Blob)) {
			Blob blob = GetBlobFromGO (go);
			if (blob == null) { return; } // Oops!
			if (type == Types.Good) {
				StickToBlob (blob, col);
			}
			// Bad??
			else {
				DamageBlob (blob);
			}
		}

		if (go.layer == LayerMask.NameToLayer(LayerNames.Paddle)) {
			OnHitPaddle ();
		}
	}
	private void OnTriggerEnter2D (Collider2D col) {
		// If I'm a baddie, nix me!!
		if (type==Types.Bad && col.gameObject.layer == LayerMask.NameToLayer(LayerNames.Paddle)) {
			OnHitPaddle ();
		}
	}
	private void StickToBlob (Blob _blob, Collision2D col) {
		myBlob = _blob;
		myBlob.OnDebriAdded (this);

		// Update my physical properties!
		this.gameObject.layer = LayerMask.NameToLayer(LayerNames.Blob);
//		rigidbody.bodyType = RigidbodyType2D.Kinematic;
//		rigidbody.velocity = Vector2.zero;
//		rigidbody.angularVelocity = 0;
		this.transform.SetParent (myBlob.tf_MyDebris);
		SpringJoint2D springJoint = gameObject.AddComponent<SpringJoint2D>();
		springJoint.autoConfigureDistance = false;
		springJoint.distance = Vector2.Distance(this.gameObject.transform.localPosition, col.gameObject.transform.localPosition) * 1.2f;
		springJoint.connectedBody = col.rigidbody;

		// TEMP!
		this.GetComponent<SpriteRenderer>().color = Color.green;
		AudioController.getSingleton().PlaySFX(SoundClipId.SFX_CORE_HIT, 0.1f);
	}
	private void DamageBlob (Blob blob) {
		blob.GetHitByDebri (this);
	}

	private void OnHitPaddle () {
		// Am I on the Blob?? Then do nothin'!
		if (myBlob != null) { return; }
		// If I'm bad, then blow me right up!
//		if (type == Types.Bad) {
			// TODO: Particle fx.
			Destroy (gameObject);
//		}

		AudioController.getSingleton().PlaySFX(SoundClipId.SFX_PADDLE_HIT, 0.4f);
	}

	public void BlowUp () {
		// TODO: Unify this with hitting the paddle?
		Destroy (gameObject);
	}

}
