using UnityEngine;
using System.Collections;

public class FootControl : MonoBehaviour {

	public float power = 5f;

	public Transform footTransform;
	public Rigidbody2D footRB;

	public float alpha;
	// Use this for initialization
	void Start () {
		/*
		footTransform = this.transform;
		footRB = GetComponent<Rigidbody2D> ();
		*/
	}
	
	// Update is called once per frame
	void Update () {

		// This line should be in RotateFoot when I'm done debugging it
			
		alpha = Mathf.Atan ((footTransform.position.x - this.transform.position.x) / (footTransform.position.y - this.transform.position.y ) ) ;

		//print (alpha * 180 / Mathf.PI);

		if (GlobalVariables.pControl) {


			if (Input.GetKey("a")) {
				RotateFoot (true);
			}
			if (Input.GetKey("d")) {
				RotateFoot (false);
			}


		
		
		} 
		else {
			AutoControl ();
			//print (Mathf.Cos (-45));
		}

	}

	public void AutoControl () {


	}
		
	public void RotateFoot(bool clockwise) {
		float powerAdded = power;
		if (clockwise)
			powerAdded = -powerAdded;

		if (!footAbove ())
			footRB.AddForce (new Vector2 (-powerAdded * Mathf.Cos (alpha), powerAdded * Mathf.Sin (alpha)));
		else
			footRB.AddForce (new Vector2 (powerAdded * Mathf.Cos (alpha), -powerAdded * Mathf.Sin (alpha)));
	}

	public bool footAbove() {
		if (footTransform.position.y - this.transform.position.y > 0)
			return true;
		else
			return false;
	}
	public bool footToTheRight() {
		if (footTransform.position.x - this.transform.position.x > 0)
			return true;
		else
			return false;
	}
}
