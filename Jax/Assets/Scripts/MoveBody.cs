using UnityEngine;
using System.Collections;

public class MoveBody : MonoBehaviour {

	public float moveSpeed = 15;

	private SpringControl sc;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sc = GetComponent<SpringControl>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (sc.retracted && rb.velocity.x <= 5) {
			if (Input.GetKey("a"))
				rb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Force);
			if (Input.GetKey("d"))
				rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Force);
		}

	}
}
