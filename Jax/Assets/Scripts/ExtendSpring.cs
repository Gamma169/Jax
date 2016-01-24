using UnityEngine;
using System.Collections;

public class ExtendSpring : MonoBehaviour {

	public float regSpringLength = 3f;
	public float extSpringLength = 5f;

	public SpringJoint2D spring;
	public Transform footTransform;
	public Transform springSpriteTrans;

	private bool jiggle;
	private Rigidbody2D rb;
	private float startSpringLength;
	private Vector3 startSpringSpriteSize;
	private float startDistance;


	// Use this for initialization
	void Start () {
		jiggle = false;
		spring = GetComponent<SpringJoint2D> ();
		rb = GetComponent<Rigidbody2D> ();
		startSpringLength = spring.distance;
		startSpringSpriteSize = springSpriteTrans.localScale;
		startDistance = Vector3.Distance(footTransform.position, transform.position);
	}


	// Update is called once per frame
	void Update () {
		
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
			
		float springXPos = (this.transform.position.x + footTransform.position.x) / 2;
		float springYPos = (this.transform.position.y + footTransform.position.y) / 2;
		float springZPos = (this.transform.position.z + footTransform.position.z) / 2;
		springSpriteTrans.position = new Vector3 (springXPos, springYPos, springZPos);




		float dist = Vector3.Distance(footTransform.position, transform.position);



		/*
		if (Input.GetKey ("space") && spring.distance <= extSpringLength)
			spring.distance = spring.distance + .05f;
		if (!Input.GetKey ("space") && spring.distance >= regSpringLength )
			spring.distance = spring.distance - .05f;
		*/
	}
}
