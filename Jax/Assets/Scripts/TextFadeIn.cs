using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFadeIn : MonoBehaviour {

	public Color textColor;
	public Text[] texts;
	public int[] startTimeWaits;
	public int[] fadeInSpeeds;

	private int[] timeToFades;
	private float[] textAlphas;
	private bool playerInBounds;


	// Use this for initialization
	void Start () {
		timeToFades = new int[texts.Length];
		textAlphas = new float[texts.Length];
		for (int i=0; i<startTimeWaits.Length; i++)
			timeToFades[i] = startTimeWaits[i];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		for (int i = 0; i < texts.Length; i++) {
			if (playerInBounds) {
				if (timeToFades[i] > 0)
					timeToFades[i]--;
				if (timeToFades[i] <= 0)
					FadeIn(i);
			}
			else {
				timeToFades[i] = startTimeWaits[i];
				FadeOut(i);
			}
		}
	}


	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
			playerInBounds = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
			playerInBounds = false;
	}


	void FadeIn(int i) {
		if (fadeInSpeeds[i] == 0) {
			textAlphas[i] = 1f;
		}
		else {
			if (textAlphas[i] < 1)
				textAlphas[i] += 0.01f * fadeInSpeeds[i];
			if (textAlphas[i] > 1)
				textAlphas[i] = 1;
		}
		Color c = new Color(textColor.r, textColor.g, textColor.b, textAlphas[i]);
		texts[i].color = c;
	}

	void FadeOut(int i) {
		if (fadeInSpeeds[i] == 0) {
			textAlphas[i] = 0f;
		}
		else {
			if (textAlphas[i] > 0)
				textAlphas[i] -= 0.01f * fadeInSpeeds[i];
			if (textAlphas[i] < 0)
				textAlphas[i] = 0;
		}
		Color c = new Color(textColor.r, textColor.g, textColor.b, textAlphas[i]);
		texts[i].color = c;
	}

}
