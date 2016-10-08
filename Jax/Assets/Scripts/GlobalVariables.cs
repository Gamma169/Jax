using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GlobalVariables : MonoBehaviour {
	public const string BOUNDRY_LAYER = "Boundries";

	public static Vector3[] playerPositions;

	public static bool pControl;

	public static bool invertLegControl;

	public static bool mouseControl;

	public static Rigidbody2D[] playerRBs;

	//  This int determines what checkpoint is active and so where to spawn when a player is killed.  If you're starting a level to begin with, it should be -1.
	public static int spawnAtCheckpoint;
	public static Checkpoint[] checkpoints;

	public GameObject playerPrefab;
	public GameObject PE;

	private GameObject[] players;
	private PlayerInfo[] pis;
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
			pis = new PlayerInfo[players.Length];
			for (int i = 0; i < playerPositions.Length; i++) {
				playerPositions[i] = players[i].transform.position;
				playerRBs[i] = players[i].GetComponent<Rigidbody2D>();
				pis[i] = players[i].GetComponent<PlayerInfo>();
				//print(pis[i]);
			}
		}
		else {
			noPlayers = true;
			print("Error: Player not found in scene");
		}
		pControl = true;
		invertLegControl = false;

		spawnAtCheckpoint = -1;
		// NOTE:  Checkpoints need to be 0-indexed for this to work.  It will throw an array index out of bounds error if there is an issue.  See the checkpoint script for more info
		GameObject[] cpts = GameObject.FindGameObjectsWithTag("Checkpoint");
		checkpoints = new Checkpoint[cpts.Length];
		for (int i = 0; i < cpts.Length; i++) {
			// We do the below, because the FindObjectsWithTag returns the objects in an unpredictable order, and so we want to sort them into their proper positions.
			Checkpoint cp = cpts[i].GetComponent<Checkpoint>();
			checkpoints[cp.thisCheckpoint] = cp;
		}


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

		if (Input.GetKeyDown("m"))
			mouseControl = !mouseControl;

		//if we go through the array and never change this variable, that means all the players have been destroyed (and a new one hasn't been created due to a checkpoint) and the level will reset
		noPlayers=true;
		for (int i = 0; i < players.Length; i++) {
			if (players[i] != null) {
				noPlayers = false;

				playerPositions[i] = players[i].transform.position;

				// =====================================================
				// The next section deal with destroying a player if he comes in contact with a hazard
				// When the player starts destroying, we create an explosion and add it to the corresponding place in the array
				if (pis[i].destroying && !pis[i].destroyed) 
					exps[i] = CreateExplosion(players[i]);
				// Once the explosion finishes exploding and destroys itself, this line will destroy the corresponding player with the finished explosion
				// Because of checkpoints, we then spawn a new player at the given checkpoint without resetting the level (if a checkpoint has been reached)
				if (exps[i] == null && pis[i].destroyed) {
					//if (spawnAtCheckpoint != 1) {
						bool ableProtract = players[i].GetComponent<SpringControl>().ableProtract;
						bool ableExtend = players[i].GetComponent<SpringControl>().ableProtract;
					//}
					GameObject.Destroy(players[i]);      //This is the destoyed function as before.


					//  If we hit a checkpoint and therefore have changed the spawnAtCheckpoint variable, we will spawn a new player object at the checkpoint's location.
					//  Otherwise, we won't spawn a player and the level will just reset
					if (spawnAtCheckpoint != -1) {
						// NOTE:  Make sure that the checkpoints in the scene are numbered starting from a 0 index in the order that they appear in the hierarchy.  More info in the checkpoints script
						players[i] = (GameObject)Instantiate(playerPrefab, checkpoints[spawnAtCheckpoint].Position(), Quaternion.identity);
						SpringControl pSC = players[i].GetComponent<SpringControl>();
						pSC.ableProtract = ableProtract;
						pSC.ableExtend = ableExtend;
						pis[i] = players[i].GetComponent<PlayerInfo>();
						playerRBs[i] = players[i].GetComponent<Rigidbody2D>();
					}

				}
				//  =======================================================
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
		spawnAtCheckpoint = -1;
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

}

