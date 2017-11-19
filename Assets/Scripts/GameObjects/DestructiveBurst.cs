using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiveBurst : MonoBehaviour {
	// Components
	[SerializeField] private SpriteRenderer bodySprite;
	[SerializeField] private CircleCollider2D circleCollider;
	// Properties
	private float alpha;

	public void Initialize (Transform parentTransform, Vector2 pos, float radius) {
		this.transform.SetParent (parentTransform);
		this.transform.localPosition = pos;

		circleCollider.radius = radius;
		GameUtils.SizeSprite (bodySprite, radius*2,radius*2);

		alpha = 0.6f;
		ApplyAlpha ();
	}

	private void ApplyAlpha () {
		GameUtils.SetSpriteAlpha (bodySprite, alpha);
	}

	private void FixedUpdate () {
//		alpha -= 0.03f;
//		ApplyAlpha ();
//		if (alpha <= 0) {
//			Destroy (this.gameObject);
//		}
	}

//	private void OnTriggerEnter2D (Collider2D col) {
//
//	}

}
