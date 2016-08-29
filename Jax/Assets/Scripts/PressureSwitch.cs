using UnityEngine;
using System.Collections;

public class PressureSwitch : MonoBehaviour {

	public bool active;

	public Color deactColor;
	public Color activColor;

	// I made this an array so the switch can activate a bunch of things instead of just one.
	public GameObject[] activatees;
	private MovementActivator[] MAScripts;

	private SliderJoint2D SLJ;
	private SpriteRenderer SR;

	private JointTranslationLimits2D oldLimits;
	private JointTranslationLimits2D newLimits;

	//I only have this so I don't have to constantly run a for loop.  I only want to run it once when the state has changed.
	private bool stateChange;

	// Use this for initialization
	void Start () {

		MAScripts = new MovementActivator[activatees.Length];
		//SAScripts.Length = activatees.Length;
		// Go through the array of activatable objects and get references to the activation scripts in those objects
		for (int i=0; i<activatees.Length; i++) {
			MAScripts[i] = activatees[i].GetComponent<MovementActivator>();
			//SAScripts[i] = activatees[i].GetComponent<SpringActivator>();
		}

		SLJ = GetComponent<SliderJoint2D>();
		SR = GetComponent<SpriteRenderer>();

		oldLimits = SLJ.limits;

		newLimits.max = .18f;
		newLimits.min = .05f;

		SR.color = deactColor;
	}
	
	// Update is called once per frame
	void Update () {


		active = SLJ.jointTranslation < 0.25f;
		//Only do this if the state has changed since the last frame
		if (stateChange != active) {
			//Run through the arrays and change the states of the array elements
			for (int i = 0; i < MAScripts.Length; i++) {
				if (MAScripts[i])
					MAScripts[i].scriptActive = active;
				//if (SAScripts[i])
				//SAScripts[i].scriptActive = active;
			}

			if (active) {
				SR.color = activColor;
				SLJ.limits = newLimits;
			}
			else {
				SR.color = deactColor;
				SLJ.limits = oldLimits;
			}

			stateChange = active;
		}


	
	}
}
