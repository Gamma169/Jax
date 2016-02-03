using UnityEngine;
using System.Collections;

public class SliderControl : MonoBehaviour {


	public float regLength;
	public float extendLength = 25f;

	public SliderJoint2D SD;

	// Use this for initialization
	void Start () {
		SD = GetComponent<SliderJoint2D> ();
		regLength = SD.connectedAnchor.y;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("space") && SD.connectedAnchor.y <= extendLength)
			SD.connectedAnchor = new Vector2(0, SD.connectedAnchor.y +  0.6f);
		else if (! Input.GetKey ("space") && SD.connectedAnchor.y >= regLength)
			SD.connectedAnchor = new Vector2(0, SD.connectedAnchor.y -  0.6f);
	}
}
