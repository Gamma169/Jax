using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoIndicator : MonoBehaviour {

	public Image im;

	// Use this for initialization
	void Start () {
		im = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GlobalVariables.pControl)
			im.color = Color.red;
		else
			im.color = Color.green;
	}
}
