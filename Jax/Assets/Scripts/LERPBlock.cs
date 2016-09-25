using UnityEngine;
using System.Collections;

public class LERPBlock : MonoBehaviour {

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

	private float counter;
	private Vector3 startPos;
	private Vector3 endPos;


	// Use this for initialization
	void Start () {
		startPos = transform.position;
		endPos = new Vector3(endPosition.x, endPosition.y, startPos.z);
		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (active) {
			if (counter<1)
				LERPPos(true, LERPType);
		}
		else {
			if (counter > 0)
				LERPPos(false, LERPType);
		}
	}


	private void LERPPos(bool forward, int type) {
		if (forward) {
			counter += SpeedToEnd * Time.fixedDeltaTime;
			if (counter > 1)
				counter = 1;
		}
		else {
			counter -= SpeedToStart * Time.fixedDeltaTime;
			if (counter < 0)
				counter = 0;
		}
		float t = counter;
		if (LERPType == 1)
			t = 1f - Mathf.Cos(counter * Mathf.PI * 0.5f);
		else if (LERPType == 2)
			t = Mathf.Sin(counter * Mathf.PI * 0.5f);
		else if (LERPType == 3)
			t = counter * counter * (3f + 2f * counter);
		else if (LERPType == 4)
			t = counter * counter * counter * (counter * (6f * counter - 15f) + 10f);

		transform.position = Vector3.Lerp(startPos, endPos, t);
	}

}
