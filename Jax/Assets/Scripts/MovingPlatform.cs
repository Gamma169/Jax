using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public bool horizontal;
	public int loopTime;
	public int speed;

	private int counter;
	private bool moveForward;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();

		rb.isKinematic = true;
		rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		if (horizontal)
			rb.constraints = RigidbodyConstraints2D.FreezePositionY;
		else
			rb.constraints = RigidbodyConstraints2D.FreezePositionX;

		counter = loopTime;
		moveForward = true;
	}
	
	void FixedUpdate () {

		if (horizontal) {
			if (moveForward)
				rb.velocity = new Vector2(.5f * speed, 0);
			else
				rb.velocity = new Vector2(-.5f * speed, 0);
					
		}
		else {
			if (moveForward)
				rb.velocity = new Vector2(0, .5f * speed);
			else
				rb.velocity = new Vector2(0, -.5f * speed);
		}

		counter--;
		if (counter == 0) {
			counter = loopTime;
			moveForward = !moveForward;
		}

	}
}
