using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LevelChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			int i = SceneManager.GetActiveScene().buildIndex;
			GlobalVariables.spawnAtCheckpoint = -1;
			SceneManager.LoadScene(i + 1);
		}
	}
}
