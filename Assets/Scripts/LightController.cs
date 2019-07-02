using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LightController : MonoBehaviour {

	//public Vector3 lightChangeMenuPosition;

	public int hoursUsedPerMonth;
	private LightBulb currentLightBulb;
	private int hoursUntilExpiration;
	private bool expired;

	private GameManager gameManager;

	private SpriteRenderer sr;
	private SpriteRenderer highlight;

	private bool toggledOn = true;

	void Start() {
		gameManager = FindObjectOfType<GameManager>();
	}

	public void OnMouseUpAsButton() {
		if(!GameManager.IsPointerOverGameObject()) {
			gameManager.ClickLight(this);
		}
	}

	public void SetLightBulb(LightBulb light) {
		//Really should be called in Awake but I'm struggling rn
		sr = GetComponent<SpriteRenderer>();
		highlight = transform.GetChild(0).GetComponent<SpriteRenderer>();
		highlight.enabled = false;

		currentLightBulb = light;

		sr.sprite = light.litSprite;
		highlight.sprite = light.glowSprite;

		hoursUntilExpiration = light.hoursBeforeBurnout;
		expired = false;

		TurnOn();
	}

	public LightBulb GetLightBulb() {
		return currentLightBulb;
	}

	public void DecreaseMonthlyHours() {
		hoursUntilExpiration -= hoursUsedPerMonth;
	}

	public int GetHoursRemaining() {
		return hoursUntilExpiration;
	}

	public void SetLightExpired(bool exp) {
		expired = exp;

		if(expired) {
			sr.sprite = currentLightBulb.unlitSprite;

			highlight.enabled = true;
			//Pulse light
		} else {
			sr.sprite = currentLightBulb.litSprite;

			highlight.enabled = false;

			TurnOn();
		}
	}

	public bool GetLightExpired() {
		return expired;
	}

	public void ToggleOn() {
		toggledOn = !toggledOn;
		if(toggledOn != true) {
			sr.sprite = currentLightBulb.unlitSprite;
		} else {
			sr.sprite = currentLightBulb.litSprite;
		}
	}

	public bool IsToggledOff() {
		return !toggledOn;
	}

	public void TurnOn() {
		toggledOn = true;
		sr.sprite = currentLightBulb.litSprite;
	}
}
