using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExtendGUI : MonoBehaviour {
	
	private Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = ("Extend Length (UP/DOWN):  " + SliderControl.extendLength + " \n" + "Extend Speed (LEFT/RGHT): " + SliderControl.extendSpeed);
	 
	}
}
