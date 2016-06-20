﻿using UnityEngine;
using System.Collections;

public class MoveBody : MonoBehaviour {

	public float maxSpeed = 4.5f;
	public float moveAcc = 50f;
	public float airAcc = 10f;

	private bool onGround;

	private SpringControl sc;
	private Rigidbody2D rb;
	//private BoxCollider2D bc;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sc = GetComponent<SpringControl>();
		//bc = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//First check if it's retracted
		if (sc.retracted) {
			//if it's on the ground, then you can accelerate at max spped
			if (onGround) {
				if (Input.GetKey ("a") && rb.velocity.x >= -maxSpeed)
					rb.AddForce (Vector2.left * moveAcc, ForceMode2D.Force);
				if (Input.GetKey ("d") && rb.velocity.x <= maxSpeed)
					rb.AddForce (Vector2.right * moveAcc, ForceMode2D.Force);
			} 
			// if you're in the air, then it's slower
			else {
				if (Input.GetKey ("a") && rb.velocity.x >= -maxSpeed)
					rb.AddForce (Vector2.left * airAcc, ForceMode2D.Force);
				if (Input.GetKey ("d") && rb.velocity.x <= maxSpeed)
					rb.AddForce (Vector2.right * airAcc, ForceMode2D.Force);
			}
		}

	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground")
			onGround = true;
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.tag == "Ground")
			onGround = false;
	}
}
