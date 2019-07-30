using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowingHighlight : MonoBehaviour {

	private Image image;

	private void Start() {
		image = GetComponent<Image>();
	}

	public void ActivateGlow() {
		StartCoroutine(Glow());
	}
	
	public IEnumerator Glow() {
		WaitForFixedUpdate waiter = new WaitForFixedUpdate();

		Color startColor = Color.red;

		float currentTimer = 0f;
		float fullCycle = 2f;
		bool goingUp = true;

		while(gameObject.activeSelf) {
			if(goingUp) {
				currentTimer += Time.deltaTime;
			} else {
				currentTimer -= Time.deltaTime;
			}

			if(currentTimer >= fullCycle) {
				goingUp = false;
			} else if(currentTimer <= 0) {
				goingUp = true;
			}

			image.color = Color.Lerp(Color.clear, startColor, currentTimer / fullCycle);

			yield return waiter;
		}
	}
}
