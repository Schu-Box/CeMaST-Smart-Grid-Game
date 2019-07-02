using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHome : MonoBehaviour {

	private GameManager gameManager;
	
	private List<RoomController> rooms = new List<RoomController>();

	private AudioManager audioManager;

	public void SetPlayerHome() {
		gameManager = FindObjectOfType<GameManager>();
		audioManager = FindObjectOfType<AudioManager>();

		SetHomeRooms();
		DeactivateAllRooms();
	}

	private void SetHomeRooms() {
		for(int i = 0; i < transform.childCount; i++) {
			RoomController room = transform.GetChild(i).GetComponent<RoomController>();
			room.SetRoom();
			rooms.Add(room);
		}
	}

	public List<RoomController> GetRoomList() {
		return rooms;
	}

	public RoomController GetRoom(string name) {
		for(int i = 0; i < rooms.Count; i++) {
			if(name == rooms[i].name) {
				return rooms[i];
			}
		}

		Debug.Log("Ain't no room like that");
		return null;
	}

	public void EnterRoom(RoomController room) {
		DeactivateAllRooms();

		room.gameObject.SetActive(true);

		room.EnableApplianceHighlights();
	}

	public void DeactivateAllRooms() {
		for(int i = 0; i < rooms.Count; i++) {
			rooms[i].gameObject.SetActive(false);
		}
	}

	public void UpgradeAppliance(ApplianceController ac) { //Should only be called if the appliance can actually be upgraded
		audioManager.Play("Appliance Upgrade");

		Appliance upgrade = ac.GetNextAvailableAppliance();
		ac.StartCoroutine(ac.ReplaceAppliance(upgrade));

		GameManager.player.SpendMoney(upgrade.cost); //Subtract the cost of upgrade

		gameManager.applianceMenu.SetActive(false);
	}

	public void ChangeLightBulb(LightController light, LightBulb newBulb) {
		audioManager.Play("Light Replace");

		GameManager.player.SpendMoney(newBulb.cost);

		light.SetLightBulb(newBulb);

		gameManager.AdjustDarkness();
		gameManager.CloseLight();
	}

	public int GetMonthlyEnergyUse() {
		int total = 0;

		for(int i = 0; i < rooms.Count; i++) {
			for(int j = 0; j < rooms[i].GetApplianceList().Count; j++) {
				total += rooms[i].GetApplianceList()[j].GetCurrentAppliance().kwhPerMonth;
			}
			for(int l = 0; l < rooms[i].GetLightList().Count; l++) {
				LightController light = rooms[i].GetLightList()[l];
				total += (light.GetLightBulb().kwh * light.hoursUsedPerMonth / 1000);
			}
		}

		return total;
	}

	public int GetEnergyBill() {
		int bill = (int)(GetMonthlyEnergyUse() * GameManager.kwhCost);

		return bill;
	}
}
