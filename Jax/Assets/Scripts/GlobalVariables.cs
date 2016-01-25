using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {

	public static bool pControl;


	// Use this for initialization
	void Start () {
		pControl = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("x"))
			pControl = !pControl;
			
	}
}
