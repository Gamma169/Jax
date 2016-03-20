using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GlobalVariables : MonoBehaviour {

	public static Vector3[] playerPositions;

	public static bool pControl;

	public static bool invertLegControl;

	private GameObject[] players;

	// Use this for initialization
	void Start () {

		// Get positions of all players in scene
		players = GameObject.FindGameObjectsWithTag ("Player");
		if (players.Length >= 1) {
			playerPositions = new Vector3[players.Length];
			for (int i = 0; i < playerPositions.Length; i++) {
				playerPositions [i] = players [i].transform.position;
			}
		}
		else {
			print("Error: Player not found in scene");
		}
		pControl = true;
		invertLegControl = false;

		/*
		print (Mathf.Cos (0));
		print (Mathf.Cos (-Mathf.PI));
		print (Mathf.Cos (-Mathf.PI * 3 / 4));
		print (Mathf.Cos (-Mathf.PI / 2));
		print (Mathf.Cos (-Mathf.PI / 4));
	
		print ("Positive");
		print (Mathf.Cos (0));
		print (Mathf.Cos (Mathf.PI / 4));
		print (Mathf.Cos (Mathf.PI / 2));
		print (Mathf.Cos (Mathf.PI * 3 / 4));
		print (Mathf.Cos (Mathf.PI));
		*/
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("r"))
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	
		for (int i = 0; i < players.Length; i++) {
			playerPositions [i] = players [i].transform.position;
		}
	
	
	}
}
