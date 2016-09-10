using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GlobalVariables : MonoBehaviour {
	public const string BOUNDRY_LAYER = "Boundries";

	public static Vector3[] playerPositions;

	public static bool pControl;

	public static bool invertLegControl;

	public static Rigidbody2D[] playerRBs;

	public GameObject PE;

	private GameObject[] players;
	// We have an array of explosions corresponding to the players so if they hit hazards, the explosions will know which are associated with each player
	private GameObject[] exps;
	//This variable is to reset the level if all the players are destroyed
	private bool noPlayers;

	// Use this for initialization
	void Start () {

		// Get positions of all players in scene
		players = GameObject.FindGameObjectsWithTag ("Player");
		exps = new GameObject[players.Length];
		if (players.Length >= 1) {
			playerPositions = new Vector3[players.Length];
			playerRBs = new Rigidbody2D[players.Length];
			for (int i = 0; i < playerPositions.Length; i++) {
				playerPositions[i] = players[i].transform.position;
				playerRBs[i] = players[i].GetComponent<Rigidbody2D>();
				//print(playerRBs[i]);
			}
		}
		else {
			noPlayers = true;
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

		if (Input.GetKeyDown(KeyCode.Escape) || noPlayers)
			ResetLevel();

		//if we go through the array and never change this variable, that means all the players have been destroyed and the level will reset
		noPlayers=true;
		for (int i = 0; i < players.Length; i++) {
			if (players[i] != null) {
				noPlayers = false;
				playerPositions[i] = players[i].transform.position;
				PlayerInfo pi = players[i].GetComponent<PlayerInfo>();
				// The next four lines deal with destroying a player if he comes in contact with a hazard
				// When the player starts destroying, we create an explosion and add it to the corresponding place in the array
				if (pi.destroying) 
					exps[i] = CreateExplosion(players[i]);
				// Once the explosion finishes exploding and destroys itself, this line will destroy the corresponding player with the finished explosion
				if (exps[i] == null && pi.destroyed)
					GameObject.Destroy(players[i]);
			}
		}
	}


	public GameObject CreateExplosion(GameObject p) {
		PlayerInfo pi = p.GetComponent<PlayerInfo>();
		pi.destroying = false;
		pi.destroyed = true;
		Transform t = p.GetComponent<Transform>();
		GameObject explosion = (GameObject) Instantiate(PE, t.position, Quaternion.identity);
		return explosion;
	
	}



	public static void ResetLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

}
