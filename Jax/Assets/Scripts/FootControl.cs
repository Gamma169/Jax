using UnityEngine;
using System.Collections;

public class FootControl : MonoBehaviour {

	public float power = 5f;

	public Transform footTransform;
	public Rigidbody2D footRB;

	public float alpha;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// This line should be in RotateFoot when I'm done debugging it
			
		alpha = Mathf.Atan ((footTransform.position.x - this.transform.position.x) / (footTransform.position.y - this.transform.position.y ) ) ;

		print (alpha * 180 / Mathf.PI);

		if (GlobalVariables.pControl) {


			if (Input.GetKey("a")) {
				RotateFoot (false);
			}
			if (Input.GetKey("d")) {
				RotateFoot (true);
			}


		
		
		} 
		else {
			AutoControl ();
			//print (Mathf.Cos (-45));
		}

	}

	public void AutoControl () {


	}

	/*==============  I know that this method could be written more simply, but this way it follows my equations in my notebook and derivations for power    ========*/
	public void RotateFoot(bool clockwise) {
		float poweradded = power;
		if (clockwise)
			poweradded = -poweradded;

		float powerX;
		float powerY;

		if (footAbove ())
			powerX = poweradded;
		else
			powerX = -poweradded;

		if (footToTheRight ())
			powerY = -poweradded;
		else
			powerY = poweradded;
		

		footRB.AddForce (new Vector2(powerX * Mathf.Cos(alpha), powerY * Mathf.Sin(alpha)));

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
