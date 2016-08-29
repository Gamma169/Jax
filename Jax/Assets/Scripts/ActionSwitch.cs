using UnityEngine;
using System.Collections;

public class ActionSwitch : MonoBehaviour {

	public bool active;
	//Do we want it to be toggleable or only one-time use
	public bool toggle;

	public Color deactColor;
	public Color activColor;

	//I only have this so I don't have to constantly run a for loop.  I only want to run it once when the state has changed.
	private bool stateChange;

	// I made this an array so the switch can activate a bunch of things instead of just one.
	public GameObject[] activatees;
	private MovementActivator[] MAScripts;
//	private SpringActivator[] SAScripts		//Curently I don't have any spring activators, but when I do, I can come back to this script and just add them quickly.

	private SpriteRenderer SR;


	// Use this for initialization
	void Start () {
		MAScripts = new MovementActivator[activatees.Length];
		//SAScripts.Length = activatees.Length;
		// Go through the array of activatable objects and get references to the activation scripts in those objects
		for (int i=0; i<activatees.Length; i++) {
			MAScripts[i] = activatees[i].GetComponent<MovementActivator>();
			//SAScripts[i] = activatees[i].GetComponent<SpringActivator>();
		}
		SR = GetComponent<SpriteRenderer>();
		stateChange = active;
	}
	
	// Update is called once per frame
	void Update () {

		//Only do this if the state has changed since the last frame
		if (stateChange != active) {
			//Run through the arrays and change the states of the array elements
			for (int i = 0; i < MAScripts.Length; i++) {
				if (MAScripts[i])
					MAScripts[i].scriptActive = active;
				//if (SAScripts[i])
				//SAScripts[i].scriptActive = active;
			}
			//Change the color
			if (active)
				SR.color = activColor;
			else
				SR.color = deactColor;
			//Set the states to be the same
			stateChange = active;
		}
			
	}

	// We only want to activate if the player is close enough
	void OnTriggerStay2D(Collider2D other) {
		if (!toggle && Input.GetKey(KeyCode.RightControl) && other.gameObject.tag == "Player")
			active = true;
		else if (Input.GetKeyDown(KeyCode.RightControl) && other.gameObject.tag == "Player") {
			if (toggle)
				active = !active;
			else
				active = true;
		}
	
	}
}
