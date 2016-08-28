using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFadeIn : MonoBehaviour {

	public int startTimeWait = 60;
	public int fadeInSpeed = 5;


	private Text text;
	private int timeToFade;
	private float textAlpha;
	private bool playerInBounds;


	// Use this for initialization
	void Start () {
		timeToFade = startTimeWait;
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (playerInBounds) {
			if (timeToFade > 0)
				timeToFade--;
			if (timeToFade <= 0)
				FadeIn();
		}
		else {
			timeToFade = startTimeWait;
			FadeOut();
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


	void FadeIn() {
		if (fadeInSpeed == 0) {
			textAlpha = 1f;
		}
		else {
			if (textAlpha < 1)
				textAlpha += 0.01f * fadeInSpeed;
			if (textAlpha > 1)
				textAlpha = 1;
		}
		Color c = new Color(1f, 1f, 1f, textAlpha);
		text.color = c;
	}

	void FadeOut() {
		print("test");
		if (fadeInSpeed == 0) {
			textAlpha = 0f;
		}
		else {
			if (textAlpha > 0)
				textAlpha -= 0.01f * fadeInSpeed;
			if (textAlpha < 0)
				textAlpha = 0;
		}
		Color c = new Color(1f, 1f, 1f, textAlpha);
		text.color = c;
	}

}
