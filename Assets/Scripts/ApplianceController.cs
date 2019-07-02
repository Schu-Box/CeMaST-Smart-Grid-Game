using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ApplianceController : MonoBehaviour {

	public ApplianceType type;

	public List<Appliance> applianceTree;

	private Appliance currentAppliance;

	private GameManager gameManager;

	private SpriteRenderer spriteRenderer;
	private SpriteRenderer highlight;

	private Coroutine highlightCoroutine;

	void Awake() {
		gameManager = FindObjectOfType<GameManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		highlight = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
	}

	private void OnMouseUpAsButton() {
		if(!GameManager.IsPointerOverGameObject()) {
			gameManager.ClickAppliance(this);
		}
	}

	public Appliance GetCurrentAppliance() {
		if(currentAppliance == null) {
			Debug.Log("Appliance is null");
		}
		return currentAppliance;
	}

	public void SetCurrentAppliance(Appliance newApp) {
		spriteRenderer = GetComponent<SpriteRenderer>();

		currentAppliance = newApp;
		spriteRenderer.sprite = newApp.sprite;
	}

	public Appliance GetNextAvailableAppliance() {
		for(int a = 0; a < applianceTree.Count; a++) {
			if(applianceTree[a] == currentAppliance) {
				if(a < applianceTree.Count - 1) {
					return applianceTree[a + 1];
				} else {
					Debug.Log("Appliance fully upgraded.");
					//It's fully upgraded
				}
			}
		}

		return null;
	}

	public bool HasMaxUpgrade() {
		if(currentAppliance == applianceTree[applianceTree.Count - 1]) {
			return true;
		} else {
			return false;
		}
	}

	public bool CanUpgrade() {
		if(currentAppliance == applianceTree[applianceTree.Count - 1]) {
			return false;
		} else {
			if(GetNextAvailableAppliance().cost > GameManager.player.GetWealth()) {
				return false;
			} else {
				return true;
			}
		}
	}

	public void ShowHighlight() {
		highlight.enabled = true;

		highlightCoroutine = StartCoroutine(PulseHighlight());
	}

	public void HideHighlight() {
		if(highlightCoroutine != null) {
			StopCoroutine(highlightCoroutine);
		}

		highlight.enabled = false;
	}

	public IEnumerator PulseHighlight() {
		highlight.color = Color.clear;

		/*
		float randoDelay = Random.value / 4;
		yield return new WaitForSeconds(randoDelay);
		*/

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float step = 0f;
		float offset = 0.5f + (Random.value * 0.1f);
		bool increasing = true;
		while(true) {
			if(increasing) {
				step += (Time.deltaTime * offset);
			} else {
				step -= (Time.deltaTime * offset);
			}

			highlight.color = Color.Lerp(Color.clear, Color.white, step);

			if(step >= 1) {
				increasing = false;
			} else if(step <= 0.5) {
				increasing = true;
			}

			yield return waiter;
		}
	}

	public IEnumerator ReplaceAppliance(Appliance newApp) {
		Vector3 startScale = transform.localScale;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float timer = 0f;
		float duration = 0.6f;
		while(timer < duration) {
			timer += Time.deltaTime;

			transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer/duration);

			yield return waiter;
		}

		SetCurrentAppliance(newApp);

		Vector3 overExpand = startScale * 1.1f;

		timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			transform.localScale = Vector3.Lerp(Vector3.zero, overExpand, timer/duration);

			yield return waiter;
		}

		timer = 0f;
		duration = 0.1f;
		while(timer < duration) {
			timer += Time.deltaTime;
			
			transform.localScale = Vector3.Lerp(overExpand, startScale, timer/duration);

			yield return waiter;
		}

		gameManager.AdvanceMonth();
	}
}
