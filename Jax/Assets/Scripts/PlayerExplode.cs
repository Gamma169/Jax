using UnityEngine;
using System.Collections;

public class PlayerExplode : MonoBehaviour {

	public Color startCol;
	public Color endCol;

	private float maxSize = .5f;

	private float LERP;
	private bool bigger = true;

	private SpriteRenderer sr;
	private Transform tr;

	// Use this for initialization
	void Start () {
		tr = GetComponent<Transform>();
		sr = GetComponent<SpriteRenderer>();
		LERP = .1f;
		StartCoroutine("Explode");
	}
	
	// Update is called once per frame
	void Update () {
		//sr.color = Color.Lerp(startCol, endCol, Mathf.PingPong(Time.time, 1));

	}


	public IEnumerator Explode() {
		while (true) {
			
			tr.localScale = new Vector3(maxSize * LERP, maxSize * LERP, 1);
			sr.color = Color.Lerp(startCol, endCol, LERP);
			//print(LERP);
			LERP = LERP + (1 / 15);
			if (LERP >= 1) {
				LERP = 1;
				break;
			}
			yield return new WaitForSeconds(.017f);
		}
		while (true) {
			tr.localScale = new Vector3(maxSize * LERP, maxSize * LERP, 1);
			sr.color = Color.Lerp(startCol, endCol, LERP);
			LERP += (1 / 90);
			if (LERP <=  .1f) {
				LERP = .1f;
				break;
			}
			yield return new WaitForSeconds(.017f);
		}
		GameObject.Destroy(this.gameObject);
	
	}
}
