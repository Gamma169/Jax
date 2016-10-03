using UnityEngine;
using System.Collections;

public class MoveBody : MonoBehaviour {

	// This is only to test out the motion on the new animation
	//public bool temp = false;

	public float maxSpeed = 4f;
	public float slowedSpeed = 2f;
	public float moveAcc = 50f;
	public float airAcc = 10f;

	private bool onGround;
	private float movingPlatformSpeed;

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
			//if it's on the ground, then you can accelerate to the max spped
			if (onGround) {
				// Holding shift will make you move faster
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
					if (Input.GetKey("a") && rb.velocity.x >= -maxSpeed + movingPlatformSpeed/2)		//If you're on a platform the platform is moving, you should increase the maximum speed at which the body can move in relation to the speed of the moving platform
						rb.AddForce(Vector2.left * moveAcc, ForceMode2D.Force);
					if (Input.GetKey("d") && rb.velocity.x <= maxSpeed + movingPlatformSpeed/2)
						rb.AddForce(Vector2.right * moveAcc, ForceMode2D.Force);
				}
				else {
					if (Input.GetKey("a") && rb.velocity.x >= -slowedSpeed + movingPlatformSpeed/2)
						rb.AddForce(Vector2.left * moveAcc, ForceMode2D.Force);
					if (Input.GetKey("d") && rb.velocity.x <= slowedSpeed + movingPlatformSpeed/2)
						rb.AddForce(Vector2.right * moveAcc, ForceMode2D.Force);
				}
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

	// This is fairly complicated because of the moving platform motion.  
	// Basically, if the ground you're on is a moving platform, a bunch more checks need to be made to make sure your motion stays relative to the platform and not the world
	void OnCollisionStay2D(Collision2D other) {
		MovingPlatform mvp;
		if (other.gameObject.tag == "Ground") {
			onGround = true;
			mvp = other.gameObject.GetComponent<MovingPlatform>();
			if (mvp) {
				if (mvp.isActiveAndEnabled && !mvp.isDone()) {
					if (mvp.path[mvp.getOnPathpart()] == -2)
						movingPlatformSpeed = -mvp.speeds[mvp.getOnPathpart()] / 2;
					else if (mvp.path[mvp.getOnPathpart()] == 2)
						movingPlatformSpeed = mvp.speeds[mvp.getOnPathpart()] / 2;
					else
						movingPlatformSpeed = 0;
				}
				else
					movingPlatformSpeed = 0;
			}
		}
	}


	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground")
			onGround = true;
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.tag == "Ground") {
			onGround = false;
			movingPlatformSpeed = 0;
		}
	}

}
