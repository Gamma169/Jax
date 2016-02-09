﻿using UnityEngine;
using System.Collections;

public class SpringControl : MonoBehaviour {

	public static float springFreq;
	public static float springDamp;

	public float regSpringLength;
	public float extSpringLength;

	public SpringJoint2D spring;
	public Transform footTransform;
	public Transform springSpriteTrans;

	private bool jiggle;
	private Rigidbody2D rb;
	//private float startSpringLength;
	private Vector3 startSpringSpriteSize;
	private float startDistance;


	// Use this for initialization
	void Start () {
		jiggle = false;
		spring = GetComponent<SpringJoint2D> ();
		rb = GetComponent<Rigidbody2D> ();
		//startSpringLength = spring.distance;
		startSpringSpriteSize = springSpriteTrans.localScale;
		startDistance = Vector3.Distance(footTransform.position, transform.position);

		springFreq = spring.frequency;
		springDamp = spring.dampingRatio;
	}


	// Update is called once per frame
	void Update () {
			
		if (Input.GetKeyDown ("p") && springFreq < 15f)
			springFreq += 0.5f;
		if (Input.GetKeyDown ("o") && springFreq > 2f)
			springFreq -= 0.5f;

		if (Input.GetKeyDown ("l") && springDamp < 1f)
			springDamp += 0.1f;
		if (Input.GetKeyDown ("k") && springDamp > 0f)
			springDamp -= 0.1f;

		spring.frequency = springFreq;
		spring.dampingRatio = springDamp;
		


		float springXPos = (this.transform.position.x + footTransform.position.x) / 2;
		float springYPos = (this.transform.position.y + footTransform.position.y) / 2;
		float springZPos = (this.transform.position.z + footTransform.position.z) / 2;
		springSpriteTrans.position = new Vector3 (springXPos, springYPos, springZPos);


		float dist = Vector3.Distance(footTransform.position, transform.position);
		float scale = dist / startDistance;
		springSpriteTrans.localScale = new Vector3 (startSpringSpriteSize.x * (1/scale), startSpringSpriteSize.y * scale, startSpringSpriteSize.z);

		float angle = Mathf.Atan2 (this.transform.position.x - footTransform.position.x, this.transform.position.y - footTransform.position.y) * 180 / Mathf.PI;
		springSpriteTrans.rotation = Quaternion.Euler(new Vector3 (0, 0, -angle));


		if (GlobalVariables.pControl) {
			if (Input.GetKey ("space")) {
				if (!jiggle) {
					jiggle = true;
					rb.AddForce (transform.up * 0.001f);
				}
				spring.distance = extSpringLength;
			} 
			else {
				jiggle = false;
				spring.distance = regSpringLength;
			}
		}


		else {
			AutoControl ();
		}

		/*
		if (Input.GetKey ("space") && spring.distance <= extSpringLength)
			spring.distance = spring.distance + .05f;
		if (!Input.GetKey ("space") && spring.distance >= regSpringLength )
			spring.distance = spring.distance - .05f;
		*/
	}


	private void AutoControl() {
	
	
	}
}
