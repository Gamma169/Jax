using UnityEngine;
using System.Collections;

public class SpringControl : MonoBehaviour {

	//  Use this to make the speeds a litle clearer on some of the "change" functions
	public const int INSTANTLY = 0;

	//public bool ableHop;
	public bool ableProtract;
	public bool ableExtend;

	public float regSpringLength;
	public float extSpringLength;
	public float regDampRatio = 0.3f;
	public float lockoutDampRatio = 0.85f;
	public float regSpringFreq = 2.9f;
	public float retractSpringFreq = 17f;
	//These are required so that the foot doesn't spin around like crazy if it's in motion and retracted.
	public float retractDrag = 5f;
	public float regDrag = 0;

	public Transform footTransform;
	public GameObject springSprite;

	public bool retracted;
	private bool justProtracted;
	private bool justRetracted;
	private int pLockoutTimer;
	// To implement super-jump, bring pLockoutStartTime down to about 10 or less
	private int pLockoutStartTime = 28;
	private int rLockoutTimer;
	// To implement float bring this down to 5 or less as well
	private int rLockoutStartTime = 20;

	public bool clamped;

	private float footDist;
	private float clampedCheckDist;

	private int fixedJointRegPos = 1;
	private int fixedJointUpPos = -1;

	private Rigidbody2D footrb;
	private Rigidbody2D rb;
	private SpringJoint2D spring;
	private FixedJoint2D fj;

	private Vector3 startSpringSpriteSize;
	private float startDistance;


	// Use this for initialization
	void Start () {
		spring = GetComponent<SpringJoint2D>();
		Rigidbody2D[] arr = GetComponentsInChildren<Rigidbody2D>();
		rb = arr[0];
		footrb = arr[1];
		fj = GetComponent<FixedJoint2D>();
		//startSpringLength = spring.distance;
		startSpringSpriteSize = springSprite.transform.localScale;
		startDistance = extSpringLength - 1;  //Vector3.Distance(footTransform.position, transform.position);

	}

	void Update() {

		if (Input.GetKeyDown("p"))
			GameObject.Destroy(gameObject);

		// This seems overly-complicated and redundant, but I think all these clauses are necessary.  Maybe I should come back to them to look it over, but so far it works.
		// Added the IsKinematic requirement because I activate IsKinematic when the body is destroyed and don't want to be able to extend
		if (Input.GetKeyDown(KeyCode.E) && !justProtracted && !justRetracted && ableProtract && !rb.isKinematic) {
			retracted = !retracted;
			if (retracted && !justRetracted) {
				justRetracted = true;
				rLockoutTimer = rLockoutStartTime;
			}
			if (!retracted && !justProtracted) {
				justProtracted = true;
				pLockoutTimer = pLockoutStartTime;
			}
		}

		if (Input.GetKey("w") && retracted && fj.enabled && !rb.isKinematic)
			ChangeFixedJointLength(fixedJointUpPos, INSTANTLY);
		else
			ChangeFixedJointLength(fixedJointRegPos, INSTANTLY);

		springSprite.SetActive(!retracted && footDist > .5f);	

		//This all deals with the spring's sprite location, rotation and scale
		if (springSprite.activeSelf) {
			float springXPos = (this.transform.position.x + footTransform.position.x) / 2;
			float springYPos = (this.transform.position.y + footTransform.position.y) / 2;
			float springZPos = (this.transform.position.z + footTransform.position.z) / 2;
			springSprite.transform.position = new Vector3 (springXPos, springYPos, springZPos);

			float scale = footDist / startDistance;
			springSprite.transform.localScale = new Vector3 (startSpringSpriteSize.x * (1/scale), startSpringSpriteSize.y * scale, startSpringSpriteSize.z);

			float angle = Mathf.Atan2 (this.transform.position.x - footTransform.position.x, this.transform.position.y - footTransform.position.y) * 180 / Mathf.PI;
			springSprite.transform.rotation = Quaternion.Euler(new Vector3 (0, 0, -angle));
		}
	}
		
