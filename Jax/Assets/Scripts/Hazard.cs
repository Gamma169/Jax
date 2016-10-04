using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			// We set destroying true here, and then destroyed when the explosion is created in the global variables object
			PlayerInfo pInfo = other.GetComponent<PlayerInfo>();
			if (!pInfo.destroyed)
				pInfo.destroying = true;
			//GlobalVariables.DestroyPlayer(other.gameObject);
		}
			
		FreeBoxControl fbc = other.gameObject.GetComponent<FreeBoxControl>();
		if (fbc) {
			fbc.StartDestroy();
		}
	
	
	}
}
