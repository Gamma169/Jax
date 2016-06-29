using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Vector2 MinCameraBounds;
	public Vector2 MaxCameraBounds;

	[Tooltip("The difference in X position between the player and camera before the camera starts moving")]
	public float playerRangeX = 4f;

	[Tooltip("How far the player has to move away from the center of the camera before the camera follow's the player's speed exactly")]
	public float playerMaxX = 4f;

	public float increaseCameraSizeAtY;
	//[Tooltip("This is a great tooltip")]
	public float YSizeScalingFactor;

	private Rigidbody2D rb;
	private Vector3 newPos;
	private Camera cam;
	private float startY;
	private float regSize;

	// Use this for initialization
	void Start() {
		rb = this.GetComponent<Rigidbody2D>();
		cam = this.GetComponent<Camera>();
		regSize = cam.orthographicSize;
		startY = this.transform.position.y;
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		if (GlobalVariables.playerPositions.Length >= 2) {
			//Have yet to do multiple players
		}
		else if (GlobalVariables.playerPositions.Length == 1) {
			
			if ((GlobalVariables.playerPositions[0].x > this.transform.position.x + playerRangeX && this.transform.position.x < MaxCameraBounds.x) ||
				(GlobalVariables.playerPositions[0].x < this.transform.position.x - playerRangeX && this.transform.position.x > MinCameraBounds.x)) {				
				if (GlobalVariables.playerPositions[0].x - this.transform.position.x < playerMaxX || GlobalVariables.playerRBs[0].velocity.x < .2f)
					rb.velocity = new Vector2(GlobalVariables.playerPositions[0].x - this.transform.position.x, 0);
				else
					rb.velocity = new Vector2(GlobalVariables.playerRBs[0].velocity.x, 0);

			}
			/*	
			else if (GlobalVariables.playerPositions[0].x < this.transform.position.x - playerRangeX && this.transform.position.x > MinCameraBounds.x ) {
				rb.velocity = new Vector2(GlobalVariables.playerPositions[0].x - this.transform.position.x, 0);
			}
			*/
			else {
				rb.velocity = new Vector2(0,0);
			}


			if (GlobalVariables.playerPositions[0].y > increaseCameraSizeAtY) {
				cam.orthographicSize = regSize + ((GlobalVariables.playerPositions[0].y - increaseCameraSizeAtY) / YSizeScalingFactor);
				this.transform.position = new Vector3 (this.transform.position.x,  startY + ((GlobalVariables.playerPositions [0].y - increaseCameraSizeAtY) / YSizeScalingFactor), this.transform.position.z);
			}
			
		}
		else {
			//Don't know what to do for no players, but may be necessary later
		}
	}
}
