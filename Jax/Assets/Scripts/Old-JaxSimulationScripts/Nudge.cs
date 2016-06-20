using UnityEngine;
using System.Collections;

public class Nudge : MonoBehaviour {

	public static float power;


	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		power = 5f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("up"))
			power += 2.5f;
		if (Input.GetKeyDown ("down") && power > 0)
			power -= 2.5f;



		if (Input.GetKeyDown ("left")) {
			rb.AddForce (new Vector2(-1 * power, 0), ForceMode2D.Impulse);
		}
		if (Input.GetKeyDown ("right")) {
			rb.AddForce (new Vector2(1 * power, 0), ForceMode2D.Impulse);
		}
			
	}
}
