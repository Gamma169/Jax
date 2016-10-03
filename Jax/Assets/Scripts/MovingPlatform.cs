using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	private const int LEFT = -2;
	private const int DOWN = -1;
	private const int STILL = 0;
	private const int UP = 1;
	private const int RIGHT = 2;

	[Tooltip("'Do Once' will override 'loop' if both are checked")]
	public bool doOnce;  // If checked, will override loop and do its action only once
	public bool loop;  //This will determine if the platform loops through its path back and forth, or just does it repediately
	[Tooltip("Time (in 1/60 of a second) before the movement path starts")]
	public int startWaitTime = 0;

	[Tooltip("-2 = Left.  -1 = Dowm.  0 = Still.  1 = Up.  2 = Right.")]
	public int[] path;  // This array hold the path info for each platform
	//-2 Means Left
	//-1 Means Down
	//0 Means Stay Still
	//1 Means Up
	//2 Means Right
	public float[] distTime;	//The distance it moves or the time it takes for it to stay still if it's in still spot of path
	public float[] speeds;  	//The speed of velocity for each segment of path.  Speed is not taken into account if it's staying still.

	/* NOTE:  All three arrays need to be the same length.  If not, the shortest array will be used as the length for the full path  */

	private int pathLength;
	private  int onPathPart;
	private bool doneWaiting;		// This is what starts the script
	private float counter;
	private bool setCounter;
	private bool moveForward;
	private bool done; //This is for the doOnce.  If it is done with its path, it won't loop again.

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();

		rb.isKinematic = true;
		rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

		// Finding the shortest array.  They should be all the same length.  But if not, refer to note above.
		if (path.Length <= distTime.Length && path.Length <= speeds.Length)
			pathLength = path.Length;
		else if (distTime.Length <= speeds.Length)
			pathLength = distTime.Length;
		else
			pathLength = speeds.Length;

		counter = 0;
		setCounter = false;
		onPathPart = 0;
		moveForward = true;

		if (doOnce)
			loop = false;

		doneWaiting = false;

		this.enabled = false;
	}
	
	void FixedUpdate () {
		if (startWaitTime > 0)
			startWaitTime--;
		else
			doneWaiting = true;

		if (doneWaiting)
			FollowPath();
	}

	/*
		distance = speed x time
		Since we know distance and speed, we need to find time.
		time = distance / speed
	*/

	//NOTE that because distance is set on time, we need to keep this in fixedupdate method because that time is fixed and doesn't fluctuate
	public void MoveDistance(int direction, float distance, float speed) {
		if (!setCounter) {
			counter =  (distance * 100 / speed );
			setCounter = true;
		}
		if (direction == LEFT) {
			rb.velocity = new Vector2(-.5f * speed, 0);
		}
		else if (direction == DOWN) {
			rb.velocity = new Vector2(0, -.5f * speed);
		}
		else if (direction == UP) {
			rb.velocity = new Vector2(0, .5f * speed);
		}
		else if (direction == RIGHT) {
			rb.velocity = new Vector2(.5f * speed, 0);
		}

		counter--;
		if (counter <= 0) {
			if (moveForward)
				onPathPart++;
			else
				onPathPart--;
			setCounter = false;
		}
	}

	public void StayStill(float time) {
		if (!setCounter) {
			counter = time;
			setCounter = true;
		}
		rb.velocity = Vector2.zero;
		counter--;

		if (counter <= 0) {
			if (moveForward)
				onPathPart++;
			else
				onPathPart--;
			setCounter = false;
		}
	
	}

	public void FollowPath() {
		if (pathLength > 0 && !done) {
			if (path[onPathPart] == 0)
				StayStill(distTime[onPathPart]);
			else {
				if (moveForward)
					MoveDistance(path[onPathPart], distTime[onPathPart], speeds[onPathPart]);
				else
					MoveDistance(-path[onPathPart], distTime[onPathPart], speeds[onPathPart]);
				}
			if (onPathPart >= pathLength || onPathPart < 0) {
				if (doOnce) {
					done = true;
					rb.velocity = Vector2.zero;
					onPathPart = 0; // We only need this line if we're resetting the movement with a reset switch
				}
				else if (loop) {
					if (moveForward)
						onPathPart--;
					else
						onPathPart++;
					moveForward = !moveForward;
				}
				else {
					onPathPart = 0;
				}
			}
		}
	}

	public void resetDone() {
		done = false;
	}

	public bool isDone() {
		return done;
	}

	public int getOnPathpart() {
		return onPathPart;
	}

}
