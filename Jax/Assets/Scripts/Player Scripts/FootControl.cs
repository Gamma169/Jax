using UnityEngine;
using System.Collections;

public class FootControl : MonoBehaviour {

    public float footRotatePower = 4f;
    public float bodyRotatePower = 7f;
    public float mouseFootRotatePower = 8f;
    public float mouseBodyRotatePower = 14f;

    //public Collider2D footCol;

    private float alpha;

    private Rigidbody2D footRB;
    private Rigidbody2D bodyRB;
    private SpringControl SC;
    private Transform footTransform;

    //andrew changes
    //public Transform mouseTarget;

    private Vector2 mousePosition; 

    // Use this for initialization
    void Start () {
        Rigidbody2D[] rbar = GetComponentsInChildren<Rigidbody2D>();
        bodyRB = rbar[0];
        footRB = rbar[1];
        SC = GetComponent<SpringControl>();
        footTransform = GetComponentsInChildren<Transform>()[1];

        //mouseTarget = new GameObject().transform;
        //transform.parent = mouseTarget;

    }


    /*=====================
     *   So this is odd putting it here, but I think it belongs here.
     *   This script moves the foot clockwise and counterclockwise based on its location relative to the hip when the foot is protracted
     *   It does this by adding a force continually at the correct angle on the foot
     * 
     *   I believe that when the foot is on the ground or stuck, this force should also be added to the body in the opposite way
     *   I don't know where else to put that force so it's going in here
     * 
     *   NOTE:  I'm not sure how to yet add the force less if the foot is moving or pushing, but I'll think about it
     *          So, currently, if the foot is touching anything, it will add the force to the body as well
     * 
     */

    void FixedUpdate () {

        float yDiff = footTransform.position.y - this.transform.position.y;
        //print(yDiff);
        if (yDiff == 0)
            yDiff = 0.0001f;

        alpha = Mathf.Atan ((footTransform.position.x - this.transform.position.x) / (yDiff ) ) ;

        /*  This was to see if I can change the rotation of the foot in relation to the body.  Apparantly I cannot, so I'll have to think of a new way to change the orientation when I'm clamped.
            Quaternion q = new Quaternion();
            q.eulerAngles = new Vector3(0,0,-alpha * 180 / Mathf.PI);
            footTransform.rotation = q;
            */

        if (GlobalVariables.pControl && !SC.retracted && !bodyRB.isKinematic) {
            
			// To control with the mouse
			if (GlobalVariables.mouseControl) {
				/*
				Vector2 currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				if (mousePosition.x - currentMousePosition.x > 0) {
					RotateFootWithMouse(!GlobalVariables.invertLegControl);
				}
				if (mousePosition.x - currentMousePosition.x < 0) {
					RotateFootWithMouse(GlobalVariables.invertLegControl);
				}
				mousePosition = currentMousePosition;
				*/
                ControlFootWithMouse();
			}
			else {
				if (Input.GetKey("a")) {
					RotateFoot (!GlobalVariables.invertLegControl);
				}
				if (Input.GetKey("d")) {
					RotateFoot (GlobalVariables.invertLegControl);
				}
			}
            
        } 
        else {
            AutoControl();
        }
    }



    public void AutoControl() {

    }

    /*================
     *   In this rotate foot function I add the force to the foot when the key is pressed
     *   Also, I added the force to the hip if the foot is touching another object
     *
     *   Also, also, I know there's a lot of unnecessary code here, but I think this is the clearest way to lay it out according to the math
     */
    public void RotateFoot(bool clockwise) {
        float footRotatePowerAdded = footRotatePower;
        float bodyRotatePowerAdded = bodyRotatePower;

        if (clockwise) {
            footRotatePowerAdded = -footRotatePowerAdded;
            bodyRotatePowerAdded = -bodyRotatePowerAdded;
        }

        if (!footAbove ())
            footRB.AddForce (new Vector2 (-footRotatePowerAdded * Mathf.Cos (alpha), footRotatePowerAdded * Mathf.Sin (alpha)));
        else
            footRB.AddForce (new Vector2 (footRotatePowerAdded * Mathf.Cos (alpha), -footRotatePowerAdded * Mathf.Sin (alpha)));

        //  This is where I check if the foot is touching another object.  
        //  I need to figure out a better way to do this.
        //  Ideally, we only want it to touch the boundries layer
		// TODO:  Instead of negating it when the mouse control is implemented, we should probably make the bodyRotatePowerAdded less or something.  Need to look into it to make it work fine when the MouseControl method is done
		if (footRB.IsTouchingLayers(Physics2D.AllLayers) && !GlobalVariables.mouseControl) {
            if (footAbove ())
                bodyRB.AddForce (new Vector2 (-bodyRotatePowerAdded * Mathf.Cos (alpha), bodyRotatePowerAdded * Mathf.Sin (alpha)));
            else
                bodyRB.AddForce (new Vector2 (bodyRotatePowerAdded * Mathf.Cos (alpha), -bodyRotatePowerAdded * Mathf.Sin (alpha)));
        }
    }

	// This function should make the foot point in the direction of the mouse.  It should be implemented with the RotateFoot method to avoid issues.  I've already updated the RotateFoot method to accomidate it so it doesn't rotate the body when on the ground.  
	// See the TODO in the method for more info
	public void ControlFootWithMouse() {
        /*
		Vector3 v3Pos = Camera.main.WorldToScreenPoint (mouseTarget.position);
        v3Pos = Input.mousePosition - v3Pos;
        float angle = Mathf.Atan2 (v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;

        footTransform.position = mouseTarget.position;
        footTransform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

        Debug.Log(v3Pos);
        */
	}

    public bool footAbove() {
        if (footTransform.position.y - this.transform.position.y > 0)
            return true;
        else
            return false;
    }
    public bool footToTheRight() {
        if (footTransform.position.x - this.transform.position.x > 0)
            return true;
        else
            return false;
    }

}
