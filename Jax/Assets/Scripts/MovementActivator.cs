using UnityEngine;
using System.Collections;

public class MovementActivator : MonoBehaviour {

	public Color deactColor;
	public Color activColor;

	public bool useSprite;
	public Sprite activeSprite;

	public bool scriptActive;

	// These variables are for a reset switch
	private bool resetter;
	private bool done;

	private SpriteRenderer SR;
	private Sprite deactSprite;
	private MovingPlatform MPScript;
	private Rigidbody2D rb;
	private LERPBlock lb;

	// Use this for initialization
	void Start () {
		MPScript = GetComponent<MovingPlatform>();
		SR = GetComponent<SpriteRenderer>();
		deactSprite = SR.sprite;
		rb = GetComponent<Rigidbody2D>();
		lb = GetComponent<LERPBlock>();

		if (useSprite && !activeSprite) {
			useSprite = false; 
			Debug.Log("Warning: active sprite for object " + gameObject.name + " is not set");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (MPScript) {
			MPScript.enabled = scriptActive;

			// Make sure that if the resetter is active, then the movement script is labeled as doOnce
			if (resetter && !MPScript.doOnce)
				MPScript.doOnce = true;
		

			if (!scriptActive)
				rb.velocity = Vector2.zero;

			if (MPScript.enabled) {
				done = MPScript.isDone();
			}
		}

		if (lb) {
			lb.active = scriptActive;
		} 


		if (scriptActive) {
			if (useSprite)
				SR.sprite = activeSprite;
			else
				SR.color = activColor;
		}
		else {
			if (useSprite)
				SR.sprite = deactSprite;
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