	void FixedUpdate () {

		//I'm checking to see if I'm clamped by comparing the distance between the foot the previous frame and not
		//There are also a bunch of other requirements for being clamped, and so if all of them are true, then it's clamped
		//Instead of assigning clamped straight, I did it like this because of the movement requirement.  I eventually want to be clamped on moving platforms, and don't want it to register unclamped because of the movement
		footDist = Vector3.Distance(footTransform.position, transform.position);
		if (retracted && 																				//Must be retracted to be clamped
			clampedCheckDist - footDist < .01f && 														//The foot needs to not be moving in towards the body anymore
			footDist - spring.distance > .25f && 														//The distance between the foot and body needs to be greater than what the spring wants to set it at (meaning there's something between the foot and the body stopping it from getting closer)
				((Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2f) + Mathf.Pow(rb.velocity.y, 2f)) < .05f &&			//The body needs not to be moving too fast
				Mathf.Sqrt(Mathf.Pow(footrb.velocity.x, 2f) + Mathf.Pow(footrb.velocity.y, 2f)) <.05f) ||	//The foot needs not to be moving (too fast)
				(rb.velocity.x - footrb.velocity.x < .1f && rb.velocity.y - footrb.velocity.y < .1f && 		// OR both the foot and body are moving at the same velocity.
					footDist > .75f))																		// AND the foot distance is far from the body (this is because that at small distances, the foot is close and activates clamp briefly)
			)
			clamped = true;
		if (!retracted || footDist - spring.distance < .25f || clampedCheckDist - footDist > .01f)
			clamped = false;

		clampedCheckDist = footDist;

		// Since I'm incrementing something and it's based on time, I NEED to have this in FixedUpdate()
		// These lines act a lockout function so that I don't super-jump or float. Of course, if I set the start timers to about 0 or so, then they act as if they weren't there
		if (justProtracted) {
			pLockoutTimer--;
			if (pLockoutTimer <= 0)
				justProtracted = false;
		}
		if (justRetracted) {
			rLockoutTimer--;
			if (rLockoutTimer <= 0)
				justRetracted = false;
		}

		//print(justProtracted);

		if (GlobalVariables.pControl) {

			// I only extend on the spacebar if I have not just protracted or just retracted otherwise I keep it in the contracted position.
			if (Input.GetKey("space") && !justProtracted && !justRetracted && !rb.isKinematic) {
				ExtendSpring();
			}
			else {
				ContractSpring();
			}
		}
		else {
			AutoControl ();
		}
	}


	private void AutoControl() {
	
	
	}

	//Self-explanitory function.  Contract to 0 or regSpringLength if retracted or protracted and add drag to avoid crazy spinning.  Activate Fixed joint if the foot reaches all the way back to body.
	public void ContractSpring() {
		if (retracted) {
			ChangeSpringLength(0, INSTANTLY);
			if (clamped) {
				footrb.drag = regDrag;
				//We need to change frequency slowly if clamped because jumping to too high a frequency quickly can add too much velocity to the foot or body causing it to "phase" through obstacles
				ChangeSpringFrequency(retractSpringFreq, 30f);
			}
			else {
				if (footDist > .7) {
					footrb.drag = retractDrag;
					ChangeSpringFrequency(retractSpringFreq, 1f);	//This line may give issues in the future because of increasing the frequency.  Might not be necessary but is good to have.
				}
				else {
					ChangeSpringFrequency(retractSpringFreq, 100f);
					spring.enabled = false;
					fj.enabled = true;
					footrb.drag = regDrag;
				}
			}
		}
		else {
			spring.enabled = true;
			fj.enabled = false;
			ChangeSpringFrequency(regSpringFreq, INSTANTLY);
			ChangeSpringLength(regSpringLength, 100);
			footrb.drag = regDrag;
		}

	}

	// See notes for Contract Spring Function
	public void ExtendSpring() {
		if (retracted) {
			ChangeSpringLength(.7f, INSTANTLY);
			if (clamped) {
				footrb.drag = regDrag;
				ChangeSpringFrequency(retractSpringFreq, 30f);
			}
			else {
				if (footDist > .85f) {
					footrb.drag = retractDrag;
					ChangeSpringFrequency(retractSpringFreq, 1f);   //This line may give issues in the future because of increasing the frequency.  Might not be necessary.

				}
				else {
					ChangeSpringFrequency(retractSpringFreq, 100f);
					spring.enabled = true;
					fj.enabled = false;
					footrb.drag = regDrag;
				}
			}
		}
		else if (ableExtend) {
			ChangeSpringFrequency(regSpringFreq, INSTANTLY);
			spring.enabled = true;
			fj.enabled = false;
			ChangeSpringLength(extSpringLength, 50);
			footrb.drag = regDrag;
		}
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
			if (spring.distance < toLength) {
				spring.distance += (0.01f * atSpeed);
				//This line is necessary here in this place to make sure we don't fluctuate.  I could possiby give a range to where it should stop, but that might not end up in the right spot
				if (spring.distance > toLength)
					spring.distance = toLength;
			}
			else if (spring.distance > toLength) {
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


	/*** This function isn't as necessary as the others, but instead of changing the frequency instantly, I might want to change it gradually so things don't shoot out of control; note that when atSpeed is set to 0, the change is instant    ***/
	public void ChangeSpringFrequency(float toFreq, float atSpeed) {
		if (atSpeed == 0)
			spring.frequency = toFreq;
		else {
			if (toFreq > spring.frequency) {
				spring.frequency += (.01f * atSpeed);
				if (spring.frequency > toFreq)
					spring.frequency = toFreq;
			}
			else if (toFreq < spring.frequency) {
				spring.frequency -= (.01f * atSpeed);
				if (spring.frequency < toFreq)
					spring.frequency = toFreq;
			}
		
		}
	
	
	}

	/*
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground")
			onGround = true;
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.tag == "Ground")
			onGround = false;
	}
	*/	

}
