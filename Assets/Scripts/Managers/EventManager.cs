using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager {
	// Actions and Event Variables
	public delegate void NoParamAction ();

	public event NoParamAction SomethingEvent;

	// Events
	public void OnSomething () { if (SomethingEvent!=null) { SomethingEvent (); } }



}




