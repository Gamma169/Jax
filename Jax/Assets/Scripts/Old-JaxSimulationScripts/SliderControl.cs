using UnityEngine;
using System.Collections;

public class SliderControl : MonoBehaviour {


	public float regLength;
	public static float extendLength = 25f;
	public static float extendSpeed = 0.6f;

	public SliderJoint2D SD;

	// Use this for initialization
	void Start () {
		SD = GetComponent<SliderJoint2D> ();
		regLength = SD.connectedAnchor.y;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("up") && extendLength < 32)
			extendLength += 1f;
		if (Input.GetKeyDown ("down")  && extendLength >= regLength + 1f)
			extendLength -= 1f;

		if (Input.GetKeyDown ("right") && extendSpeed < 5)
			extendSpeed += 0.1f;
		if (Input.GetKeyDown ("left")  && extendSpeed >= 0)
			extendSpeed -= 0.1f;


		if (Input.GetKey ("space") && SD.connectedAnchor.y <= extendLength)
			SD.connectedAnchor = new Vector2(0, SD.connectedAnchor.y +  extendSpeed);
		else if (! Input.GetKey ("space") && SD.connectedAnchor.y >= regLength)
			SD.connectedAnchor = new Vector2(0, SD.connectedAnchor.y -  extendSpeed);
	}
}
