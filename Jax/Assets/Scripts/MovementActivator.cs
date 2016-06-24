using UnityEngine;
using System.Collections;

public class MovementActivator : MonoBehaviour {

	public Color deactColor;
	public Color activColor;

	public bool scriptActive;

	private MovingPlatform MPScript;
	private SpriteRenderer SR;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		MPScript = GetComponent<MovingPlatform>();
		SR = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		MPScript.enabled = scriptActive;

		if (scriptActive)
			SR.color = activColor;
		else {
			SR.color = deactColor;
			rb.velocity = Vector2.zero;
		}
	}
}
