using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager {
	// Actions and Event Variables
	public delegate void Vector2Action (Vector2 pos);

	public event Vector2Action AddParticleBurstEvent;

	// Events
	public void AddParticleBurst (Vector2 pos) { if (AddParticleBurstEvent!=null) { AddParticleBurstEvent (pos); } }



}




