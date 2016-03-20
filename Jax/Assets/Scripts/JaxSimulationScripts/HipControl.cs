using UnityEngine;
using System.Collections;

public class HipControl : MonoBehaviour {

	private Rigidbody2D RB2D;

	public static float hipMass;

	// Use this for initialization
	void Start () {
		RB2D = GetComponent<Rigidbody2D> ();
		hipMass = RB2D.mass;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown ("m") && hipMass < 20f)
			hipMass += 0.5f;
		if (Input.GetKeyDown ("n") && hipMass > 0.5f)
			hipMass -= 0.5f;

		RB2D.mass = hipMass;
			
	}
}
