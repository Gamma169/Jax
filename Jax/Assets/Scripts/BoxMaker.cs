using UnityEngine;
using System.Collections;

public class BoxMaker : MonoBehaviour {

	public GameObject boxPrefab;

	public bool oneBox = true;

	public Vector2 InstatiatePostion;

	public Color activeCol;
	public Color deactCol;

	public bool active;
	private bool lockout;
	private Vector3 InstPos;
	private GameObject actualBox;

	private SpriteRenderer SR;

	// Use this for initialization
	void Start () {
		InstPos = new Vector3(InstatiatePostion.x, InstatiatePostion.y, transform.position.z);
		SR = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (active)
			SR.color = activeCol;
		else {
			SR.color = deactCol;
			if (oneBox)
				DestroyBox();
		}
		lockout = false;

	}

	public void CreateBox() {
		if (actualBox && oneBox)
			GameObject.Destroy(actualBox);
		actualBox = (GameObject)Instantiate(boxPrefab, InstPos, Quaternion.identity);
		
	}

	public void DestroyBox() {
		if (actualBox)
			GameObject.Destroy(actualBox);
	}




	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) {
				if (!lockout) {
					if (oneBox)
						active = !active;
					else {
						active = true;
						StartCoroutine("Unlock");
					}
					lockout = true;
					if (active)
						CreateBox();
				}
			}
		}
	}

	private IEnumerator Unlock() {
		yield return new WaitForSeconds(0.25f);
		active = false;
		lockout = false;
	}

}
