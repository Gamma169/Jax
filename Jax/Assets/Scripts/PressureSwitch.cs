﻿using UnityEngine;
using System.Collections;

public class PressureSwitch : MonoBehaviour {

	public bool active;

	[Tooltip("'reset' will automatically make it a 'locking' switch if both are checked")]
	public bool reset = false;
	public bool locking = true;


	public Color deactColor;
	public Color activColor;

	public bool useSprites;
	public Sprite activeSprite;

	// I made this an array so the switch can activate a bunch of things instead of just one.
	public GameObject[] activatees;
	private MovementActivator[] MAScripts;
	private WallSpringActivator[] WallSpringScipts;

	private SliderJoint2D SLJ;
	private SpriteRenderer SR;
	private Sprite deactSprite;

	private JointTranslationLimits2D oldLimits;
	private JointTranslationLimits2D newLimits;

	//I only have this so I don't have to constantly run a for loop.  I only want to run it once when the state has changed.
	private bool stateChange;

	private bool resetting;
	//private int resetCounter;

	// Use this for initialization
	void Start () {

		MAScripts = new MovementActivator[activatees.Length];
		WallSpringScipts = new WallSpringActivator[activatees.Length];

		// Go through the array of activatable objects and get references to the activation scripts in those objects
		for (int i=0; i<activatees.Length; i++) {
			MAScripts[i] = activatees[i].GetComponent<MovementActivator>();
			if (MAScripts[i])
				MAScripts[i].setResetter(reset);
			WallSpringScipts[i] = activatees[i].GetComponent<WallSpringActivator>();
		}

		SLJ = GetComponent<SliderJoint2D>();
		SR = GetComponent<SpriteRenderer>();
		deactSprite = SR.sprite;

		oldLimits = SLJ.limits;

		newLimits.max = .18f;
		newLimits.min = .05f;

		if (reset) {
			locking = true;
		}

		if (useSprites && !activeSprite) {
			useSprites = false;
			Debug.Log("Warning: ActiveSprite not set for " + gameObject.name);
		}
		// This shouldn't just be an "else" statement because we update useSprites in the line above
		if (!useSprites)
			SR.color = deactColor;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		active = SLJ.jointTranslation < 0.25f && !resetting;

		
		//Only do this if the state has changed since the last frame
		if (stateChange != active) {
			//Run through the arrays and change the states of the array elements
			for (int i = 0; i < activatees.Length; i++) {

				if (MAScripts[i]) {
					if (!active && reset) {
						MAScripts[i].resetMP();
						resetting = false;
					}
					else 
						MAScripts[i].scriptActive = active;
				}	
				if (WallSpringScipts[i])
					WallSpringScipts[i].active = active;
			}

			// Change the Sprites/Color and limits if active
			if (active) {
				if (useSprites)
					SR.sprite = activeSprite;
				else
					SR.color = activColor;

				if (locking)
					SLJ.limits = newLimits;
			}
			else {
				if (useSprites)
					SR.sprite = deactSprite;
				else
					SR.color = deactColor;

				SLJ.limits = oldLimits;
			}

			stateChange = active;
		}

		// This is to reset the movement scripts
		if (active && reset) {
			bool allDone = true;
			for (int i = 0; i < MAScripts.Length; i++) {
				if (MAScripts[i] && !MAScripts[i].isDone() || WallSpringScipts[i]) {
					allDone = false;
				}
			}
			if (allDone) {
				StartCoroutine("Reset");
			}
		}
	
	}


	IEnumerator Reset() {
		active = false;
		SLJ.limits = oldLimits;
		resetting = true;
		yield return new WaitForSeconds(0.5f);
		resetting = false;
	}
}
