using UnityEngine;
using System.Collections;

public class LineUtils {


	// ----------------------------------------------------------------
	//  Getters
	// ----------------------------------------------------------------
	static public float GetAngle (Vector2 pointA, Vector2 pointB) {
		return Mathf.Rad2Deg * Mathf.Atan2(pointB.y-pointA.y, pointB.x-pointA.x);
	}
	public static float GetLength (Vector2 lineStart,Vector2 lineEnd) {
		return Vector2.Distance(lineStart, lineEnd);
	}
	public static Vector2 GetCenterPos (Vector2 lineStart,Vector2 lineEnd) {
		return Vector2.Lerp (lineStart, lineEnd, 0.5f);
	}




}




