using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
	[Header("Start Screen")]
	public GameObject startPanel;
	[Header("Headers")]
	public TextMeshProUGUI headerMonthText;
	public GameObject infoButton;

	[Header("Blueprint")]
	public Button blueprintButton;
	public Image blueprintButtonHighlight;
	public GameObject blueprintPanel;
	private Vector3 blueprintPanelOff;
	private Vector3 blueprintPanelOn;
	private bool blueprintOn = false;

	[Header("Appliance Menu")]
	public GameObject applianceMenu;
	public TextMeshProUGUI applianceName;
	public TextMeshProUGUI currentApplianceModelName;
	public TextMeshProUGUI currentApplianceEnergyUse;

	public TextMeshProUGUI upgradeAvailableText;
	public TextMeshProUGUI nextApplianceModelName;
	public TextMeshProUGUI nextApplianceEnergyUse;
	public ButtonController upgradeApplianceButton;

	[Header("Light Menu")]
	public GameObject lightMenu;
	public List<GameObject> lightChangeButtons;
	//public TextMeshProUGUI 

	[Header("Monthly Report")]
	public GameObject monthlyReportPanel;
	public TextMeshProUGUI monthlyReportTitleText;
	public StatSlider moneySlider;
	public StatSlider energySlider;
	public Button monthlyReportContinueButton;
	public TextMeshProUGUI currentObjectiveText;
	public GameObject successStreakPanel;
	public TextMeshProUGUI successStreakText;
	public TextMeshProUGUI legendEnergyBill;
	public TextMeshProUGUI legendExpenses;
	
	
	private Vector3 monthlyReportOff;
	private Vector3 monthlyReportOn;

	[Header("Audio Toggle")]
	public Image audioButtonImage;
	public Sprite audioOnSprite;
	public Sprite audioOffSprite;

	private bool muted = false;

	[Header("Scene Objects")]
	public SpriteRenderer darkness;
	public SpriteRenderer superDarkness;
	private DoorController playerDoor;

	[Header("Discussion UI")]
	public GameObject discussionGroup;
	public TextMeshProUGUI discussionText;
	public Transform discussionResponseButtonHolder;
	public Image successHighlight;
	public TypewriterText typewriterDiscussionText;

	[Header("Neighborhood Objects")]
	public List<Neighbor> neighborList = new List<Neighbor>();
	public List<GameObject> neighborhoodHouses = new List<GameObject>();
	public SpriteRenderer neighborhoodBackgroundSpriteRenderer;
	public Sprite winterSprite;
	public Sprite springSprite;
	public Sprite summerSprite;
	public Sprite fallSprite;
	
	[Header("Neighborhood UI")]
	public Button leftCycleArrow;
	public Button rightCycleArrow;

	[Header("Construct Wind Farm")]
	public TextMeshProUGUI constructionTitleText;
	public ButtonController constructWindFarmButton;
	public TextMeshProUGUI constructFarmButtonText;
	public GameObject constructWindFarmPanel;
	public TextMeshProUGUI neighborContributionText;
	public TextMeshProUGUI constructionCostText;
	public TextMeshProUGUI timeToCompleteText;

	private Vector3 constructionOff;
	private Vector3 constructionOn;

	[Header("Books")]
	public GameObject book1;
	public GameObject book2;

	private Vector3 bookOff;
	private Vector3 bookOn;

	[Header("Game Over Screen")]
	public GameObject gameOverPanel;
	public TextMeshProUGUI gameOverText;
	public TextMeshProUGUI personalBestText;

	private Vector3 gameOverOff;
	private Vector3 gameOverOn;

	

	private List<BlueprintRoomController> blueprintRoomList = new List<BlueprintRoomController>();
	private List<LightType> lightTypes = new List<LightType>();

	[Header("Scriptable Objects")]
	public LightBulb incandescentLight;
	public LightBulb cflLight;
	public LightBulb ledLight;
	public PlayerHome playerHome;

	public static Neighbor player;
	public static bool lightExpiredThisMonth = false;
	public static bool frontDoorInteractable = false;
	public static bool menuOpen = false;
	public static float kwhCost = 0.14f;
	private int month = 0;
	private int year = 2019;

	private Neighbor smartCompanyEmployee = new Neighbor("Erica");

	private bool playerPromoted = false;
	private bool metAllNeighbors = false;
	private bool startedWindFarmConstruction = false;
	private bool convincedAllNeighbors = false;
	private int promotionIncome = 500;

	private bool choseWind = false;
	private bool choseSolar = false;
	private bool choseBiomass = false;
	private bool justSpokeToNeighbor = false;

	private string farmTypeString;

	private int successStreak = 0;

	private RoomController currentRoom;
	private Discussion currentDiscussion;
	private Neighbor currentNeighbor;

	private DiscussionManager discussionManager;
	private AudioManager audioManager;


	void Start() {
		//Camera.main.aspect = 2048f / 1536f;
		if(!PlayerPrefs.HasKey("personalBest")) {
			PlayerPrefs.SetInt("personalBest", 999999999);
		}

		applianceMenu.SetActive(false);
		lightMenu.SetActive(false);
		infoButton.SetActive(false);
		discussionGroup.SetActive(false);
		leftCycleArrow.gameObject.SetActive(false);
		rightCycleArrow.gameObject.SetActive(false);
		constructWindFarmButton.gameObject.SetActive(false);
		successStreakPanel.SetActive(false);
		
		successHighlight.enabled = false;

		darkness.gameObject.SetActive(true);
		playerHome.gameObject.SetActive(true);

		discussionManager = FindObjectOfType<DiscussionManager>();
		audioManager = FindObjectOfType<AudioManager>();

		blueprintButton.onClick.AddListener(() => ToggleBlueprint());
		blueprintPanelOff = blueprintPanel.transform.position;
		blueprintPanelOn = blueprintPanelOff;
		blueprintPanelOn.y = 0;

		monthlyReportOff = monthlyReportPanel.transform.position;
		monthlyReportOn = Vector3.zero;

		constructionOff = constructWindFarmPanel.transform.position;
		constructionOn = Vector3.zero;

		bookOff = book1.transform.position;
		bookOn = Vector3.zero;

		gameOverOff = gameOverPanel.transform.position;
		gameOverOn = Vector3.zero;

		/*
		Debug.Log("DEBUG MODE ENABLED");
		playerPromoted = true;
		*/

		startPanel.SetActive(true);

		year = System.DateTime.Today.Year;

		month = System.DateTime.Today.Month - 1;

		if(Screen.width != 2048) {
			discussionText.fontSizeMax = 20;
		}
	}

	public void StartGame() {
		startPanel.SetActive(false);

		player = new Neighbor("Player");
		playerHome.SetPlayerHome();

		SetNeighbors();
		discussionManager.SetDiscussions();

		SetFirstMonthData();
		SetNewMonthData();

		headerMonthText.text = player.monthData[month].monthName + " " + player.monthData[month].year;

		SetBlueprintRooms();

		playerDoor = playerHome.GetRoom("Living Room").gameObject.GetComponentInChildren<DoorController>();

		currentDiscussion = discussionManager.GetDiscussion("instruction");
		currentNeighbor = smartCompanyEmployee;

		GoToRoom(playerHome.GetRoom("Living Room"));
	}

	public void SetNeighbors() {
		HomeController playerHouse = neighborhoodHouses[0].GetComponent<HomeController>();
		playerHouse.SetHome(player);
		playerHouse.transform.GetChild(0).GetComponent<DoorController>().neighborOwner = player;

		for(int i = 1; i < neighborhoodHouses.Count; i++) {
			//This generates a blank neighbor

			Neighbor newNeighbor = new Neighbor("");
			HomeController home = neighborhoodHouses[i].GetComponent<HomeController>();

			neighborList.Add(newNeighbor);

			home.SetHome(newNeighbor);

			home.transform.GetChild(0).GetComponent<DoorController>().neighborOwner = newNeighbor;
			home.gameObject.SetActive(false);
		}

		neighborList[0].name = "Leon";
		neighborList[1].name = "Amina";
		neighborList[2].name = "Terrance";
		neighborList[3].name = "Gerald";

		/*
		neighborList[0].caresEnvironment = true;
		neighborList[0].caresMoney = true;
		neighborList[0].caresTechnology = true;
		*/

		neighborList[0].caresEnvironment = true;

		neighborList[1].caresMoney = true;
		neighborList[1].caresTechnology = true;

		neighborList[2].caresTechnology = true;

		neighborList[3].caresEnvironment = true;
		neighborList[3].caresMoney = true;
	}

	public void SetFirstMonthData() {
		for(int i = 0; i < month; i++) {
			player.monthData.Add(new MonthData("", year));
		}
	}

	public void SetNewMonthData() {
		string monthString = "MonthString";

		if (month % 12 == 0) {
			if(month != 0) {
				year++;
			}

			monthString = "January";

			SetSeason("winter");
		} else if (month % 12 == 1) {
			monthString = "February";

			SetSeason("winter");
		} else if (month % 12 == 2) {
			monthString = "March";

			SetSeason("spring");
		} else if (month % 12 == 3) {
			monthString = "April";

			SetSeason("spring");
		} else if (month % 12 == 4) {
			monthString = "May";

			SetSeason("spring");
		} else if (month % 12 == 5) {
			monthString = "June";

			SetSeason("summer");
		} else if (month % 12 == 6) {
			monthString = "July";

			SetSeason("summer");
		} else if (month % 12 == 7) {
			monthString = "August";

			SetSeason("summer");
		} else if (month % 12 == 8) {
			monthString = "September";

			SetSeason("fall");
		} else if (month % 12 == 9) {
			monthString = "October";

			SetSeason("fall");
		} else if (month % 12 == 10) {
			monthString = "November";

			SetSeason("fall");
		} else if (month % 12 == 11) {
			monthString = "December";

			SetSeason("winter");
		}

		//for (int i = 0; i < neighborhoodHouses.Count; i++) {
			Neighbor neighbor = player;

			neighbor.monthData.Add (new MonthData (monthString, year));
			neighbor.monthData[month].moneyThisMonth = neighbor.GetWealth ();

			/*
			if(neighbor.monthData.Count > 1) {
				neighbor.monthData[month].moneyChangeThisMonth = neighbor.monthData[month].moneyThisMonth - neighbor.monthData[month - 1].moneyThisMonth;
			}
			*/

			neighbor.monthData[month].energyUsedThisMonth = playerHome.GetMonthlyEnergyUse();
			neighbor.monthData[month].energyBillThisMonth = playerHome.GetEnergyBill();

			if(playerPromoted) {
				neighbor.monthData[month].incomeThisMonth = promotionIncome;
			}
		//}
	}

	public void SetSeason(string season) {
		switch(season) {
			case "winter":
				neighborhoodBackgroundSpriteRenderer.sprite = winterSprite;
				break;
			case "spring":
				neighborhoodBackgroundSpriteRenderer.sprite = springSprite;
				break;
			case "summer":
				neighborhoodBackgroundSpriteRenderer.sprite = summerSprite;
				break;
			case "fall":
				neighborhoodBackgroundSpriteRenderer.sprite = fallSprite;
				break;
			default:
				Debug.Log("That season doesn't exist.");
				break;
		}
	}

	public void SetBlueprintRooms() {
		Debug.Log("Setting the blueprint ROOMS");

		for(int i = 0; i < blueprintPanel.transform.childCount; i++) {
			blueprintRoomList.Add(blueprintPanel.transform.GetChild(i).GetComponent<BlueprintRoomController>());
			blueprintRoomList[i].SetRoom(playerHome.GetRoomList()[i]);
		}
	}

	public void ToggleBlueprint() {
		if(!blueprintOn) {
			OpenBlueprint();
		} else {
			CloseBlueprint();
		}
	}

	public void OpenBlueprint() {
		Debug.Log("Opening the Blueprint");
		audioManager.Play("Click");

		if(frontDoorInteractable) {
			playerDoor.SetInteractable(false);
		}

		if(applianceMenu.gameObject.activeSelf) {
			CloseAppliance();
		}

		if(lightMenu.gameObject.activeSelf) {
			CloseLight();
		}

		menuOpen = true;

		if(!blueprintOn) {
			StartCoroutine(MoveObjectToPosition(blueprintPanel, blueprintPanelOn, 0.3f));
		}

		for(int i = 0; i < blueprintRoomList.Count; i++) {
			blueprintRoomList[i].SetHighlight();
		}
		
		blueprintOn = true;
	}

	public void CloseBlueprint() {
		audioManager.Play("Close");

		if(frontDoorInteractable) {
			playerDoor.SetInteractable(true);
		}

		if(applianceMenu.gameObject.activeSelf) {
			CloseAppliance();
		}

		menuOpen = false;

		if(blueprintOn) {
			StartCoroutine(MoveObjectToPosition(blueprintPanel, blueprintPanelOff, 0.3f));
		}

		blueprintOn = false;
	}

	public IEnumerator MoveObjectToPosition(GameObject obj, Vector3 endPosition, float duration) {
		Vector3 startPosition = obj.transform.position;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			obj.transform.position = Vector3.Lerp(startPosition, endPosition, timer/duration);

			yield return waiter;
		}
	}

	public void GoToRoom(RoomController room) {
		audioManager.AmbientStop("Knocking"); //if knocking is happening

		audioManager.Play("Go To Room");

		currentRoom = room;

		ApplianceCollidersEnabled(true);

		CloseBlueprint();

		playerHome.EnterRoom(room);

		room.TurnOnLights();

		AdjustDarkness();

		if(room.roomName == "Living Room") {
			playerDoor.ResetDoor();
			SetFrontDoorInteraction();
		}

		SetBlueprintButtonHighlight();
	}

	public void SetBlueprintButtonHighlight() {
		if(currentRoom.CanInteractWithSomething()) { //If the player has nothing to interact with, highlight the blueprint button
			blueprintButtonHighlight.enabled = false;
		} else {
			blueprintButtonHighlight.enabled = true;
		}
	}

	public void AdjustDarkness() {
		float darknessPercentage = currentRoom.GetPercentLightsExpired();

		float maxDarkness = 0.7f;
		if(currentRoom.roomName == "Living Room") {
			maxDarkness = 0.3f;
		}
		
		Color darknessColor = Color.black;
		darknessColor = Color.Lerp(Color.clear, darknessColor, darknessPercentage * maxDarkness);
		darkness.color = darknessColor;
	}

	public void ClickAppliance(ApplianceController ac) {
		if(!lightExpiredThisMonth && !menuOpen) {
			audioManager.Play("Appliance Click");

			menuOpen = true;

			ApplianceCollidersEnabled(false);

			applianceMenu.SetActive(true);

			float menuX = applianceMenu.transform.position.x;
			if(ac.transform.position.x > 0) {
				if(menuX > 0) {
					Vector3 newPos = applianceMenu.transform.position;
					newPos.x = -menuX;
					applianceMenu.transform.position = newPos;
				} // else it's good
			} else {
				if(menuX < 0) {
					Vector3 newPos = applianceMenu.transform.position;
					newPos.x = -menuX;
					applianceMenu.transform.position = newPos;
				} // else it's good
			}

			currentRoom.DisableApplianceHighlights();

			upgradeApplianceButton.GetComponent<Button>().onClick.RemoveAllListeners();
			upgradeApplianceButton.Unpressed();

			applianceName.text = ac.type.ToString();

			Appliance currentAppliance = ac.GetCurrentAppliance();

			currentApplianceModelName.text = currentAppliance.applianceName;
			currentApplianceEnergyUse.text = currentAppliance.kwhPerMonth + " kWh per month";

			Appliance nextAppliance = ac.GetNextAvailableAppliance();

			if(nextAppliance != null) {
				nextApplianceModelName.text = nextAppliance.applianceName;
				nextApplianceEnergyUse.text = nextAppliance.kwhPerMonth + " kWh per month";

				upgradeApplianceButton.GetButton().onClick.AddListener(() => playerHome.UpgradeAppliance(ac));
				upgradeApplianceButton.SetText("Upgrade for $" + nextAppliance.cost);

				if(player.GetWealth() > nextAppliance.cost) {
					upgradeAvailableText.text = "Available Upgrade";

					upgradeApplianceButton.Enable();
				} else {
					upgradeAvailableText.text = "Can't Afford Upgrade";

					upgradeApplianceButton.Disable();
				}
			} else {
				upgradeAvailableText.text = "No Upgrade Available";

				nextApplianceModelName.text = "";
				nextApplianceEnergyUse.text = "";

				upgradeApplianceButton.SetText("");
				upgradeApplianceButton.Disable();
			}
		}
	}

	public void ApplianceCollidersEnabled(bool enable) {
		for(int i = 0; i < currentRoom.GetApplianceList().Count; i++) {
			currentRoom.GetApplianceList()[i].GetComponent<Collider2D>().enabled = enable;
		}
	}

	public void CloseAppliance() {
		audioManager.Play("Close");

		menuOpen = false;

		ApplianceCollidersEnabled(true);

		applianceMenu.SetActive(false);

		currentRoom.EnableApplianceHighlights();
	}

	public void ClickLight(LightController light) {
		if(light.GetLightExpired()) {
			audioManager.Play("Click");

			menuOpen = true;
			lightMenu.SetActive(true);

			ApplianceCollidersEnabled(false);

			lightChangeButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = incandescentLight.typeName;
			lightChangeButtons[0].transform.GetChild(1).GetComponent<Image>().sprite = incandescentLight.litSprite;
			lightChangeButtons[0].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Lasts " + incandescentLight.hoursBeforeBurnout.ToString() + " hours";
			lightChangeButtons[0].transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
			lightChangeButtons[0].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => playerHome.ChangeLightBulb(light, incandescentLight));
			lightChangeButtons[0].transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = "Buy for $" + incandescentLight.cost;

			lightChangeButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = cflLight.typeName;
			lightChangeButtons[1].transform.GetChild(1).GetComponent<Image>().sprite = cflLight.litSprite;
			lightChangeButtons[1].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Lasts " + cflLight.hoursBeforeBurnout.ToString() + " hours";
			lightChangeButtons[1].transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
			lightChangeButtons[1].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => playerHome.ChangeLightBulb(light, cflLight));
			lightChangeButtons[1].transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = "Buy for $" + cflLight.cost;

			lightChangeButtons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ledLight.typeName;
			lightChangeButtons[2].transform.GetChild(1).GetComponent<Image>().sprite = ledLight.litSprite;
			lightChangeButtons[2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Lasts " + ledLight.hoursBeforeBurnout.ToString() + " hours";
			lightChangeButtons[2].transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
			lightChangeButtons[2].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => playerHome.ChangeLightBulb(light, ledLight));
			lightChangeButtons[2].transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = "Buy for $" + ledLight.cost;
		} else {
			light.ToggleOn();
			AdjustDarkness();
		}
	}

	public void CloseLight() {
		audioManager.Play("Close");

		menuOpen = false;
		lightMenu.SetActive(false);

		ApplianceCollidersEnabled(true);

		bool allLightsActive = true;
		for(int i = 0; i < playerHome.GetRoomList().Count; i++) {
			for(int j = 0; j < playerHome.GetRoomList()[i].GetLightList().Count; j++) {
				if(playerHome.GetRoomList()[i].GetLightList()[j].GetLightExpired()) {
					allLightsActive = false;
					break;
				}
			}
		}

		if(allLightsActive) {
			lightExpiredThisMonth = false;

			SetFrontDoorInteraction();

			SetBlueprintButtonHighlight();
		}
	}

	public void AdvanceMonth() {
		audioManager.Play("Click");

		Debug.Log("Advanced to next month");
		CloseAppliance();

		discussionGroup.SetActive(false);

		player.PayEnergyBill(player.monthData[month].energyBillThisMonth);

		if(playerPromoted) {
			player.GainIncome(promotionIncome);
		}

		month++;

		SetNewMonthData();

		DecayLightBulbs();

		StartCoroutine(TransitionToEndMonth());
	}

	public IEnumerator TransitionToEndMonth() {
		WaitForFixedUpdate waiter = new WaitForFixedUpdate();

		Vector3 startCamPosition = Camera.main.transform.position;
		Vector3 endCamPosition = new Vector3(0, 0, -10);

		float startCamSize = Camera.main.orthographicSize;
		float endCamSize = 5f;

		float duration = 1f;
		float timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			float step = timer/duration;

			Camera.main.transform.position = Vector3.Lerp(startCamPosition, endCamPosition, step);
			Camera.main.orthographicSize = Mathf.Lerp(startCamSize, endCamSize, step);

			superDarkness.color = Color.Lerp(Color.clear, Color.black, step);

			yield return waiter;
		}

		Camera.main.transform.position = endCamPosition;
		Camera.main.orthographicSize = endCamSize;

		superDarkness.color = Color.black;

		DisplayMonthlyReportPanel();
	}

	public IEnumerator TransitionToStartMonth() {
		WaitForFixedUpdate waiter = new WaitForFixedUpdate();

		for(int i = 0; i < neighborhoodHouses.Count; i++) {
			neighborhoodHouses[i].SetActive(false);
		}
		playerHome.gameObject.SetActive(true);

		UndisplayMonthylReportPanel();

		StartNextMonth();

		float duration = 1f;
		float timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			superDarkness.color = Color.Lerp(Color.black, Color.clear, timer/duration);

			yield return waiter;
		}

		superDarkness.color = Color.clear;
	}

	public void DecayLightBulbs() {
		for(int i = 0; i < playerHome.GetRoomList().Count; i++) {
			for(int j = 0; j < playerHome.GetRoomList()[i].GetLightList().Count; j++) {
				playerHome.GetRoomList()[i].GetLightList()[j].DecreaseMonthlyHours();
			}
		}
	}

	public void DisplayMonthlyReportPanel() {
		StartCoroutine(MoveObjectToPosition(monthlyReportPanel, monthlyReportOn, 0.5f));

		monthlyReportTitleText.text = player.monthData [month - 1].monthName + " " + player.monthData[month - 1].year + " Report";

		int maxMoney = 5000;
		int minMoney = 0;
		if(player.GetWealth() > 5000) {
			maxMoney = 10000;
		} else if(player.GetWealth() > 10000) {
			maxMoney = 20000;
		} else if(player.GetWealth() > 20000) {
			maxMoney = 50000;
		}

		moneySlider.SetSlider(maxMoney, minMoney, player.monthData[month - 1].moneyThisMonth, player.monthData[month - 1].energyBillThisMonth, player.monthData[month - 1].moneySpentThisMonth);
		
		int maxEnergy = 1250;
		int minEnergy = 1000;
		if(player.monthData[month].energyUsedThisMonth > 1000) {
			minEnergy = 1000;
		} else {
			minEnergy = 750;
		}

		int energyChange = player.monthData[month - 1].energyUsedThisMonth - player.monthData[month].energyUsedThisMonth;
		energySlider.SetSlider(maxEnergy, minEnergy, player.monthData[month - 1].energyUsedThisMonth, energyChange, 0);

		monthlyReportContinueButton.onClick.RemoveAllListeners();
		monthlyReportContinueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Next Month";
		monthlyReportContinueButton.onClick.AddListener(() => StartCoroutine(TransitionToStartMonth()));

		string objectiveText = "Current Objective: ";
		if(!playerPromoted) {
			objectiveText += "Upgrade appliances around the house.";
		} else if(!metAllNeighbors) {
			objectiveText += "Introduce yourself to all of your neighbors.";
		} else if(!convincedAllNeighbors) {
			objectiveText += "Convince each of your neighbors to contribute towards construction.";
		} else {
			objectiveText += "Complete the construction project.";
		}
		currentObjectiveText.text = objectiveText;

		if(successStreak > 2 && successStreak % 3 == 0 && justSpokeToNeighbor) {
			DisplaySuccessStreakPopup();
		} else {
			successStreakPanel.SetActive(false);
		}

		legendEnergyBill.text = "Energy Bill ($" + player.monthData[month - 1].energyBillThisMonth + ")";
		legendExpenses.text = "Expenses ($" + player.monthData[month - 1].moneySpentThisMonth + ")";
	}

	public void DisplaySuccessStreakPopup() {
		audioManager.Play("Success Streak");

		successStreakPanel.SetActive(true);
		successStreakText.text = "You've successfully convinced neighbors to improve their energy consumption behaviors " + successStreak + " times in a row! As a reward, your monthly income has been increased by $200.";

		player.monthlySalary += 200;
	}

	public void CloseSuccessStreakPopup() {
		audioManager.Play("Close");

		successStreakPanel.SetActive(false);
	}
	
	public void DisplayLastMonthlyReportPanel() {
		audioManager.Play("Click");

		StartCoroutine(MoveObjectToPosition(monthlyReportPanel, monthlyReportOn, 0.5f));

		monthlyReportContinueButton.onClick.RemoveAllListeners();
		monthlyReportContinueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
		monthlyReportContinueButton.onClick.AddListener(() => UndisplayMonthylReportPanel());
	}

	public void UndisplayMonthylReportPanel() {
		audioManager.Play("Close");

		StartCoroutine(MoveObjectToPosition(monthlyReportPanel, monthlyReportOff, 1f));
	}

	public void StartNextMonth() {
		audioManager.Play("Next Month");

		justSpokeToNeighbor = false;

		infoButton.SetActive(true); //This is kind of stupid to call this every month but whatevs
		blueprintButton.gameObject.SetActive(true);

		currentNeighbor = null;

		if(startedWindFarmConstruction) {
			CheckForConvincing();
		}

		/*
		for(int i = 0; i < neighborhoodHouses.Count; i++) { //This is also dumb af
			neighborhoodHouses[i].transform.GetComponentInChildren<DoorController>().ResetDoor();
		}
		*/

		headerMonthText.text = player.monthData[month].monthName + " " + player.monthData[month].year;

		CheckForExpiredLights();

		//Go To Bedroom Replacement
		currentRoom = playerHome.GetRoom("Bedroom");
		ApplianceCollidersEnabled(true);
		CloseBlueprint();
		playerHome.EnterRoom(currentRoom);
		SetBlueprintButtonHighlight();
		currentRoom.TurnOnLights();
		AdjustDarkness();

		currentDiscussion = null;

		/*
		if(!playerPromoted) {
			frontDoorInteractable = false;
		}
		*/

		SetFrontDoorInteraction();
	}

	public void CheckForExpiredLights() {
		for(int i = 0; i < playerHome.GetRoomList().Count; i++) {
			for(int j = 0; j < playerHome.GetRoomList()[i].GetLightList().Count; j++) {
				if(playerHome.GetRoomList()[i].GetLightList()[j].GetHoursRemaining() <= 0) {
					lightExpiredThisMonth = true;

					playerHome.GetRoomList()[i].GetLightList()[j].SetLightExpired(true);
				}
			}
		}
	}

	public void SetFrontDoorInteraction() {
		bool noAvailableUpgrades = true;
		for(int i = 0; i < playerHome.GetRoomList().Count; i++) {
			for(int j = 0; j < playerHome.GetRoomList()[i].GetApplianceList().Count; j++) {
				if(playerHome.GetRoomList()[i].GetApplianceList()[j].CanUpgrade()) {
					noAvailableUpgrades = false;
					break;
				}
			}
		}

		if(lightExpiredThisMonth) { //If a light is expired, don't let them leave the house
			frontDoorInteractable = false;
		} else {
			if(!playerPromoted) { //If the player hasn't been promoted yet
				if(noAvailableUpgrades) { //If there are no available upgrades
					frontDoorInteractable = true;
					
					currentDiscussion = discussionManager.GetDiscussion("promotion");
				} else {
					if(currentDiscussion != null) {
						frontDoorInteractable = true;
					} else {
						frontDoorInteractable = false;
					}
				}
			} else { //If the player has already been promoted, they can leave whenevs
				frontDoorInteractable = true;

				if(metAllNeighbors && !startedWindFarmConstruction) {
					currentDiscussion = discussionManager.GetDiscussion("startConstruction");
				} else {
					currentDiscussion = null;
				}
			}
		}

		if(frontDoorInteractable) {
			playerDoor.GetComponent<BoxCollider2D>().enabled = true;

			if(currentDiscussion != null) {
				audioManager.AmbientPlay("Knocking");
			}
		} else {
			playerDoor.GetComponent<BoxCollider2D>().enabled = false;
		}

		playerDoor.SetInteractable(frontDoorInteractable);
	}

	public void InteractWithPlayerDoor() {
		audioManager.Play("Door");
		audioManager.AmbientStop("Knocking");

		if(currentDiscussion != null) {
			DisplayDiscussion(currentDiscussion);
		} else {
			LeaveHouse();
		}

		playerDoor.GetComponent<BoxCollider2D>().enabled = false;
	}

	public void LeaveHouse() {
		Debug.Log("Leaving the house");
		playerHome.gameObject.SetActive(false);

		blueprintButton.gameObject.SetActive(false);

		leftCycleArrow.gameObject.SetActive(true);
		rightCycleArrow.gameObject.SetActive(true);

		for(int i = 0; i < neighborhoodHouses.Count; i++) {
			neighborhoodHouses[i].SetActive(false);
		}

		GoToHouse(neighborhoodHouses[0]);

		if(metAllNeighbors) {
			constructWindFarmButton.gameObject.SetActive(true);
		}
	}

	public void ReturnToHouse() {
		playerHome.gameObject.SetActive(true);

		blueprintButton.gameObject.SetActive(true);

		for(int i = 0; i < neighborhoodHouses.Count; i++) {
			neighborhoodHouses[i].SetActive(false);
		}

		GoToRoom(playerHome.GetRoom("Living Room"));

		constructWindFarmButton.gameObject.SetActive(false);
	}

	public void GoToHouse(GameObject house) {
		audioManager.Play("Go To House");

		for(int i = 0; i < neighborhoodHouses.Count; i++) {
			neighborhoodHouses[i].SetActive(false);
		}

		house.SetActive(true);

		DoorController houseDoor = house.transform.GetComponentInChildren<DoorController>();
		Neighbor neighbor = house.GetComponent<HomeController>().GetHomeOwner();

		if(neighbor == player) {
			currentDiscussion = null;

			houseDoor.SetInteractable(true);
		} else  {
			if(!neighbor.hasMetPlayer) {
				currentDiscussion = discussionManager.GetDiscussion("introduction", neighbor);
			} else {
				if(discussionManager.GetRandomDiscussion(neighbor) != null) {
					currentDiscussion = discussionManager.GetRandomDiscussion(neighbor);
				} else { //Neighbor has finished all discussions
					currentDiscussion = null;
				}
			}

			if(currentDiscussion != null) {
				houseDoor.SetInteractable(true);
			} else {
				houseDoor.SetInteractable(false);
			}
		}
		

		leftCycleArrow.onClick.RemoveAllListeners();
		rightCycleArrow.onClick.RemoveAllListeners();

		int currentHouseIndex = -1;
		for(int i = 0; i < neighborhoodHouses.Count; i++) {
			if(neighborhoodHouses[i] == house) {
				currentHouseIndex = i;
				break;
			}
		}
		GameObject leftHouse;
		if(currentHouseIndex > 0) {
			leftHouse = neighborhoodHouses[currentHouseIndex - 1];
		} else {
			leftHouse = neighborhoodHouses[neighborhoodHouses.Count - 1];
		}
		GameObject rightHouse;
		if(currentHouseIndex < neighborhoodHouses.Count - 1) {
			rightHouse = neighborhoodHouses[currentHouseIndex + 1];
		} else {
			rightHouse = neighborhoodHouses[0];
		}

		leftCycleArrow.onClick.AddListener(() => GoToHouse(leftHouse));
		rightCycleArrow.onClick.AddListener(() => GoToHouse(rightHouse));

		if((currentHouseIndex % 2 == 1) || currentHouseIndex == neighborhoodHouses.Count - 1) {
			neighborhoodBackgroundSpriteRenderer.transform.localEulerAngles = Vector3.zero;
		} else {
			neighborhoodBackgroundSpriteRenderer.transform.localEulerAngles = new Vector3(0, 180, 0);
		}
	}

	public void InteractWithNeighbor(Neighbor neighbor) {
		Debug.Log("Interacting with " + neighbor.name);

		audioManager.Play("Door");

		currentNeighbor = neighbor;

		leftCycleArrow.gameObject.SetActive(false);
		rightCycleArrow.gameObject.SetActive(false);

		constructWindFarmButton.gameObject.SetActive(false);

		if(neighbor == player) {
			ReturnToHouse();
		} else {
			Vector3 newCam = neighbor.GetHome().GetNeighborPosition();
			newCam.z = -10;
			Camera.main.transform.position = newCam;
			StartCoroutine(ZoomAndInteract(newCam, neighbor));

			if(!neighbor.hasMetPlayer) {
				neighbor.hasMetPlayer = true;

				CheckIfMetAllNeighbors();
			}
		}
	}

	public void CheckIfMetAllNeighbors() {
		bool metThemAll = true;
		for(int i = 0; i < neighborList.Count; i++) {
			if(!neighborList[i].hasMetPlayer) {
				metThemAll = false;
				break;
			}
		}

		if(metThemAll) {
			metAllNeighbors = true;
		}
	}

	public IEnumerator ZoomAndInteract(Vector3 zoomPosition, Neighbor neighbor) {

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float duration = 0.5f;
		float timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			Camera.main.transform.position = Vector3.Lerp(Vector3.zero, zoomPosition, timer/duration);
			Camera.main.orthographicSize = Mathf.Lerp(5, 2.5f, timer/duration);

			yield return waiter;
		}

		Camera.main.transform.position =  zoomPosition;
		Camera.main.orthographicSize = 2.5f;

		DisplayDiscussion(currentDiscussion);
	}

	public void DisplayDiscussion(Discussion discussion) {
		blueprintButton.gameObject.SetActive(false);
		infoButton.gameObject.SetActive(false);

		discussionGroup.SetActive(true);
		string discussionString = '"' + discussion.introduction + '"';
		typewriterDiscussionText.SetStringAndStart(discussionString);

		for(int i = 0; i < discussionResponseButtonHolder.childCount; i++) {
			ButtonController button = discussionResponseButtonHolder.GetChild(i).GetComponent<ButtonController>();
			if(i < discussion.responses.Count) {
				Response r = discussion.responses[i];

				button.gameObject.SetActive(true);
				string buttonString = "";
				if(r.type == ResponseType.Environment || r.type == ResponseType.Money || r.type == ResponseType.Technology) {
					buttonString = "Discuss ";
				}
				buttonString += r.type.ToString();
				if(r.type == ResponseType.Greet) {
					if(currentNeighbor != null) {
						buttonString += " " + currentNeighbor.name;
					}
				}
				button.SetText(buttonString);
				button.GetButton().onClick.RemoveAllListeners();
				button.GetButton().onClick.AddListener(() => SelectResponse(r));
			} else {
				button.gameObject.SetActive(false);
			}
		}
		discussionResponseButtonHolder.gameObject.SetActive(false);
	}

	public void ActivateDiscussionButtons() {
		discussionResponseButtonHolder.gameObject.SetActive(true);
	}

	public void SelectResponse(Response response) {
		audioManager.Play("Click");

		string responseString = response.response;
		discussionText.fontStyle = FontStyles.Italic;
		typewriterDiscussionText.SetStringAndStart(responseString);

		if(response.key == "promotion") {
			Debug.Log("Player is promoted!");
			playerPromoted = true;
		}

		if(response.key == "startConstruction") {
			startedWindFarmConstruction = true;
			if(response.type == ResponseType.Wind) {
				choseWind = true;
				farmTypeString = "wind farm";
			} else if(response.type == ResponseType.Solar) {
				choseSolar = true;
				farmTypeString = "solar farm";
			} else if(response.type == ResponseType.Biomass) {
				choseBiomass = true;
				farmTypeString = "leaf-litter biomass farm";
			}

			constructFarmButtonText.text = "Construct " + farmTypeString;
		}

		for(int i = 0; i < discussionResponseButtonHolder.childCount; i++) {
			ButtonController button = discussionResponseButtonHolder.GetChild(i).GetComponent<ButtonController>();
			if(i == 0) {
				button.SetText("Continue");
				button.GetButton().onClick.RemoveAllListeners();
				button.GetButton().onClick.AddListener(() => ReactToResponse(response));
			} else {
				button.gameObject.SetActive(false);
			}
		}
		discussionResponseButtonHolder.gameObject.SetActive(false);
	}

	public void ReactToResponse(Response response) {
		audioManager.Play("Click");

		string reactionString = '"' + response.reaction + '"';
		discussionText.fontStyle = FontStyles.Normal;
		typewriterDiscussionText.SetStringAndStart(reactionString);

		ButtonController button = discussionResponseButtonHolder.GetChild(0).GetComponent<ButtonController>();
		button.GetButton().onClick.RemoveAllListeners();
		button.GetButton().onClick.AddListener(() => AdvanceMonth());

		discussionResponseButtonHolder.gameObject.SetActive(false);

		//Check to see if this discussion should be removed from the available discussions
		if(response.type == ResponseType.Environment || response.type == ResponseType.Money || response.type == ResponseType.Technology) {
			if(response.successful) {
				justSpokeToNeighbor = true;

				discussionManager.RemoveDiscussion(currentDiscussion);

				successStreak++;

				StartCoroutine(FlashSuccess(true));

				if(response.key == "convince") {
					currentNeighbor.willContribute = true;
				}
			} else  {
				successStreak = 0;

				StartCoroutine(FlashSuccess(false));
			}
		}
	}

	public IEnumerator ShakeScreen(float intensityModifier) {
		Vector3 startCameraPosition = Camera.main.transform.position;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float duration = 0.4f;
		float timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			Camera.main.transform.position = startCameraPosition + ((Vector3)Random.insideUnitCircle * intensityModifier);

			yield return waiter;
		}

		Camera.main.transform.position = startCameraPosition;
	}

	public IEnumerator FlashSuccess(bool success) {
		

		successHighlight.enabled = true;

		Color baseColor;
		if(success) {
			baseColor = Color.green;

			audioManager.Play("Success");
		} else {
			baseColor = Color.red;

			audioManager.Play("Failure");

			StartCoroutine(ShakeScreen(0.1f));
		}

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		Color firstColor = Color.Lerp(Color.clear, baseColor, 0.8f);
		float duration = 0.2f;
		float timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			successHighlight.color = Color.Lerp(Color.clear, firstColor, timer/duration);

			yield return waiter;
		}

		Color secondColor = Color.Lerp(firstColor, Color.clear, 0.7f);
		duration = 0.2f;
		timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			successHighlight.color = Color.Lerp(firstColor, secondColor, timer/duration);

			yield return waiter;
		}

		duration = 0.4f;
		timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			successHighlight.color = Color.Lerp(secondColor, baseColor, timer/duration);

			yield return waiter;
		}

		duration = 2.5f;
		timer = 0f;
		while(timer < duration) {
			timer += Time.deltaTime;

			successHighlight.color = Color.Lerp(baseColor, Color.clear, timer/duration);

			yield return waiter;
		}

		successHighlight.enabled = false;
	}

	public void CheckForConvincing() {
		bool allNeighborsConributing = true;
		for(int i = 0; i < neighborList.Count; i++) {
			if(!neighborList[i].willContribute) {
				allNeighborsConributing = false;

				if(discussionManager.GetRandomDiscussion(neighborList[i]) == null) {
					discussionManager.AddConvincingDiscussions(farmTypeString, neighborList[i]);
				}
			}
		}

		if(allNeighborsConributing) {
			string typeString = "";
			if(choseSolar) {
				typeString = "solar";
			} else if(choseWind) {
				typeString = "wind";
			} else if(choseBiomass) {
				typeString = "biomass";
			}
			discussionManager.AddFinalDiscussion(typeString);

			currentDiscussion = discussionManager.GetDiscussion("endGame");
		}
	}

	public void OpenWindFarmConstructionPanel() {
		audioManager.Play("Click");

		StartCoroutine(MoveObjectToPosition(constructWindFarmPanel, constructionOn, 0.5f));

		DetermineConstructionCost();

		if(choseWind) {
			constructionTitleText.text = "Wind Farm Construction";
		} else if(choseSolar) {
			constructionTitleText.text = "Solar Farm Construction";
		} else if(choseBiomass) {
			constructionTitleText.text = "Leaf-Litter Biomass Construction";
		}
	}

	public void DetermineConstructionCost() {
		int baseConstructionCost = 100000; //a milly?
		int numNeighborsContributing = 0; //starts with player
		for(int i = 0; i < neighborList.Count; i++) {
			if(neighborList[i].willContribute) {
				numNeighborsContributing++;
			}
		}

		int constructionCost = baseConstructionCost - ((baseConstructionCost / (neighborList.Count + 1)) * numNeighborsContributing);

		neighborContributionText.text = numNeighborsContributing + "/" + (neighborList.Count) + " Neighbors Contributing";
		constructionCostText.text = "Costs $" + constructionCost;


		int netChangePerMonth = player.monthData[month].incomeThisMonth - player.monthData[month].energyBillThisMonth;
		Debug.Log("Net change: " + netChangePerMonth);

		int moneyNeeded = constructionCost - player.GetWealth();

		int monthsToComplete = moneyNeeded / netChangePerMonth;
		
		int yearsToComplete = monthsToComplete / 12;
		int extraMonthsToComplete = monthsToComplete % 12;

		timeToCompleteText.text = yearsToComplete + " Years and " + extraMonthsToComplete + " Months to Construct";

		endDate = GetMonthString(extraMonthsToComplete) + " " + (year + yearsToComplete);
	}

	private string endDate;
	public string GetMonthString(int m) {
		string monthString = "";

		if (m % 12 == 0) {
			monthString = "January";
		} else if (m % 12 == 1) {
			monthString = "February";
		} else if (m % 12 == 2) {
			monthString = "March";
		} else if (m % 12 == 3) {
			monthString = "April";
		} else if (m % 12 == 4) {
			monthString = "May";
		} else if (m % 12 == 5) {
			monthString = "June";
		} else if (m % 12 == 6) {
			monthString = "July";
		} else if (m % 12 == 7) {
			monthString = "August";
		} else if (m % 12 == 8) {
			monthString = "September";
		} else if (m % 12 == 9) {
			monthString = "October";
		} else if (m % 12 == 10) {
			monthString = "November";
		} else if (m % 12 == 11) {
			monthString = "December";
		}

		return monthString;
	}

	public void ConstructWindFarm() {
		StartCoroutine(MoveObjectToPosition(gameOverPanel, gameOverOn, 0.5f));

		string choiceName = "";
		if(choseWind) {
			choiceName = "wind";
		} else if(choseSolar) {
			choiceName = "solar";
		} else if(choseBiomass) {
			choiceName = "leaf-litter biomass";
		} else {
			Debug.Log("The game is messed up. How'd it get messed up? You never chose a thang for the energy self-sufficiency.");
		}

		gameOverText.text = "You constructed the " + choiceName + " farm in " + endDate + " and thanks to your efforts improving your neighbors' energy consumption your community generates 100% of the energy it consumes.";


		//This would need quite a bit of refactoring to get working properly. Not priority.
		if(PlayerPrefs.GetInt("personalBest") > month) {
			PlayerPrefs.SetInt("personalBest", month);

			//personalBestText.text = "Personal Best: " + endDate;
		} else {
			//personalBestText.text = "Personal Best: " + GetMonthString(PlayerPrefs.GetInt("personalBest"));
		}
	}
	
	public void CloseWindFarmConstructionPanel() {
		audioManager.Play("Close");

		StartCoroutine(MoveObjectToPosition(constructWindFarmPanel, constructionOff, 0.5f));
	}

	public void OpenBook(bool bedBook) {
		menuOpen = true;

		if(bedBook) {
			StartCoroutine(MoveObjectToPosition(book1, bookOn, 0.5f));
		} else {
			StartCoroutine(MoveObjectToPosition(book2, bookOn, 0.5f));
		}
	}

	public void CloseBook(bool bedBook) {
		menuOpen = false;

		if(bedBook) {
			StartCoroutine(MoveObjectToPosition(book1, bookOff, 0.5f));
		} else {
			StartCoroutine(MoveObjectToPosition(book2, bookOff, 0.5f));
		}
	}

	public void ToggleMute() {
		if(!muted) {
			audioManager.Mute();

			audioButtonImage.sprite = audioOffSprite;
		} else {
			audioManager.Unmute();

			audioButtonImage.sprite = audioOnSprite;
		}

		muted = !muted;
	}

	public void RestartGame() {
		SceneManager.LoadScene(0);
	}

	public static bool IsPointerOverGameObject() {
		if(EventSystem.current.IsPointerOverGameObject()) {
			Debug.Log("POINTER OVER GAME OBJECT");

			return true;
		}

		for(int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch(i);

			//if(touch.phase == TouchPhase.Began) {

				if(EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
					return true;
				}
			//}
		}

		return false;
	}
}
