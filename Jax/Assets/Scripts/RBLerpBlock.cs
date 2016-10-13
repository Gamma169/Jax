using UnityEngine;
using System.Collections;

public class RBLerpBlock : MonoBehaviour {

	public bool active;

	public Vector2 endPosition;

	[Tooltip("0- Linear, 1-Coserp, 2- Sinerp, 3- Smoothstep, 4-Smootherstep")]
	public int LERPType = 0;
	// Coserp- "Ease In" t = 1f - MAthf.Cos(t * Mathf.PI * 0.5f)
	// Sinerp- "Ease Out" t = Mathf.sin(t * MAthf.PI * 0.5f)
	// Smoothstep- t = t*t * (3f = 2f*t)
	// Smootherstep- t = t*t*t * (t * (6f*t - 15f) + 10f)



	public float SpeedToEnd = 2;
	public float SpeedToStart = 2;


	private int counter;
	private Vector2 startPos;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		counter = 0;
		rb = GetComponent<Rigidbody2D>();
		startPos = new  Vector2(transform.position.x, transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		if (active)
			moveToPosition(endPosition);
		else
			moveToPosition(startPos);
	}


	public void moveToPosition(Vector2 pos) {
	
	}
}
