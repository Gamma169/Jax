using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public int thisCheckpoint;

	public Color deacColor;
	public Color actiColor;

	private Vector3 position;

	private bool active;

	private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
	
		position = transform.position;
		rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		// This variable is just so we don't constantly update the color every frame.  Hopefully saves a tiny bit of overhead.
		bool changed = false;

		if (GlobalVariables.spawnAtCheckpoint == thisCheckpoint) {
			active = true;
			changed = true;
		}
		else {
			active = false;
			changed = true;
		}
			
		if (changed) {
			if (active) {
				rend.flipY = false;
				rend.color = actiColor;
			}
			else {
				rend.flipY = true;
				rend.color = deacColor;
			}
		}
	}


	public Vector3 Position() {
		return position;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
			GlobalVariables.spawnAtCheckpoint = thisCheckpoint;
	}

}
