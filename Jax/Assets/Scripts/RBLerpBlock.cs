using UnityEngine;
using System.Collections;

public class RBLerpBlock : MonoBehaviour {

	public bool active;

	public Vector2 endPos;

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
	// This is in case I nest the object in something else that is moving
	private Rigidbody2D parentRB;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		parentRB = transform.parent.GetComponent<Rigidbody2D>();
		startPos = new  Vector2(transform.localPosition.x, transform.localPosition.y);

		if (!useSpeed) {
			vFor = new Vector2((endPos.x - startPos.x) / timeToEnd, (endPos.y - startPos.y) / timeToEnd);
			vBack = new Vector2((startPos.x - endPos.x) / timeToStart, (startPos.y - endPos.y) / timeToStart);
		}
		else {
			float a = startPos.x;
			float b = startPos.y;
			float x = endPos.x;
			float y = endPos.y;
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
		//print(rb.velocity);
	}

	public void moveForward(){
		if ((Mathf.Abs(transform.localPosition.x - endPos.x) < .05f && Mathf.Abs(transform.localPosition.y - endPos.y) < .05f) || findPlace() >= 1) {
			rb.velocity = Vector2.zero;
			if (findPlace() > 1)
				transform.localPosition = endPos;
		}
		else {
			
			if (parentRB) {
				/*Vector3 locVel = transform.InverseTransformDirection(new Vector3(parentRB.velocity.x, parentRB.velocity.y, 0));
				locVel += new Vector3(vFor.x, vFor.y, 0);
				locVel = transform.TransformDirection(locVel);
				rb.velocity = new Vector2(locVel.x, locVel.y);*/
				rb.velocity = parentRB.velocity + vBack;
			}
			else {
				rb.velocity = vFor;
			}
		}
	}

	public void moveBack() {
		if ((Mathf.Abs(transform.localPosition.x - startPos.x) < .05f && Mathf.Abs(transform.localPosition.y - startPos.y) < .05f) || findPlace() <= 0) {
			rb.velocity = Vector2.zero;
			if (findPlace() < 0)
				transform.localPosition = startPos;
		}
		else {
			if (parentRB)
				rb.velocity = parentRB.velocity + vBack;
			else
				rb.velocity = vBack;
		}
	}

	public float findPlace(){
		float t = Mathf.Sqrt(Mathf.Pow(transform.localPosition.x - startPos.x, 2) + Mathf.Pow(transform.localPosition.y - startPos.y, 2)) / Mathf.Sqrt(Mathf.Pow(endPos.x - startPos.x, 2) + Mathf.Pow(endPos.y - startPos.y, 2));	

		if ((endPos.x > startPos.x && transform.localPosition.x < startPos.x) || (endPos.y > startPos.y && transform.localPosition.y < startPos.y))
				t = -t;
		else if ((endPos.x < startPos.x && transform.localPosition.x > startPos.x) || (endPos.y < startPos.y && transform.localPosition.y > startPos.y))
				t = -t;

		return t;
	}
}
