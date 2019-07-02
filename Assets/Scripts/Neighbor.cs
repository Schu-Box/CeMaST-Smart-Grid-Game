using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor {
	public bool player = false;

	public string name;

	public int startingWealth = 2000;
	private int wealth;

	public int monthlySalary = 0; //Should be private eventually

	public bool hasMetPlayer = false; //Should be private
	public bool caresEnvironment = false;
	public bool caresMoney = false;
	public bool caresTechnology = false;

	public bool willContribute = false;

	public List<MonthData> monthData = new List<MonthData> ();

	private HomeController home;

	public Neighbor(string n) { //Constructor
		name = n;
		wealth = startingWealth;
	}
	public void SetHome(HomeController h) {
		home = h;
	}

	public HomeController GetHome() {
		return home;
	}

	public int GetWealth() {
		return wealth;
	}

	public void SpendMoney(int moneySpent) {
		wealth -= moneySpent;

		monthData[monthData.Count - 1].moneySpentThisMonth += moneySpent;
	}

	public void PayEnergyBill(int bill) {
		wealth -= bill;
	}

	public void GainIncome(int income) {
		wealth += income;

		monthData[monthData.Count - 1].incomeThisMonth += income;
	}
}

public class MonthData {

	public string monthName;
	public int year;
	public int averageTemperature; //unused

	public int moneyThisMonth;
	//public int moneyChangeThisMonth = 0;
	public int moneySpentThisMonth;
	public int energyUsedThisMonth;
	public int energyBillThisMonth;
	public int incomeThisMonth;

	public MonthData(string name, int y) {
		monthName = name;
		year = y;
	}
}
