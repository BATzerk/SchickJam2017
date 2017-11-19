using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRoom : MonoBehaviour {

	float speed = 1f;

	void Update()
	{
		// Rotate the object around its local X axis at 1 degree per second
	//	transform.Rotate(Vector3.right * Time.deltaTime);

		// ...also rotate around the World's Y axis
		transform.Rotate(Vector3.forward * Time.deltaTime * speed, Space.World);
	}
}
