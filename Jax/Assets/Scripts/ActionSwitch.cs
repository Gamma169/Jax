using UnityEngine;
using System.Collections;

public class ActionSwitch : MonoBehaviour {

	public bool active;
	//Do we want it to be toggleable or only one-time use
	public bool toggle;
	public bool reset; // If it's a reset switch, it will reset once all the moving platforms have finished their motion
	// Note, will override toggle
	public float resetTime = 0.25f;

	public Color deactColor;
	public Color activColor;

	//I only have this so I don't have to constantly run a for loop.  I only want to run it once when the state has changed.
	private bool stateChange;
	// This is the player object has two colliders on it, so it screws with the OnTriggerStay function, calling it three times in quick succession
	private bool lockout;
	private bool playerInDist;

	// I made this an array so the switch can activate a bunch of things instead of just one.
	// NOTE: Because I'm using the action switch to activate more than one object, I should have all my objects be of the same type.
	//       IN theory, I can have them be of different types, but then it may screw with the reset variable.  
	//		 I can't have a Box Maker that is able to do multiple boxes and movement scripts together.  But one box is fine.
	public GameObject[] activatees;
	private MovementActivator[] MAScripts;
	private BoxMaker[] BMakerScripts;
	private bool BReset;
//	private SpringActivator[] SAScripts		//Curently I don't have any spring activators, but when I do, I can come back to this script and just add them quickly.

	private SpriteRenderer SR;


	// Use this for initialization
	void Start () {
		MAScripts = new MovementActivator[activatees.Length];
		BMakerScripts = new BoxMaker[activatees.Length];
		//SAScripts.Length = activatees.Length;
		// Go through the array of activatable objects and get references to the activation scripts in those objects
		for (int i=0; i<activatees.Length; i++) {
			MAScripts[i] = activatees[i].GetComponent<MovementActivator>();
			if (MAScripts[i])
				MAScripts[i].setResetter(reset);

			BMakerScripts[i] = activatees[i].GetComponent<BoxMaker>();
			if (BMakerScripts[i] && BMakerScripts[i].numBoxes >= 1) {
				if (BMakerScripts[i].numBoxes == 1)
					toggle = true;   // Box makers that create only one box should always be toggle switches so that we don't get the box stuck and can just come back to the switch to destroy it
				else
					BReset = true;
				reset = false;  // This needs to be set so we don't run into issues resetting movement scripts that aren't there
			}
			//SAScripts[i] = activatees[i].GetComponent<SpringActivator>();
		}
		SR = GetComponent<SpriteRenderer>();
		stateChange = active;

		if (reset || BReset)
			toggle = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Only do this if the state has changed since the last frame
		if (stateChange != active) {
			//Run through the arrays and change the states of the array elements
			for (int i = 0; i < activatees.Length; i++) {
				if (MAScripts[i]) {
					if (!active && reset) {
						MAScripts[i].resetMP();
					}
					else 
						MAScripts[i].scriptActive = active;
				}
				if (BMakerScripts[i]) {
					BMakerScripts[i].active = active;
					if (active)
						BMakerScripts[i].CreateBox();
				
				}
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
		// This is to reset for the movement scripts
		if (active && reset) {
			bool allDone = true;
			for (int i = 0; i < MAScripts.Length; i++) {
				if (MAScripts[i] && !MAScripts[i].isDone())
					allDone = false;
			}
			if (allDone) {
				active = false;
			}
		}
		// This is to reset for the box maker script if we have a box maker that can create more than one box
		if (active && BReset && !lockout) {
			lockout = true;
			StartCoroutine("BoxReset");
		}
			
		// This needs to be here instead of the OnTriggerStay because it was giving issues not registerring things properly otherwise
		if ((Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)) && playerInDist) {
			if (toggle) {
				if (!lockout) {
					lockout = true;
					active = !active;
					StartCoroutine("Unlock");
				}
			}
			else
				active = true;
		}
	
	
	
	
	}

	// We only want to activate if the player is close enough
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
			playerInDist = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
			playerInDist = false;
	}


	IEnumerator Unlock() {
		//yield return new WaitForSeconds(0.08f);
		yield return new WaitForSeconds(0.05f);
		lockout = false;
	}

	IEnumerator BoxReset() {
		yield return new WaitForSeconds(resetTime);
		active = false;
		lockout = false;

	}
}
