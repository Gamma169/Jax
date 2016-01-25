using UnityEngine;
using System.Collections;

public class FootControl : MonoBehaviour {

	public float power = .1f;

	public Transform footTransform;
	public Rigidbody2D footRB;

	public float alpha;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// This line should be in RotateFoot when I'm done debugging it
			
		alpha = Mathf.Atan ((this.transform.position.x - footTransform.position.x) / (this.transform.position.y - footTransform.position.y) ) ;

		if (this.transform.position.y - footTransform.position.y >= 0)
			alpha = -alpha;

		print (alpha * 180 / Mathf.PI);


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

	public void RotateFoot(bool left) {

		if (left)
			footRB.AddForce (new Vector2(Mathf.Cos(-alpha) * power, Mathf.Sin(-alpha) * power));
		else
			footRB.AddForce (new Vector2(Mathf.Cos(-alpha) * -power, Mathf.Sin(-alpha) * -power));
	
	}
}
