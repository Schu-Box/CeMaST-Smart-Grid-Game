using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterText : MonoBehaviour {

	private GameManager gameManager;
	private AudioManager audioManager;

	private TextMeshProUGUI t;
	private string fullString;
	private int charactersPerFrame = 2;
	private int framesElapsed;

	void Awake() {
		t = GetComponent<TextMeshProUGUI>();
		gameManager = FindObjectOfType<GameManager>();
		audioManager = FindObjectOfType<AudioManager>();
	}

	public void SetStringAndStart(string newString) {
		fullString = newString;
		framesElapsed = 0;

		StartCoroutine(TypewriteText());
	}

	public IEnumerator TypewriteText() {
		bool fullyDisplayed = false;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		while(!fullyDisplayed) {

			if(framesElapsed % 4 == 0) {
				audioManager.Play("Keystroke");
			}

			framesElapsed++;
			t.text = GetCurrentString(fullString, (framesElapsed * charactersPerFrame));

			if(t.text != fullString) {
				yield return waiter;
			} else {
				fullyDisplayed = true;
			}
		}

		gameManager.ActivateDiscussionButtons();
	}

	private string GetCurrentString(string full, int characterCount) {
		int subReturned = characterCount;
		if(characterCount > full.Length) {
			subReturned = full.Length;
		}
		return full.Substring(0, subReturned);
	}
}
