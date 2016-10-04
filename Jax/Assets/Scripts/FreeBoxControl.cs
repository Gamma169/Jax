using UnityEngine;
using System.Collections;

public class FreeBoxControl : MonoBehaviour {

	//public GameObject explosion;

	private Rigidbody2D rb2d;
	private BoxCollider2D bc;
	private SpriteRenderer rend;


	// Use this for initialization
	void Start () {

		rb2d = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
		rend = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartDestroy() {
		StartCoroutine("DestroyBox");
	}

	IEnumerator DestroyBox() {
		rb2d.isKinematic = true;
		bc.enabled = false;
		Color startCol = new Color(rend.color.r, rend.color.g, rend.color.b, 0.8f);
		Color endCol = new Color(rend.color.r, rend.color.g, rend.color.b, 0.1f);
		for (int i = 100; i > 0; i--) {
			rend.color = Color.Lerp(endCol, startCol, i/100f);
			yield return null;
		}
		Destroy(this.gameObject);
	}

}
