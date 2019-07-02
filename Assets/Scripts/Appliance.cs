using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ApplianceType {
	Washer,
	Dryer,
	Dishwasher,
	Refrigerator,
	Oven,
	Microwave
}

[CreateAssetMenu(fileName = "Appliance", menuName = "Appliance", order = 2)]
public class Appliance : ScriptableObject {
	public string applianceName;
	public Sprite sprite;
	public int cost;
	public int kwhPerMonth;

	public ApplianceType applianceType;

	public Appliance(ApplianceType type, string n, int c, int kwh) {
		applianceType = type;
		name = n;
		cost = c;
		kwhPerMonth = kwh;
	}
}
