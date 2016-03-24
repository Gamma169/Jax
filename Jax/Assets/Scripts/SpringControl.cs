using UnityEngine;
using System.Collections;

public class SpringControl : MonoBehaviour {

	//public const float EX_SPING_LOCKOUT_TIME = .1f;

	public float regSpringLength;
	public float extSpringLength;
	public float regDampRatio = 0.3f;
	public float lockoutDampRatio = 0.85f;
	public float regSpringFreq = 2.9f;
	public float retractSpringFreq = 14f;

	public SpringJoint2D spring;
	public Transform footTransform;
	public GameObject springSprite;

	public bool retracted;

	private Rigidbody2D rb;
	private FixedJoint2D fj;
	//private SliderJoint2D sj;
	//private float startSpringLength;
	private Vector3 startSpringSpriteSize;
	private float startDistance;
	private bool exSpringLockout;

	// Use this for initialization
	void Start () {
		spring = GetComponent<SpringJoint2D>();
		rb = GetComponent<Rigidbody2D>();
		fj = GetComponent<FixedJoint2D>();
		//startSpringLength = spring.distance;
		startSpringSpriteSize = springSprite.transform.localScale;
		startDistance = Vector3.Distance(footTransform.position, transform.position);

	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.E)) 
			retracted = !retracted;

		springSprite.SetActive(!retracted);	

		if (springSprite.activeSelf) {
			float springXPos = (this.transform.position.x + footTransform.position.x) / 2;
			float springYPos = (this.transform.position.y + footTransform.position.y) / 2;
			float springZPos = (this.transform.position.z + footTransform.position.z) / 2;
			springSprite.transform.position = new Vector3 (springXPos, springYPos, springZPos);

			float dist = Vector3.Distance(footTransform.position, transform.position);
			float scale = dist / startDistance;
			springSprite.transform.localScale = new Vector3 (startSpringSpriteSize.x * (1/scale), startSpringSpriteSize.y * scale, startSpringSpriteSize.z);

			float angle = Mathf.Atan2 (this.transform.position.x - footTransform.position.x, this.transform.position.y - footTransform.position.y) * 180 / Mathf.PI;
			springSprite.transform.rotation = Quaternion.Euler(new Vector3 (0, 0, -angle));
		}
	}
		
	void FixedUpdate () {
		
		if (retracted) {
			if (spring.distance > 0.01f)
				ChangeSpringLength(0, 25);	
			else {
				
				spring.frequency = retractSpringFreq;
				spring.enabled = false;
				fj.enabled = true;
			}

		}
		// This section actually does a little magic based on how I set up the "ExtendSpring" and "ContractSpring" functions
		// You don't have to change the length when extending because it's already changed in checking when space is pressed
		else {
			if (spring.distance < regSpringLength)
				exSpringLockout = true;
			else
				exSpringLockout = false;
			spring.frequency = regSpringFreq;
			spring.enabled = true;
			fj.enabled = false;
		}
			
		if (GlobalVariables.pControl) {

			if (Input.GetKey("space") && !exSpringLockout) 
				ExtendSpring();
			else 
				ContractSpring();
		}
		else {
			AutoControl ();
		}
	}


	private void AutoControl() {
	
	
	}

	public void ExtendSpring() {
		if (retracted) {
			spring.enabled = true;
			fj.enabled = false;
			spring.frequency = retractSpringFreq;
			ChangeSpringLength(.7f, 0);
		}
		else
			ChangeSpringLength(extSpringLength, 0);
	}

	public void ContractSpring() {
		if (retracted) {
			spring.frequency = regSpringFreq;

			if (spring.enabled) {
				ChangeSpringLength(0, 50);
				if (spring.distance == 0) {
					spring.enabled = false;
					fj.enabled = true;
				}
			}
			else
				ChangeFixedJointLength(1, 100);
		}
		else
			ChangeSpringLength(regSpringLength, 50);
	}

	//Only need this to "wake up" the rigidbody2D, but I changed it to "Never Sleep" so it should be good.
	private void Jiggle() {
		rb.AddForce (transform.up * 0.001f);
	}

	/*** This method changes the spring length at a given speed (to be eyeballed).  If atSpeed is set to 0, it changes instantly ***/
	public void ChangeSpringLength(float toLength, float atSpeed ) {
		if (atSpeed == 0)
			spring.distance = toLength;
		else {
			if (toLength >= spring.distance) {
				// This lockout happens so that you don't shoot up much higher than possible when first entering the extended spring
				if (exSpringLockout) {
					spring.dampingRatio = lockoutDampRatio;
				}
				else {
					spring.dampingRatio = regDampRatio;
				}

				spring.distance += (0.01f * atSpeed);
				//This line is necessary here in this place to make sure we don't fluctuate.  I could possiby give a range to where it should stop, but that might not end up in the right spot
				if (spring.distance > toLength)
					spring.distance = toLength;
			}
			else {
				spring.distance -= (0.01f * atSpeed);
				//This is same as the comment above
				if (spring.distance < toLength)
					spring.distance = toLength;
			}
		}	
	}

	/*** This function changes the Fixed Joint Length in a downwards Y direction relative to the body;  Note that when atSpeed is set to 0, the change is instant    ***/
	public void ChangeFixedJointLength(float toLength, float atSpeed) {
		if (atSpeed == 0) 
			fj.connectedAnchor = new Vector2(fj.connectedAnchor.x, toLength);	
		else {
			if (toLength >= fj.connectedAnchor.y) {
				fj.connectedAnchor = new Vector2(fj.connectedAnchor.x, fj.connectedAnchor.y + (0.01f * atSpeed));
				//These checks are necessary for the above reasons
				if (fj.connectedAnchor.y > toLength)
					fj.connectedAnchor = new Vector2 (fj.connectedAnchor.x, toLength);
			}
			else {
				fj.connectedAnchor = new Vector2(fj.connectedAnchor.x, fj.connectedAnchor.y - (0.01f * atSpeed));
				//These checks are necessary for the above reasons
				if (fj.connectedAnchor.y < toLength)
					fj.connectedAnchor = new Vector2 (fj.connectedAnchor.x, toLength);
			}
		}
	}
		

}
