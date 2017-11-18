using UnityEngine;
using System.Collections;
using System.IO;

public class GameUtils {

	public static string GetSecondsToTimeString (float _totalSeconds) {
		string minutes = Mathf.Floor (_totalSeconds / 60).ToString("0");
		string seconds = Mathf.Floor (_totalSeconds % 60).ToString("00");
		return minutes + ":" + seconds;
	}

	/** Provides the index of which available screen resolution the current Screen.width/Screen.height combo is at. Returns null if there's no perfect fit. */
	public static int GetClosestFitScreenResolutionIndex () {
		for (int i=0; i<Screen.resolutions.Length; i++) {
			// We found a match!?
			if (Screen.width==Screen.resolutions[i].width && Screen.height==Screen.resolutions[i].height) {
				return i;
			}
		}
		return -1; // Hmm, nah, the current resolution doesn't match anything our monitor recommends.
	}

	/** This function simulates ONE frame's worth of our standard easing, but from one COLOR to another. :)
	 easing: Higher is slower. */
	public static Color EaseColorOneStep (Color appliedColor, Color targetColor, float easing) {
		return new Color (appliedColor.r+(targetColor.r-appliedColor.r)/easing, appliedColor.g+(targetColor.g-appliedColor.g)/easing, appliedColor.b+(targetColor.b-appliedColor.b)/easing);
	}

	/** The final alpha will be the provided alpha * the color's base alpha. */
	public static void SetSpriteColorWithCompoundAlpha (SpriteRenderer sprite, Color color, float alpha) {
		sprite.color = new Color (color.r, color.g, color.b, color.a*alpha);
	}
	/** The final alpha will be the provided alpha * the color's base alpha. */
	public static void SetUIGraphicColorWithCompoundAlpha (UnityEngine.UI.Graphic uiGraphic, Color color, float alpha) {
		uiGraphic.color = new Color (color.r, color.g, color.b, color.a*alpha);
	}

	/** The sprite's base color alpha is ignored/overridden. */
	public static void SetSpriteAlpha(SpriteRenderer sprite, float alpha) {
		sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, alpha);
	}
	public static void SetTextMeshAlpha(TextMesh textMesh, float alpha) {
		textMesh.color = new Color (textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
	}
	public static void SetUIGraphicAlpha (UnityEngine.UI.Graphic uiGraphic, float alpha) {
		uiGraphic.color = new Color (uiGraphic.color.r, uiGraphic.color.g, uiGraphic.color.b, alpha);
	}

	static public void SizeSprite(SpriteRenderer sprite, float desiredWidth,float desiredHeight, bool doPreserveRatio=false) {
		if (sprite == null) {
			Debug.LogError("Oops! We've passed in a null SpriteRenderer into GameUtils.SizeSprite.");
			return;
		}
		if (sprite.sprite == null) {
			Debug.LogError("Oops! We've passed in a null SpriteRenderer WITHOUT a Sprite into GameUtils.SizeSprite.");
			return;
		}
		// Reset my sprite's scale; find out its neutral size; scale the images based on the neutral size.
		sprite.transform.localScale = Vector2.one;
		float imgW = sprite.sprite.bounds.size.x;
		float imgH = sprite.sprite.bounds.size.y;
		if (doPreserveRatio) {
			if (imgW > imgH) {
				desiredHeight *= imgH/imgW;
			}
			else if (imgW < imgH) {
				desiredWidth *= imgW/imgH;
			}
		}
		sprite.transform.localScale = new Vector2(desiredWidth/imgW, desiredHeight/imgH);
	}
//	static public void SizeMeshRenderer (MeshRenderer mesh, float desiredDiameter) { SizeMeshRenderer (mesh, desiredDiameter,desiredDiameter); }
//	static public void SizeMeshRenderer (MeshRenderer mesh, float desiredWidth,float desiredHeight, bool doPreserveRatio=false) {
//		if (mesh == null) { Debug.LogError("Oops! We've passed in a null MeshRenderer into GameUtils.SizeMeshRenderer."); return; }
//		// Reset its scale; find out its neutral size; scale the images based on the neutral size.
//		mesh.transform.localScale = Vector2.one;
//		float imgW = mesh.bounds.size.x;
//		float imgH = mesh.bounds.size.z;
//		if (doPreserveRatio) {
//			if (imgW > imgH) { desiredHeight *= imgH/imgW; }
//			else if (imgW < imgH) { desiredWidth *= imgW/imgH; }
//		}
//		mesh.transform.localScale = new Vector3 (desiredWidth/imgW, desiredHeight/imgH);
//	}
	static public void SizeUIGraphic (UnityEngine.UI.Graphic uiGraphic, float desiredWidth,float desiredHeight) {//, bool doPreserveRatio=false) {
//		uiGraphic.rectTransform.localScale = Vector2.one;
//		float imgW = uiGraphic.rectTransform.sizeDelta.bounds.size.x;
//		float imgH = sprite.sprite.bounds.size.y;
//		if (doPreserveRatio) {
//			if (imgW > imgH) {
//				desiredHeight *= imgH/imgW;
//			}
//			else if (imgW < imgH) {
//				desiredWidth *= imgW/imgH;
//			}
//		}
		uiGraphic.rectTransform.sizeDelta = new Vector2 (desiredWidth, desiredHeight);
	}


	/** Returns number of seconds elapsed since 1970. */
	public static int GetSecondsSinceEpochStart () {
		System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		return (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
	}



	public static Texture2D LoadPNG(string filePath) {
		Texture2D tex = null;
		byte[] fileData;
		
		if (File.Exists(filePath)) {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
		}
		else {
			Debug.LogWarning ("Can't find a PNG in this path: " + filePath);
		}
		return tex;
	}

	public static void SetParticleSystemEmissionEnabled (ParticleSystem particleSystem, bool isEnabled) {
		ParticleSystem.EmissionModule em;
		em = particleSystem.emission;
		em.enabled = isEnabled;
	}



}





