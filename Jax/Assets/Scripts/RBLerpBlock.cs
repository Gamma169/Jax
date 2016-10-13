using UnityEngine;
using System.Collections;

public class RBLerpBlock : MonoBehaviour {

	public bool active;

	public Vector2 endPosition;

	//[Tooltip("0- Linear, 1-Coserp, 2- Sinerp, 3- Smoothstep, 4-Smootherstep")]
	//public int LERPType = 0;
	// Coserp- "Ease In" t = 1f - MAthf.Cos(t * Mathf.PI * 0.5f)
	// Sinerp- "Ease Out" t = Mathf.sin(t * MAthf.PI * 0.5f)
	// Smoothstep- t = t*t * (3f = 2f*t)
	// Smootherstep- t = t*t*t * (t * (6f*t - 15f) + 10f)


	public bool useSpeed;
	public float speedToEnd = 3;
	public float speedToStart = 3;

	public float timeToEnd = 2;
	public float timeToStart = 2;

	private Vector2 startPos;
	private Vector2 vFor;
	private Vector2 vBack;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		startPos = new  Vector2(transform.position.x, transform.position.y);

		if (!useSpeed) {
			vFor = new Vector2((endPosition.x - startPos.x) / timeToEnd, (endPosition.y - startPos.y) / timeToEnd);
			vBack = new Vector2((startPos.x - endPosition.x) / timeToStart, (startPos.y - endPosition.y) / timeToStart);
		}
		else {
			float a = startPos.x;
			float b = startPos.y;
			float x = endPosition.x;
			float y = endPosition.y;
			float tfor = Mathf.Sqrt(Mathf.Pow(x, 2) - 2 * x * a + Mathf.Pow(a, 2) + Mathf.Pow(y, 2) - 2 * y * b + Mathf.Pow(b, 2)) / (0.5f * speedToEnd);
			float tback = Mathf.Sqrt(Mathf.Pow(x, 2) - 2 * x * a + Mathf.Pow(a, 2) + Mathf.Pow(y, 2) - 2 * y * b + Mathf.Pow(b, 2)) / (0.5f * speedToStart);

			vFor = new Vector2((x-a)/tfor, (y-b)/tfor);
			vBack = new Vector2((a-x)/tback, (b-y)/tback);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (active)
			moveForward();
		else
			moveBack();

		//print(findPlace());
	}

	public void moveForward(){
		if ((transform.position.Equals(endPosition)) || findPlace() >= 1) {
			rb.velocity = Vector2.zero;
			if (findPlace() > 1)
				transform.position = endPosition;
		}
		else
			rb.velocity = vFor;
	}

	public void moveBack() {
		if ((transform.position.Equals(startPos)) || findPlace() <= 0) {
			rb.velocity = Vector2.zero;
			if (findPlace() < 0)
				transform.position = startPos;
		}
		else
			rb.velocity = vBack;
	}

	public float findPlace(){
		float t = Mathf.Sqrt(Mathf.Pow(transform.position.x - startPos.x, 2) + Mathf.Pow(transform.position.y - startPos.y, 2)) / Mathf.Sqrt(Mathf.Pow(endPosition.x - startPos.x, 2) + Mathf.Pow(endPosition.y - startPos.y, 2));	

		if (endPosition.x > startPos.x && transform.position.x < startPos.x)
				t = -t;
		else if (endPosition.x < startPos.x && transform.position.x > startPos.x)
				t = -t;

		return t;
	}
}
