﻿using UnityEngine;
using System.Collections;

public class PlayerExplode : MonoBehaviour {

	public bool basic = true;

	public Color startCol;
	public Color endCol;

	private float maxSize = .5f;

	private float LERP;

	private SpriteRenderer sr;
	private Transform tr;

	// Use this for initialization
	void Start () {
		tr = GetComponent<Transform>();
		sr = GetComponent<SpriteRenderer>();
		LERP = .1f;
		//This next line is for creating a basic explosion via the renderer.  When using an actual explosion animation, it is not necessary.
		if (basic)
			StartCoroutine("Explode");

	}
	
	// Update is called once per frame
	void Update () {
		

	}


	public IEnumerator Explode() {
		while (true) {
			
			tr.localScale = new Vector3(maxSize * LERP, maxSize * LERP, 1);
			sr.color = Color.Lerp(startCol, endCol, LERP);
			LERP += LERP  * 0.06f;
			yield return new WaitForSeconds(.02f);
			if (LERP >= 1) {
				LERP = 1;
				break;
			}
		}
		while (true) {
			tr.localScale = new Vector3(maxSize * LERP, maxSize * LERP, 1);
			sr.color = Color.Lerp(startCol, endCol, LERP);
			LERP -= LERP  * 0.06f;
			if (LERP <=  .1f) {
				LERP = .1f;
				break;
			}
			yield return new WaitForSeconds(.02f);
		}
			
		GameObject.Destroy(this.gameObject);

	
	}


	// This method is for the animation version so that when the animation finishes, the explosion destroys itself
	void finishedAnim() {
		GameObject.Destroy(this.gameObject);
	}
}
