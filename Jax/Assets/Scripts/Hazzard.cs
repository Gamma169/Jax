using UnityEngine;
using System.Collections;

public class Hazzard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2d(Collider2D other) {
		if (other.gameObject.tag == "Player") {
	//		GlobalVariables.DestroyPlayer(other.gameObject);
		}
			
	
	
	}
}
