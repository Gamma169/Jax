using UnityEngine;
using System.Collections;

public class RepulsorField : MonoBehaviour {

	private const int LEFT = -2;
	private const int DOWN = -1;
	private const int STILL = 0;
	private const int UP = 1;
	private const int RIGHT = 2;

	[Tooltip("-2 = Left.  -1 = Dowm.  0 = Still.  1 = Up.  2 = Right.")]
	public int forceDirection = 0;
	public int maxForceStrength = 80;

	//private Rigidbody2D otherRB;

	private int forceStrength;
	private bool lockout;

	// Use this for initialization
	void Start () {
		lockout = false;
	}
	
	// Update is called once per frame
	void Update () {
		print(forceStrength);
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			
			if (!lockout) {
				forceStrength = 80;
				//IncreaseForceStrength();
				lockout = true;
			}
				
			if (forceDirection == LEFT)
				other.GetComponent<Rigidbody2D>().AddForce(forceStrength * Vector2.left);
			else if (forceDirection == RIGHT)
				other.GetComponent<Rigidbody2D>().AddForce(forceStrength * Vector2.right);
			else if (forceDirection == UP)
				other.GetComponent<Rigidbody2D>().AddForce(forceStrength * Vector2.up);
			else if (forceDirection == DOWN)
				other.GetComponent<Rigidbody2D>().AddForce(forceStrength * Vector2.down);
		}
	}

	//void OnTriggerExit2D(Collider2D other) {
		//if (other.gameObject.tag == "Player")
	//		forceStrength = 0;
	//		lockout = false;
	//}

	public IEnumerator IncreaseForceStrength() {
		
		while (forceStrength != 0 && forceStrength <= maxForceStrength) {
			
			forceStrength++;
			yield return new WaitForSeconds(.05f);
		}

	}
}
