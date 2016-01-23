using UnityEngine;
using System.Collections;

public class ExtendSpring : MonoBehaviour {

	public float regSpringLength = 3f;
	public float extSpringLength = 5f;

	public SpringJoint2D spring;

	// Use this for initialization
	void Start () {
		spring = GetComponent<SpringJoint2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("space") )
			spring.distance = extSpringLength;
		else
			spring.distance = regSpringLength;





		/*
		if (Input.GetKey ("space") && spring.distance <= extSpringLength)
			spring.distance = spring.distance + .05f;
		if (!Input.GetKey ("space") && spring.distance >= regSpringLength )
			spring.distance = spring.distance - .05f;
		*/
	}
}
