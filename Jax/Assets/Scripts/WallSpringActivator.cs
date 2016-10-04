using UnityEngine;
using System.Collections;

public class WallSpringActivator : MonoBehaviour {

	public bool active;

	public float offSpringLength = 2f;
	public float onSpringLength = 0.25f;

	public float timeToOpen = 1f;
	public float timeToClose = 1f;

	public Color activeColor;
	public Color deactColor;

	private float openLerp;
	private SpriteRenderer rend;
	private SpringJoint2D spring;

	// Use this for initialization
	void Start () {
		spring = GetComponent<SpringJoint2D>();
		//onSpringLength = spring.distance;
		spring.autoConfigureDistance = false;

		rend = GetComponentsInChildren<SpriteRenderer>()[1];
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			rend.color = activeColor;
			openLerp += Time.deltaTime / timeToOpen;
			if (openLerp > 1)
				openLerp = 1;
		}
		else {
			rend.color = deactColor;
			openLerp -= Time.deltaTime / timeToClose;
			if (openLerp < 0)
				openLerp = 0;
		}

		spring.distance = Mathf.Lerp(offSpringLength, onSpringLength, openLerp);
	}
}
