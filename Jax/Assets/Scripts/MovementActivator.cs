using UnityEngine;
using System.Collections;

public class MovementActivator : MonoBehaviour {

	public Color deactColor;
	public Color activColor;

	public bool scriptActive;

	// These variables are for a reset switch
	private bool resetter;
	private bool done;

	private SpriteRenderer SR;
	private MovingPlatform MPScript;
	private Rigidbody2D rb;
	private LERPBlock lb;

	// Use this for initialization
	void Start () {
		MPScript = GetComponent<MovingPlatform>();
		SR = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		lb = GetComponent<LERPBlock>();
	}
	
	// Update is called once per frame
	void Update () {
		if (MPScript) {
			MPScript.enabled = scriptActive;

			// Make sure that if the resetter is active, then the movement script is labeled as doOnce
			if (resetter && !MPScript.doOnce)
				MPScript.doOnce = true;
		

			if (scriptActive)
				SR.color = activColor;
			else {
				SR.color = deactColor;
				rb.velocity = Vector2.zero;
			}

			if (MPScript.enabled) {
				done = MPScript.isDone();
			}
		}
		if (lb) {
			lb.active = scriptActive;

			if (scriptActive)
				SR.color = activColor;
			else 
				SR.color = deactColor;
			
		} 
	}

	public void setResetter(bool set) {
		resetter = set;
	}

	public bool isDone() {
		return done;
	}

	public void resetMP() {
		MPScript.resetDone();
		scriptActive = false;
	}
		
}
