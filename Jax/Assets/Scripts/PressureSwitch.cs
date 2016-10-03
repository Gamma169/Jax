﻿using UnityEngine;
using System.Collections;

public class PressureSwitch : MonoBehaviour {

	public bool active;

	[Tooltip("'reset' will automatically make it a 'locking' switch if both are checked")]
	public bool reset = false;
	public bool locking = true;


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

	private bool resetTimer;
	private int resetCounter;

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

		if (reset) {
			locking = true;
			resetCounter = 30;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (resetTimer && resetCounter < 0) {
			resetCounter--;
		}
		else {
			resetTimer = false;
			resetCounter = 30;
		}

		if ((SLJ.jointTranslation < 0.25f) && !resetTimer)
			active = true;
		//Only do this if the state has changed since the last frame
		if (stateChange != active) {
			//Run through the arrays and change the states of the array elements
			for (int i = 0; i < MAScripts.Length; i++) {
				if (MAScripts[i])
					MAScripts[i].scriptActive = active;
				//if (SAScripts[i])
				//SAScripts[i].scriptActive = active;
			}
			//print("test");
			if (active) {
				SR.color = activColor;
				if (locking)
					SLJ.limits = newLimits;
			}
			else {
				SR.color = deactColor;
				SLJ.limits = oldLimits;
			}

			stateChange = active;
		}

		// This is to reset for the movement scripts
		if (active && reset) {
			bool allDone = true;
			for (int i = 0; i < MAScripts.Length; i++) {
				if (MAScripts[i] && !MAScripts[i].isDone()) {
					allDone = false;
				}
			}
			if (allDone) {
				active = false;
				resetTimer = true;
				SLJ.limits = oldLimits;
			}
		}


	
	}
}
