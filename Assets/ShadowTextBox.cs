using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowTextBox : MonoBehaviour {

	public Text textToShadow;
	private Text textBox;

	// Use this for initialization
	void Start () {
		textBox = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		if(textToShadow){

			textBox.text = textToShadow.text;
		}
	}
}
