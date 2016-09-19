using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {


	[Tooltip("Make sure that checkpoints are numbered from a 0-based index")]
	// The reason they have to be numbered this way is because they correspond to the GlobalVariabls variable SpawnAtCheckpoint and are indexed in an array in the Global Variables Object.
	// If the number is too high (meaning you don't start at 0 or you skip a number), the level will throw an array index out of bounds error instead of spawning the player at the checkpoint
	// If the number is too low or you repeat a checkpoint number (more than one checkpoint should light up if this happens), it will use the earlier checkpoint it cameacross with the FindObjectsWothTag function. (IE, unpredictable behavior)
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
