using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Vector2 MinCameraBounds;
	public Vector2 MaxCameraBounds;

	public float regSize;

	private Rigidbody2D rb;
	private Vector3 newPos;
	private Camera cam;

	// Use this for initialization
	void Start() {
		rb = this.GetComponent<Rigidbody2D>();
		cam = this.GetComponent<Camera>();
		regSize = cam.orthographicSize;
	}
	
	// Update is called once per frame
	void Update() {
		if (GlobalVariables.playerPositions.Length >= 2) {
			//Have yet to do multiple players
		}
		else if (GlobalVariables.playerPositions.Length == 1) {
			//newPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);

			if (GlobalVariables.playerPositions [0].x > this.transform.position.x + 4f) {
				//newPos.x += 0.1f;
				rb.velocity = new Vector2(GlobalVariables.playerPositions[0].x - this.transform.position.x, 0);
			}
			else if (GlobalVariables.playerPositions [0].x < this.transform.position.x - 4f) {
				//newPos.x -= 0.1f;
				rb.velocity = new Vector2(GlobalVariables.playerPositions[0].x - this.transform.position.x, 0);
			}
			else {
				rb.velocity = new Vector2(0,0);
			}
			//this.transform.position = newPos;
				
			
			
		}
		else {
			//Don't know what to do for no players, but may be necessary later
		}
	}
}
