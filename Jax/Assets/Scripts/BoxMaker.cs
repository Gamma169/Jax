using UnityEngine;
using System.Collections;

public class BoxMaker : MonoBehaviour {

	public GameObject boxPrefab;

	public int numBoxes = 1;

	public bool active;

	public bool randForceCreation;
	public float forceStrength = 1;

	private bool lockout;
	private Vector3 InstPos;
	private Queue boxes;

	// Use this for initialization
	void Start () {
		boxes = new Queue();
		//boxes.Enqueue( (GameObject)Instantiate(boxPrefab, this.transform.position, Quaternion.identity));
	}
	
	// Update is called once per frame
	void Update () {
	

		if (!active) {
			if (numBoxes == 1 && boxes.Count >=  1)
				DestroyBox();
		}
		lockout = false;

	}

	public void CreateBox() {
		if (boxes.Count >= numBoxes)
			GameObject.Destroy((GameObject)boxes.Dequeue());
		GameObject go = (GameObject)Instantiate(boxPrefab, this.transform.position, Quaternion.identity);
		if (randForceCreation)
			AddRandForce(go);
		boxes.Enqueue(go);
		
	}

	public void DestroyBox() {
		if (boxes.Count >= 1)
			GameObject.Destroy((GameObject)boxes.Dequeue());
	}
		
	private IEnumerator Unlock() {
		yield return new WaitForSeconds(0.25f);
		lockout = false;
	}

	public bool isLocked() {
		return lockout;
	}

	public void AddRandForce(GameObject ob) {
		Rigidbody2D rb = ob.GetComponent<Rigidbody2D>();
		rb.AddForce(new Vector2(Random.Range(-forceStrength, forceStrength), Random.Range(0,forceStrength)), ForceMode2D.Impulse);
	}

}
