using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

	public string roomName;

	private List<ApplianceController> applianceList = new List<ApplianceController>();

	private List<LightController> lightList = new List<LightController>();

	private GameManager gameManager;

	public void SetRoom() {
		gameManager = FindObjectOfType<GameManager>();

		for(int i = 0; i < transform.childCount; i++) {
			GameObject obj = transform.GetChild(i).gameObject;
			if(obj.CompareTag("Appliance")) {
				ApplianceController ac = obj.GetComponent<ApplianceController>();
				applianceList.Add(ac);

				ac.SetCurrentAppliance(ac.applianceTree[0]);
			} else if(obj.CompareTag("Light")) {
				LightController lb = obj.GetComponent<LightController>();
				lightList.Add(lb);

				lb.SetLightBulb(gameManager.incandescentLight);
			}
		}
	}

	public void DisableApplianceHighlights() {
		for(int i = 0; i < applianceList.Count; i++) {
			applianceList[i].HideHighlight();
		}
	}
	
	public void EnableApplianceHighlights() {
		for(int i = 0; i < applianceList.Count; i++) {
			ApplianceController ac = GetApplianceList()[i];
			if(GameManager.lightExpiredThisMonth || !ac.CanUpgrade()) {
				ac.HideHighlight();
			} else {
				ac.ShowHighlight();
			}
		}
	}

	public List<ApplianceController> GetApplianceList() {
		return applianceList;
	}

	public List<LightController> GetLightList() {
		return lightList;
	}

	public float GetPercentLightsExpired() {
		int lightsExpired = 0;

		for(int i = 0; i < lightList.Count; i++) {
			if(lightList[i].GetLightExpired() || lightList[i].IsToggledOff()) {
				lightsExpired++;
			}
		}

		return (float)lightsExpired / (float)lightList.Count;
	}

	public bool CanInteractWithSomething() {
		bool upgradeAvailable = false;
		for (int i = 0; i < applianceList.Count; i++) {
			if(!applianceList[i].HasMaxUpgrade()) {
				upgradeAvailable = true;
				break;
			}
		}

		bool lightExpired = false;
		for(int i = 0; i < lightList.Count; i++) {
			if(lightList[i].GetLightExpired()) {
				lightExpired = true;
				break;
			}
		}

		if(roomName == "Living Room" && GameManager.frontDoorInteractable) {
			return true;
		} else {
			if(upgradeAvailable || lightExpired) {
				return true;
			} else {
				return false;
			}
		}
	}	

	public void TurnOnLights() {
		for(int i = 0; i < lightList.Count; i++) {
			if(!lightList[i].GetLightExpired()) {
				lightList[i].TurnOn();
			}
		}
	}
}

