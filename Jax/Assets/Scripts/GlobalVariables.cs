using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GlobalVariables : MonoBehaviour {

	public static bool pControl;


	// Use this for initialization
	void Start () {
		pControl = true;

		/*
		print (Mathf.Cos (0));
		print (Mathf.Cos (-Mathf.PI));
		print (Mathf.Cos (-Mathf.PI * 3 / 4));
		print (Mathf.Cos (-Mathf.PI / 2));
		print (Mathf.Cos (-Mathf.PI / 4));
	
		print ("Positive");
		print (Mathf.Cos (0));
		print (Mathf.Cos (Mathf.PI / 4));
		print (Mathf.Cos (Mathf.PI / 2));
		print (Mathf.Cos (Mathf.PI * 3 / 4));
		print (Mathf.Cos (Mathf.PI));
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("x"))
			pControl = !pControl;
			

		if (Input.GetKeyDown ("r"))
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
		

		if (Input.GetKeyDown ("1"))
			SceneManager.LoadScene (0);

		if (Input.GetKeyDown ("2"))
			SceneManager.LoadScene (1);

	}
}
