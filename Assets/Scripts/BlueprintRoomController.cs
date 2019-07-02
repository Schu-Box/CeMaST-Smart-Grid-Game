using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintRoomController : MonoBehaviour {

	private GameManager gameManager;
	private Button button;
	private RoomController room;
	private Image highlight;

	public void SetRoom(RoomController r) {
		room = r;

		gameManager = FindObjectOfType<GameManager>();
		button = GetComponent<Button>();
		highlight = transform.GetChild(0).GetComponent<Image>();

		button.onClick.AddListener(() => gameManager.GoToRoom(room));
	}

	public void SetHighlight() {
		if(GameManager.lightExpiredThisMonth) {
			bool lightExpired = false;
			for(int i = 0; i < room.GetLightList().Count; i++) {
				if(room.GetLightList()[i].GetLightExpired()) {
					lightExpired = true;
					break;
				}
			}

			if(lightExpired) {
				highlight.enabled = true;
				highlight.color = Color.red;
			} else {
				highlight.enabled = false;
			}
		} else {
			bool upgradeAvailable = false;
			for(int i = 0; i < room.GetApplianceList().Count; i++) {
				if(room.GetApplianceList()[i].CanUpgrade()) {
					upgradeAvailable = true;
					break;
				}
			}

			if(upgradeAvailable || (room.roomName == "Living Room" && GameManager.frontDoorInteractable)) {
				highlight.enabled = true;
				highlight.color = Color.white;
			} else {
				highlight.enabled = false;
			}
		}
	}
}
