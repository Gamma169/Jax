using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {


	// I need both of these to act as a lockout function so that we don't create a million explosions when hitting a hazard
	public bool destroying;
	public bool destroyed;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (destroyed)
			rb.isKinematic = true;
			
	}
}
